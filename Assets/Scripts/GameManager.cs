using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int parkedCarsCount = 0;
    private int circlingCarsCount = 0;
    private bool entryOccupied = false;
    private float totalRevenue = 0f;

    public void AddRevenue(float amount)
    {
        totalRevenue += amount;
    }

    public float GetTotalRevenue()
    {
        return totalRevenue;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void AddParkedCar()
    {
        parkedCarsCount++;
    }

    public void RemoveParkedCar()
    {
        if (parkedCarsCount > 0)
        {
            parkedCarsCount--;
        }
    }

    public void AddCirclingCar()
    {
        circlingCarsCount++;
    }

    public void RemoveCirclingCar()
    {
        if (circlingCarsCount > 0)
        {
            circlingCarsCount--;
        }
    }

    public int GetParkedCarsCount()
    {
        return parkedCarsCount;
    }

    public int GetCirclingCarsCount()
    {
        return circlingCarsCount;
    }

    public bool CanEnterParking()
    {
        return !entryOccupied && circlingCarsCount < 15;
    }

    public void OccupyEntry()
    {
        entryOccupied = true;
    }

    public void FreeEntry()
    {
        entryOccupied = false;
    }

}