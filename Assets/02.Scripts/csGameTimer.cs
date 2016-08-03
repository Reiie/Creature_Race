using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csGameTimer : MonoBehaviour
{
    private int Timer = 0;

    bool Timer_Bool = false;

    public GameObject Count_1;  
    public GameObject Count_2;  
    public GameObject Count_3; 
    public GameObject Count_Start;

    public float Play_Time;
    public Text Show_Play_Time;

    void Start()
    {
        Timer = 0;

        Count_1.SetActive(false);
        Count_2.SetActive(false);
        Count_3.SetActive(false);
        Count_Start.SetActive(false);

    }

    void Update()
    {
        Play_Time = Play_Time + Time.deltaTime;

        //string Play_Time_Text = Play_Time.ToString();

        string Play_Time_Text = Play_Time.ToString("N2");


        if (Timer_Bool == true)
        {
            Show_Play_Time.text = Play_Time_Text;
        }

        if (Timer == 0)
        {
            Time.timeScale = 0.0f;
        }

        if (Timer <= 90)
        {
            Timer++;

            if (Timer < 30)
            {
                Count_3.SetActive(true);
            }
            if (Timer > 30)
            {
                Count_3.SetActive(false);
                Count_2.SetActive(true);
            }
            if (Timer > 60)
            {
                Count_2.SetActive(false);
                Count_1.SetActive(true);
            }
            if (Timer >= 90)
            {
                Count_1.SetActive(false);
                Count_Start.SetActive(true);
                StartCoroutine(this.LoadingEnd());

                Timer_Bool = true;

                Time.timeScale = 1.0f;
            }
        }
    }

    IEnumerator LoadingEnd()
    {
        yield return new WaitForSeconds(1.0f);
        Count_Start.SetActive(false);
    }
}
  
