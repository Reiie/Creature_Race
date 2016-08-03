using UnityEngine;
using System.Collections;

public class csCarSpeedTest : MonoBehaviour
{
    CarController obj;
    CarAIControl aa;
    public bool WaterTile = false;

    void Start()
    {
        obj = GameObject.Find("Car").GetComponent<CarController>();
        aa = GameObject.Find("Car").GetComponent<CarAIControl>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick()
    {
      //  Rigidbody body = GameObject.Find("Car").GetComponent<Rigidbody>();
       // body.velocity = body.velocity * 2;
        obj.m_Topspeed = 30;

        Debug.Log("클릭");
      //  aa.m_Driving = true;
    }

    public void velocityButton()
    {
        obj.m_Topspeed = 0;

        Debug.Log("클릭");
        /*
        Rigidbody body = GameObject.Find("Car").GetComponent<Rigidbody>();
        body.velocity = Vector3.zero;
        aa = GameObject.Find("Car").GetComponent<CarAIControl>();
        */
        //  aa.m_Driving = false;

    }

    public void jumpButton()
    {
        GameObject.Find("Car").GetComponent<Rigidbody>().AddForce(Vector3.up * 150000.0f);
    }

    public void targetMissing()
    {
        
        obj.m_Topspeed = 0;
        aa.m_Target = null;
        //  Rigidbody body = GameObject.Find("Car").GetComponent<Rigidbody>();
        // body.velocity = Vector3.zero;
    }

    public void setTarget()
    {
        aa.m_Target = GameObject.Find("Target").GetComponent<Transform>();
        obj.m_Topspeed = 20;
    }
}