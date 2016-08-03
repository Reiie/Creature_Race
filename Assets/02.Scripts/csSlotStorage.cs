using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;



public class csSlotStorage : MonoBehaviour {



    public GameObject shop;
    public GameObject Factory;
    public csSlotStorage Front_Leg;
    public csSlotStorage Back_Leg;
    public UILabel sellPrice;

    public int index;

    public csItem.ItemType slotType;

    public csInventory manager;

    public csSeletedItem selectedItem;

    private Stack<csItem> items;

    public Stack<csItem> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    public UILabel itemAmount;

    public UISprite sprite;

    public UISprite rankSprite;

    public UIAtlas defaultAtlas;


    private UIRoot mRoot;

    public string defaultSpriteName;

    public bool IsEmpty
    {
        get
        {
            return items.Count == 0;
        }
    }

    public csItem CurrentItem
    {
        get
        {
            try
            {
                return items.Peek();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }
        }
    }

    public bool IsAvailable
    {
        get
        {
            return CurrentItem.itemMaxAmount > items.Count;
        }
    }


    private GameObject clone;

    public bool isDragOk;

    void Start()
    {
       
        items = new Stack<csItem>();

        mRoot = NGUITools.FindInParents<UIRoot>(transform.parent);

        manager = GameObject.Find("Backpack").GetComponent<csInventory>();

        Front_Leg = GameObject.Find("FrontLeg").GetComponent<csSlotStorage>();

        Back_Leg = GameObject.Find("BackLeg").GetComponent<csSlotStorage>();

        selectedItem = GameObject.Find("SelectedItem").GetComponent<csSeletedItem>();

        shop = GameObject.Find("Shop");
        Factory = GameObject.Find("Factory");

    }
    // isDragOk도 재설정해야댐

    public void AddItem(csItem item)
    {
        isDragOk = true;
        items.Push(item);

        if (items.Count > 1)
        {
            itemAmount.text = items.Count.ToString();
        }

        ChangeSprite(item.itemAtlas, item.itemSpriteName);
    }

    public void AddAllItem(Stack<csItem> items)
    {
        isDragOk = true;

        this.items = new Stack<csItem>(items);

        
        if (items.Count > 1)
        {
            
            itemAmount.text = items.Count.ToString();
        }
        else
        {
            itemAmount.text = string.Empty;
        }

        ChangeSprite(CurrentItem.itemAtlas, CurrentItem.itemSpriteName);
    }


    void rankColor()
    {
        if (CurrentItem != null)
        {
            switch (CurrentItem.rank)
            {
                case 1:
                    rankSprite.color = Color.black;
                    break;

                case 2:
                    rankSprite.color = Color.blue;
                    break;
                case 3:
                    rankSprite.color = Color.green;
                    break;
                case 4:
                    rankSprite.color = Color.yellow;
                    break;
                case 5:
                    rankSprite.color = Color.cyan;
                    break;
                case 6:
                    rankSprite.color = Color.red;
                    break;
            }
        }
        else
        {
            rankSprite.color = Color.black;
        }
    }

    public void ChangeSprite(UIAtlas atlas, string name)
    {
        
        if (name.Length < 1)
        {
            sprite.atlas = atlas;
            sprite.spriteName = "";
        }
        else
        {
            string removeName = name.Remove(name.Length - 1, 1);
            string insertName = removeName.Insert(removeName.Length, "1");

            sprite.atlas = atlas;
            sprite.spriteName = insertName;
        }

        rankColor();
    }

    public void SortSprite()
    {
        if (items.Count != 0 )
        {
            string removeName = CurrentItem.itemSpriteName.Remove(CurrentItem.itemSpriteName.Length - 1,1);
            string insertName = removeName.Insert(removeName.Length, "1");
            sprite.atlas = defaultAtlas;
            sprite.spriteName = insertName;
        }
        else
        {
            sprite.atlas = defaultAtlas;
            sprite.spriteName = "";
        }

        if (items.Count > 1)
        {
            itemAmount.text = items.Count.ToString();
        }
        else
        {
            itemAmount.text = string.Empty;
        }

        rankColor();
    }

    public void OnDragStart()
    {
        if (!isDragOk) return;

        if (slotType == csItem.ItemType.Sell)
        {
            return;
        }

        clone = NGUITools.AddChild(transform.parent.gameObject, gameObject);
        clone.transform.localPosition = transform.localPosition;
        clone.transform.localRotation = transform.localRotation;
        clone.transform.localScale = transform.localScale;
        clone.GetComponent<BoxCollider>().enabled = false;
        clone.transform.SetParent(UIDragDropRoot.root);

     
    }
    
    public void OnDrag(Vector2 delta)
    {
        if (!isDragOk) return;

        if (slotType == csItem.ItemType.Sell)
        {
            return;
        }

        clone.transform.localPosition += (Vector3)(delta * mRoot.pixelSizeAdjustment);

    
    }
    
    void OnDragEnd()
    {
        if (!isDragOk) return;

        if (slotType == csItem.ItemType.Sell)
        {
            return;
        }

        NGUITools.Destroy(clone.gameObject);
    }
    
    void OnDrop(GameObject dropped)
    {
        Debug.Log("빈빈" + manager.emptySlots);
        bool isInvenToInven = true;
        bool isEquip = false;

        string FromItemCode = null;
        int FromIndex = -1;
        int FromItemCount = -1;
        int ToIndex = -1;

        csSlotStorage tmpslot = dropped.GetComponent<csSlotStorage>();

        if (tmpslot == null || tmpslot.Items.Count == 0)
            return;

        if (tmpslot.slotType == csItem.ItemType.Sell)
        {
            return;
        }
        
        // 파는곳에 두면
        if (slotType == csItem.ItemType.Sell)
        {
            csSlotStorage slot = dropped.GetComponent<csSlotStorage>();

            if (slot == null || slot.Items.Count == 0)
                return;

            Stack<csItem> tmp = new Stack<csItem>(this.Items);

            this.AddAllItem(slot.Items);

            if (tmp.Count == 0)
            {
                slot.ClearSlot();
                manager.emptySlots++;
            }
            else
            {
                slot.AddAllItem(tmp);
            }

            int price = 0;
            string FromCode="";
            int FromCount = 0;
            if(slot.Items.Count != 0)
            {
                FromCode = slot.Items.Peek().itemCode;
                FromCount = slot.Items.Count;
                
            }
            else
            {
                FromCode = "";
            }
            Debug.Log(price+"팔꺼야");

            price = Items.Peek().sell_Price * Items.Count;
            sellPrice.text = price.ToString();
            SqlQurey(1, 4, slot.index, FromCode, FromCount, true);
        }

        // 아니면
        else
        {
            //인벤에 놨을때
            if (slotType == csItem.ItemType.None)
            {
                Debug.Log("인벤에 놨다");
                csSlotStorage slot = dropped.GetComponent<csSlotStorage>();

                if (slot == null || slot.Items.Count == 0)
                    return;


                csItem item = slot.items.Peek();

                bool isReturn = false;

                if (items.Count != 0)
                {
                    Debug.Log("놓은곳이 빈곳이 아니다 리턴하자");
                    isReturn = true;

                    if (items.Peek().itemType == item.itemType && items.Peek().legCount == item.legCount)
                    {
                        Debug.Log("리턴할라했는데 종류와 다리수가 같은거 였다 리턴 ㄴㄴ");
                        isReturn = false;
                    }
                    else
                    {
                        Debug.Log("종류가다르거나 다리수가 틀리다");
                    }
                }

                // 인벤에서 인벤은 리턴안함
                if (slot.slotType == csItem.ItemType.None)
                {
                    isReturn = false;
                }

                if (isReturn)
                {
                    Debug.Log("리턴");
                    return;
                }
                // 내거 백업
                Stack<csItem> tmp = new Stack<csItem>(this.Items);

                this.AddAllItem(slot.Items);

                // 놓은곳이 빈공간이면
                if (tmp.Count == 0)
                {
                    Debug.Log("빈 인벤에 놨다");
                    //장비에서 인벤에 놨으면
                    if (slot.slotType != csItem.ItemType.None)
                    {
                        isInvenToInven = false;
                        manager.emptySlots--;
                        Debug.Log("장비창에서 인벤에 놨다");
                        if (slot.slotType == csItem.ItemType.Head)
                        {
                            FromIndex = 0;
                            Debug.Log("머리에서 가져왔음");
                        }
                        else if (slot.slotType == csItem.ItemType.Tail)
                        {
                            FromIndex = 1;
                            Debug.Log("꼬리에서 가져왔음");
                        }
                        else if (slot.slotType == csItem.ItemType.Front_Leg)
                        {
                            Debug.Log("앞다리에서 가져왔음");
                            if (slot.items.Peek().legCount == 2)
                            {
                                Debug.Log("가져온 아이템이 2족이였다 뒷다리 x표시 지우자");
                                Back_Leg.sprite.spriteName = "";
                            }
                            FromIndex = 2;

                        }
                        else if (slot.slotType == csItem.ItemType.Back_Leg)
                        {
                            Debug.Log("뒷다리에서 가져왔음");
                            if (slot.items.Peek().legCount == 2)
                            {
                                Debug.Log("가져온 아이템이 2족이였다 앞다리 x표시 지우자");
                                Front_Leg.sprite.spriteName = "";
                            }
                            FromIndex = 3;
                        }

                        ToIndex = index;
                        Debug.Log("제거만하는 조립");
                        manager.Assembly(item, 1);
                    }
                    // 인벤에서 인벤
                    else
                    {
                        Debug.Log("인벤에서 인벤으로 옮겼다");
                        FromIndex = slot.index;
                        ToIndex = index;
                    }
                    slot.ClearSlot();
                }
                else
                {
                    Debug.Log("놓은 곳이 빈공간이 아니다");
                    slot.AddAllItem(tmp);

                    if (slot.slotType != csItem.ItemType.None)
                    {
                        Debug.Log("장비에서 가져왔다");
                        Debug.Log("장비에서 인벤으로 가져왔을떄 조립");

                        manager.Assembly(slot.items.Peek(), 0);

                    }
                    else
                    {
                        Debug.Log("인벤에서 가져옴");
                        FromIndex = slot.index;
                        ToIndex = index;
                    }
                }
                if (slot.items.Count > 0)
                {

                    FromItemCode = slot.items.Peek().itemCode;
                }
                FromItemCount = slot.items.Count;
            }
            //장비창에 놨을때
            else
            {
                Debug.Log("장비창에 놨다");
                csSlotStorage slot = dropped.GetComponent<csSlotStorage>();

                if (slot == null || slot.Items.Count == 0)
                    return;

                csItem item = slot.items.Peek();

                //다리 옮길때
                if (item.itemType == csItem.ItemType.Front_Leg || item.itemType == csItem.ItemType.Back_Leg)
                {
                    Debug.Log("앞다리거나 뒷다리를 가져왔다");
                    bool isReturn = true;
                    // 2족다리 옮길때
                    if (item.legCount == 2)
                    {
                        Debug.Log("2족다리를 가져왔다");
                        // 앞다리에 놨을때 
                        if (slotType == csItem.ItemType.Front_Leg)
                        {
                            Debug.Log("앞다리장비창에 놨다");
                            //뒷다리 비어있으면 가능
                            if (Back_Leg.items.Count == 0)
                            {
                                Debug.Log("뒷다리가 비어있다 가져온 2족다리를 놓을수 있다 뒷다리에 x표시");
                                isReturn = false;
                                Back_Leg.sprite.spriteName = "X";
                            }
                            else
                            {
                                Debug.Log("뒷다리에 뭔가 장착되어있다");
                            }

                        }
                        // 뒷다리에 놨을때
                        if (slotType == csItem.ItemType.Back_Leg)
                        {
                            Debug.Log("뒷다리장비창에 놨다");
                            // 앞다리 비어있으면 가능
                            if (Front_Leg.items.Count == 0)
                            {
                                Debug.Log("앞다리가 비어있다");
                                if (Items.Count == 0)
                                {
                                    Debug.Log("뒷다리칸이 비어있어서 놓을수 있다 ");
                                    isReturn = false;
                                    Front_Leg.sprite.spriteName = "X";
                                }
                                else
                                {
                                    Debug.Log("뒷다리칸에 무언가 장착되어있다");
                                    if (CurrentItem.legCount == item.legCount)
                                    {
                                        Debug.Log("같은 2족이 뒷다리칸에 장착되어있어 교체가능하다");
                                        isReturn = false;
                                        Front_Leg.sprite.spriteName = "X";
                                    }
                                    else
                                    {
                                        Debug.Log("뒷다리에 장착되어 있는 것이 2족이 아니다");
                                    }
                                }


                            }
                        }
                    }
                    // 4족다리 옮길때
                    else if (item.legCount == 4)
                    {
                        Debug.Log("4족다리를 가져왔다");
                        // 앞다리에 놨을때
                        if (slotType == csItem.ItemType.Front_Leg)
                        {
                            Debug.Log("앞다리장비창에 놨다");

                            if (Back_Leg.items.Count != 0)
                            {
                                Debug.Log("뒷다리에 뭔가가 장착되어있다");
                                if (Back_Leg.items.Peek().legCount == 4)
                                {
                                    Debug.Log("뒷다리에 4족이 껴져있었다");
                                    if (item.itemType == slotType)
                                    {
                                        Debug.Log("가져온아이템이 앞다리가 맞다");
                                        isReturn = false;
                                    }
                                    else
                                    {
                                        Debug.Log("가져온아이템이 앞다리가 아니다");
                                    }
                                }
                                else
                                {
                                    Debug.Log("뒷다리에 4족다리가 아닌것이 장착되어 있다");
                                }
                            }
                            else
                            {
                                Debug.Log("뒷다리가 비어있다");
                                if (item.itemType == slotType)
                                {
                                    if (Items.Count == 0)
                                    {
                                        Debug.Log("뒷다리와 앞다리가 비어있어서 놓을수 있다");
                                        isReturn = false;
                                    }
                                    else
                                    {
                                        Debug.Log("앞다리에 뭔가가 장착되어있다");
                                        if (item.legCount == CurrentItem.legCount)
                                        {
                                            Debug.Log("가져온아이템이 앞다리가 맞고 다리수도 같다");
                                            isReturn = false;

                                        }
                                    }
                                }
                                else
                                {
                                    Debug.Log("뒷다리가 비어있지만 가져온아이템이 뒷다리가 아니다");
                                }
                            }
                        }
                        // 뒷다리에 놨을때
                        else if (slotType == csItem.ItemType.Back_Leg)
                        {
                            Debug.Log("뒷다리장비창에 놨다");
                            if (Front_Leg.items.Count != 0)
                            {
                                Debug.Log("앞다리에 뭔가가 장착되어있다");
                                if (Front_Leg.items.Peek().legCount == 4)
                                {
                                    Debug.Log("앞다리에 4족다리가 껴져있었다");
                                    if (item.itemType == slotType)
                                    {
                                        Debug.Log("가져온 아이템이 뒷다리가 맞다");
                                        isReturn = false;
                                    }
                                    else
                                    {
                                        Debug.Log("가져온 아이템이 뒷다리가 아니다");
                                    }
                                }
                                else
                                {
                                    Debug.Log("뒷다리에 4족다리가 아닌것이 장착되어있다");
                                }
                            }
                            else
                            {
                                Debug.Log("앞다리가 비어 있다");
                                if (item.itemType == slotType)
                                {
                                    if (Items.Count == 0)
                                    {
                                        Debug.Log("뒷다리 장비칸이 비어있다");
                                        isReturn = false;
                                    }
                                    else
                                    {
                                        Debug.Log("뒷다리에 무언가 껴져있다");

                                        if (item.legCount == CurrentItem.legCount)
                                        {
                                            Debug.Log("가져온아이템이 뒷다리가 맞고 다리수도 같다");
                                            isReturn = false;
                                        }

                                    }

                                }
                                else
                                {
                                    Debug.Log("앞다리가 비어있지만 가져온 아이템이 뒷다리가 아니다");
                                }
                            }
                        }

                    }

                    if (isReturn)
                    {
                        Debug.Log("리턴");
                        return;

                    }
                }
                // 나머지 옮길때
                else
                {
                    Debug.Log("다리가 아닌 아이템을 옮기고 있다");
                    if (item.itemType != slotType)
                    {
                        Debug.Log("가져온아이템과 놓은곳이 틀리다 리턴");
                        return;
                    }
                    else
                    {
                        Debug.Log("가져온아이템고 놓은곳이 맞다");
                    }
                }


                //장비창
                Stack<csItem> tmp = new Stack<csItem>(this.Items);

                this.AddAllItem(slot.Items);
                //인벤에서 장비에 놨으면
                if (tmp.Count == 0)
                {
                    slot.ClearSlot();
                    manager.emptySlots++;
                }
                else
                {
                    slot.AddAllItem(tmp);
                }

                manager.Assembly(item, 0);
                Debug.Log("생성도하는 조립");
                if (slot.items.Count > 0)
                {
                    FromItemCode = slot.items.Peek().itemCode;
                }
                FromItemCount = slot.items.Count;
                FromIndex = (int)slotType - 1; // 장비창인덱스

                ToIndex = slot.index; // 인벤인덱스
                isInvenToInven = false;
                isEquip = true;
            }
            Debug.Log("옮기기 성공");
            //인벤에서 인벤
            if (isInvenToInven)
            {

                SqlQurey(0, FromIndex, ToIndex, FromItemCode, FromItemCount, isEquip);
            }

            else
            {
                SqlQurey(1, FromIndex, ToIndex, FromItemCode, FromItemCount, isEquip);
            }
        }


    }
    
    void OnClick()
    {
        if (slotType == csItem.ItemType.Sell)
        {
            manager.sellClear();
        }
        else
        {
            GameObject parent = null;
            parent = GameObject.Find("PreviewPart");

            // 미리보기 삭제
            Transform[] SpawnObject;
            SpawnObject = parent.GetComponentsInChildren<Transform>();

            for (int i = 1; i < SpawnObject.Length; i++)
            {
                Destroy(SpawnObject[i].gameObject);
            }

            manager.ClearMakeWindow();
            if (items.Count > 0)
            {
                selectedItem.ChangeSprite(items.Peek().itemSpriteName, items.Peek().etc, rankSprite.color,items.Peek().itemName);

                // 누른게 설계도라면
                if (items.Peek().itemType == csItem.ItemType.BluePrint)
                {
                    // 해당하는 부품을 찾는다
                    int i = 0;
                    for (i = 0; i < csItemDataBase.Instance().Database.Count; i++)
                    {
                        // 찾았으면
                        if (items.Peek().itemCode == csItemDataBase.Instance().Database[i].blueprint)
                        {
                            if (shop.activeSelf == false && Factory.activeSelf == true)
                            {
                                string name = csItemDataBase.Instance().Database[i].itemCode;
                                string removeName = name.Remove(name.Length - 1, 1);
                                string insertName = removeName.Insert(removeName.Length, "1");
                                string lastPartName = "PartPrefabs/" + insertName;

                                GameObject part = Resources.Load(lastPartName, typeof(GameObject)) as GameObject;

                                GameObject goTemp = Instantiate(part) as GameObject;

                                //Vector3 tmp = parent.transform.position;
                                Quaternion temp = goTemp.transform.rotation;

                                
                                goTemp.transform.parent = parent.transform;
                                goTemp.transform.localPosition = Vector3.zero;
                                goTemp.transform.localRotation = temp;
                                break;
                            }
                        }
                    }
                    manager.MaterialCheck(items.Peek(), index, i);
                }

            }
            else
            {

                selectedItem.ChangeSprite("", "", rankSprite.color,"");
            }

        }
    }


    
    public void ClearSlot()
    {
        isDragOk = false;
        items.Clear();
        ChangeSprite(defaultAtlas, defaultSpriteName);
        itemAmount.text = string.Empty;
    }

    public void SqlQurey(int type ,int FromIndex, int ToIndex, string FromItemCode,int FromItemCount,bool isEquip)
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

        // 인벤에서 인벤
        if (type == 0)
        {
            sqlQuery = "UPDATE Inventory SET \"Code\" = \""+FromItemCode+"\", \"Amount\" = "+FromItemCount+ " WHERE \"Index\" = " + FromIndex + ""; 
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();


            sqlQuery = "UPDATE Inventory SET \"Code\" = \"" + items.Peek().itemCode + "\", \"Amount\" = " + items.Count + " WHERE \"Index\" = " + ToIndex + "";
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteNonQuery();
            

        }
        else if(type == 1)
        {
            
            //인벤에서 장비
            if (isEquip)
            {
   
                sqlQuery = "UPDATE Equipement SET \"Code\" = \"" + items.Peek().itemCode + "\", \"Amount\" = " + items.Count + " WHERE \"Index\" = " + FromIndex + "";
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();

                sqlQuery = "UPDATE Inventory SET \"Code\" = \"" + FromItemCode + "\", \"Amount\" = " + FromItemCount + " WHERE \"Index\" = " + ToIndex + "";
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
            else
            {
                sqlQuery = "UPDATE Equipement SET \"Code\" = \"" + FromItemCode + "\", \"Amount\" = " + FromItemCount + " WHERE \"Index\" = " + FromIndex + "";         
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
                
                
                sqlQuery = "UPDATE Inventory SET \"Code\" = \"" + items.Peek().itemCode + "\", \"Amount\" = " + items.Count + " WHERE \"Index\" = " + ToIndex + "";
                dbcmd.CommandText = sqlQuery;
                dbcmd.ExecuteNonQuery();
            }
        }
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }
}

