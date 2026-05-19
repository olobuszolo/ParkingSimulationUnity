using UnityEngine;

public class ParkingSpot : MonoBehaviour
{
    public bool isOccupied = false;

    private bool isReserved = false;

    public GameObject occupyingCar = null;

    private Renderer spotRenderer;

    [SerializeField] private Transform parkingPoint;

    [SerializeField] private Transform entryWaypoint;

    [SerializeField] private Transform exitWaypoint;

    private Color originalColor;

    private void Start()
    {
        spotRenderer = GetComponent<Renderer>();

        // tworzy kopiê materia³u dla tego miejsca
        spotRenderer.material = new Material(spotRenderer.material);

        originalColor = spotRenderer.material.color;

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

        Material mat = spotRenderer.material;

        if (isOccupied)
        {
            Color transparentRed = new Color(1f, 0f, 0f, 0.35f);
            mat.color = transparentRed;
        }
        else
        {
            mat.color = originalColor;
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

    public Transform GetExitWaypoint()
    {
        return exitWaypoint;
    }

}