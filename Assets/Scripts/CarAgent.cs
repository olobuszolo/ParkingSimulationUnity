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
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!isCircling)
            {
                StartCircling();
            }
            else
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
}