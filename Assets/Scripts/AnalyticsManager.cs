// ==================== AnalyticsManager.cs ====================
// Kullanýcý davranýþlarýný izleyen ve raporlayan sistem
using UnityEngine;
using System.Collections.Generic;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }

    private Dictionary<string, int> eventCounts = new Dictionary<string, int>();
    private float sessionStartTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            sessionStartTime = Time.time;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LogEvent(string eventName, Dictionary<string, object> parameters = null)
    {
        // Event sayýsýný artýr
        if (!eventCounts.ContainsKey(eventName))
        {
            eventCounts[eventName] = 0;
        }
        eventCounts[eventName]++;

        // Unity Analytics'e gönder (Production'da aktif olacak)
#if UNITY_ANALYTICS
        UnityEngine.Analytics.Analytics.CustomEvent(eventName, parameters);
#endif

        // Debug log
        Debug.Log($"Analytics Event: {eventName}");
        if (parameters != null)
        {
            foreach (var kvp in parameters)
            {
                Debug.Log($"  {kvp.Key}: {kvp.Value}");
            }
        }
    }

    public void LogScreenView(string screenName)
    {
        LogEvent("screen_view", new Dictionary<string, object>
        {
            { "screen_name", screenName },
            { "session_time", Time.time - sessionStartTime }
        });
    }

    public Dictionary<string, int> GetEventCounts()
    {
        return new Dictionary<string, int>(eventCounts);
    }
}