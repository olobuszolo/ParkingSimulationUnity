using UnityEngine;

public class ParkingLot : MonoBehaviour
{
    public ParkingSpot[] parkingSpots;

    private void Start()
    {
        parkingSpots = FindObjectsByType<ParkingSpot>(FindObjectsSortMode.None);

        Debug.Log("Parking system initialized.");
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

    public int GetCurrentPrice()
    {
        float occupancy = GetOccupancyPercentage();

        if (occupancy < 30f)
        {
            return 5;
        }

        if (occupancy < 70f)
        {
            return 10;
        }

        return 20;
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
}