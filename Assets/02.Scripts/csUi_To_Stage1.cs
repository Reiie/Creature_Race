using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csUi_To_Stage1 : MonoBehaviour
{
    AsyncOperation async;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2.5f);

        async = SceneManager.LoadSceneAsync("STAGE_2");

        while (!async.isDone)
        {
            yield return true;
        }
    }
}
