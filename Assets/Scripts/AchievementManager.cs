// ==================== AchievementManager.cs ====================
// Başarım sistemini yöneten script
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class Achievement
{
    public string id;
    public string title;
    public string description;
    public Sprite icon;
    public bool isUnlocked;
    public System.DateTime unlockedDate;
}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [Header("Achievement Definitions")]
    public List<Achievement> achievements = new List<Achievement>();

    [Header("Notification UI")]
    public GameObject achievementNotificationPrefab;
    public Transform notificationParent;
    public AudioClip achievementSound;

    private Dictionary<string, Achievement> achievementDict;
    private const string SAVE_KEY = "Achievements";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAchievements();
            LoadAchievements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeAchievements()
    {
        achievementDict = new Dictionary<string, Achievement>();

        // Başarımları tanımla
        achievements = new List<Achievement>
        {
            new Achievement
            {
                id = "first_object_placed",
                title = "İlk Adım",
                description = "İlk gök cismini AR'a yerleştirdin!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "Sun_story_complete",
                title = "Güneş Uzmanı",
                description = "Güneş'in hikayesini tamamladın!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "Moon_story_complete",
                title = "Ay Kâşifi",
                description = "Ay'ın hikayesini tamamladın!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "Earth_story_complete",
                title = "Dünya Kahramanı",
                description = "Dünya'nın hikayesini tamamladın!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "all_stories_complete",
                title = "Uzay Profesörü",
                description = "Tüm hikayeleri tamamladın!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "Sun_quiz_complete",
                title = "Güneş Bilgini",
                description = "Güneş quizini tamamladın!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "Sun_quiz_perfect",
                title = "Güneş Ustası",
                description = "Güneş quizinde tam puan aldın!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "all_quizzes_perfect",
                title = "Mükemmeliyetçi",
                description = "Tüm quizlerde tam puan aldın!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "eclipse_created",
                title = "Tutulma Yaratıcısı",
                description = "Güneş tutulması oluşturdun!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "all_objects_placed",
                title = "Koleksiyoner",
                description = "Tüm gök cisimlerini yerleştirdin!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "10_minutes_playtime",
                title = "Meraklı Öğrenci",
                description = "10 dakika boyunca keşfettin!",
                isUnlocked = false
            },
            new Achievement
            {
                id = "screenshot_taken",
                title = "Fotoğrafçı",
                description = "İlk uzay fotoğrafını çektin!",
                isUnlocked = false
            }
        };

        foreach (var achievement in achievements)
        {
            achievementDict[achievement.id] = achievement;
        }
    }

    public void UnlockAchievement(string achievementId)
    {
        if (!achievementDict.ContainsKey(achievementId))
        {
            Debug.LogWarning($"Achievement not found: {achievementId}");
            return;
        }

        Achievement achievement = achievementDict[achievementId];

        if (achievement.isUnlocked)
        {
            return; // Zaten açık
        }

        achievement.isUnlocked = true;
        achievement.unlockedDate = System.DateTime.Now;

        // Bildirim göster
        ShowAchievementNotification(achievement);

        // Ses çal
        if (achievementSound != null)
        {
            AudioManager.Instance.PlaySFX(achievementSound);
        }

        // Analytics
        AnalyticsManager.Instance.LogEvent("achievement_unlocked", new Dictionary<string, object>
        {
            { "achievement_id", achievementId },
            { "title", achievement.title }
        });

        // Kaydet
        SaveAchievements();

        Debug.Log($"🏆 Achievement Unlocked: {achievement.title}");
    }

    void ShowAchievementNotification(Achievement achievement)
    {
        if (achievementNotificationPrefab != null && notificationParent != null)
        {
            GameObject notification = Instantiate(achievementNotificationPrefab, notificationParent);

            // Notification'daki text ve image'ları doldur
            TextMeshProUGUI titleText = notification.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descText = notification.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            Image iconImage = notification.transform.Find("Icon").GetComponent<Image>();

            if (titleText) titleText.text = achievement.title;
            if (descText) descText.text = achievement.description;
            if (iconImage && achievement.icon) iconImage.sprite = achievement.icon;

            // 3 saniye sonra yok et
            Destroy(notification, 3f);
        }
    }

    void SaveAchievements()
    {
        AchievementSaveData saveData = new AchievementSaveData();
        saveData.unlockedAchievements = new List<string>();

        foreach (var achievement in achievements)
        {
            if (achievement.isUnlocked)
            {
                saveData.unlockedAchievements.Add(achievement.id);
            }
        }

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    void LoadAchievements()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            AchievementSaveData saveData = JsonUtility.FromJson<AchievementSaveData>(json);

            foreach (string id in saveData.unlockedAchievements)
            {
                if (achievementDict.ContainsKey(id))
                {
                    achievementDict[id].isUnlocked = true;
                }
            }
        }
    }

    public List<Achievement> GetAllAchievements()
    {
        return achievements;
    }

    public float GetCompletionPercentage()
    {
        int unlocked = 0;
        foreach (var achievement in achievements)
        {
            if (achievement.isUnlocked) unlocked++;
        }
        return (float)unlocked / achievements.Count * 100f;
    }
}

[System.Serializable]
public class AchievementSaveData
{
    public List<string> unlockedAchievements;
}