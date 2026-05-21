using UnityEngine;
using System.Collections.Generic;

public class ParkingLot : MonoBehaviour
{
    public ParkingSpot[] parkingSpots;

    [SerializeField] private Transform[] queuePoints;

    private List<CarAgent> waitingCars = new List<CarAgent>();

    private void Start()
    {
        parkingSpots = FindObjectsByType<ParkingSpot>(FindObjectsSortMode.None);
    }

    public int GetFreeSpotsCount()
    {
        int freeSpots = 0;

        foreach (ParkingSpot spot in parkingSpots)
        {
            if (spot.IsFree())
            {
                freeSpots++;
            }
        }

        return freeSpots;
    }

    public int GetOccupiedSpotsCount()
    {
        return parkingSpots.Length - GetFreeSpotsCount();
    }

    public float GetOccupancyPercentage()
    {
        if (parkingSpots.Length == 0)
        {
            return 0f;
        }

        return (float)GetOccupiedSpotsCount() / parkingSpots.Length * 100f;
    }

    public float GetCurrentPrice()
    {
        float occupancy = GetOccupancyPercentage();

        if (occupancy >= SimulationSettings.highThreshold)
        {
            return SimulationSettings.highPrice;
        }

        if (occupancy >= SimulationSettings.mediumThreshold)
        {
            return SimulationSettings.mediumPrice;
        }

        return SimulationSettings.lowPrice;
    }

    public ParkingSpot GetFreeParkingSpot()
    {
        foreach (ParkingSpot spot in parkingSpots)
        {
            if (spot.IsFree())
            {
                return spot;
            }
        }

        return null;
    }

    public bool HasFreeSpots()
    {
        return GetFreeSpotsCount() > 0;
    }

    public int JoinQueue(CarAgent car)
    {
        if (!waitingCars.Contains(car))
        {
            waitingCars.Add(car);
        }

        return waitingCars.IndexOf(car);
    }

    public void LeaveQueue(CarAgent car)
    {
        if (waitingCars.Contains(car))
        {
            waitingCars.Remove(car);

            UpdateQueuePositions();
        }
    }

    public Transform GetQueuePoint(int index)
    {
        if (index >= queuePoints.Length)
        {
            index = queuePoints.Length - 1;
        }

        return queuePoints[index];
    }

    public bool IsFirstInQueue(CarAgent car)
    {
        return waitingCars.Count > 0 &&
               waitingCars[0] == car;
    }

    public void UpdateQueuePositions()
    {
        for (int i = 0; i < waitingCars.Count; i++)
        {
            Transform queuePoint = GetQueuePoint(i);

            waitingCars[i].MoveToQueuePoint(queuePoint);
        }
    }

}