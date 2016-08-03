using UnityEngine;
using System.Collections;

public class csMoving_Hurdle : MonoBehaviour
{
    float Speed = 5.0f;
    float Hurdle_Speed;
    bool Limit = false;

    void Start()
    {
        
    }

    void Update()
    {
        float Hurdle_Speed = Speed * Time.deltaTime;

        if (Limit == false)
        {
            transform.Translate(Vector3.forward * Hurdle_Speed);
        }
        else if (Limit == true)
        {
            transform.Translate(Vector3.back * Hurdle_Speed);
        }
        
    }

    void OnTriggerEnter(Collider limit)
    {
        if (limit.tag == "Moving_Hurdle_Right_Limit")
        {
            Limit = false;
        }

        if (limit.tag == "Moving_Hurdle_Left_Limit")
        {
            Limit = true;
        }
    }

}
