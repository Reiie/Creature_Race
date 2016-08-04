using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class csAds : MonoBehaviour
{

    void Start()
    {

    }

    public void ShowDefaultAd()
    {
#if UNITY_ADS
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
#endif
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady())
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = HandleShowResult;
            Advertisement.Show(null, options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                UserManager.Instance().AdsSkip();
                Debug.Log("광고끝");
                break;

            case ShowResult.Skipped:

                break;

            case ShowResult.Failed:

                break;
        }
    }
}
