using UnityEngine;

public class ParkingSpot : MonoBehaviour
{
    public bool isOccupied = false;

    private bool isReserved = false;

    public GameObject occupyingCar = null;

    private Renderer spotRenderer;

    [SerializeField] private Transform parkingPoint;

    [SerializeField] private Transform entryWaypoint;

    private void Start()
    {
        spotRenderer = GetComponent<Renderer>();

        UpdateColor();
    }

    public void OccupySpot(GameObject car)
    {
        isOccupied = true;
        isReserved = false;
        occupyingCar = car;

        UpdateColor();
    }

    public void FreeSpot()
    {
        isOccupied = false;
    
        isReserved = false;

        occupyingCar = null;

        UpdateColor();
    }

    public bool IsFree()
    {
        return !isOccupied && !isReserved;
    }

    private void UpdateColor()
    {
        if (spotRenderer == null)
            return;

        if (isOccupied)
        {
            spotRenderer.material.color = Color.red;
        }
        else
        {
            spotRenderer.material.color = Color.green;
        }
    }
    public Transform GetParkingPoint()
    {
        return parkingPoint;
    }

    public bool IsReserved()
    {
        return isReserved;
    }

    public void ReserveSpot()
    {
        isReserved = true;
    }

    public void ClearReservation()
    {
        isReserved = false;
    }

    public Transform GetEntryWaypoint()
    {
        return entryWaypoint;
    }

}