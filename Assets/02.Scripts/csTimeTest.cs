using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csTimeTest : MonoBehaviour
{
    GameObject obj;
    GameObject obj2;
    GameObject obj3;
    Text txt;
    Text txt2;
    Text txt3;
    System.DateTime ButtonClickTime;
    System.DateTime current;
    System.DateTime TamSaStart;
    System.DateTime TamSaEnd;
    System.TimeSpan tmp;

    void Start ()
    {
        obj = GameObject.Find("Timetxt");
        obj2 = GameObject.Find("Timetxt2");
        obj3 = GameObject.Find("Timetxt3");
        txt = obj.GetComponent<Text>();
        txt2 = obj2.GetComponent<Text>();
        txt3 = obj3.GetComponent<Text>();
 
        TamSaStart = System.DateTime.Now;
    }
	
	void Update ()
    {
        txt.text = System.DateTime.Now.ToString("hh:mm:ss");  
    }

    public void ButtonClick()
    {
        ButtonClickTime = System.DateTime.Now;
        txt2.text = ButtonClickTime.ToString("hh:mm:ss");
        
        
    }

    public void Check()
    {
        tmp = System.DateTime.Now - ButtonClickTime;
        txt3.text = ""+tmp.Minutes;

    }
}
