using UnityEngine;
using System.Collections;
using System;

public class csDay : MonoBehaviour
{

    public csWork.Work myWork = csWork.Work.Nowork;
    
    public int index;
    public int isLastWork;
    public UILabel dayLabel;
    public UISprite icon;
    public UISprite sprite;
    public DateTime dt;
    csMonth parent_month;
    CalenderManager manager;
    public int shardCount;
    public int before_shardCount = 0;

    public GameObject StaminaFail;
    public GameObject DayFail;
    public GameObject WorkFail;

    // Use this for initialization

    void Awake()
    {
      //  DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        StaminaFail = GameObject.Find("StaminaFail");
        DayFail = GameObject.Find("DayFail");
        WorkFail = GameObject.Find("WorkFail");

        parent_month = gameObject.transform.parent.GetComponent<csMonth>();
        manager = GameObject.Find("Month_DayGrid").GetComponent<CalenderManager>();
      //  Debug.Log(dt);
        dayLabel.text = Convert.ToString(dt.Day);

        // 전달 공유
        if (parent_month.dt.Year == 2040)
        {
            if (parent_month.dt.Month != 1)
            {
                before_shardCount = manager.MyMonths[parent_month.dt.Month - 2].MyDays[0].shardCount;
            }
            
            shardCount = manager.MyMonths[parent_month.dt.Month].startIndex + (41 - parent_month.endIndex);
        }
        else
        {
            before_shardCount = manager.MyMonths[parent_month.dt.Month - 2 + 12].MyDays[0].shardCount;
            if (parent_month.dt.Month != 12)
            {
                shardCount = manager.MyMonths[parent_month.dt.Month + 12].startIndex + (41 - parent_month.endIndex);
            }
        }
    }


    bool CheckStamina(csWork work)
    {
        DateTime nowDate = UserManager.Instance().nowDate;
        int myStamina = UserManager.Instance().stamina;
        Debug.Log("처음스태미나 " + myStamina);

        int myIndex = -1;
        int workstartIndex = -1;

        //첫날 인덱스 찾기
        for (int i = 0; i < parent_month.MyDays.Count; i++)
        {
            csDay tmp = parent_month.MyDays[i];
            if (tmp.dt == nowDate)
            {
                workstartIndex = i;
                break;
            }
        }
            // 인덱스 찾기
        for (int i = 0; i < parent_month.MyDays.Count; i++)
        {
            csDay tmp = parent_month.MyDays[i];
            if (tmp.GetInstanceID() == GetInstanceID())
            {
                myIndex = i;
                break;
            }
        }

        Debug.Log("첫날인덱스 " + workstartIndex);
        Debug.Log("지금인덱스 " + myIndex);
        
        int nextMonthIndex = 0;

        for (int i = 0; i < 7; i++)
        {

            if ((workstartIndex + i) < 42)
            {
                csWork.Work tmpWork = csWork.Work.Nowork;
                if (work.r_Time == 1)
                {
                    if ((workstartIndex + i) == myIndex)
                    {
                        tmpWork = work.WorkName;
                    }
                    else
                    {
                        tmpWork = parent_month.MyDays[workstartIndex + i].myWork;
                    }
                }
                else if (work.r_Time == 2)
                {
                    if ((workstartIndex + i) == myIndex)
                    {
                        tmpWork = work.WorkName;
                        Debug.Log(work.WorkName);
                    }
                    else if ((workstartIndex + i) == (myIndex + 1))
                    {
                        tmpWork = work.WorkName;
                    }
                    else
                    {
                        tmpWork = parent_month.MyDays[workstartIndex + i].myWork;
                    }

                }
                Debug.Log(work.WorkName);
                switch (tmpWork)
                {
                    
                    case csWork.Work.Nowork:                       
                        break;
                    case csWork.Work.Kangnam:
                        myStamina -= 12;
                        break;
                    case csWork.Work.Kangbuk:
                        myStamina -= 12;
                        break;
                    case csWork.Work.Alba:
                        myStamina -= 11;
                        break;
                    case csWork.Work.CheonHo:
                        myStamina -= 12;
                        break;
                    case csWork.Work.Labor:
                        myStamina -= 12;
                        break;
                    case csWork.Work.Plant:
                        myStamina -= 15;
                        break;
                    case csWork.Work.Repair:
                        myStamina -= 8;
                        break;
                    case csWork.Work.Rest:
                        myStamina += 25;
                        break;
                }
                Debug.Log("스태미나" + myStamina);
                if(myStamina < 0)
                {
                    return false;
                }                 
            }
            else if ((workstartIndex + i) >= 42)
            {          
                csMonth tmp;
                if (parent_month.dt.Month == dt.Month)
                {
                    tmp = manager.MyMonths[dt.Month];
                }
                else
                {
                    tmp = manager.MyMonths[dt.Month - 1];
                }

                csWork.Work tmpWork = csWork.Work.Nowork;

                if (work.r_Time == 1)
                {
                    if ((workstartIndex + i) == myIndex)
                    {
                        tmpWork = work.WorkName;
                    }
                    else
                    {
                        tmpWork = tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork;
                    }
                }
                else if (work.r_Time == 2)
                {
                    if ((workstartIndex + i) == myIndex)
                    {
                        tmpWork = work.WorkName;
                    }
                    else if((workstartIndex + i) == (myIndex + 1))
                    {
                        tmpWork = work.WorkName;
                    }
                    else
                    {
                        tmpWork = tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork;
                    }

                }
                switch (tmpWork)
                {
                    case csWork.Work.Nowork:
                        break;
                    case csWork.Work.Kangnam:
                        myStamina -= 12;
                        break;
                    case csWork.Work.Kangbuk:
                        myStamina -= 12;
                        break;
                    case csWork.Work.Alba:
                        myStamina -= 11;
                        break;
                    case csWork.Work.CheonHo:
                        myStamina -= 12;
                        break;
                    case csWork.Work.Labor:
                        myStamina -= 12;
                        break;
                    case csWork.Work.Plant:
                        myStamina -= 15;
                        break;
                    case csWork.Work.Repair:
                        myStamina -= 8;
                        break;
                    case csWork.Work.Rest:
                        myStamina += 25;
                        break;
                }

                if (myStamina < 0)
                { 
                    return false;
                }

                nextMonthIndex++;
            }
        }

        return true;
    }

    void OnDrop(GameObject dropped)
    {
        csWork work = dropped.GetComponent<csWork>();
        if (work == null)
            return;

        int myIndex = -1;

        // 인덱스 찾기
        for (int i = 0; i < parent_month.MyDays.Count; i++)
        {
            csDay tmp = parent_month.MyDays[i];
            if (tmp.GetInstanceID() == GetInstanceID())
            {
                myIndex = i;
                break;
            }
        }
        
        // 스케줄 등록 가능한지 확인

        DateTime nowDate = UserManager.Instance().nowDate;

        // 현재날짜보다 뒤에등록하면 취소
        if(nowDate > dt)
        {
            Debug.Log("뒤다");
            DayFail.SetActive(true);
            return;
        }


        int nextMonthIndex = 0;

        for (int i = 0; i < work.r_Time; i++)
        {

            if ((myIndex + i) < 42)
            {
                if (parent_month.MyDays[myIndex + i].myWork != csWork.Work.Nowork)
                {
                    Debug.Log("스케줄못함");
                    WorkFail.SetActive(true);
                    return;
                }
                if(nowDate.AddDays(7) <= parent_month.MyDays[myIndex + i].dt)
                {
                    Debug.Log("날짜초과");
                    DayFail.SetActive(true);
                    return;
                }

            }
            else if ((myIndex + i) >= 42)
            {

                csMonth tmp;
                if (parent_month.dt.Month == dt.Month)
                {
                    tmp = manager.MyMonths[dt.Month];
                }
                else
                {
                    tmp = manager.MyMonths[dt.Month - 1];
                }
                if (tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork != csWork.Work.Nowork)
                {
                    Debug.Log("담달스케줄못함");
                    WorkFail.SetActive(true);
                    return;
                }
                
                if(nowDate.AddDays(7) <= tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].dt)
                {
                    Debug.Log("날짜초과");
                    DayFail.SetActive(true);
                }

                nextMonthIndex++;
            }
        }

        nextMonthIndex = 0;

        // 스태미나 부족하면 안되게 해야댐

        if(!CheckStamina(work))
        {
            Debug.Log("스태미나부족으로 실패");
            StaminaFail.SetActive(true);
            return;
        }

        // 해당 작업으로 아이콘 바꾸기
        for (int i = 0; i < work.r_Time; i++)
        {
            if ((myIndex + i) < 42)
            {
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (work.WorkName == csWork.Work.Kangnam)
                {
                    
                    parent_month.MyDays[myIndex + i].myWork = csWork.Work.Kangnam;                   
                    parent_month.MyDays[myIndex + i].icon.spriteName = "work_kangnam";
                    parent_month.MyDays[myIndex + i].icon.alpha = 0.8f;

                    manager.changeDay.Add(parent_month.MyDays[myIndex + i]);

                    // 마지막 작업일
                    if (i == (work.r_Time - 1))
                    {
                        parent_month.MyDays[myIndex + i].isLastWork = 1;
                    }

                    // 다음달 공유부분
                    if (i + myIndex >= ( 42 - shardCount))
                    {
                        Debug.Log("담달");

                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month + 12];
                        }

                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].myWork = csWork.Work.Kangnam;
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.spriteName = "work_kangnam";
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(myIndex + i) - (42 - shardCount)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(myIndex + i) - (42 - shardCount)]);
                    }
                    // 전달 공유부분

                    else if (i + myIndex < before_shardCount)
                    {
                        Debug.Log("전달");
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2 + 12];
                        }

                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].myWork = csWork.Work.Kangnam;
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.spriteName = "work_kangnam";
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(42 - before_shardCount) + (myIndex + i)]);
                    }
                }
                //////////////////////////////////////
                else if (work.WorkName == csWork.Work.Kangbuk)
                {
                    parent_month.MyDays[myIndex + i].myWork = csWork.Work.Kangbuk;
                    parent_month.MyDays[myIndex + i].icon.spriteName = "work_kangbuk";
                    parent_month.MyDays[myIndex + i].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        parent_month.MyDays[myIndex + i].isLastWork = 1;
                    }

                    manager.changeDay.Add(parent_month.MyDays[myIndex + i]);

                    if (i + myIndex >= (42 - shardCount))
                    {
                        Debug.Log("담달");
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month + 12];
                        }

                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].myWork = csWork.Work.Kangbuk;
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.spriteName = "work_kangbuk";
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(myIndex + i) - (42 - shardCount)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(myIndex + i) - (42 - shardCount)]);
                    }

                    // 전달 공유부분

                    else if (i + myIndex < before_shardCount)
                    {

                        Debug.Log("전달");
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2 + 12];
                        }

                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].myWork = csWork.Work.Kangbuk;
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.spriteName = "work_kangbuk";
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(42 - before_shardCount) + (myIndex + i)]);
                    }

                }
                else if (work.WorkName == csWork.Work.Alba)
                {
                    parent_month.MyDays[myIndex + i].myWork = csWork.Work.Alba;
                    parent_month.MyDays[myIndex + i].icon.spriteName = "work_alba";
                    parent_month.MyDays[myIndex + i].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        parent_month.MyDays[myIndex + i].isLastWork = 1;
                    }

                    manager.changeDay.Add(parent_month.MyDays[myIndex + i]);

                    if (i + myIndex >= (42 - shardCount))
                    {
                        Debug.Log("담달");
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month + 12];
                        }

                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].myWork = csWork.Work.Alba;
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.spriteName = "work_alba";
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(myIndex + i) - (42 - shardCount)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(myIndex + i) - (42 - shardCount)]);
                    }

                    // 전달 공유부분

                    else if (i + myIndex < before_shardCount)
                    {                   
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2 + 12];
                        }

                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].myWork = csWork.Work.Alba;
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.spriteName = "work_alba";
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(42 - before_shardCount) + (myIndex + i)]);
                    }

                }
                else if (work.WorkName == csWork.Work.CheonHo)
                {
                    parent_month.MyDays[myIndex + i].myWork = csWork.Work.CheonHo;
                    parent_month.MyDays[myIndex + i].icon.spriteName = "work_cheonho";
                    parent_month.MyDays[myIndex + i].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        parent_month.MyDays[myIndex + i].isLastWork = 1;
                    }

                    manager.changeDay.Add(parent_month.MyDays[myIndex + i]);

                    if (i + myIndex >= (42 - shardCount))
                    {
                        Debug.Log("담달");
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month + 12];
                        }

                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].myWork = csWork.Work.CheonHo;
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.spriteName = "work_cheonho";
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(myIndex + i) - (42 - shardCount)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(myIndex + i) - (42 - shardCount)]);
                    }

                    // 전달 공유부분

                    else if (i + myIndex < before_shardCount)
                    {
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2 + 12];
                        }

                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].myWork = csWork.Work.CheonHo;
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.spriteName = "work_cheonho";
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(42 - before_shardCount) + (myIndex + i)]);
                    }

                }
                else if (work.WorkName == csWork.Work.Labor)
                {
                    parent_month.MyDays[myIndex + i].myWork = csWork.Work.Labor;
                    parent_month.MyDays[myIndex + i].icon.spriteName = "work_labor";
                    parent_month.MyDays[myIndex + i].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        parent_month.MyDays[myIndex + i].isLastWork = 1;
                    }

                    manager.changeDay.Add(parent_month.MyDays[myIndex + i]);

                    if (i + myIndex >= (42 - shardCount))
                    {
                        Debug.Log("담달");
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month + 12];
                        }

                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].myWork = csWork.Work.Labor;
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.spriteName = "work_labor";
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(myIndex + i) - (42 - shardCount)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(myIndex + i) - (42 - shardCount)]);
                    }

                    // 전달 공유부분

                    else if (i + myIndex < before_shardCount)
                    {
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2 + 12];
                        }

                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].myWork = csWork.Work.Labor;
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.spriteName = "work_labor";
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(42 - before_shardCount) + (myIndex + i)]);
                    }

                }
                //--------------
                else if (work.WorkName == csWork.Work.Plant)
                {
                    parent_month.MyDays[myIndex + i].myWork = csWork.Work.Plant;
                    parent_month.MyDays[myIndex + i].icon.spriteName = "work_plant";
                    parent_month.MyDays[myIndex + i].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        parent_month.MyDays[myIndex + i].isLastWork = 1;
                    }

                    manager.changeDay.Add(parent_month.MyDays[myIndex + i]);

                    if (i + myIndex >= (42 - shardCount))
                    {
                        Debug.Log("담달");
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month + 12];
                        }

                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].myWork = csWork.Work.Plant;
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.spriteName = "work_plant";
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(myIndex + i) - (42 - shardCount)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(myIndex + i) - (42 - shardCount)]);
                    }

                    // 전달 공유부분

                    else if (i + myIndex < before_shardCount)
                    {
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2 + 12];
                        }

                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].myWork = csWork.Work.Plant;
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.spriteName = "work_plant";
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(42 - before_shardCount) + (myIndex + i)]);
                    }

                }
                //----------------
                else if (work.WorkName == csWork.Work.Repair)
                {
                    parent_month.MyDays[myIndex + i].myWork = csWork.Work.Repair;
                    parent_month.MyDays[myIndex + i].icon.spriteName = "work_repair";
                    parent_month.MyDays[myIndex + i].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        parent_month.MyDays[myIndex + i].isLastWork = 1;
                    }

                    manager.changeDay.Add(parent_month.MyDays[myIndex + i]);

                    if (i + myIndex >= (42 - shardCount))
                    {
                        Debug.Log("담달");
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month + 12];
                        }

                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].myWork = csWork.Work.Repair;
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.spriteName = "work_repair";
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(myIndex + i) - (42 - shardCount)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(myIndex + i) - (42 - shardCount)]);
                    }

                    // 전달 공유부분

                    else if (i + myIndex < before_shardCount)
                    {
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2 + 12];
                        }

                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].myWork = csWork.Work.Repair;
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.spriteName = "work_repair";
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(42 - before_shardCount) + (myIndex + i)]);
                    }

                }
                else if (work.WorkName == csWork.Work.Rest)
                {
                    parent_month.MyDays[myIndex + i].myWork = csWork.Work.Rest;
                    parent_month.MyDays[myIndex + i].icon.spriteName = "work_rest";
                    parent_month.MyDays[myIndex + i].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        parent_month.MyDays[myIndex + i].isLastWork = 1;
                    }

                    manager.changeDay.Add(parent_month.MyDays[myIndex + i]);

                    if (i + myIndex >= (42 - shardCount))
                    {
                        Debug.Log("담달");
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month + 12];
                        }

                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].myWork = csWork.Work.Rest;
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.spriteName = "work_rest";
                        tmp.MyDays[(myIndex + i) - (42 - shardCount)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(myIndex + i) - (42 - shardCount)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(myIndex + i) - (42 - shardCount)]);
                    }

                    // 전달 공유부분

                    else if (i + myIndex < before_shardCount)
                    {
                        csMonth tmp;
                        if (parent_month.dt.Year == 2040)
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2];
                        }
                        else
                        {
                            tmp = manager.MyMonths[parent_month.dt.Month - 2 + 12];
                        }

                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].myWork = csWork.Work.Rest;
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.spriteName = "work_rest";
                        tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].icon.alpha = 0.8f;

                        if (i == (work.r_Time - 1))
                        {
                            tmp.MyDays[(42 - before_shardCount) + (myIndex + i)].isLastWork = 1;
                        }

                        manager.changeDay.Add(tmp.MyDays[(42 - before_shardCount) + (myIndex + i)]);
                    }

                }
                //--------------
            }
            else if ((myIndex + i) >= 42)
            {
                csMonth tmp;
                if (parent_month.dt.Year == 2040)
                {
                    if (parent_month.dt.Month == dt.Month)
                    {
                        if (parent_month.dt.Month == 12)
                        {
                            tmp = manager.MyMonths[dt.Month];
                        }
                        else
                        {
                            tmp = manager.MyMonths[dt.Month];
                        }
                    }
                    else
                    {
                        if (parent_month.dt.Month == 12)
                        {
                            tmp = manager.MyMonths[dt.Month + 11];
                        }
                        else
                        {
                            tmp = manager.MyMonths[dt.Month - 1];
                        }             
                    }
                }
                else
                {
                    if (parent_month.dt.Month == dt.Month)
                    {
                        tmp = manager.MyMonths[dt.Month + 12];
                    }
                    else
                    {
                        tmp = manager.MyMonths[dt.Month - 1 + 12];
                    }
                }

                if (work.WorkName == csWork.Work.Kangnam)
                {
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork = csWork.Work.Kangnam;
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.spriteName = "work_kangnam";
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].isLastWork = 1;
                    }

                    manager.changeDay.Add(tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex]);
                }
                else if (work.WorkName == csWork.Work.Kangbuk)
                {
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork = csWork.Work.Kangbuk;
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.spriteName = "work_kangbuk";
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].isLastWork = 1;
                    }

                    manager.changeDay.Add(tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex]);
                }
                else if (work.WorkName == csWork.Work.Alba)
                {
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork = csWork.Work.Alba;
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.spriteName = "work_alba";
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].isLastWork = 1;
                    }

                    manager.changeDay.Add(tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex]);
                }
                //--------------------
                else if (work.WorkName == csWork.Work.CheonHo)
                {
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork = csWork.Work.CheonHo;
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.spriteName = "work_cheonho";
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].isLastWork = 1;
                    }

                    manager.changeDay.Add(tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex]);
                }
                //-------------------
                else if (work.WorkName == csWork.Work.Labor)
                {
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork = csWork.Work.Labor;
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.spriteName = "work_labor";
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].isLastWork = 1;
                    }

                    manager.changeDay.Add(tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex]);
                }
                //------------------

                else if (work.WorkName == csWork.Work.Plant)
                {
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork = csWork.Work.Plant;
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.spriteName = "work_plant";
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].isLastWork = 1;
                    }

                    manager.changeDay.Add(tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex]);
                }
                //------------------
                else if (work.WorkName == csWork.Work.Repair)
                {
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork = csWork.Work.Repair;
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.spriteName = "work_repair";
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].isLastWork = 1;
                    }

                    manager.changeDay.Add(tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex]);
                }
                else if (work.WorkName == csWork.Work.Rest)
                {
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].myWork = csWork.Work.Rest;
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.spriteName = "work_rest";
                    tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].icon.alpha = 0.8f;

                    if (i == (work.r_Time - 1))
                    {
                        tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex].isLastWork = 1;
                    }

                    manager.changeDay.Add(tmp.MyDays[(42 - (parent_month.endIndex + 1)) + tmp.startIndex + nextMonthIndex]);
                }
                //---------------
                nextMonthIndex++;
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        
    }
    
}
