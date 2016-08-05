using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csNull_To_Intro : MonoBehaviour
{
    AsyncOperation async;

    IEnumerator Start()
    {
        async = SceneManager.LoadSceneAsync("1. Intro_Scene");

        while (!async.isDone)
        {
            yield return true;
        }
    }
}
