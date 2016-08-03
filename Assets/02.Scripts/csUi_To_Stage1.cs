using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csUi_To_Stage1 : MonoBehaviour
{
    float Timer = 0;

    void Update()
    {
        Timer = Timer + Time.deltaTime;

        if (Timer > 3)
        {
            Timer = 0;
            SceneManager.LoadScene("STAGE_2");
        }
    }
}
