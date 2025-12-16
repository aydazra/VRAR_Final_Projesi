// ==================== ObjectPanel.cs ====================
using UnityEngine;
using UnityEngine.UI;
using System;

public class ObjectPanel : MonoBehaviour
{
    [Header("Buttons")]
    public Button sunButton;
    public Button earthButton;
    public Button moonButton;

    // Events
    public Action onSunButtonClicked;
    public Action onEarthButtonClicked;
    public Action onMoonButtonClicked;

    void Start()
    {
        sunButton.onClick.AddListener(() => onSunButtonClicked?.Invoke());
        earthButton.onClick.AddListener(() => onEarthButtonClicked?.Invoke());
        moonButton.onClick.AddListener(() => onMoonButtonClicked?.Invoke());
    }
}
