using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csLoadingToUiScene : MonoBehaviour
{
    AsyncOperation async;

    IEnumerator Start()
    {
        async = SceneManager.LoadSceneAsync("3. UiScene");

        while (!async.isDone)
        {
            yield return true;
        }
    }
}
