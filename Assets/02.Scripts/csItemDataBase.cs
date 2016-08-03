using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

public class csItemDataBase : MonoBehaviour {

    static csItemDataBase _instance = null;

    public List<csItem> Database;
    public List<myItem> inven;
    public List<myItem> equip;

    public List<csItem> PartDB;
    public List<csItem> BlueprintDB;
    public List<csItem> MaterialDB;

    public List<csItem> RankSix;
    public List<csItem> RankFive;
    public List<csItem> RankFour;
    public List<csItem> RankThree;
    public List<csItem> RankTwo;
    public List<csItem> RankOne;

    public struct myItem
    {
        public string code;
        public int amount;
    }

   

    public static csItemDataBase Instance()
    {
        return _instance;
    }
    
    // Use this for initialization
    void Start () {

        if (_instance == null)
            _instance = this;
        // 아이템정보
        Database = new List<csItem>();
        
        // 인벤토리 정보
        inven = new List<myItem>();

        // 장비 정보
        equip = new List<myItem>();

        PartDB = new List<csItem>();
        BlueprintDB = new List<csItem>();
        MaterialDB = new List<csItem>();

        RankSix = new List<csItem>();
        RankFive = new List<csItem>();
        RankFour = new List<csItem>();
        RankThree = new List<csItem>();
        RankTwo = new List<csItem>();
        RankOne = new List<csItem>();


        SqlQurey(0);
        SqlQurey(1);
        SqlQurey(2);
        SqlQurey(3);
        SqlQurey(4);
    }


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

        int count = 0;

        if (type == 0)
        {
            //셀렉트
            sqlQuery = "SELECT * " + "FROM Item";
            dbcmd.CommandText = sqlQuery;

            IDataReader reader = dbcmd.ExecuteReader();

            
            while (reader.Read())
            {
                csItem tmp = new csItem();

                tmp.itemCode = reader.GetString(0);
                tmp.itemType = (csItem.ItemType)reader.GetInt32(1);
                tmp.itemName = reader.GetString(2);
                tmp.rank = reader.GetInt32(3);
                tmp.legCount = reader.GetInt32(4);
                tmp.power = reader.GetInt32(5);
                tmp.balance = reader.GetInt32(6);
                tmp.max_speed = reader.GetInt32(7);
                tmp.hp = reader.GetInt32(8);
                tmp.sec_heal = reader.GetInt32(9);
                tmp.jumpPower = reader.GetInt32(10);
                tmp.cliberPower = reader.GetInt32(11);
                tmp.blueprint = reader.GetString(12);
                tmp.buy_Price = reader.GetInt32(13);
                tmp.sell_Price = reader.GetInt32(14);
                tmp.etc = reader.GetString(15);
                tmp.ability = reader.GetInt32(16);
                tmp.itemMaxAmount = 1;
                tmp.itemSpriteName = tmp.itemCode;

                tmp.itemID = count;
                count++;

                tmp.initialization();
                Database.Add(tmp);
                PartDB.Add(tmp);

                switch(tmp.rank)
                {
                    case 6:
                        RankSix.Add(tmp);
                        break;
                    case 5:
                        RankFive.Add(tmp);
                        break;
                    case 4:
                        RankFour.Add(tmp);
                        break;
                    case 3:
                        RankThree.Add(tmp);
                        break;
                    case 2:
                        RankTwo.Add(tmp);
                        break;                   
                    case 1:
                        RankOne.Add(tmp);
                        break;
                }

            }
            reader.Close();
            reader = null;
        }

        // 인벤읽어오기
        if (type == 1)
        {
            sqlQuery = "SELECT \"Code\", \"Amount\"" + "FROM Inventory";
            dbcmd.CommandText = sqlQuery;

            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                myItem tmp;
                tmp.code = reader.GetString(0);

                tmp.amount = reader.GetInt32(1);


                inven.Add(tmp);
            }
            reader.Close();
            reader = null;
        }

        // 장비읽어오기
        if (type == 2)
        {
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
        }
        
        // 설계도 읽어오기
        if (type == 3)
        { 
            //셀렉트
            sqlQuery = "SELECT * " + "FROM BluePrint";
            dbcmd.CommandText = sqlQuery;

            IDataReader reader = dbcmd.ExecuteReader();


            while (reader.Read())
            {
                csItem tmp = new csItem();

                tmp.itemCode = reader.GetString(0);
                tmp.itemName = reader.GetString(1);
                tmp.rank = reader.GetInt32(2);
                tmp.itemType = (csItem.ItemType)reader.GetInt32(3);

                tmp.l_metal = reader.GetInt32(4);
                tmp.m_metal = reader.GetInt32(5);
                tmp.h_metal = reader.GetInt32(6);

                tmp.l_circuit = reader.GetInt32(7);
                tmp.m_circuit = reader.GetInt32(8);
                tmp.h_circuit = reader.GetInt32(9);

                tmp.l_junk = reader.GetInt32(10);
                tmp.m_junk = reader.GetInt32(11);
                tmp.h_junk = reader.GetInt32(12);

                tmp.sell_Price = reader.GetInt32(13);
                tmp.etc = reader.GetString(14);

                tmp.itemSpriteName = tmp.itemCode;
                tmp.itemMaxAmount = 999;
                tmp.itemID = count;
                count++;
                tmp.initialization();
                Database.Add(tmp);
                BlueprintDB.Add(tmp);

                switch (tmp.rank)
                {
                    case 6:
                        RankSix.Add(tmp);
                        break;
                    case 5:
                        RankFive.Add(tmp);
                        break;
                    case 4:
                        RankFour.Add(tmp);
                        break;
                    case 3:
                        RankThree.Add(tmp);
                        break;
                    case 2:
                        RankTwo.Add(tmp);
                        break;
                    case 1:
                        RankOne.Add(tmp);
                        break;
                }

            }
            reader.Close();
            reader = null;
        }
        // 재료 읽어오기
        if (type == 4)
        {
            //셀렉트
            sqlQuery = "SELECT * " + "FROM Material";
            dbcmd.CommandText = sqlQuery;

            IDataReader reader = dbcmd.ExecuteReader();


            while (reader.Read())
            {
                csItem tmp = new csItem();

                tmp.itemCode = reader.GetString(0);
                tmp.itemName = reader.GetString(1);
                tmp.rank = reader.GetInt32(2);
                tmp.itemType = (csItem.ItemType)reader.GetInt32(3);
                tmp.sell_Price = reader.GetInt32(4);
                tmp.subType = (csItem.SubType)reader.GetInt32(5);
                tmp.etc = reader.GetString(6);

                tmp.itemSpriteName = tmp.itemCode;
                tmp.itemMaxAmount = 999;
                tmp.itemID = count;
                count++;
                tmp.initialization();
                Database.Add(tmp);
                MaterialDB.Add(tmp);

            }
            reader.Close();
            reader = null;
        }


        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    
    }
  

}
