using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum DriverType
{
    Flexible,
    Inflexible
}

public class CarAgent : MonoBehaviour
{
    public Transform parkingEntryPoint;

    public Transform[] circlePoints;

    private NavMeshAgent agent;

    [Header("Parking")]
    [SerializeField] private ParkingLot parkingLot;

    [Header("Flexible Driver")]
    [SerializeField] private float flexibleMaxPrice = 10f;

    [Header("Inflexible Driver")]
    [SerializeField] private float inflexibleMaxPrice = 20f;

    [Header("Driver Settings")]
    [SerializeField] private DriverType driverType;

    [SerializeField] private float maxAcceptedPrice;

    [SerializeField] private Renderer carRenderer;

    [SerializeField] private Material flexibleMaterial;

    [SerializeField] private Material inflexibleMaterial;

    private int currentCircleIndex = 0;

    private bool isCircling = false;

    private bool countedAsCircling = false;

    [SerializeField] private float parkingCheckInterval = 5f;

    private float parkingCheckTimer = 0f;

    private bool isParking = false;
    private bool isParked = false;

    private ParkingSpot targetParkingSpot;

    private bool isGoingToPark = false;
    private Transform targetEntryWaypoint;
    [SerializeField] private Transform entryReleaseWaypoint;
    [SerializeField] private Transform entryOccupyWaypoint;

    [SerializeField] private float parkedTime = 10f;

    // cofanie z miejsca parkingowego
    [SerializeField] private float reverseDistance = 2f;

    [SerializeField] private float reverseSpeed = 2f;

    [SerializeField] private Transform exitTargetPoint;

    private bool isLeavingParking = false;

    [SerializeField] private Transform leavePoint;

    private bool shouldLeaveMap = false;

    private bool isLeavingMap = false;

    private Transform targetExitWaypoint;

    private bool hasLeftParking = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        SetupDriver();

        agent.SetDestination(parkingEntryPoint.position);
    }

    private void SetupDriver()
    {
        int randomDriver = Random.Range(0, 2);

        if (randomDriver == 0)
        {
            driverType = DriverType.Flexible;
            maxAcceptedPrice = flexibleMaxPrice;

            if (carRenderer != null && flexibleMaterial != null)
            {
                carRenderer.material = flexibleMaterial;
            }
        }
        else
        {
            driverType = DriverType.Inflexible;
            maxAcceptedPrice = inflexibleMaxPrice;

            if (carRenderer != null && inflexibleMaterial != null)
            {
                carRenderer.material = inflexibleMaterial;
            }
        }

    }

    private void Update()
    {
        if (!agent.enabled)
        {
            return;
        }

        if (
            isLeavingMap &&
            !agent.pathPending &&
            agent.remainingDistance < 0.5f
        )
        {
            Destroy(gameObject);

            return;
        }

        if (
            isParking &&
            !agent.pathPending &&
            agent.remainingDistance < 0.5f
        )
        {
            agent.isStopped = true;

            if (targetParkingSpot != null)
            {
                targetParkingSpot.OccupySpot(gameObject);

                GameManager.Instance.AddParkedCar();
            }

            isParking = false;
            isParked = true;

            StartCoroutine(LeaveParkingAfterTime());

            return;
        }

        if (!isParked && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!isCircling && !isParking)
            {
                if (!hasLeftParking)
                {
                    if (GameManager.Instance.CanEnterParking())
                    {
                        DecideParking();
                    }
                    else
                    {
                        if (!agent.isStopped)
                        {
                            agent.isStopped = true;
                        }
                    }
                }
            }
            else if (isCircling)
            {
                GoToNextCirclePoint();
            }
        }

        if (isCircling)
        {
            parkingCheckTimer += Time.deltaTime;

            if (parkingCheckTimer >= parkingCheckInterval)
            {
                parkingCheckTimer = 0f;

                DecideParking();
            }
        }

        if (
            !isCircling &&
            !isParking &&
            !isLeavingMap &&
            !isParked &&
            targetExitWaypoint != null &&
            !agent.pathPending &&
            agent.remainingDistance < 0.5f
        )
        {
            isCircling = true;

            shouldLeaveMap = true;

            currentCircleIndex = GetClosestCirclePointIndex(targetExitWaypoint);

            currentCircleIndex++;

            if (currentCircleIndex >= circlePoints.Length)
            {
                currentCircleIndex = 0;
            }

            agent.SetDestination(
                circlePoints[currentCircleIndex].position
            );

            targetExitWaypoint = null;

            return;
        }

        if (
            agent.enabled &&
            agent.isStopped &&
            !isParking &&
            !isParked
        )
        {
            if (GameManager.Instance.CanEnterParking())
            {
                agent.isStopped = false;

                DecideParking();
            }
        }

    }

    private void StartCircling()
    {
        ClearReservation();
        if (isCircling)
        {
            return;
        }

        isCircling = true;

        if (!countedAsCircling)
        {
            GameManager.Instance.AddCirclingCar();
            countedAsCircling = true;
        }

        currentCircleIndex = 0;

        agent.SetDestination(circlePoints[currentCircleIndex].position);
    }

    private void GoToNextCirclePoint()
    {
        Transform currentWaypoint =
            circlePoints[currentCircleIndex];

        if (
            shouldLeaveMap &&
            currentWaypoint.name.Contains("CirclePoint_3")
        )
        {
            shouldLeaveMap = false;

            isLeavingMap = true;

            isCircling = false;

            agent.SetDestination(leavePoint.position);

            return;
        }

        if (currentWaypoint == entryOccupyWaypoint)
{
            GameManager.Instance.OccupyEntry();
        }

        if (currentWaypoint == entryReleaseWaypoint)
        {
            GameManager.Instance.FreeEntry();
        }

        if (
            isGoingToPark &&
            currentWaypoint == targetEntryWaypoint
        )
        {
            if (countedAsCircling)
            {
                GameManager.Instance.RemoveCirclingCar();
                countedAsCircling = false;
            }

            isParking = true;
            isCircling = false;
            isGoingToPark = false;

            agent.SetDestination(
                targetParkingSpot.GetParkingPoint().position
            );

            return;
        }

        currentCircleIndex++;

        if (currentCircleIndex >= circlePoints.Length)
        {
            currentCircleIndex = 0;
        }

        agent.SetDestination(
            circlePoints[currentCircleIndex].position
        );
    }

    public float GetMaxAcceptedPrice()
    {
        return maxAcceptedPrice;
    }

    public DriverType GetDriverType()
    {
        return driverType;
    }

    private void DecideParking()
    {
        if (hasLeftParking)
        {
            return;
        }
        ClearReservation();

        if (parkingLot == null)
        {
            StartCircling();
            return;
        }

        bool hasFreeSpot = parkingLot.HasFreeSpots();

        int currentPrice = parkingLot.GetCurrentPrice();

        if (hasFreeSpot && currentPrice <= maxAcceptedPrice)
        {
            ClearReservation();

            ParkingSpot freeSpot = parkingLot.GetFreeParkingSpot();

            targetParkingSpot = freeSpot;

            if (targetParkingSpot != null)
            {
                targetParkingSpot.ReserveSpot();

                targetEntryWaypoint = freeSpot.GetEntryWaypoint();
                targetExitWaypoint = freeSpot.GetExitWaypoint();

                isGoingToPark = true;

                if (!isCircling)
                {
                    isCircling = true;

                    currentCircleIndex = 0;

                    agent.SetDestination(
                        circlePoints[currentCircleIndex].position
                    );
                }

            }
        }
        else
        {
            StartCircling();
        }
    }

    public void SetParkingLot(ParkingLot lot)
    {
        parkingLot = lot;
    }

    public void SetParkingEntryPoint(Transform entryPoint)
    {
        parkingEntryPoint = entryPoint;
    }

    public void SetCirclePoints(Transform[] points)
    {
        circlePoints = points;
    }

    public void SetEntryReleaseWaypoint(
        Transform waypoint
    )
    {
        entryReleaseWaypoint = waypoint;
    }

    public void SetEntryOccupyWaypoint(
        Transform waypoint
    )
    {
        entryOccupyWaypoint = waypoint;
    }

    private void ClearReservation()
    {
        if (targetParkingSpot != null)
        {
            targetParkingSpot.ClearReservation();
        }
    }

    private IEnumerator LeaveParkingAfterTime()
    {
        yield return new WaitForSeconds(parkedTime);

        if (targetParkingSpot != null)
        {
            targetParkingSpot.FreeSpot();
        }

        GameManager.Instance.RemoveParkedCar();

        StartCoroutine(LeaveParkingRoutine());
    }

    private IEnumerator LeaveParkingRoutine()
    {
        isLeavingParking = true;

        agent.enabled = false;

        float reverseTime = 1f;

        float timer = 0f;

        while (timer < reverseTime)
        {
            timer += Time.deltaTime;

            transform.position -=
                transform.forward * reverseSpeed * Time.deltaTime;

            transform.Rotate(
                0f,
                0f * Time.deltaTime,
                0f
            );

            yield return null;
        }

        agent.enabled = true;

        isLeavingParking = false;

        agent.enabled = true;

        isLeavingParking = false;

        isParked = false;

        hasLeftParking = true;

        agent.SetDestination(
            targetExitWaypoint.position
        );
    }

    public void SetLeavePoint(Transform point)
    {
        leavePoint = point;
    }

    private int GetClosestCirclePointIndex(
        Transform targetPoint
    )
    {
        for (int i = 0; i < circlePoints.Length; i++)
        {
            if (circlePoints[i] == targetPoint)
            {
                return i;
            }
        }

        return 0;
    }

}