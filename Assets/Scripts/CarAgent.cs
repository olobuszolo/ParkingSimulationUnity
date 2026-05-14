using UnityEngine;
using UnityEngine.AI;

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

    private bool isParking = false;

    private ParkingSpot targetParkingSpot;

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

        Debug.Log(
            gameObject.name +
            " | Driver Type: " + driverType +
            " | Max Price: " + maxAcceptedPrice
        );
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
            }

            Debug.Log(gameObject.name + " parked.");

            isParking = false;

            enabled = false;

            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!isCircling && !isParking)
            {
                DecideParking();
            }
            else if (isCircling)
            {
                GoToNextCirclePoint();
            }
        }
    }

    private void StartCircling()
    {
        isCircling = true;

        currentCircleIndex = 0;

        agent.SetDestination(circlePoints[currentCircleIndex].position);
    }

    private void GoToNextCirclePoint()
    {
        currentCircleIndex++;

        if (currentCircleIndex >= circlePoints.Length)
        {
            currentCircleIndex = 0;
        }

        agent.SetDestination(circlePoints[currentCircleIndex].position);
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
        if (parkingLot == null)
        {
            StartCircling();
            return;
        }

        bool hasFreeSpot = parkingLot.HasFreeSpots();

        int currentPrice = parkingLot.GetCurrentPrice();

        if (hasFreeSpot && currentPrice <= maxAcceptedPrice)
        {
            ParkingSpot freeSpot = parkingLot.GetFreeParkingSpot();

            targetParkingSpot = freeSpot;

            if (freeSpot != null)
            {
                isParking = true;

                agent.SetDestination(
                    freeSpot.GetParkingPoint().position
                );

                Debug.Log(gameObject.name + " decided to park.");
            }
        }
        else
        {
            Debug.Log(gameObject.name + " decided to circle.");

            StartCircling();
        }
    }

}