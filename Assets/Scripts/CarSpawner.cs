using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] carPrefabs;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private Transform entryReleaseWaypoint;
    [SerializeField] private Transform entryOccupyWaypoint;

    [Header("References")]
    [SerializeField] private ParkingLot parkingLot;

    [SerializeField] private Transform parkingEntryPoint;

    [SerializeField] private Transform[] circlePoints;

    [SerializeField] private Transform leavePoint;

    private float spawnTimer = 0f;

    private float currentSpawnInterval;

    private void Start()
    {
        SetRandomSpawnTime();
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= currentSpawnInterval)
        {
            SpawnCar();

            spawnTimer = 0f;

            SetRandomSpawnTime();
        }
    }

    private void SetRandomSpawnTime()
    {
        currentSpawnInterval = Random.Range(
            SimulationSettings.minSpawnTime,
            SimulationSettings.maxSpawnTime
        );
    }

    private void SpawnCar()
    {
        GameObject randomCarPrefab =
            carPrefabs[Random.Range(0, carPrefabs.Length)];

        GameObject spawnedCar = Instantiate(
            randomCarPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        CarAgent carAgent = spawnedCar.GetComponent<CarAgent>();

        if (carAgent != null)
        {
            carAgent.SetParkingLot(parkingLot);

            carAgent.SetParkingEntryPoint(parkingEntryPoint);

            carAgent.SetCirclePoints(circlePoints);

            carAgent.SetEntryReleaseWaypoint(
                entryReleaseWaypoint
            );

            carAgent.SetEntryOccupyWaypoint(
                entryOccupyWaypoint
            );

            carAgent.SetLeavePoint(leavePoint);
        }
    }

}