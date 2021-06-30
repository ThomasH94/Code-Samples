using Firebase;
using Firebase.Analytics;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script initializes Firebase and is recommended 
/// from FireBase to get the app talking to 
/// the Firebase network to start tracking Firebase analytics
/// </summary>
public class FireBaseManager : BaseSingleton<FireBaseManager>
{
    #region FireBase Events Keys
    public enum FireBaseKey
    {
        Gold = 0,
        Ad,
        Enemy,
        PlayerDeaths,
        AdSuccess
    }

    private Dictionary<FireBaseKey, string> FireBaseEvents = new Dictionary<FireBaseKey, string>
    {
        { FireBaseKey.Gold, "gold_collected" },
        { FireBaseKey.Ad, "ads_watched_successfully" },
        { FireBaseKey.Enemy, "enemies_destroyed" },
        { FireBaseKey.PlayerDeaths, "player_deaths" },
        { FireBaseKey.AdSuccess, "ad_success" }
    };
    #endregion

    /// <summary>
    /// Debug tool for resetting if necessary
    /// NOTE: If you reset analytics, you will receive a new device
    /// in the Firebase Consoles DebugView which you will need to 
    /// switch to if you want to continue getting anayltics
    /// </summary>
    [SerializeField] private bool shouldReset = false;

    protected override void Awake()
    {
        base.Awake();
        InitializeFireBase();
    }

    /// <summary>
    /// Initialize FireBase Analytics and Event Tracking
    /// </summary>
    private void InitializeFireBase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            if (shouldReset)
                ResetAnalytics();

            // This would be logged when we load levels, but we'll load it 
            // here manually for now
            LogFireBaseEvent(FirebaseAnalytics.EventLevelStart);
        });
    }

    /// <summary>
    /// Checks if the given key is in the dictionary, and logs
    /// an event based on the key
    /// </summary>
    /// <param name="fireBaseEvent"></param>
    /// <param name="amount"></param>
    public void LogFireBaseEvent(FireBaseKey key)
    {
        if(FireBaseEvents.ContainsKey(key))
        {
            FirebaseAnalytics.LogEvent(FireBaseEvents[key]);
        }
    }

    /// <summary>
    /// Method overload for sending an amount
    /// </summary>
    /// <param name="key"></param>
    /// <param name="amount"></param>
    public void LogFireBaseEvent(FireBaseKey key, double amount)
    {
        if (FireBaseEvents.ContainsKey(key))
        {
            FirebaseAnalytics.LogEvent(FireBaseEvents[key], new Parameter(key.ToString(), amount));

            Debug.Log($"{name} has logged an event successfully: {FireBaseEvents[key]} with the amount: {amount}");
        }
    }

    /// <summary>
    /// Method overload for string events
    /// </summary>
    /// <param name="eventName"></param>
    public void LogFireBaseEvent(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName);
    }

    /// <summary>
    /// WARNING: This method will Reset ALL Analytics for this device
    /// Use sparingly 
    /// </summary>
    public void ResetAnalytics()
    {
        FirebaseAnalytics.ResetAnalyticsData();
    }

    private void OnDestroy()
    {
        LogFireBaseEvent(FirebaseAnalytics.EventLevelEnd);
    }
}
