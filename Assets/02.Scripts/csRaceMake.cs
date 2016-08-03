using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

public class csRaceMake : MonoBehaviour {

    public struct myItem
    {
        public string code;
        public int amount;
    }

    public GameObject MyPlayer;

    public List<myItem> equip;
    // Use this for initialization
    void Start () {
        equip = new List<myItem>();
        SqlQurey();

        AddEquip();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddEquip()
    {
        for (int i = 0; i < 4; i++)
        {

            csRaceMake.myItem tmp = equip[i];

            for (int j = 0; j < csItemDataBase.Instance().PartDB.Count; j++)
            {
                if (tmp.code == csItemDataBase.Instance().PartDB[j].itemCode)
                {

                    // 머리
                    if (i == 0)
                    {                      
                        PreloadAssembly(csItemDataBase.Instance().PartDB[j]);
                    }
                    // 꼬리
                    else if (i == 1)
                    {                      
                        PreloadAssembly(csItemDataBase.Instance().PartDB[j]);
                    }
                    // 앞다리
                    else if (i == 2)
                    {
                        
                        PreloadAssembly(csItemDataBase.Instance().PartDB[j]);
                    }
                    // 뒷다리
                    else if (i == 3)
                    {
                        
                        PreloadAssembly(csItemDataBase.Instance().PartDB[j]);
                    }               
                    break;
                }
            }
        }
    }


    public void PreloadAssembly(csItem item)
    {
        GameObject parent = null;

        if (item.itemType == csItem.ItemType.Head)
        {
            parent = MyPlayer.transform.FindChild("Head_Point").gameObject;
            Debug.Log("케케");
        }
        else if (item.itemType == csItem.ItemType.Tail)
        {
            
            parent = MyPlayer.transform.FindChild("Tail_Point").gameObject;
        }
        else if (item.itemType == csItem.ItemType.Front_Leg)
        {
            if (item.legCount == 2)
            {
                
                parent = MyPlayer.transform.FindChild("MiddleLeg_Point").gameObject;
            }
            else {
                
                parent = MyPlayer.transform.FindChild("FrontLeg_Point").gameObject;
            }
        }
        else if (item.itemType == csItem.ItemType.Back_Leg)
        {         
            parent = MyPlayer.transform.FindChild("BackLeg_Point").gameObject;
        }

        string name = item.itemCode;
        string removeName = name.Remove(name.Length - 1, 1);
        string insertName = removeName.Insert(removeName.Length, "1");

        string lastPartName = "PartPrefabs/" + insertName;


        GameObject part = Resources.Load(lastPartName, typeof(GameObject)) as GameObject;
        
        GameObject goTemp = Instantiate(part) as GameObject;
        goTemp.name = insertName;
        Vector3 tmp = goTemp.transform.position;
        Quaternion temp = goTemp.transform.rotation;
        Vector3 sale = goTemp.transform.localScale;
        goTemp.transform.parent = parent.transform;
        goTemp.transform.localPosition = tmp;
        goTemp.transform.localRotation = temp;
        goTemp.transform.localScale = sale;
        
    }




    public void SqlQurey()
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
        // 아이템정보 읽어오기
        if (Application.platform == RuntimePlatform.Android)
        {
            conn = "URI=file:" + Application.persistentDataPath + "/raceDB.sqlite"; //Path to databse on Android
        }
        else { conn = "URI=file:" + Application.streamingAssetsPath + "/raceDB.sqlite"; }

        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery;

            sqlQuery = "SELECT \"Code\", \"Amount\"" + "FROM Equipement";
            dbcmd.CommandText = sqlQuery;

            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                myItem tmp;
                tmp.code = reader.GetString(0);
                tmp.amount = reader.GetInt32(1);
                equip.Add(tmp);
            }
            reader.Close();
            reader = null;

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
