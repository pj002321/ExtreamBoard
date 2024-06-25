using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsControll : AdmobAdsScript
{
    private static AdsControll instance;
    public static AdsControll Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AdsControll();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            EnterRewardAd();
            ExitBannerAd();
        }
        else
        {
            EnterBannerAd(scene);
        }
    }
    void EnterRewardAd()
    {
        LoadRewardedAd();
        ShowRewardedAd();
    }

    void EnterBannerAd(Scene scene)
    {
        if (scene.buildIndex == 0)
        {
            LoadTopBannerAd();
        }
        else if (scene.buildIndex>=2 && scene.buildIndex <=4)
        {
            LoadBottomBannerAd();
        }
    }
    void ExitBannerAd()
    {
        DestroyBannerAd();
    }



}
