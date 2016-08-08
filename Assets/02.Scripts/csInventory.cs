using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

public class csInventory : MonoBehaviour {

    public GameObject rewardWindow;
    public UILabel rewardSell;

    public GameObject makeFail;

    public csSlotStorage sell;
    public UILabel sellPrice;

    bool makePossible = false;

    public int needMetalCount = 0;
    public int needCircuitCount = 0;
    public int needJunkCount = 0;

    public string metalSpriteName = null;
    public string circuitSpriteName = null;
    public string junkSpriteName = null;

    public int metalIndex = -1;
    public int circuitIndex = -1;
    public int junkIndex = -1;

    public int MyMetalCount = 0;
    public int MyCircuitCount = 0;
    public int MyJunkCount = 0;

    public int blueprintIndex = -1;
    public int makeIndex = -1;

    public GameObject makeCheck;

    public UISprite metalSprite;
    public UISprite circuitSprite;
    public UISprite junkSprite;

    public UILabel metalLabel;
    public UILabel circuitLabel;
    public UILabel junkLabel;

    public UILabel metalNameLabel;
    public UILabel circuitNameLabel;
    public UILabel junkNameLabel;

    public csSeletedItem selectedItem;

    public GameObject PreviewPart;


    public List<csSlotStorage> MySlots;
    public GameObject slotPrefabs;
    public UIGrid m_Grid;
    public int emptySlots;

    public csItemDataBase itemdatabase;


    // Use this for initialization
    void Start () {
    
        MySlots = new List<csSlotStorage>();


        for(int i = 0; i < 28; i++)
        {
            GameObject slot = NGUITools.AddChild(gameObject, slotPrefabs);

            slot.GetComponent<csSlotStorage>().index = i;
            MySlots.Add(slot.GetComponent<csSlotStorage>());
        }
        emptySlots = 28;
        
        m_Grid.Reposition();


        StartCoroutine(MyItemRegister());

        metalSprite = GameObject.Find("metaItemSprite").GetComponent<UISprite>();
        circuitSprite = GameObject.Find("circuitItemSprite").GetComponent<UISprite>();
        junkSprite = GameObject.Find("junkItemSprite").GetComponent<UISprite>();

        metalLabel = GameObject.Find("metalCountLabel").GetComponent<UILabel>();
        circuitLabel = GameObject.Find("circuitCountLabel").GetComponent<UILabel>();
        junkLabel = GameObject.Find("junkCountLabel").GetComponent<UILabel>();

        metalNameLabel = GameObject.Find("metalLabel").GetComponent<UILabel>();
        circuitNameLabel = GameObject.Find("circuitLabel").GetComponent<UILabel>();
        junkNameLabel = GameObject.Find("junkLabel").GetComponent<UILabel>();

        selectedItem = GameObject.Find("SelectedItem").GetComponent<csSeletedItem>();
    }

    public void MaterialCheck(csItem blueprint,int index,int createIndex)
    {
        // 설계도 위치
        blueprintIndex = index;
        makeIndex = createIndex;

        // 어떤 등급의 금속이 필요한지 조사
        if (blueprint.l_metal > 0)
        {
            metalSpriteName = "Material_0011";
            needMetalCount = blueprint.l_metal;
            metalNameLabel.text = "불순물 섞인 금속";
        }
        else if (blueprint.m_metal > 0)
        {
            metalSpriteName = "Material_0012";
            needMetalCount = blueprint.m_metal;
            metalNameLabel.text = "보통 금속";
        }
        else if (blueprint.h_metal > 0)
        {
            metalSpriteName = "Material_0013";
            needMetalCount = blueprint.h_metal;
            metalNameLabel.text = "정제된 금속";
        }

        // 어떤 등급의 회로가 필요한지 조사
        if (blueprint.l_circuit > 0)
        {
            circuitSpriteName = "Material_0021";
            needCircuitCount = blueprint.l_circuit;
            circuitNameLabel.text = "낡은 회로";
        }
        else if (blueprint.m_circuit > 0)
        {
            circuitSpriteName = "Material_0022";
            needCircuitCount = blueprint.m_circuit;
            circuitNameLabel.text = "일반 회로";
        }
        else if (blueprint.h_circuit > 0)
        {
            circuitSpriteName = "Material_0023";
            needCircuitCount = blueprint.h_circuit;
            circuitNameLabel.text = "고급 회로";
        }

        // 어떤 등급의 잡동사니가 필요한지 조사
        if (blueprint.l_junk > 0)
        {
            junkSpriteName = "Material_0031";
            needJunkCount = blueprint.l_junk;
            junkNameLabel.text = "녹슨 잡동사니";
        }
        else if (blueprint.m_junk > 0)
        {
            junkSpriteName = "Material_0032";
            needJunkCount = blueprint.m_junk;
            junkNameLabel.text = "잡동사니";
        }
        else if (blueprint.h_junk > 0)
        {
            junkSpriteName = "Material_0033";
            needJunkCount = blueprint.h_junk;
            junkNameLabel.text = "고급 잡동사니";
        }

        // 각 재료에 맞게 재료창 스프라이트 변경
        metalSprite.spriteName = metalSpriteName;
        circuitSprite.spriteName = circuitSpriteName;
        junkSprite.spriteName = junkSpriteName;

        for (int i = 0; i < 28; i++)
        {
            // 비어있지 않은곳만 조사
            if (MySlots[i].Items.Count != 0)
            {
                // 들어있는게 금속인지 조사
                if (MySlots[i].Items.Peek().itemCode == metalSpriteName)
                {
                    // 재료 위치 기억
                    if (metalIndex == -1)
                    {
                        metalIndex = i;
                    }
                    MyMetalCount = MyMetalCount + MySlots[i].Items.Count;

                }
                // 들어있는게 회로인지 조사
                else if (MySlots[i].Items.Peek().itemCode == circuitSpriteName)
                {
                    if (circuitIndex == -1)
                    {
                        circuitIndex = i;
                    }
                    MyCircuitCount = MyCircuitCount + MySlots[i].Items.Count;
                }
                // 들어있는게 잡동사니인지 조사
                else if (MySlots[i].Items.Peek().itemCode == junkSpriteName)
                {
                    if (junkIndex == -1)
                    {
                        junkIndex = i;
                    }
                    MyJunkCount = MyJunkCount + MySlots[i].Items.Count;
                }
            }
            else
            {
               // Debug.Log("비어있는곳");
            }
        }

        bool isMake = true;

        // 3가지 재료중 하나라도 안되면 제작 불가능
        if (MyMetalCount < needMetalCount)
        {
            isMake = false;
            metalSprite.alpha = 0.5f;
            metalLabel.color = Color.red;
        }
        if (MyCircuitCount < needCircuitCount)
        {
            isMake = false;
            circuitSprite.alpha = 0.5f;
            circuitLabel.color = Color.red;
        }

        if (MyJunkCount < needJunkCount)
        {
            isMake = false;
            junkSprite.alpha = 0.5f;
            junkLabel.color = Color.red;
        }

        if(isMake)
        {
            makePossible = true;
        }
        metalLabel.text = "" + MyMetalCount + "  /  " + needMetalCount;
        circuitLabel.text = "" + MyCircuitCount + "  /  " + needCircuitCount;
        junkLabel.text = "" + MyJunkCount + "  /  " + needJunkCount;

    }

    public void MakePopupTrue()
    {
        if(makePossible)
        {
           PreviewPart.GetComponent<TweenPosition>().PlayReverse();
           makeCheck.SetActive(true);

        }
        else
        {
            makeFail.SetActive(true);
            Debug.Log("만들수없다");
        }           
    }

    public void PopupFalse()
    {
        PreviewPart.GetComponent<TweenPosition>().PlayForward();
    }

    public void Buy()
    {

        // 돈이 있어야 가능

        if (UserManager.Instance().myGold >= 2000)
        {
            Debug.Log("샀다");
            UserManager.Instance().myGold -= 2000;
            UserManager.Instance().DateLabelUpdate();

            int random = UnityEngine.Random.Range(1, 101);
            if (random <= 2)
            {
                int sixRandom = UnityEngine.Random.Range(0, itemdatabase.RankSix.Count);
                AddItem(itemdatabase.RankSix[sixRandom], 1);
            }
            else if (2 < random && random <= 5)
            {
                int fiveRandom = UnityEngine.Random.Range(0, itemdatabase.RankFive.Count);
                AddItem(itemdatabase.RankFive[fiveRandom], 1);
            }
            else if (5 < random && random <= 15)
            {
                int fourRandom = UnityEngine.Random.Range(0, itemdatabase.RankFour.Count);
                AddItem(itemdatabase.RankFour[fourRandom], 1);
            }
            else if (15 < random && random <= 35)
            {
                int threeRandom = UnityEngine.Random.Range(0, itemdatabase.RankThree.Count);
                AddItem(itemdatabase.RankThree[threeRandom], 1);
            }
            else if (35 < random && random <= 65)
            {
                int twoRandom = UnityEngine.Random.Range(0, itemdatabase.RankTwo.Count);
                AddItem(itemdatabase.RankTwo[twoRandom], 1);
            }
            else if (65 < random && random <= 100)
            {
                int oneRandom = UnityEngine.Random.Range(0, itemdatabase.RankOne.Count);
                AddItem(itemdatabase.RankOne[oneRandom], 1);

            }
        }
        else
        {
            Debug.Log("돈부족하다");
        }

    }

    public void ItemSell()
    {      
        UserManager.Instance().myGold += int.Parse(sellPrice.text);
        if (int.Parse(sellPrice.text) > 0)
        {
            
            rewardSell.text = sellPrice.text;
            UserManager.Instance().DateLabelUpdate();
            sellPrice.text = "0";
            sell.ClearSlot();
            sellSql();
            openReward();
        }
    }

    public void sellClear()
    {

        int count = sell.Items.Count;
        if (count != 0)
        {
            csItem item = sell.Items.Peek();
            AddItem(item, count);
            sell.ClearSlot();

            SellSqlQurey(4, "0", 0);
        }
        sellPrice.text = "0";
    }
    public void PartMake()
    {
        // 리더보드 갱신
        PlayerPrefs.SetInt("MixCount", PlayerPrefs.GetInt("MixCount") + 1);
        UserManager.Instance().Mix_LeaderBoard();
        
        // 재료 메탈
        for (int i = 0; i < needMetalCount; i++)
        {
            MySlots[metalIndex].Items.Pop();
        }
        // 수량 조절
        if (MySlots[metalIndex].Items.Count > 1)
        {
           MySlots[metalIndex].itemAmount.text = MySlots[metalIndex].Items.Count.ToString();
        }
        else
        {
            MySlots[metalIndex].itemAmount.text = string.Empty;
        }

        if(MySlots[metalIndex].Items.Count == 0)
        {
            MySlots[metalIndex].ChangeSprite(MySlots[metalIndex].defaultAtlas, "");
        }

        // 재료 회로
        for (int i = 0; i < needCircuitCount; i++)
        {
            MySlots[circuitIndex].Items.Pop();
        }
        // 수량 조절
        if (MySlots[circuitIndex].Items.Count > 1)
        {
            MySlots[circuitIndex].itemAmount.text = MySlots[circuitIndex].Items.Count.ToString();
        }
        else
        {
            MySlots[circuitIndex].itemAmount.text = string.Empty;
        }

        if (MySlots[circuitIndex].Items.Count == 0)
        {
            MySlots[circuitIndex].ChangeSprite(MySlots[circuitIndex].defaultAtlas, "");
        }

        // 재료 잡동사니
        for (int i = 0; i < needJunkCount; i++)
        {
            MySlots[junkIndex].Items.Pop();
        }
        // 수량 조절
        if (MySlots[junkIndex].Items.Count > 1)
        {
            MySlots[junkIndex].itemAmount.text = MySlots[junkIndex].Items.Count.ToString();
        }
        else
        {
            MySlots[junkIndex].itemAmount.text = string.Empty;
        }

        if (MySlots[junkIndex].Items.Count == 0)
        {
            MySlots[junkIndex].ChangeSprite(MySlots[junkIndex].defaultAtlas, "");
        }


        // 설계도 감소시키기
        MySlots[blueprintIndex].Items.Pop();

        if(MySlots[blueprintIndex].Items.Count > 1)
        {
            MySlots[blueprintIndex].itemAmount.text = MySlots[blueprintIndex].Items.Count.ToString();
        }
        else
        {
            MySlots[blueprintIndex].itemAmount.text = string.Empty;
        }

        if (MySlots[blueprintIndex].Items.Count == 0)
        {
            MySlots[blueprintIndex].ChangeSprite(MySlots[blueprintIndex].defaultAtlas, "");
        }


        // 제작한 아이템 추가
        Debug.Log("make" + makeIndex);
        AddItem(csItemDataBase.Instance().Database[makeIndex], 1);

        // 인벤 디비 갱신
        WholeSqlQurey();

        // 초기화
        ClearMakeWindow();
    }

    public void ClearMakeWindow()
    {
        selectedItem.ChangeSprite("", "", Color.white,"");

        // 미리보기 삭제
        GameObject parent = null;
        parent = GameObject.Find("PreviewPart");

        Transform[] SpawnObject;
        SpawnObject = parent.GetComponentsInChildren<Transform>();

        for (int i = 1; i < SpawnObject.Length; i++)
        {
            Destroy(SpawnObject[i].gameObject);
        }


        blueprintIndex = -1;
        makeIndex = -1;
        makePossible = false;
        // 필요한 재료 갯수
        needMetalCount = 0;
        needCircuitCount = 0;
        needJunkCount = 0;

        // 해당하는 재료의 이미지이름
        metalSpriteName = null;
        circuitSpriteName = null;
        junkSpriteName = null;

        // 인벤 조사한 후 해당 재료의 위치
        metalIndex = -1;
        circuitIndex = -1;
        junkIndex = -1;

        // 내가 가지고있는 재료의 수
        MyMetalCount = 0;
        MyCircuitCount = 0;
        MyJunkCount = 0;

        metalLabel.color = Color.blue;
        circuitLabel.color = Color.blue;
        junkLabel.color = Color.blue;
        metalSprite.alpha = 1.0f;
        circuitSprite.alpha = 1.0f;
        junkSprite.alpha = 1.0f;
        metalLabel.text = "";
        circuitLabel.text = "";
        junkLabel.text = "";
        metalSprite.spriteName = string.Empty;
        circuitSprite.spriteName = string.Empty;
        junkSprite.spriteName = string.Empty;
        metalNameLabel.text = "금속";
        circuitNameLabel.text = "회로";
        junkNameLabel.text = "잡동사니";
    }

    IEnumerator MyItemRegister()
    {
        yield return new WaitForSeconds(0.05f);

        csAddItem.Instance().AddItem();
        csAddItem.Instance().AddEquip();

    }
    public bool AddItem(csItem item, int amount)
    {
        if (item.itemMaxAmount == 1)
        {
            
            PlaceEmpty(item, 1);
            return true;
        }
        else
        {
            for (int i = 0; i < MySlots.Count; i++)
            {
                csSlotStorage tmp = MySlots[i];
                
                if (!tmp.IsEmpty)
                {
                    
                    if (tmp.CurrentItem.itemID == item.itemID && tmp.IsAvailable)
                    {
                        for (int j = 0; j < amount && tmp.IsAvailable; j++)
                        {
                            tmp.AddItem(item);
                            
                        }
                        SqlQurey(i, tmp.Items.Peek().itemCode, tmp.Items.Count);
                        return true;
                    }
                }
            }

            Debug.Log(emptySlots);
            // 슬롯에 맞는 아이템이 없는경우
            if (emptySlots > 0)
            {
                Debug.Log("슬롯에 맞는게 없다");
                PlaceEmpty(item, amount);
                return true;
            }

        }
        return false;
    }

    private bool PlaceEmpty(csItem item, int amount)
    {
        
        if (emptySlots > 0)
        {
            Debug.Log("빈공간잇음");
            for (int i = 0; i < MySlots.Count; i++)
            {
                csSlotStorage tmp = MySlots[i];

                if (tmp.IsEmpty)
                {
                    Debug.Log("빈공간 인덱스 : " + i);
                    tmp.AddItem(item);

                    for (int j = 0; j < amount - 1 && tmp.IsAvailable; j++)
                    {
                        tmp.AddItem(item);

                    }
                    
                    emptySlots--;
                    Debug.Log("빈공간 : " + emptySlots);

                    SqlQurey(i, tmp.Items.Peek().itemCode, tmp.Items.Count);
                    return true;           
                }
            }
        }
        return false;
    }

    public void Assembly(csItem item, int type)
    {
        
        GameObject parent = null;
        Transform[] SpawnObject;

        if (item.itemType == csItem.ItemType.Head)
        {
            parent = GameObject.Find("Head_Point");
        }
        else if (item.itemType == csItem.ItemType.Tail)
        {
            parent = GameObject.Find("Tail_Point");
        }
        else if (item.itemType == csItem.ItemType.Front_Leg)
        {
            if (item.legCount == 2)
            {
                parent = GameObject.Find("MiddleLeg_Point");
            }
            else {
                parent = GameObject.Find("FrontLeg_Point");
            }

        }
        else if (item.itemType == csItem.ItemType.Back_Leg)
        {
            parent = GameObject.Find("BackLeg_Point");
        }

        SpawnObject = parent.GetComponentsInChildren<Transform>();

        for (int i = 1; i < SpawnObject.Length; i++)
        {
            Destroy(SpawnObject[i].gameObject);
        }
        
        // 새로운 장비가 장착 됐을때만
        if (type == 0)
        {
            string name = item.itemCode;
            string removeName = name.Remove(name.Length - 1, 1);
            string insertName = removeName.Insert(removeName.Length, "1");
            string lastPartName = "PartPrefabs/" + insertName;

            GameObject part = Resources.Load(lastPartName, typeof(GameObject)) as GameObject;
            
            GameObject goTemp = Instantiate(part) as GameObject;

            Vector3 tmp = goTemp.transform.position;
            Quaternion temp = goTemp.transform.rotation;


            goTemp.transform.parent = parent.transform;
            goTemp.transform.localPosition = tmp;
            goTemp.transform.localRotation = temp;
        }
        
    }

    public void SqlQurey(int Index, string ItemCode, int ItemCount)
    {
      //  Debug.Log("저장하냐");
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

            sqlQuery = "UPDATE Inventory SET \"Code\" = \"" + ItemCode + "\", \"Amount\" = " + ItemCount + " WHERE \"Index\" = " + Index + "";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

    public void openReward()
    {
        rewardWindow.SetActive(true);
    }
    public void closeReward()
    {
        rewardWindow.SetActive(false);
        rewardSell.text = "0";
    }

    public void SellSqlQurey(int Index, string ItemCode, int ItemCount)
    {
        //  Debug.Log("저장하냐");
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

        sqlQuery = "UPDATE Equipement SET \"Code\" = \"" + ItemCode + "\", \"Amount\" = " + ItemCount + " WHERE \"Index\" = " + Index + "";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }


    public void WholeSqlQurey()
    {
        //  Debug.Log("저장하냐");
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
        for (int i = 0; i < 28; i++)
        {
            string code = null;

            if (MySlots[i].Items.Count != 0)
            {
                code = MySlots[i].Items.Peek().itemCode;
            }

            sqlQuery = "UPDATE Inventory SET \"Code\" = \"" + code + "\", \"Amount\" = " + MySlots[i].Items.Count + " WHERE \"Index\" = " + i + "";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();
        }

        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void sellSql()
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

        string str = "0";
        int zero = 0;
        int index = 4;
        sqlQuery = "UPDATE Equipement SET \"Code\" = \"" + str + "\", \"Amount\" = " + zero + " WHERE \"Index\" = " + index + "";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
  
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
