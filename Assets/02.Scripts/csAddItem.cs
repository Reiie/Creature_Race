using UnityEngine;
using System.Collections;

public class csAddItem : MonoBehaviour
{

    public csSlotStorage head;
    public csSlotStorage tail;
    public csSlotStorage front_leg;
    public csSlotStorage back_leg;
    public csSlotStorage sell;

    public int amount;

    public csInventory inventory;

    public csItemDataBase itemdatabase;

    

    static csAddItem _instance = null;

    public static csAddItem Instance()
    {
        return _instance;
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public void AddItem()
    {
        for (int i = 0; i < 28; i++)
        {
            csItemDataBase.myItem tmp = itemdatabase.inven[i];

            for (int j = 0; j < itemdatabase.Database.Count; j++)
            {
                if(tmp.code == itemdatabase.Database[j].itemCode)
                {
                    for (int k = 0; k < tmp.amount; k++)
                    {
                        inventory.MySlots[i].AddItem(itemdatabase.Database[j]);
                    }
                    inventory.emptySlots--;
                    break;
                }
            }
         }
    }
    
    public void AddEquip()
    {
        for(int i = 0; i< 5; i++)
        {
            
            csItemDataBase.myItem tmp = itemdatabase.equip[i];

            for (int j= 0; j < itemdatabase.Database.Count; j++)
            {
                if(tmp.code == itemdatabase.Database[j].itemCode)
                {

                    // 머리
                    if (i == 0)
                    {
                        head.AddItem(itemdatabase.Database[j]);
                        PreloadAssembly(itemdatabase.Database[j]);
                    }
                    // 꼬리
                    else if (i == 1)
                    {
                        tail.AddItem(itemdatabase.Database[j]);
                        PreloadAssembly(itemdatabase.Database[j]);
                    }
                    // 앞다리
                    else if (i == 2)
                    {
                        front_leg.AddItem(itemdatabase.Database[j]);
                        PreloadAssembly(itemdatabase.Database[j]);
                        if (itemdatabase.Database[j].legCount == 2)
                        {
                            back_leg.sprite.spriteName = "X";

                        }
                    }
                    // 뒷다리
                    else if (i == 3)
                    {
                        back_leg.AddItem(itemdatabase.Database[j]);
                        PreloadAssembly(itemdatabase.Database[j]);
                        if (itemdatabase.Database[j].legCount == 2)
                        {
                            front_leg.sprite.spriteName = "X";

                        }
                    }
                    else if (i == 4)
                    {
                        for (int k = 0; k < tmp.amount; k++)
                        {
                            sell.AddItem(itemdatabase.Database[j]);
                        }


                        inventory.AddItem(itemdatabase.Database[j], tmp.amount);
                        sell.ClearSlot();

                        inventory.SellSqlQurey(4, "0", 0);
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


    public void AddOrc()
    {

        int random = Random.Range(0, 250);

        inventory.AddItem(itemdatabase.Database[523], amount);
      /*  if (random == 0)
        {
            inventory.AddItem(itemdatabase.Database[0], amount);
        }
        else if(random == 1)
        {
            inventory.AddItem(itemdatabase.Database[1], amount);
        }
        else if(random == 2)
        {
            inventory.AddItem(itemdatabase.Database[2], amount);
        }
        else if(random == 3)
        {
            inventory.AddItem(itemdatabase.Database[3], amount);
        }
        else
        {
            inventory.AddItem(itemdatabase.Database[4], amount);
        }*/
    }


}
