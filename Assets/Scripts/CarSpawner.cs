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

    [SerializeField] private float spawnInterval = 3f;

    private float spawnTimer = 0f;

    [SerializeField] private Transform leavePoint;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;

            SpawnCar();
        }
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