using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Source.Helpers
{
    public class AdsManager : Singleton<AdsManager>, IDestroyableSingleton
    {
        #region PublicFields

        #endregion

        #region Properties

        public bool CanDestroyed => false;

        #endregion

        #region PrivateFields

        private InterstitialAd _interstitial;

        #endregion

        #region PublicMethods

        public void Initialize()
        {
#if UNITY_ANDROID
            const string appId = "ca-app-pub-2026374515474711~3474586947";
#elif UNITY_IPHONE
            const string appId = "appID";
        #else
            const string appId = "unexpected_platform";
        #endif

            MobileAds.Initialize(appId);
            RequestInterstitial();
        }

        public void ShowInterstitial()
        {
            if (_interstitial.IsLoaded())
            {
                _interstitial.Show();
            }
        }

        #endregion

        #region PrivateMethods

        private void RequestInterstitial()
        {
#if UNITY_ANDROID
            var adUnitId = "ca-app-pub-2026374515474711/4915838730";
            var adUnitIdTest = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        var adUnitId = "ca-app-pub-3940256099942544/4411468910";
    #else
        var adUnitId = "unexpected_platform";
    #endif

            _interstitial?.Destroy();
            // Initialize an InterstitialAd.
//            _interstitial = new InterstitialAd(adUnitId);
            _interstitial = new InterstitialAd(adUnitId);

            // Create an empty ad request.
//            AdRequest request = new AdRequest.Builder().Build();
            AdRequest request = new AdRequest.Builder()
//                .AddTestDevice("7395F5BAB577560C9646E63D0FD73051")
                .Build();

            _interstitial.OnAdClosed += InterstitialOnOnAdClosed;
            _interstitial.OnAdFailedToLoad += InterstitialOnOnAdFailedToLoad;
            _interstitial.OnAdOpening += InterstitialOnOnAdOpening;
            // Load the interstitial with the request.
            _interstitial.LoadAd(request);
        }

        private void InterstitialOnOnAdOpening(object sender, EventArgs e)
        {
            AudioManager.Instance.MuteAudios();
            Time.timeScale = 0;
        }

        private void InterstitialOnOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            Time.timeScale = 1;
            AudioManager.Instance.Initialize();
        }

        private void InterstitialOnOnAdClosed(object sender, EventArgs e)
        {
            Time.timeScale = 1;
            AudioManager.Instance.Initialize();
            RequestInterstitial();
        }

        #endregion
    }
}