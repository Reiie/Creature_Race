using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csLoadingToUiScene : MonoBehaviour
{
    float Timer = 0;

    void Update()
    {
        Timer = Timer + Time.deltaTime;

        if (Timer > 5)
        {
            Timer = 0;
            SceneManager.LoadScene("3. UiScene");   
        }
    }

}
