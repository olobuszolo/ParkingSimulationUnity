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
            agent.isStopped &&
            !isCircling &&
            !isParking &&
            !isParked &&
            GameManager.Instance.CanEnterParking()
        )
        {
            agent.isStopped = false;

            agent.SetDestination(parkingEntryPoint.position);

            DecideParking();
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

        Destroy(gameObject);
    }

}