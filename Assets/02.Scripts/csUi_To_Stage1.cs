using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csUi_To_Stage1 : MonoBehaviour
{
    AsyncOperation async;

    IEnumerator Start()
    {
        async = SceneManager.LoadSceneAsync("STAGE_2");

        while (!async.isDone)
        {
            yield return true;
        }
    }
}
