using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csScene_Change : MonoBehaviour
{
    public void GoToLoadingScene_IntroToUIScene()
    {
        SceneManager.LoadScene("2. Intro_To_Ui_Loading");
    }
}
