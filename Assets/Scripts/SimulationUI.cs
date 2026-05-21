using TMPro;
using UnityEngine;

public class SimulationUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParkingLot parkingLot;
    [SerializeField] private GameManager gameManager;

    [Header("UI Texts")]
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI freeSpotsText;
    [SerializeField] private TextMeshProUGUI parkedCarsText;
    [SerializeField] private TextMeshProUGUI circlingCarsText;
    [SerializeField] private TextMeshProUGUI totalRevenueText;

    private void Update()
    {
        if (parkingLot != null)
        {
            priceText.text = "Price: " + parkingLot.GetCurrentPrice();
            freeSpotsText.text = "Free spots: " + parkingLot.GetFreeSpotsCount();
        }

        if (gameManager != null)
        {
            parkedCarsText.text = "Parked cars: " + gameManager.GetParkedCarsCount();
            circlingCarsText.text = "Circling cars: " + gameManager.GetCirclingCarsCount();
            totalRevenueText.text = "Total Revenue: " + gameManager.GetTotalRevenue();
        }
    }
}