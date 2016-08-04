using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using GooglePlayGames;

public class csShowLeaderBorad : MonoBehaviour
{
    void Start()
    {
        GooglePlayGames.PlayGamesPlatform.Activate();
    }

    public void ShowLeaderBorad()
    {
        Social.ShowLeaderboardUI();
    }
}
