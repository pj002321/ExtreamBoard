using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using TMPro;
using UnityEngine.SceneManagement;

public class AdmobAdsScript : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;

    public string appId = "";

#if UNITY_ANDROID
    public string bannerID = "ca-app-pub-9194202079810745/2487922064";
    public string interID = "ca-app-pub-9194202079810745/8081377134";
    public string rewardID = "ca-app-pub-9194202079810745/3870669385";
    public string nativeID = "ca-app-pub-9194202079810745/7889805444";

#endif 
    BannerView bannerView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;

    private void Start()
    {
        
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {
            print("Ads Initialised !!");
        });

        
    }
   
    #region Banner

    public void LoadTopBannerAd()
    {   
        //create a banner
        CreateBannerView(AdSize.Banner,AdPosition.Top);

        //listen to banner events
        ListenToBannerEvent();

        // load the banner
        if (bannerView == null)
        {
            CreateBannerView(AdSize.Banner,AdPosition.Top);
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading banner Ad !!");
        bannerView.LoadAd(adRequest); // show the Banner on the Screen
    }
    public void LoadBottomBannerAd()
    {
        //create a banner
        CreateBannerView(AdSize.Leaderboard,AdPosition.Bottom);

        //listen to banner events
        ListenToBannerEvent();

        // load the banner
        if (bannerView == null)
        {
            CreateBannerView(AdSize.Leaderboard,AdPosition.Top);
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading banner Ad !!");
        bannerView.LoadAd(adRequest); // show the Banner on the Screen
    }
    void CreateBannerView(AdSize Banner,AdPosition position)
    {
        if (bannerView != null)
        {
            DestroyBannerAd();
        }
        bannerView = new BannerView(bannerID, Banner, position);
    }
    void ListenToBannerEvent()
    {
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}."+ adValue.Value+adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
    public void DestroyBannerAd()
    {
        if(bannerView != null)
        {
            print("Destroying banner Ad");
            bannerView.Destroy();
            bannerView = null;
        }
    }

    #endregion

    #region Interstitial
    public void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(interID, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if(error!=null || ad == null)
            {
                print("Interstitial ad failed to load" + error);
                return;
            }

            print("Interstitial ad loaded !!" + ad.GetResponseInfo());
            interstitialAd = ad;
            InterstitialEvent(interstitialAd);
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd()) 
        {
            interstitialAd.Show();
        }
        else 
        {
            print("Interstitial ad not ready !!");
        }
    }

    public void InterstitialEvent(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Interstitial ad paid {0} {1}."+
                adValue.Value+
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    #endregion

    #region Rewarded

    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
           rewardedAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardID, adRequest, (RewardedAd ad, LoadAdError error) => 
        {
            if (error != null || ad == null)
            {
                print("Rewarded failed to laod" + error);
                return;
            }

            print("Rewardede ad loaded !!");
            rewardedAd = ad;
            RewaededAdEvents(rewardedAd);
        });
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd!=null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                print("Give reward to player !!");
                // Reward to Player : Game Item
            });
        }
        else
        {
            print("Rewarded is not Ready");
        }
    }
    public void RewaededAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}."+
                adValue.Value+
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    #endregion

    #region Native

   // public Image img;
    private NativeAd nativeAd;
    public void RequestNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder(nativeID).ForNativeAd().Build();

        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;

        var adRequest = new AdRequest();
        adLoader.LoadAd(adRequest);
    }

    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e)
    {
        print("Native ad loaded");
        this.nativeAd = e.nativeAd;

        Texture2D iconTexture = this.nativeAd.GetIconTexture();
        Sprite sprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height),
            Vector2.one * 0.5f);

        //img.sprite = sprite;
    }

    private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs  e)
    {
        print("Native ad failed loaded");
    }
    #endregion
}
