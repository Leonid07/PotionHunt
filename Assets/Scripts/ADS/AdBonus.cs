using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdBonus : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string adPlacementIdIOS = "Rewarded_iOS";
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    string _adUnitId = null;

    public Button buttonWatchAdsCoin;
    public Button buttonWatchAdsEnergy;

    private void Awake()
    {
#if UNITY_IOS
        _adUnitId = adPlacementIdIOS;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
    }

    private void Start()
    {
        LoadAd();
        buttonWatchAdsCoin.onClick.AddListener(() => { ShowAdAndCoin(); });

        buttonWatchAdsEnergy.onClick.AddListener(() => { ShowAdAndEnergy(); });
    }
    public void LoadAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }
    int i = 0;
    public void ShowAdAndCoin()
    {
        i = 0;
        Advertisement.Show(_adUnitId, this);
        LoadAd();
    }
    public void ShowAdAndEnergy()
    {
        i = 1;
        Advertisement.Show(_adUnitId, this);
        LoadAd();
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            if (i == 0)
            {
                PlayerManager.InstancePlayer.goldCount += 150;
                PlayerManager.InstancePlayer.SaveGold();
            }
            if (i == 1)
            {
                PlayerManager.InstancePlayer.energyCount += 10;
                if (PlayerManager.InstancePlayer.energyCount >= PlayerManager.InstancePlayer.maxEnergy)
                {
                    PlayerManager.InstancePlayer.energyCount = PlayerManager.InstancePlayer.maxEnergy;
                }
                PlayerManager.InstancePlayer.SaveEnergy();
            }
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad Loaded: {placementId}");
    }
}