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
    [SerializeField] private TMP_InputField lowThresholdInput;
    [SerializeField] private TMP_InputField mediumThresholdInput;
    [SerializeField] private TMP_InputField highThresholdInput;

    [Header("Objects")]
    [SerializeField] private GameObject mainMenuCanvas;

    [SerializeField] private GameObject simulationCanvas;

    [SerializeField] private MonoBehaviour carSpawner;

    public void StartSimulation()
    {
        Debug.Log("Simulation Started");

        Debug.Log("Min Spawn: " + minSpawnInput.text);

        Debug.Log("Max Spawn: " + maxSpawnInput.text);

        mainMenuCanvas.SetActive(false);

        simulationCanvas.SetActive(true);

        carSpawner.enabled = true;
    }
}