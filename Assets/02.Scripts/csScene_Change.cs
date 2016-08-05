using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class csScene_Change : MonoBehaviour
{
    void Start()
    {
        GooglePlayGames.PlayGamesPlatform.Activate();
    }

    public void GoToLoadingScene_IntroToUIScene()
    {
        string unLockAchievement_id = "CgkI9ZbZnpIHEAIQAQ";

        Social.ReportProgress(unLockAchievement_id, 100.0f, (bool success) => 
        {});

        SceneManager.LoadScene("2. Intro_To_Ui_Loading");
    }
}
