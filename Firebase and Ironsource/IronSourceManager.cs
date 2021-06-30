using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will be responsibl for intializing IronSource, 
/// keeping track of IronSource Callbacks, and logging Ad Events
/// </summary>
public class IronSourceManager : BaseSingleton<IronSourceManager>
{
    /// <summary>
    /// The app key as shown in the IronSource project
    /// </summary>
    private const string APP_KEY = "db326e41";

    /// <summary>
    /// Flag to see if banners should be shown 
    /// Currently setup with Amazon Banners
    /// </summary>
    [SerializeField] private bool showBanner = false;

    void Start()
    {
        InitIronSource();

        if(showBanner)
            DisplayBanner();
    }

    private void OnEnable()
    {
        // Register for Interstital Ad Events
        IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

        // Register for Rewarded Ads
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
    }

    private void OnDisable()
    {
        // De-Register for Interstital Ad Events
        IronSourceEvents.onInterstitialAdReadyEvent -= InterstitialAdReadyEvent;
        IronSourceEvents.onInterstitialAdLoadFailedEvent -= InterstitialAdLoadFailedEvent;
        IronSourceEvents.onInterstitialAdShowSucceededEvent -= InterstitialAdShowSucceededEvent;
        IronSourceEvents.onInterstitialAdShowFailedEvent -= InterstitialAdShowFailedEvent;
        IronSourceEvents.onInterstitialAdClickedEvent -= InterstitialAdClickedEvent;
        IronSourceEvents.onInterstitialAdOpenedEvent -= InterstitialAdOpenedEvent;
        IronSourceEvents.onInterstitialAdClosedEvent -= InterstitialAdClosedEvent;

        // De-Register for Rewarded Ads
        IronSourceEvents.onRewardedVideoAdOpenedEvent -= RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClickedEvent -= RewardedVideoAdClickedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent -= RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent -= RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent -= RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent -= RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent -= RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent -= RewardedVideoAdShowFailedEvent;
    }

    /// <summary>
    /// Initialize the connection to the IronSource Network
    /// </summary>
    private void InitIronSource()
    {
        //For Rewarded Video
        IronSource.Agent.init(APP_KEY, IronSourceAdUnits.REWARDED_VIDEO);

        //For Interstitial
        IronSource.Agent.init(APP_KEY, IronSourceAdUnits.INTERSTITIAL);

        //For Banners
        IronSource.Agent.init(APP_KEY, IronSourceAdUnits.BANNER);

        // Load an interstitial so one is available as soon as possible
        IronSource.Agent.loadInterstitial();
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    #region Interstitial
    private void InterstitialAdClosedEvent()
    {
       
    }

    private void InterstitialAdOpenedEvent()
    {
        
    }

    private void InterstitialAdClickedEvent()
    {
        
    }

    private void InterstitialAdShowFailedEvent(IronSourceError obj)
    {
        
    }

    private void InterstitialAdShowSucceededEvent()
    {

    }

    private void InterstitialAdLoadFailedEvent(IronSourceError obj)
    {
        
    }

    /// <summary>
    /// This method will show an interstitial ad when it becomes available
    /// </summary>
    private void InterstitialAdReadyEvent()
    {

    }
    #endregion

    #region Rewarded
    private void RewardedVideoAdShowFailedEvent(IronSourceError obj)
    {

    }

    private void RewardedVideoAdRewardedEvent(IronSourcePlacement obj)
    {

    }

    private void RewardedVideoAdEndedEvent()
    {

    }

    private void RewardedVideoAdStartedEvent()
    {

    }

    private void RewardedVideoAvailabilityChangedEvent(bool obj)
    {

    }

    private void RewardedVideoAdClosedEvent()
    {

    }

    private void RewardedVideoAdClickedEvent(IronSourcePlacement obj)
    {

    }

    private void RewardedVideoAdOpenedEvent()
    {

    }
    #endregion

    /// <summary>
    /// Display a Banner 
    /// Defaults to Amazon Banner for now
    /// </summary>
    private void DisplayBanner()
    {
        IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        StartCoroutine(DisplayBannerRoutine());
    }

    /// <summary>
    /// Simulate showing a banner
    /// Hardcoding values just for demonstration
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayBannerRoutine()
    {
        yield return new WaitForSeconds(5);
        IronSource.Agent.displayBanner();
    }



}