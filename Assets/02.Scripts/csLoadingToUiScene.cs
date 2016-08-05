using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csLoadingToUiScene : MonoBehaviour
{
    AsyncOperation async;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2.5f);

        async = SceneManager.LoadSceneAsync("3. UiScene");

        while (!async.isDone)
        {
            yield return true;
        }
    }
}
