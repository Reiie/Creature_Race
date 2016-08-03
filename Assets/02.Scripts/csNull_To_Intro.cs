using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class csNull_To_Intro : MonoBehaviour
{
    float Timer = 0;

    void Update()
    {
        Timer = Timer + Time.deltaTime;

        if (Timer > 7)
        {
            Timer = 0;
            SceneManager.LoadScene("1. Intro_Scene");
        }
    }
}
