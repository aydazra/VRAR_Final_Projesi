// ==================== ARObjectManager.cs ====================
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.InputSystem.XR;

public class ARObjectManager : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public GameObject arCamera;

    [Header("Celestial Bodies Prefabs")]
    public GameObject sunPrefab;
    public GameObject earthPrefab;
    public GameObject moonPrefab;

    [Header("UI References")]
    public UIController uiController;
    public ObjectPanel objectPanel;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject selectedPrefab;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private GameObject earth;
    private GameObject moon;
    private bool isSimulationRunning = false;
    private float simulationSpeed = 1f;

    void Start()
    {
        // Panel butonlarýna listener ekle
        objectPanel.onSunButtonClicked += () => SelectObject("Sun");
        objectPanel.onEarthButtonClicked += () => SelectObject("Earth");
        objectPanel.onMoonButtonClicked += () => SelectObject("Moon");
    }

    void Update()
    {
        // Dokunma kontrolü
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && selectedPrefab != null)
            {
                PlaceObject(touch.position);
            }
        }

        // Simülasyon güncellemesi
        if (isSimulationRunning)
        {
            UpdateSimulation();
        }
    }

    void SelectObject(string objectType)
    {
        switch (objectType)
        {
            case "Sun":
                selectedPrefab = sunPrefab;
                AudioManager.Instance.PlayInfo("sun");
                break;
            case "Earth":
                selectedPrefab = earthPrefab;
                AudioManager.Instance.PlayInfo("earth");
                break;
            case "Moon":
                selectedPrefab = moonPrefab;
                AudioManager.Instance.PlayInfo("moon");
                break;
        }

        uiController.ShowPlacementIndicator(true);
    }

    void PlaceObject(Vector2 touchPosition)
    {
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // Nesneyi yerleþtir
            GameObject spawnedObject = Instantiate(selectedPrefab, hitPose.position, hitPose.rotation);
            spawnedObjects.Add(spawnedObject);

            // Özel nesne kontrolü
            CelestialBody celestialBody = spawnedObject.GetComponent<CelestialBody>();

            if (celestialBody.bodyType == CelestialBody.BodyType.Earth)
            {
                earth = spawnedObject;
            }
            else if (celestialBody.bodyType == CelestialBody.BodyType.Moon && earth != null)
            {
                moon = spawnedObject;
                SetupMoonOrbit();
            }

            // Bilgi panelini göster
            uiController.ShowInfoPanel(celestialBody.GetInfo());

            selectedPrefab = null;
            uiController.ShowPlacementIndicator(false);
        }
    }

    void SetupMoonOrbit()
    {
        if (moon != null && earth != null)
        {
            // Ay'ý Dünya'nýn etrafýna yerleþtir
            moon.transform.parent = earth.transform;
            moon.transform.localPosition = new Vector3(0.5f, 0, 0); // Dünya'dan 0.5 birim uzakta

            uiController.ShowSimulationControls(true);
        }
    }

    void UpdateSimulation()
    {
        if (earth != null)
        {
            // Dünya kendi etrafýnda dönsün (1 saniyede 1 derece = gerçekte 24 saat)
            earth.transform.Rotate(Vector3.up, 5f * simulationSpeed * Time.deltaTime);
        }

        if (moon != null && earth != null)
        {
            // Ay Dünya etrafýnda dönsün (1 saniyede yaklaþýk 0.5 derece = gerçekte 29 gün)
            earth.transform.Rotate(Vector3.up, 2f * simulationSpeed * Time.deltaTime);
        }
    }

    public void ToggleSimulation()
    {
        isSimulationRunning = !isSimulationRunning;
        uiController.UpdatePlayButton(isSimulationRunning);
    }

    public void SetSimulationSpeed(float speed)
    {
        simulationSpeed = speed;
    }

    public void ResetScene()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear();
        earth = null;
        moon = null;
        isSimulationRunning = false;
        uiController.ShowSimulationControls(false);
    }
}