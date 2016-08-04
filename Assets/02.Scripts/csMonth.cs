using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class csMonth : MonoBehaviour {


    public GameObject dayPrefabs;

    public List<csDay> MyDays;

    public UIGrid m_Grid;

    public DateTime dt;

    

    public UILabel YearLabel;

    public UILabel MonthLabel_1;

    public UILabel MonthLabel_2;

    public int startIndex = 0;

    public int endIndex = 0;

    bool check = false;
    
    // Use this for initialization

    void Awake()
    {
       // DontDestroyOnLoad(gameObject);
    }
    void Start() {


        startIndex = CalenderManager.space;

        MyDays = new List<csDay>();
        string year = dt.ToString("yyyy");
        string month = dt.ToString("MM");

        YearLabel.text = year.Substring(3);

        MonthLabel_1.text = month.Substring(0, 1);
        MonthLabel_2.text = month.Substring(1);

        DateTime tmp = dt.AddDays(-CalenderManager.space);

        for (int i = 0; i < 42; i++)
        {
            GameObject day = NGUITools.AddChild(gameObject, dayPrefabs);

            // 다음달 위해서 달수가 증가한 인덱스 체크 후 빈공간 계산
            if (dt.Year == tmp.Year)
            {
                if (dt.Month + 1 == tmp.Month && !check)
                {
                    check = true;
                    CalenderManager.space = i % 7;
                    endIndex = i - 1;
                    //Debug.Log(CalenderManager.space);
                }
            }
            else if (dt.Year + 1 == tmp.Year)
            {
                if (dt.Month > tmp.Month && !check)
                {
                    check = true;
                    CalenderManager.space = i % 7;
                    endIndex = i - 1;

                }
            }

            if (i % 7 == 0)
            {
                day.GetComponent<csDay>().dayLabel.color = Color.red;
            }

            // 이번달 아닌 일들 보기 폰트
            if (dt.Month != tmp.Month)
            {
                day.GetComponent<csDay>().dayLabel.fontSize = 30;
                day.GetComponent<csDay>().dayLabel.alpha = 0.5f;
            }

            day.GetComponent<csDay>().dt = tmp;
            tmp = tmp.AddDays(1);

            if (dt.Year == 2040)
            {
                day.GetComponent<csDay>().index = (dt.Month - 1) * 42 + i;
            }
            else
            {
                day.GetComponent<csDay>().index = (dt.Month - 1 + 12) * 42 + i;
            }
            MyDays.Add(day.GetComponent<csDay>());


        }
        DateTime tmpday = new DateTime(2041, 12, 1);
        if (dt == tmpday)
        {
            CalenderManager.space = 0;
        }
        m_Grid.Reposition();
        
        //  Debug.Log("스타트" + startIndex);
        //  Debug.Log("끝" + endIndex);
    }
	

}
