using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

public class CalenderManager : MonoBehaviour {

    // Use this for initialization

    public int presentPage = 0;

    public List<csDay> changeDay;
    
    public GameObject monthPrefabs;

    public List<csMonth> MyMonths;
    
    public UIGrid m_Grid;

    public DateTime date;

    public static int space = 0;

    int[,] unDoSchedule;

    int[,] unDoLastWork;

    void Awake()
    {
     //   DontDestroyOnLoad(gameObject);

        date = new DateTime(2040, 1 , 1);
        MyMonths = new List<csMonth>();
        
        unDoSchedule = new int[24, 42];
        unDoLastWork = new int[24, 42];

        for (int i = 0; i < 24; i++)
        {
            GameObject month = NGUITools.AddChild(gameObject, monthPrefabs);
            month.GetComponentInChildren<csMonth>().dt = date;
          //  Debug.Log(date);
            date =  date.AddMonths(1);

            MyMonths.Add(month.GetComponentInChildren<csMonth>());
        }

        m_Grid.Reposition();

        SqlQurey(0);
        
    }

    void OnEnable()
    {
        if(UserManager.Instance().nowDate.Year == 2040)
        {
            StartCoroutine(PageInit(UserManager.Instance().nowDate.Month - 1));
        }
        else
        {
            StartCoroutine(PageInit(UserManager.Instance().nowDate.Month - 1 + 12));
        }
    }

    void Start()
    {
        StartCoroutine(backUp());      
    }

    IEnumerator backUp()
    {
        yield return new WaitForSeconds(0.1f);
        initialization();
    }
    
    public void ScheduleDecide()
    {
        for (int i = 0; i < 24; i++)
        {
            csMonth tmpMonth = MyMonths[i];
            for (int j = 0; j < 42; j++)
            {
                csDay tmpDay = tmpMonth.MyDays[j];
                unDoSchedule[i, j] = (int)tmpDay.myWork;
                unDoLastWork[i, j] = tmpDay.isLastWork;
            }
        }
        // 업데이트문
        UserManager.Instance().WorkUpdate();
        SqlQurey(1);
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void initialization()
    {
        for (int i = 0; i < 24; i++)
        {
            csMonth tmpMonth = MyMonths[i];
            for (int j = 0; j < 42; j++)
            {
                csDay tmpDay = tmpMonth.MyDays[j];

                tmpDay.myWork = (csWork.Work)unDoSchedule[i, j];
                tmpDay.isLastWork = unDoLastWork[i, j];

                switch (tmpDay.myWork)
                {
                    case csWork.Work.Nowork:
                        tmpDay.icon.spriteName = string.Empty;
                        break;
                    case csWork.Work.Kangnam:
                        tmpDay.icon.spriteName = "work_kangnam";
                        tmpDay.icon.alpha = 0.8f;
                        break;
                    case csWork.Work.Kangbuk:
                        tmpDay.icon.spriteName = "work_kangbuk";
                        tmpDay.icon.alpha = 0.8f;
                        break;
                    case csWork.Work.Alba:
                        tmpDay.icon.spriteName = "work_alba";
                        tmpDay.icon.alpha = 0.8f;
                        break;
                    case csWork.Work.CheonHo:
                        tmpDay.icon.spriteName = "work_cheonho";
                        tmpDay.icon.alpha = 0.8f;
                        break;
                    case csWork.Work.Labor:
                        tmpDay.icon.spriteName = "work_labor";
                        tmpDay.icon.alpha = 0.8f;
                        break;
                    case csWork.Work.Plant:
                        tmpDay.icon.spriteName = "work_plant";
                        tmpDay.icon.alpha = 0.8f;
                        break;
                    case csWork.Work.Repair:
                        tmpDay.icon.spriteName = "work_repair";
                        tmpDay.icon.alpha = 0.8f;
                        break;
                    case csWork.Work.Rest:
                        tmpDay.icon.spriteName = "work_rest";
                        tmpDay.icon.alpha = 0.8f;
                        break;
                    case csWork.Work.Race:
                        tmpDay.icon.spriteName = "RaceFlag";
                        tmpDay.icon.alpha = 0.8f;
                        break;
                    default:
                        break;
                }
            }
        }
        changeDay.Clear();
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ScheduleCancle()
    {
        for (int k = 0; k < changeDay.Count; k++)
        {
            csDay tmp = changeDay[k];

            int index = tmp.index;

            int i = index / 42;
            int j = index % 42;

            tmp.myWork = (csWork.Work) unDoSchedule[i, j];
            tmp.isLastWork = unDoLastWork[i, j];

            tmp.icon.spriteName = string.Empty;
            tmp.icon.alpha = 1.0f;
            /*
            switch (tmp.myWork)
            {
                case csWork.Work.Nowork:
                    tmp.sprite.color = Color.white;
                    break;
                case csWork.Work.Kangnam:
                    tmp.sprite.color = Color.blue;
                    break;
                case csWork.Work.Kangbuk:
                    tmp.sprite.color = Color.red;
                    break;
                case csWork.Work.Alba:
                    tmp.sprite.color = Color.yellow;
                    break;
                case csWork.Work.CheonHo:
                    tmp.sprite.color = Color.yellow;
                    break;
                case csWork.Work.Labor:
                    tmp.sprite.color = Color.yellow;
                    break;
                case csWork.Work.Plant:
                    tmp.sprite.color = Color.yellow;
                    break;
                case csWork.Work.Repair:
                    tmp.sprite.color = Color.yellow;
                    break;
                case csWork.Work.Rest:
                    tmp.sprite.color = Color.yellow;
                    break;
                default:
                    break;
            }
            */
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        changeDay.Clear();
    }

    // 질의함수
    public void SqlQurey(int type)
    {
        string m_ConnectionString;
        string m_SQLiteFileName = "raceDB.sqlite";
        string conn;
#if UNITY_EDITOR
        m_ConnectionString = "URI=file:" + Application.streamingAssetsPath + "/" + m_SQLiteFileName;
        //m_ConnectionString = "URI=file:" + Application.dataPath + "/" + m_SQLiteFileName;
#else
            // check if file exists in Application.persistentDataPath
            var filepath = string.Format("{0}/{1}", Application.persistentDataPath, m_SQLiteFileName);

            if (!File.Exists(filepath))
            {
                // if it doesn't ->
                // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
                WWW loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + m_SQLiteFileName);  // this is the path to your StreamingAssets in android
                loadDb.bytesDownloaded.ToString();
                while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                // then save to Application.persistentDataPath
                File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                     var loadDb = Application.dataPath + "/Raw/" + m_SQLiteFileName;  // this is the path to your StreamingAssets in iOS
                    // then save to Application.persistentDataPath
                    File.Copy(loadDb, filepath);
#elif UNITY_WP8
                    var loadDb = Application.dataPath + "/StreamingAssets/" + m_SQLiteFileName;  // this is the path to your StreamingAssets in iOS
                    // then save to Application.persistentDataPath
                    File.Copy(loadDb, filepath);
#elif UNITY_WINRT
      var loadDb = Application.dataPath + "/StreamingAssets/" + m_SQLiteFileName;  // this is the path to your StreamingAssets in iOS
      // then save to Application.persistentDataPath
      File.Copy(loadDb, filepath);
#else
     var loadDb = Application.dataPath + "/StreamingAssets/" + m_SQLiteFileName;  // this is the path to your StreamingAssets in iOS
     // then save to Application.persistentDataPath
     File.Copy(loadDb, filepath);

#endif
            }

            m_ConnectionString = "URI=file:" + filepath;
#endif

        ///////////////////////////////////////////////////////////////////[DB Path]
        if (Application.platform == RuntimePlatform.Android)
        {
            conn = "URI=file:" + Application.persistentDataPath + "/raceDB.sqlite"; //Path to databse on Android
        }
        else { conn = "URI=file:" + Application.streamingAssetsPath + "/raceDB.sqlite"; } //Path to database Else
                                                                                                ///////////////////////////////////////////////////////////////////[DB Path]

        ///////////////////////////////////////////////////////////////////[DB Connection]
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        ///////////////////////////////////////////////////////////////////[DB Connection]


        ///////////////////////////////////////////////////////////////////[DB Query]
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery;

        //셀렉트
        if (type == 0) {

            sqlQuery = "SELECT * " + "FROM Schedule";
            dbcmd.CommandText = sqlQuery;

            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                int index = reader.GetInt32(0);
                int work = reader.GetInt32(1);              
                int lastWork = reader.GetInt32(2);
                int i = index / 42;
                int j = index % 42;
                
                unDoSchedule[i, j] = work;
                unDoLastWork[i, j] = lastWork;

            }

            reader.Close();
            reader = null;
        }
        // 업데이트
        else
        {          
            for(int k= 0; k < changeDay.Count; k++)
            {
                csDay tmp = changeDay[k];

                int index = tmp.index;
                int i = index / 42;
                int j = index % 42;

                sqlQuery = "UPDATE Schedule SET \"Work\" = " + unDoSchedule[i, j] + ", \"IsLast\" = "+ unDoLastWork[i, j] + " WHERE \"Index\" = " + index + "";
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
            changeDay.Clear();
        }

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
   
    }

    public void nextPage()
    {      
        if (presentPage < 23)
        {
            GetComponent<UICenterOnChild>().CenterOn(MyMonths[presentPage + 1].GetComponent<Transform>());
            presentPage++;
        }
        else
        {
            GetComponent<UICenterOnChild>().CenterOn(MyMonths[0].GetComponent<Transform>());
            presentPage = 0;
        }
    }

    public void backPage()
    {
        if (presentPage > 0)
        {
            GetComponent<UICenterOnChild>().CenterOn(MyMonths[presentPage - 1].GetComponent<Transform>());
            presentPage--;
        }
        else
        {
            GetComponent<UICenterOnChild>().CenterOn(MyMonths[23].GetComponent<Transform>());
            presentPage = 23;
        }
    }

    IEnumerator PageInit(int index)
    {
        yield return new WaitForEndOfFrame();
        GetComponent<UICenterOnChild>().CenterOn(MyMonths[index].GetComponent<Transform>());
        presentPage = index;
    }
}
