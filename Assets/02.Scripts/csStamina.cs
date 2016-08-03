using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class csStamina : MonoBehaviour {

    public GameObject GagePrefabs;
    public List<GameObject> gages;

    public UIGrid m_Grid;
    // Use this for initialization
    void Start () {

        gages = new List<GameObject>();

        for(int i=0;i < 10; i++)
        {
            GameObject gage = NGUITools.AddChild(gameObject, GagePrefabs);

            gages.Add(gage);
        }
        m_Grid.Reposition();

        checkStamina();
    }
	
	public void checkStamina()
    {
        int stamina = UserManager.Instance().stamina;
      
        int count = 0;
        while(stamina > 0 )
        {
            stamina = stamina - 10;
            count++;
        }
        if (count != 0)
        {
            for (int i = 0; i < count - 1; i++)
            {
                gages[i].SetActive(true);
            }
        }

        if (count != 0)
        {
            for (int j = count - 1; j < 10; j++)
            {
                gages[j].SetActive(false);
            }
        }
        else if(count == 0)
        {
            for (int j = count; j < 10; j++)
            {
                gages[j].SetActive(false);
            }
        }

        // 스태미나가 10단위면
        if(stamina == 0)
        {
            if (count != 0)
            {
                gages[count - 1].SetActive(true);
            }
        }

    }
}
