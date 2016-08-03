using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

public class csSort : MonoBehaviour {


    public csInventory inven;


    void Start () {

    }

    // 종류로 정렬
    public void TypeSortDesc()
    {
        Debug.Log("타입 내림차순");
        //마지막 아이템있는 인덱스 찾기
        int lastIndex = inven.MySlots.Count - 1;
        for (int i = inven.MySlots.Count - 1; i >= 0; i--)
        {
            if (inven.MySlots[i].Items.Count != 0)
            {
                break;
            }
            lastIndex = i - 1;
        }

        int minIndex;
        for (int i = 0; i <inven.MySlots.Count - 1; i++)
        {
            minIndex = i;
            
            for(int j = i + 1; j < inven.MySlots.Count; j++)
            {
                if(inven.MySlots[minIndex].Items.Count == 0)
                {
                    minIndex = j;
                    continue;
                }

                if(inven.MySlots[j].Items.Count == 0)
                {
                    continue;
                }
                else if(inven.MySlots[j].Items.Peek().itemType < inven.MySlots[minIndex].Items.Peek().itemType)
                {
                    minIndex = j;
                }
            }

            Stack<csItem> tmp = new Stack<csItem>(inven.MySlots[i].Items);
            bool dragTmp = inven.MySlots[i].isDragOk;

            inven.MySlots[i].Items = new Stack<csItem>(inven.MySlots[minIndex].Items);
            inven.MySlots[i].isDragOk = inven.MySlots[minIndex].isDragOk;

            inven.MySlots[minIndex].Items = tmp;
            inven.MySlots[minIndex].isDragOk = dragTmp;

            inven.MySlots[i].SortSprite();
            inven.MySlots[minIndex].SortSprite();     
        }


        SqlQurey(lastIndex);
    }


    public void TypeSortAsc()
    {
        Debug.Log("타입 오름차순");
        //마지막 아이템있는 인덱스 찾기
        int lastIndex = inven.MySlots.Count - 1;
        for (int i = inven.MySlots.Count - 1; i >= 0; i--)
        {
            if (inven.MySlots[i].Items.Count != 0)
            {
                break;
            }
            lastIndex = i - 1;
        }

        int minIndex;
        for (int i = 0; i < inven.MySlots.Count - 1; i++)
        {
            minIndex = i;

            for (int j = i + 1; j < inven.MySlots.Count; j++)
            {
                if (inven.MySlots[minIndex].Items.Count == 0)
                {
                    minIndex = j;
                    continue;
                }

                if (inven.MySlots[j].Items.Count == 0)
                {
                    continue;
                }
                else if (inven.MySlots[j].Items.Peek().itemType > inven.MySlots[minIndex].Items.Peek().itemType)
                {
                    minIndex = j;
                }
            }

            Stack<csItem> tmp = new Stack<csItem>(inven.MySlots[i].Items);
            bool dragTmp = inven.MySlots[i].isDragOk;

            inven.MySlots[i].Items = new Stack<csItem>(inven.MySlots[minIndex].Items);
            inven.MySlots[i].isDragOk = inven.MySlots[minIndex].isDragOk;

            inven.MySlots[minIndex].Items = tmp;
            inven.MySlots[minIndex].isDragOk = dragTmp;

            inven.MySlots[i].SortSprite();
            inven.MySlots[minIndex].SortSprite();
        }


        SqlQurey(lastIndex);
    }

    // 등급으로 정렬
    public void RankSortDesc()
    {
        Debug.Log("등급 내림차순");
        //마지막 아이템있는 인덱스 찾기
        int lastIndex = inven.MySlots.Count - 1;
        for (int i = inven.MySlots.Count - 1; i >= 0; i--)
        {
            if (inven.MySlots[i].Items.Count != 0)
            {
                break;
            }
            lastIndex = i - 1;
        }

        int minIndex;
        for (int i = 0; i < inven.MySlots.Count - 1; i++)
        {
            minIndex = i;

            for (int j = i + 1; j < inven.MySlots.Count; j++)
            {
                if (inven.MySlots[minIndex].Items.Count == 0)
                {
                    minIndex = j;
                    continue;
                }

                if (inven.MySlots[j].Items.Count == 0)
                {
                    continue;
                }
                else if (inven.MySlots[j].Items.Peek().rank < inven.MySlots[minIndex].Items.Peek().rank)
                {
                    minIndex = j;
                }
            }

            Stack<csItem> tmp = new Stack<csItem>(inven.MySlots[i].Items);
            bool dragTmp = inven.MySlots[i].isDragOk;

            inven.MySlots[i].Items = new Stack<csItem>(inven.MySlots[minIndex].Items);
            inven.MySlots[i].isDragOk = inven.MySlots[minIndex].isDragOk;

            inven.MySlots[minIndex].Items = tmp;
            inven.MySlots[minIndex].isDragOk = dragTmp;

            inven.MySlots[i].SortSprite();
            inven.MySlots[minIndex].SortSprite();
        }


        SqlQurey(lastIndex);
    }

    public void RankSortAsc()
    {
        Debug.Log("등급 내림차순");
        //마지막 아이템있는 인덱스 찾기
        int lastIndex = inven.MySlots.Count - 1;
        for (int i = inven.MySlots.Count - 1; i >= 0; i--)
        {
            if (inven.MySlots[i].Items.Count != 0)
            {
                break;
            }
            lastIndex = i - 1;
        }

        int minIndex;
        for (int i = 0; i < inven.MySlots.Count - 1; i++)
        {
            minIndex = i;

            for (int j = i + 1; j < inven.MySlots.Count; j++)
            {
                if (inven.MySlots[minIndex].Items.Count == 0)
                {
                    minIndex = j;
                    continue;
                }

                if (inven.MySlots[j].Items.Count == 0)
                {
                    continue;
                }
                else if (inven.MySlots[j].Items.Peek().rank > inven.MySlots[minIndex].Items.Peek().rank)
                {
                    minIndex = j;
                }
            }

            Stack<csItem> tmp = new Stack<csItem>(inven.MySlots[i].Items);
            bool dragTmp = inven.MySlots[i].isDragOk;

            inven.MySlots[i].Items = new Stack<csItem>(inven.MySlots[minIndex].Items);
            inven.MySlots[i].isDragOk = inven.MySlots[minIndex].isDragOk;

            inven.MySlots[minIndex].Items = tmp;
            inven.MySlots[minIndex].isDragOk = dragTmp;

            inven.MySlots[i].SortSprite();
            inven.MySlots[minIndex].SortSprite();
        }


        SqlQurey(lastIndex);
    }



    public void NameSortDesc()
    {
        Debug.Log("이름 내림차순");
        //마지막 아이템있는 인덱스 찾기
        int lastIndex = inven.MySlots.Count - 1;
        for (int i = inven.MySlots.Count - 1; i >= 0; i--)
        {
            if (inven.MySlots[i].Items.Count != 0)
            {
                break;
            }
            lastIndex = i - 1;
        }

        int minIndex;
        for (int i = 0; i < inven.MySlots.Count - 1; i++)
        {
            minIndex = i;

            for (int j = i + 1; j < inven.MySlots.Count; j++)
            {
                if (inven.MySlots[minIndex].Items.Count == 0)
                {
                    minIndex = j;
                    continue;
                }

                if (inven.MySlots[j].Items.Count == 0)
                {
                    continue;
                }
                else if (true)
                {
                    int check = inven.MySlots[j].Items.Peek().itemName.CompareTo(inven.MySlots[minIndex].Items.Peek().itemName);
                    if (check == 1)
                    {
                        minIndex = j;
                    }

                }
            }

            Stack<csItem> tmp = new Stack<csItem>(inven.MySlots[i].Items);
            bool dragTmp = inven.MySlots[i].isDragOk;

            inven.MySlots[i].Items = new Stack<csItem>(inven.MySlots[minIndex].Items);
            inven.MySlots[i].isDragOk = inven.MySlots[minIndex].isDragOk;

            inven.MySlots[minIndex].Items = tmp;
            inven.MySlots[minIndex].isDragOk = dragTmp;

            inven.MySlots[i].SortSprite();
            inven.MySlots[minIndex].SortSprite();
        }


        SqlQurey(lastIndex);
    }

    public void NameSortAsc()
    {
        Debug.Log("이름 오름차순");
        //마지막 아이템있는 인덱스 찾기
        int lastIndex = inven.MySlots.Count - 1;
        for (int i = inven.MySlots.Count - 1; i >= 0; i--)
        {
            if (inven.MySlots[i].Items.Count != 0)
            {
                break;
            }
            lastIndex = i - 1;
        }

        int minIndex;
        for (int i = 0; i < inven.MySlots.Count - 1; i++)
        {
            minIndex = i;

            for (int j = i + 1; j < inven.MySlots.Count; j++)
            {
                if (inven.MySlots[minIndex].Items.Count == 0)
                {
                    minIndex = j;
                    continue;
                }

                if (inven.MySlots[j].Items.Count == 0)
                {
                    continue;
                }
                else if (true)
                {
                    
                   int check = inven.MySlots[minIndex].Items.Peek().itemName.CompareTo(inven.MySlots[j].Items.Peek().itemName);
                    if(check == 1)
                    {
                        minIndex = j;
                    }
                    
                }
            }

            Stack<csItem> tmp = new Stack<csItem>(inven.MySlots[i].Items);
            bool dragTmp = inven.MySlots[i].isDragOk;

            inven.MySlots[i].Items = new Stack<csItem>(inven.MySlots[minIndex].Items);
            inven.MySlots[i].isDragOk = inven.MySlots[minIndex].isDragOk;

            inven.MySlots[minIndex].Items = tmp;
            inven.MySlots[minIndex].isDragOk = dragTmp;

            inven.MySlots[i].SortSprite();
            inven.MySlots[minIndex].SortSprite();
        }


        SqlQurey(lastIndex);
    }

    public void SqlQurey(int lastIndex)
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
        for (int i = 0; i < lastIndex + 1; i++)
        {
            string code = null;
            
            if(inven.MySlots[i].Items.Count != 0)
            {
                code = inven.MySlots[i].Items.Peek().itemCode;
            }

            sqlQuery = "UPDATE Inventory SET \"Code\" = \"" + code + "\", \"Amount\" = " + inven.MySlots[i].Items.Count + " WHERE \"Index\" = " + i + "";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();
        }
        
     
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }
}
