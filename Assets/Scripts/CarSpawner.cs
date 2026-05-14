using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject carPrefab;

    [SerializeField] private Transform spawnPoint;

    [Header("References")]
    [SerializeField] private ParkingLot parkingLot;

    [SerializeField] private Transform parkingEntryPoint;

    [SerializeField] private Transform[] circlePoints;

    [SerializeField] private float spawnInterval = 5f;

    private float spawnTimer = 0f;

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
        GameObject spawnedCar = Instantiate(
            carPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        CarAgent carAgent = spawnedCar.GetComponent<CarAgent>();

        if (carAgent != null)
        {
            carAgent.SetParkingLot(parkingLot);

            carAgent.SetParkingEntryPoint(parkingEntryPoint);

            carAgent.SetCirclePoints(circlePoints);
        }

        Debug.Log("Car spawned.");
    }
}