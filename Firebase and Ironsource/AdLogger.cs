using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdLogger : MonoBehaviour
{
    /// <summary>
    /// The amount of ads shown successfully this session
    /// </summary>
    [SerializeField] private int successfulAdsShown = 0;

    /// <summary>
    /// The text to display the amount of ads shown this session
    /// </summary>
    [SerializeField] private TextMeshProUGUI adCounterText = null;

    /// <summary>
    /// Register for Ad Shown Events
    /// </summary>
    private void OnEnable()
    {
        // Register for Ad Events
        IronSourceEvents.onInterstitialAdShowSucceededEvent += AdShownSuccess;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += AdShownSuccess;

        // Update when enabled to get the new value of successfulAdsShown
        UpdateAdCounter();
    }

    /// <summary>
    /// Event De-Registration
    /// </summary>
    private void OnDisable()
    {
        // De-Register for Ad Events
        IronSourceEvents.onInterstitialAdShowSucceededEvent -= AdShownSuccess;
        IronSourceEvents.onRewardedVideoAdRewardedEvent -= AdShownSuccess;
    }

    /// <summary>
    /// Overloard for AdShownSuccess - Updates the
    /// Ads shown counter and updates the same text
    /// </summary>
    /// <param name="obj"></param>
    private void AdShownSuccess(IronSourcePlacement obj)
    {
        successfulAdsShown++;
        FireBaseManager.Instance.LogFireBaseEvent(FireBaseManager.FireBaseKey.AdSuccess, 1);
        UpdateAdCounter();
    }

    /// <summary>
    /// Method for handling when a new ad was succesfully
    /// watched or rewarded. 
    /// Used for debugging the amount of ads watched
    /// </summary>
    private void AdShownSuccess()
    {
        successfulAdsShown++;
        FireBaseManager.Instance.LogFireBaseEvent(FireBaseManager.FireBaseKey.AdSuccess, 1);
        UpdateAdCounter();
    }

    /// <summary>
    /// Updates the text for the amount of ads watched
    /// </summary>
    private void UpdateAdCounter()
    {
        if(adCounterText.enabled == true)
            adCounterText.text = $"Ads Viewed: {successfulAdsShown}";
    }
}
