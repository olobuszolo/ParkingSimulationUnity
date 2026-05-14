using UnityEngine;

public class ParkingSpot : MonoBehaviour
{
    public bool isOccupied = false;

    private bool isReserved = false;

    public GameObject occupyingCar = null;

    private Renderer spotRenderer;

    [SerializeField] private Transform parkingPoint;

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

        Debug.Log(gameObject.name + " is occupied.");
    }

    public void FreeSpot()
    {
        isOccupied = false;
        occupyingCar = null;

        UpdateColor();

        Debug.Log(gameObject.name + " is now free.");
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

}