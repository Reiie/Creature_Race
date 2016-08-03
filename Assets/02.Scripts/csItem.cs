using UnityEngine;
using System;
using System.Collections;


[System.Serializable]
public class csItem
{

    public enum ItemType
    {
        None,
        Head,
        Tail,
        Front_Leg,
        Back_Leg,
        BluePrint,
        Material,
        Sell,
    };

    public enum SubType
    {
        None,
        Metal,
        Circuit,
        Junk
    }

    public ItemType itemType;
    public SubType subType;
    public int itemID;

    public string itemCode;
    public string itemName;
    public int rank;
    public int itemMaxAmount;
    public int legCount;
    public int power;
    public int balance;
    public int max_speed;
    public int hp;
    public int sec_heal;
    public int jumpPower;
    public int cliberPower;
    public string blueprint;

    public int buy_Price;
    public int sell_Price;
    public string etc;
    public int ability;



    public int l_metal;
    public int m_metal;
    public int h_metal;

    public int l_circuit;
    public int m_circuit;
    public int h_circuit;

    public int l_junk;
    public int m_junk;
    public int h_junk;


    public UIAtlas itemAtlas;
    public string itemSpriteName;

    void Awake()
    {      
    }

    public void initialization()
    {
        
        itemAtlas = Resources.Load("Atlas/ItemIcon Atlas", typeof(UIAtlas)) as UIAtlas;

        // 임의로한거
     /*   if(itemID == 0)
        {
            itemSpriteName = "Orc Armor - Boots";
        }
        else if(itemID == 1)
        {
            itemSpriteName = "Orc Armor - Bracers";
        }
        else if(itemID == 2)
        {
            itemSpriteName = "Orc Armor - Shoulders";
        }
        else if(itemID == 3)
        {
            itemSpriteName = "NGUI";
        }
        else if(itemID == 4)
        {
            itemSpriteName = "Window";
        }*/
    }
}
