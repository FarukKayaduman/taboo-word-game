//using GoogleMobileAds;
//using GoogleMobileAds.Api;
//using System;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
//    private InterstitialAd interstitialAd;

//    // These ad units are configured to always serve test ads.
//#if UNITY_ANDROID
//    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
//#elif UNITY_IPHONE
//  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
//#else
//  private string _adUnitId = "unused";
//#endif

//    private void Start()
//    {
//        // Initialize the Google Mobile Ads SDK.
//        MobileAds.Initialize((InitializationStatus initStatus) =>
//        {
//            // This callback is called once the MobileAds SDK is initialized.
//        });

//        LoadInterstitialAd();

//        if(interstitialAd.CanShowAd())
//        {
//            interstitialAd.Show();
//        }
//    }

//    private void Update()
//    {
//        try
//        {
//            if(interstitialAd.CanShowAd())
//            {
//                interstitialAd.Show();
//            }
//        }
//        catch (Exception exception)
//        {
//            Debug.LogException(exception);
//        }
//    }

//    /// <summary>
//    /// Loads the interstitial ad.
//    /// </summary>
//    public void LoadInterstitialAd()
//    {
//        // Clean up the old ad before loading a new one.
//        if (interstitialAd != null)
//        {
//            interstitialAd.Destroy();
//            interstitialAd = null;
//        }

//        Debug.Log("Loading the interstitial ad.");

//        // create our request used to load the ad.
//        var adRequest = new AdRequest.Builder()
//                .AddKeyword("unity-admob-sample")
//                .Build();

//        // send the request to load the ad.
//        InterstitialAd.Load(_adUnitId, adRequest,
//            (InterstitialAd ad, LoadAdError error) =>
//            {
//                // if error is not null, the load request failed.
//                if (error != null || ad == null)
//                {
//                    Debug.LogError("interstitial ad failed to load an ad " +
//                                   "with error : " + error);
//                    return;
//                }

//                Debug.Log("Interstitial ad loaded with response : "
//                          + ad.GetResponseInfo());

//                interstitialAd = ad;
//            });
//    }


}

