using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private TMP_InputField minSpawnInput;
    [SerializeField] private TMP_InputField maxSpawnInput;

    [Header("Prices")]
    [SerializeField] private TMP_InputField lowPriceInput;
    [SerializeField] private TMP_InputField mediumPriceInput;
    [SerializeField] private TMP_InputField highPriceInput;

    [Header("Thresholds")]
    [SerializeField] private TMP_InputField mediumThresholdInput;
    [SerializeField] private TMP_InputField highThresholdInput;

    [Header("Parking Time")]
    [SerializeField] private TMP_InputField minParkingTimeInput;
    [SerializeField] private TMP_InputField maxParkingTimeInput;

    [Header("Objects")]
    [SerializeField] private GameObject mainMenuCanvas;

    [SerializeField] private GameObject simulationCanvas;

    [SerializeField] private MonoBehaviour carSpawner;

    public void StartSimulation()
    {
        SimulationSettings.minSpawnTime =
            float.Parse(minSpawnInput.text);

        SimulationSettings.maxSpawnTime =
            float.Parse(maxSpawnInput.text);

        SimulationSettings.lowPrice =
            float.Parse(lowPriceInput.text);

        SimulationSettings.mediumPrice =
            float.Parse(mediumPriceInput.text);

        SimulationSettings.highPrice =
            float.Parse(highPriceInput.text);

        SimulationSettings.mediumThreshold =
            float.Parse(mediumThresholdInput.text);

        SimulationSettings.highThreshold =
            float.Parse(highThresholdInput.text);

        SimulationSettings.minParkingTime =
            float.Parse(minParkingTimeInput.text);

        SimulationSettings.maxParkingTime =
            float.Parse(maxParkingTimeInput.text);

        mainMenuCanvas.SetActive(false);

        simulationCanvas.SetActive(true);

        carSpawner.enabled = true;
    }

    private void Start()
    {
        minSpawnInput.text =
            SimulationSettings.minSpawnTime.ToString();

        maxSpawnInput.text =
            SimulationSettings.maxSpawnTime.ToString();

        lowPriceInput.text =
            SimulationSettings.lowPrice.ToString();

        mediumPriceInput.text =
            SimulationSettings.mediumPrice.ToString();

        highPriceInput.text =
            SimulationSettings.highPrice.ToString();

        mediumThresholdInput.text =
            SimulationSettings.mediumThreshold.ToString();

        highThresholdInput.text =
            SimulationSettings.highThreshold.ToString();

        minParkingTimeInput.text =
            SimulationSettings.minParkingTime.ToString();

        maxParkingTimeInput.text =
            SimulationSettings.maxParkingTime.ToString();

    }

}