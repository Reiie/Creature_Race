using UnityEngine;
using System.Collections;

public class csCarInfoUpdate : MonoBehaviour {

    
    csCarState car_state;
    GameObject head;
    GameObject tail;
    GameObject front_Leg;
    GameObject middle_Leg;
    GameObject back_Leg;
    // Use this for initialization
    void Start()
    {
 
        car_state = GetComponent<csCarState>();
      //  head = transform.FindChild("Head_Point").gameObject;

        Transform[] tempTransforms = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in tempTransforms)
        {
            if (child.name.Contains("Head_Point"))
            {
                head = child.gameObject;
              
            }
            if (child.name.Contains("Tail_Point"))
            {
                tail = child.gameObject;
              
            }
            if (child.name.Contains("FrontLeg_Point"))
            {
                front_Leg = child.gameObject;

            }
            if (child.name.Contains("MiddleLeg_Point"))
            {
                middle_Leg = child.gameObject;
               
            }
            if (child.name.Contains("BackLeg_Point"))
            {
                back_Leg = child.gameObject;
              
            }
        }
        StateUpdate();
    }

    void StateUpdate()
    {
        Transform[] headchild;
        Transform[] tailchild;
        Transform[] f_Legchild;
        Transform[] m_Legchild;
        Transform[] b_Legchild;
        headchild = head.GetComponentsInChildren<Transform>();
        tailchild = tail.GetComponentsInChildren<Transform>();
        f_Legchild = front_Leg.GetComponentsInChildren<Transform>();
        m_Legchild = middle_Leg.GetComponentsInChildren<Transform>();
        b_Legchild = back_Leg.GetComponentsInChildren<Transform>();

        for (int i =0; i < csItemDataBase.Instance().PartDB.Count ; i++)
        {
            // 머리 찾아라
            if (headchild.Length > 1 )
            {
                if (csItemDataBase.Instance().PartDB[i].itemCode == headchild[1].gameObject.name)
                {
                    car_state.maxSpeed += csItemDataBase.Instance().PartDB[i].max_speed/2;
                    car_state.Power += csItemDataBase.Instance().PartDB[i].power;
                    car_state.HP += csItemDataBase.Instance().PartDB[i].hp;
                    car_state.MaxHp += csItemDataBase.Instance().PartDB[i].hp;
                    car_state.Sec_Heal += csItemDataBase.Instance().PartDB[i].sec_heal;
                    Debug.Log("머리 " + car_state.HP);
                    Debug.Log("머리디비 " + csItemDataBase.Instance().PartDB[i].hp);
                    if (csItemDataBase.Instance().PartDB[i].ability == 1)
                    {
                        car_state.isJumper = true;
                    }
                    else if (csItemDataBase.Instance().PartDB[i].ability == 2)
                    {
                        car_state.isClimber = true;
                    }
                    else if (csItemDataBase.Instance().PartDB[i].ability == 3)
                    {
                        car_state.isSmart = true;
                    }
                }
            }
            // 꼬리찾아라
            if (tailchild.Length > 1)
            {
                if (csItemDataBase.Instance().PartDB[i].itemCode == tailchild[1].gameObject.name)
                {
                    car_state.maxSpeed += csItemDataBase.Instance().PartDB[i].max_speed / 2;
                    car_state.Power += csItemDataBase.Instance().PartDB[i].power;
                    car_state.HP += csItemDataBase.Instance().PartDB[i].hp;
                    car_state.MaxHp += csItemDataBase.Instance().PartDB[i].hp;
                    car_state.Sec_Heal += csItemDataBase.Instance().PartDB[i].sec_heal;
                    Debug.Log("꼬리 " + car_state.HP);
                    Debug.Log("꼬리디비 " + csItemDataBase.Instance().PartDB[i].hp);
                    if (csItemDataBase.Instance().PartDB[i].ability == 1)
                    {
                        car_state.isJumper = true;
                    }
                    else if (csItemDataBase.Instance().PartDB[i].ability == 2)
                    {
                        car_state.isClimber = true;
                    }
                    else if (csItemDataBase.Instance().PartDB[i].ability == 3)
                    {
                        car_state.isSmart = true;
                    }
                }
            }
            // 앞다리 찾아라
            if (f_Legchild.Length > 1)
            {
                if (csItemDataBase.Instance().PartDB[i].itemCode == f_Legchild[1].gameObject.name)
                {
                    car_state.maxSpeed += csItemDataBase.Instance().PartDB[i].max_speed / 2;
                    car_state.Power += csItemDataBase.Instance().PartDB[i].power;
                    car_state.HP += csItemDataBase.Instance().PartDB[i].hp;
                    car_state.MaxHp += csItemDataBase.Instance().PartDB[i].hp;
                    car_state.Sec_Heal += csItemDataBase.Instance().PartDB[i].sec_heal;
 
                    if (csItemDataBase.Instance().PartDB[i].ability == 1)
                    {
                        car_state.isJumper = true;
                    }
                    else if (csItemDataBase.Instance().PartDB[i].ability == 2)
                    {
                        car_state.isClimber = true;
                    }
                    else if (csItemDataBase.Instance().PartDB[i].ability == 3)
                    {
                        car_state.isSmart = true;
                    }
                }
            }
            // 중간다리찾아라
            if (m_Legchild.Length > 1)
            {
                if (csItemDataBase.Instance().PartDB[i].itemCode == m_Legchild[1].gameObject.name)
                {
                    car_state.maxSpeed += csItemDataBase.Instance().PartDB[i].max_speed / 2;
                    car_state.Power += csItemDataBase.Instance().PartDB[i].power;
                    car_state.HP += csItemDataBase.Instance().PartDB[i].hp;
                    car_state.MaxHp += csItemDataBase.Instance().PartDB[i].hp;
                    car_state.Sec_Heal += csItemDataBase.Instance().PartDB[i].sec_heal;
                    if (csItemDataBase.Instance().PartDB[i].ability == 1)
                    {
                        car_state.isJumper = true;
                    }
                    else if (csItemDataBase.Instance().PartDB[i].ability == 2)
                    {
                        car_state.isClimber = true;
                    }
                    else if (csItemDataBase.Instance().PartDB[i].ability == 3)
                    {
                        car_state.isSmart = true;
                    }
                }
            }
            // 뒷다리 찾아라
            if (b_Legchild.Length > 1)
            {
                if (csItemDataBase.Instance().PartDB[i].itemCode == b_Legchild[1].gameObject.name)
                {
                    car_state.maxSpeed += csItemDataBase.Instance().PartDB[i].max_speed / 2;
                    car_state.Power += csItemDataBase.Instance().PartDB[i].power;
                    car_state.HP += csItemDataBase.Instance().PartDB[i].hp;
                    car_state.MaxHp += csItemDataBase.Instance().PartDB[i].hp;
                    car_state.Sec_Heal += csItemDataBase.Instance().PartDB[i].sec_heal;

                    if (csItemDataBase.Instance().PartDB[i].ability == 1)
                    {
                        car_state.isJumper = true;
                    }
                    else if (csItemDataBase.Instance().PartDB[i].ability == 2)
                    {
                        car_state.isClimber = true;
                    }
                    else if (csItemDataBase.Instance().PartDB[i].ability == 3)
                    {
                        car_state.isSmart = true;
                    }
                }
            }
        }

    //    Debug.Log("맥스_" + car_state.MaxHp);
    //    Debug.Log("피_" + car_state.HP);
    //    Debug.Log("초당_" + car_state.Sec_Heal);
        //  Debug.Log("머리이름"+child[1].gameObject.name);

        // Debug.Log("")
    }
    // Update is called once per frame
    void Update()
    {

    }
}
