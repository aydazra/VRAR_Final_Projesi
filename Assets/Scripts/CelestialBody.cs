// ==================== CelestialBody.cs ====================
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.XR;

public class CelestialBody : MonoBehaviour
{
    public enum BodyType { Sun, Earth, Moon }

    [Header("Body Information")]
    public BodyType bodyType;
    public string bodyName;
    [TextArea(3, 10)]
    public string description;
    public float rotationSpeed = 10f;

    [Header("Visual Effects")]
    public Light bodyLight; // Güneþ için ýþýk
    public ParticleSystem glowEffect; // Parýldama efekti
    public Material bodyMaterial;

    private bool isSelected = false;

    void Start()
    {
        // Materyal ayarlarý
        SetupMaterial();

        // Iþýk ayarlarý (sadece Güneþ için)
        if (bodyType == BodyType.Sun && bodyLight != null)
        {
            bodyLight.intensity = 2f;
            bodyLight.color = Color.yellow;
            bodyLight.range = 10f;
        }
    }

    void Update()
    {
        // Sürekli hafif dönüþ animasyonu (görsel)
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void SetupMaterial()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && bodyMaterial != null)
        {
            renderer.material = bodyMaterial;

            // Emisyon ayarý
            if (bodyType == BodyType.Sun)
            {
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", Color.yellow * 2f);
            }
        }
    }

    public string GetInfo()
    {
        return $"<b>{bodyName}</b>\n\n{description}";
    }

    void OnMouseDown()
    {
        // Nesneye dokunulduðunda bilgi göster
        FindObjectOfType<UIController>().ShowInfoPanel(GetInfo());
        AudioManager.Instance.PlayInfo(bodyType.ToString().ToLower());
    }
}