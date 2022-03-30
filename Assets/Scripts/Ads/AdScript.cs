using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;
using System;

public class AdScript : MonoBehaviour, IUnityAdsListener 
{
    //server values
    private string serverurl = "http://kreasaard.atwebpages.com/DiceShooter/adnet.php";
    string adnetValue = "0";
    bool isOffline=true;
    //0 for google ads
    //1 for unity ads
    //2 for automatic ads selection priority to google ads
    int googleAdLoopTurn = 1;

    //---unity ads variables
    string gameId = "4039931";
    string mySurfacingId = "rewardedVideo";
    public string surfacingId = "banner";
    bool testMode = false;

    //----google admob variable
    string AppId = "ca-app-pub-4773340848320054~2732760309";
    string RewardAdd = "ca-app-pub-4773340848320054/6247950595";//"ca-app-pub-3940256099942544/5224354917";
    string interstitialAdd = "ca-app-pub-3940256099942544/5224354917";//"ca-app-pub-4773340848320054/8532893651";//"ca-app-pub-4773340848320054/7568862044";

    public static AdScript Instance { get; private set; } // static singleton

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
        googleAdLoopTurn = 1;
        isOffline=true;
        StartCoroutine(GetValueFromServerForAdNetwork());
        InitializeUnityAds();
    }

    //common 
    public bool isAdLoaded()//for reaward ads
    {
        if (adnetValue == "0"&&!isOffline)//means google ads
        {
            if (this.rewardedAd.IsLoaded())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (adnetValue == "1"&&!isOffline)//means unity ads
        {
            if (Advertisement.IsReady(mySurfacingId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool isinterLoaded()
    {
        if (adnetValue == "0"&&!isOffline)//means google ads
        {
            if (this.interstitial.IsLoaded())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (adnetValue == "1"&&!isOffline)//means unity ads
        {
            if (Advertisement.IsReady())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void ShowIntertesialAdsSwitch()
    {
        isgotReward=false;
        if (adnetValue == "0"&&!isOffline)//means google ads inter to show
        {
            ShowInterstitial();
        }
        else if (adnetValue == "1"&&!isOffline)//means google ads inter to show
        {
            Advertisement.Show();
        }
    }

    public void ShowRewardVideoAdsSwitch()
    {
        isgotReward=false;
        if (adnetValue == "0"&&!isOffline)//means google ads reward to show
        {
            ShowRewardedAds();
        }
        else if (adnetValue == "1"&&!isOffline)//means unity ads reward to show
        {
            ShowRewardedVideo();
        }
    }

    bool isgotReward=false;
    public void GiveRewardAfterCompletion()
    {
        isgotReward=true;
    }

    public bool CheckRewardCompleted()
    {
        return isgotReward;
    }

    /*
    IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(1);
        // Check if UnityAds ready before calling Show method:
        if (isAdLoaded())
        {
            ShowRewardVideoAdsSwitch();
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
            StartCoroutine(WaitAndShow());
        }
    }*/

    //---------------Fetch value from server

    IEnumerator GetValueFromServerForAdNetwork()
    {
        WWW www = new WWW(serverurl);
        yield return www;
        if(www.text!="")
        {
            isOffline=false;
            if (www.text == "0" || www.text == "1"||www.text=="2")
            adnetValue = www.text.ToString();
            print(adnetValue);
            InitializeAdsAccordingtothevalue();
        }
    }

    void InitializeAdsAccordingtothevalue()
    {
        if (adnetValue == "0")//means initialize google ads
        {
            //idea is to first download the reward ad the intertesial ads to consume less data at a time
            RrequestRewardAd();
            StartCoroutine(InitializeGoogleAds());
        }
        else if (adnetValue == "1")//means initialize unity ads
        {
            //InitializeUnityAds();
        }
    }

    //---------------Unity Ads---------------

    #region unity ads initialization

    void InitializeUnityAds()
    {
        //unity ads setup

        //reward ads and intertesial ads
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);

        //banner ads initialization and show
        StartCoroutine(ShowBannerWhenInitialized());
        //StartCoroutine(WaitAndShow());
    }

    #endregion

    #region intertesial ads

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }

    #endregion 

    #region reward ads

    public void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady(mySurfacingId))
        {
            Advertisement.Show(mySurfacingId);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string surfacingId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            GiveRewardAfterCompletion();
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == mySurfacingId)
        {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

    #endregion

    #region banner ads

    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(surfacingId);
        
    }

    #endregion

    //---------------Google Admob---------------

    #region google ads initialze

    IEnumerator InitializeGoogleAds()
    {
        yield return new WaitForSeconds(5);

        if (this.rewardedAd.IsLoaded())
        {
            //RequestInterstitial();
            
        }
        else
        {
            googleAdLoopTurn++;
            if (googleAdLoopTurn > 5)
            {
                adnetValue = "1";
                InitializeAdsAccordingtothevalue();
            }
            else
            {
                StartCoroutine(InitializeGoogleAds());
            }
        }
        print(googleAdLoopTurn);
    }


    #endregion

    #region intertesial ads google

    private InterstitialAd interstitial;

    private void RequestInterstitial()
    {
        string adUnitId = interstitialAdd;


        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        RequestInterstitial();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }

    #endregion

    #region reward ads

    private RewardedAd rewardedAd;

    public void RrequestRewardAd()
    {
        string adUnitId = RewardAdd;

        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        this.RrequestRewardAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        GiveRewardAfterCompletion();
    }

    public void ShowRewardedAds()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    #endregion

}
