// ==================== UIController.cs ====================
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject objectSelectionPanel;
    public GameObject infoPanel;
    public GameObject simulationControlPanel;
    public GameObject placementIndicator;

    [Header("Info Panel Components")]
    public TextMeshProUGUI infoText;
    public Button closeInfoButton;

    [Header("Simulation Controls")]
    public Button playPauseButton;
    public Slider speedSlider;
    public TextMeshProUGUI speedText;
    public Button resetButton;

    [Header("References")]
    public ARObjectManager arManager;

    [Header("Icons")]
    public Sprite playIcon;
    public Sprite pauseIcon;

    void Start()
    {
        // Button listener'lar
        closeInfoButton.onClick.AddListener(HideInfoPanel);
        playPauseButton.onClick.AddListener(OnPlayPauseClicked);
        resetButton.onClick.AddListener(OnResetClicked);
        speedSlider.onValueChanged.AddListener(OnSpeedChanged);

        // Baþlangýç durumu
        infoPanel.SetActive(false);
        simulationControlPanel.SetActive(false);
        placementIndicator.SetActive(false);
    }

    public void ShowInfoPanel(string info)
    {
        infoText.text = info;
        infoPanel.SetActive(true);

        // 5 saniye sonra otomatik kapat
        Invoke("HideInfoPanel", 5f);
    }

    public void HideInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    public void ShowSimulationControls(bool show)
    {
        simulationControlPanel.SetActive(show);
    }

    public void ShowPlacementIndicator(bool show)
    {
        placementIndicator.SetActive(show);
    }

    public void UpdatePlayButton(bool isPlaying)
    {
        Image buttonImage = playPauseButton.GetComponent<Image>();
        buttonImage.sprite = isPlaying ? pauseIcon : playIcon;
    }

    void OnPlayPauseClicked()
    {
        arManager.ToggleSimulation();
    }

    void OnSpeedChanged(float value)
    {
        arManager.SetSimulationSpeed(value);
        speedText.text = $"Hýz: x{value:F1}";
    }

    void OnResetClicked()
    {
        arManager.ResetScene();
    }
}
