using GoogleMobileAds.Api;
using UnityEngine;

namespace AdMobController
{
    public class GoogleAdMobController : MonoBehaviour
    {
        public static GoogleAdMobController Instance;

        private InterstitialAd _interstitialAd;

        // Interstitial ad test ID: "ca-app-pub-3940256099942544/1033173712"
        private readonly string _interstitialAdUnitId = AdMobCredentialsManager.InterstitialAdID;

        private float _interstitialAdTimer;
        private int _adCooldownSecond;

        public void Start()
        {
            if (Instance == null)
            {
                Instance = GetComponent<GoogleAdMobController>();
            }

            _adCooldownSecond = 30;
            _interstitialAdTimer = _adCooldownSecond;

            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                LoadInterstitialAd();
            });
        }

        private void Update()
        {
            _interstitialAdTimer += Time.deltaTime;
        }

        /// <summary>
        /// Loads the interstitial ad.
        /// </summary>
        public void LoadInterstitialAd()
        {
            // Clean up the old ad before loading a new one.
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest.Builder().Build();

            // send the request to load the ad.
            InterstitialAd.Load(_interstitialAdUnitId, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    _interstitialAd = ad;
                });

            RegisterReloadHandler(_interstitialAd);

        }

        /// <summary>
        /// Shows the interstitial ad.
        /// </summary>
        public void ShowInterstitialAd()
        {
            if (_interstitialAdTimer < _adCooldownSecond)
            {
                Debug.LogError("On cooldown for interstitial ad.");
                return;
            }

            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                _interstitialAd.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }
        }

        private void RegisterReloadHandler(InterstitialAd ad)
        {
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                _interstitialAdTimer = 0;
                LoadInterstitialAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);
                
                LoadInterstitialAd();
            };
        }
    }
}