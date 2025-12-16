// ==================== QuizManager.cs ====================
// Quiz sistemini yöneten script
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public static QuizManager Instance { get; private set; }

    [Header("Quiz Panel")]
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public TextMeshProUGUI scoreText;
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public Image resultImage;
    public Sprite correctSprite;
    public Sprite wrongSprite;

    [Header("Celebration Effects")]
    public ParticleSystem confettiEffect;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip celebrationSound;

    private CelestialObjectData currentData;
    private int currentQuestionIndex = 0;
    private int correctAnswers = 0;
    private List<QuizQuestion> shuffledQuestions;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        quizPanel.SetActive(false);
        resultPanel.SetActive(false);

        // Option button listeners
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // Closure için
            optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    public void StartQuiz(string objectName)
    {
        currentData = CelestialDataManager.Instance.GetData(objectName);

        if (currentData == null || !currentData.hasQuiz)
            return;

        currentQuestionIndex = 0;
        correctAnswers = 0;

        // Sorularý karýþtýr
        shuffledQuestions = new List<QuizQuestion>(currentData.quizQuestions);
        ShuffleList(shuffledQuestions);

        quizPanel.SetActive(true);
        resultPanel.SetActive(false);

        ShowCurrentQuestion();
    }

    void ShowCurrentQuestion()
    {
        if (currentQuestionIndex >= shuffledQuestions.Count)
        {
            ShowFinalResults();
            return;
        }

        QuizQuestion question = shuffledQuestions[currentQuestionIndex];

        questionText.text = question.question;
        scoreText.text = $"Soru {currentQuestionIndex + 1}/{shuffledQuestions.Count}";

        // Þýklarý göster
        for (int i = 0; i < optionButtons.Length && i < question.options.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.options[i];
            optionButtons[i].interactable = true;

            // Renkleri sýfýrla
            ColorBlock colors = optionButtons[i].colors;
            colors.normalColor = Color.white;
            optionButtons[i].colors = colors;
        }

        // Fazla butonlarý gizle
        for (int i = question.options.Length; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(false);
        }

        resultPanel.SetActive(false);
    }

    void CheckAnswer(int selectedIndex)
    {
        QuizQuestion question = shuffledQuestions[currentQuestionIndex];
        bool isCorrect = selectedIndex == question.correctAnswer;

        // Tüm butonlarý devre dýþý býrak
        foreach (Button btn in optionButtons)
        {
            btn.interactable = false;
        }

        // Seçilen butonu vurgula
        ColorBlock colors = optionButtons[selectedIndex].colors;
        colors.normalColor = isCorrect ? Color.green : Color.red;
        optionButtons[selectedIndex].colors = colors;

        // Doðru cevabý göster (yanlýþsa)
        if (!isCorrect)
        {
            ColorBlock correctColors = optionButtons[question.correctAnswer].colors;
            correctColors.normalColor = Color.green;
            optionButtons[question.correctAnswer].colors = correctColors;
        }

        // Sonuç paneli
        resultPanel.SetActive(true);
        resultText.text = question.explanation;
        resultImage.sprite = isCorrect ? correctSprite : wrongSprite;

        // Ses efekti
        AudioManager.Instance.PlaySFX(isCorrect ? correctSound : wrongSound);

        // Skor
        if (isCorrect)
        {
            correctAnswers++;
            if (confettiEffect != null)
            {
                confettiEffect.Play();
            }
        }

        // Analytics
        AnalyticsManager.Instance.LogEvent("quiz_answer", new Dictionary<string, object>
        {
            { "object", currentData.objectName },
            { "question_index", currentQuestionIndex },
            { "correct", isCorrect }
        });

        // Sonraki soruya geç (2 saniye sonra)
        Invoke("NextQuestion", 2.5f);
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        ShowCurrentQuestion();
    }

    void ShowFinalResults()
    {
        int percentage = (correctAnswers * 100) / shuffledQuestions.Count;
        string message = "";

        if (percentage == 100)
        {
            message = $"?? MÜKEMMEL! ??\n\nTüm sorularý doðru bildin!\n" +
                     $"{correctAnswers}/{shuffledQuestions.Count} doðru!\n\n" +
                     $"Sen gerçek bir uzay kâþifi gibisin! ??";
            AudioManager.Instance.PlaySFX(celebrationSound);
            if (confettiEffect != null) confettiEffect.Play();
        }
        else if (percentage >= 70)
        {
            message = $"?? ÇOK ÝYÝ! ??\n\n{correctAnswers}/{shuffledQuestions.Count} doðru!\n\n" +
                     $"Harika bir performans! Biraz daha çalýþýrsan mükemmel olacaksýn! ??";
        }
        else if (percentage >= 50)
        {
            message = $"?? ÝYÝ! ??\n\n{correctAnswers}/{shuffledQuestions.Count} doðru!\n\n" +
                     $"Fena deðil! Biraz daha pratik yapmalýsýn. ??";
        }
        else
        {
            message = $"?? Daha Çok Çalýþmalýsýn!\n\n{correctAnswers}/{shuffledQuestions.Count} doðru\n\n" +
                     $"Üzülme! Hikayeleri tekrar oku ve tekrar dene! ??";
        }

        questionText.text = message;
        scoreText.text = $"Baþarý: %{percentage}";

        // Butonlarý gizle
        foreach (Button btn in optionButtons)
        {
            btn.gameObject.SetActive(false);
        }

        resultPanel.SetActive(false);

        // Achievement kaydet
        AchievementManager.Instance.UnlockAchievement($"{currentData.objectName}_quiz_complete");

        if (percentage == 100)
        {
            AchievementManager.Instance.UnlockAchievement($"{currentData.objectName}_quiz_perfect");
        }

        // Invoke close
        Invoke("CloseQuiz", 5f);
    }

    void CloseQuiz()
    {
        quizPanel.SetActive(false);
    }
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            T temp = list[rnd];
            list[rnd] = list[i];
            list[i] = temp;
        }
    }

}
