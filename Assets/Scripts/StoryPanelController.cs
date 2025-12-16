// ==================== StoryPanelController.cs ====================
// Hikaye panelini kontrol eden script
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StoryPanelController : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject storyPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI pageNumberText;
    public Image characterAvatar;
    public Image illustrationImage;
    public Slider progressSlider;

    [Header("Navigation Buttons")]
    public Button previousButton;
    public Button nextButton;
    public Button closeButton;
    public Button audioToggleButton;
    public Image audioIcon;
    public Sprite audioOnSprite;
    public Sprite audioOffSprite;

    [Header("Animation")]
    public Animator panelAnimator;
    public float typingSpeed = 0.03f; // Yazý hýzý

    private CelestialObjectData currentData;
    private int currentSectionIndex = 0;
    private bool isTyping = false;
    private bool audioEnabled = true;
    private Coroutine typingCoroutine;

    void Start()
    {
        // Button listeners
        previousButton.onClick.AddListener(ShowPreviousSection);
        nextButton.onClick.AddListener(ShowNextSection);
        closeButton.onClick.AddListener(ClosePanel);
        audioToggleButton.onClick.AddListener(ToggleAudio);

        storyPanel.SetActive(false);
    }

    public void OpenStory(string objectName)
    {
        currentData = CelestialDataManager.Instance.GetData(objectName);

        if (currentData == null)
        {
            Debug.LogError("Celestial data not found: " + objectName);
            return;
        }

        currentSectionIndex = 0;
        storyPanel.SetActive(true);
        panelAnimator.SetTrigger("FadeIn");

        ShowCurrentSection();
    }

    void ShowCurrentSection()
    {
        if (currentData == null || currentSectionIndex < 0 ||
            currentSectionIndex >= currentData.storySections.Count)
            return;

        StorySection section = currentData.storySections[currentSectionIndex];

        // Baþlýk
        titleText.text = section.title;
        titleText.color = currentData.themeColor;

        // Sayfa numarasý
        pageNumberText.text = $"Sayfa {section.pageNumber}";

        // Karakter avatarý
        if (currentData.characterIcon != null)
        {
            characterAvatar.sprite = currentData.characterIcon;
            characterAvatar.color = currentData.themeColor;
        }

        // Ýllüstrasyon (varsa)
        if (section.illustrationImage != null)
        {
            illustrationImage.gameObject.SetActive(true);
            illustrationImage.sprite = section.illustrationImage;
        }
        else
        {
            illustrationImage.gameObject.SetActive(false);
        }

        // Ýçerik - Typing animasyonu
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(section.content));

        // Progress slider
        progressSlider.maxValue = currentData.storySections.Count - 1;
        progressSlider.value = currentSectionIndex;

        // Buton durumlarý
        previousButton.interactable = currentSectionIndex > 0;
        nextButton.interactable = currentSectionIndex < currentData.storySections.Count - 1;

        // Sesli anlatým
        if (audioEnabled && section.audioClip != null)
        {
            AudioManager.Instance.PlayClip(section.audioClip);
        }

        // Analytics
        AnalyticsManager.Instance.LogEvent("story_section_viewed", new Dictionary<string, object>
        {
            { "object", currentData.objectName },
            { "section", currentSectionIndex },
            { "title", section.title }
        });
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        contentText.text = "";

        foreach (char c in fullText)
        {
            contentText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void ShowPreviousSection()
    {
        if (currentSectionIndex > 0)
        {
            currentSectionIndex--;
            ShowCurrentSection();
        }
    }

    void ShowNextSection()
    {
        if (currentSectionIndex < currentData.storySections.Count - 1)
        {
            currentSectionIndex++;
            ShowCurrentSection();
        }
        else
        {
            // Son bölüm - Quiz aç
            ShowQuiz();
        }
    }

    void ShowQuiz()
    {
        QuizManager.Instance.StartQuiz(currentData.objectName);
        ClosePanel();
    }

    void ToggleAudio()
    {
        audioEnabled = !audioEnabled;
        audioIcon.sprite = audioEnabled ? audioOnSprite : audioOffSprite;

        if (!audioEnabled)
        {
            AudioManager.Instance.StopAll();
        }
    }

    void ClosePanel()
    {
        panelAnimator.SetTrigger("FadeOut");
        StartCoroutine(DelayedClose());
    }

    IEnumerator DelayedClose()
    {
        yield return new WaitForSeconds(0.3f);
        storyPanel.SetActive(false);
        AudioManager.Instance.StopAll();

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
    }
}