// ==================== ProgressManager.cs ====================
// Kullanýcýnýn ilerleme durumunu kaydeden sistem
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class UserProgress
{
    public Dictionary<string, bool> storiesCompleted = new Dictionary<string, bool>();
    public Dictionary<string, int> quizScores = new Dictionary<string, int>();
    public int totalPlayTime; // Saniye cinsinden
    public int objectsPlaced;
    public int screenshotsTaken;
    public System.DateTime lastPlayDate;
}

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    private UserProgress progress;
    private const string PROGRESS_KEY = "UserProgress";
    private float sessionStartTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
            sessionStartTime = Time.time;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveProgress();
        }
    }

    void OnApplicationQuit()
    {
        SaveProgress();
    }

    public void MarkStoryComplete(string objectName)
    {
        progress.storiesCompleted[objectName] = true;
        SaveProgress();

        // Achievement kontrol
        CheckAllStoriesComplete();
    }

    public void SaveQuizScore(string objectName, int score)
    {
        if (!progress.quizScores.ContainsKey(objectName) ||
            progress.quizScores[objectName] < score)
        {
            progress.quizScores[objectName] = score;
            SaveProgress();
        }

        // Achievement kontrol
        CheckAllQuizzesPerfect();
    }

    public void IncrementObjectsPlaced()
    {
        progress.objectsPlaced++;

        if (progress.objectsPlaced == 1)
        {
            AchievementManager.Instance.UnlockAchievement("first_object_placed");
        }

        SaveProgress();
    }

    public void IncrementScreenshots()
    {
        progress.screenshotsTaken++;

        if (progress.screenshotsTaken == 1)
        {
            AchievementManager.Instance.UnlockAchievement("screenshot_taken");
        }

        SaveProgress();
    }

    void CheckAllStoriesComplete()
    {
        bool allComplete = true;
        string[] objects = { "Sun", "Moon", "Earth" };

        foreach (string obj in objects)
        {
            if (!progress.storiesCompleted.ContainsKey(obj) ||
                !progress.storiesCompleted[obj])
            {
                allComplete = false;
                break;
            }
        }

        if (allComplete)
        {
            AchievementManager.Instance.UnlockAchievement("all_stories_complete");
        }
    }

    void CheckAllQuizzesPerfect()
    {
        bool allPerfect = true;
        string[] objects = { "Sun", "Moon", "Earth" };

        foreach (string obj in objects)
        {
            if (!progress.quizScores.ContainsKey(obj) ||
                progress.quizScores[obj] < 100)
            {
                allPerfect = false;
                break;
            }
        }

        if (allPerfect)
        {
            AchievementManager.Instance.UnlockAchievement("all_quizzes_perfect");
        }
    }

    void SaveProgress()
    {
        // Oyun süresini güncelle
        progress.totalPlayTime += (int)(Time.time - sessionStartTime);
        progress.lastPlayDate = System.DateTime.Now;

        // 10 dakika achievement
        if (progress.totalPlayTime >= 600 && progress.totalPlayTime < 620)
        {
            AchievementManager.Instance.UnlockAchievement("10_minutes_playtime");
        }

        string json = JsonUtility.ToJson(progress);
        PlayerPrefs.SetString(PROGRESS_KEY, json);
        PlayerPrefs.Save();

        sessionStartTime = Time.time; // Reset
    }

    void LoadProgress()
    {
        if (PlayerPrefs.HasKey(PROGRESS_KEY))
        {
            string json = PlayerPrefs.GetString(PROGRESS_KEY);
            progress = JsonUtility.FromJson<UserProgress>(json);
        }
        else
        {
            progress = new UserProgress();
        }
    }

    public UserProgress GetProgress()
    {
        return progress;
    }
}
