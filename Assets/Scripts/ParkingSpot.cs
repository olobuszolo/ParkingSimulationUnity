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

        Material mat = spotRenderer.material;

        if (isOccupied)
        {
            // W³¹czenie transparentnoœci dla URP Lit
            mat.SetFloat("_Surface", 1); // 0=Opaque, 1=Transparent
            mat.SetOverrideTag("RenderType", "Transparent");
            mat.renderQueue = 3000;

            Color transparentRed = new Color(1f, 0f, 0f, 0.35f);

            mat.color = transparentRed;
        }
        else
        {
            // Brak zmiany koloru gdy wolne
            Color current = mat.color;

            current.a = 0f;

            mat.color = current;
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