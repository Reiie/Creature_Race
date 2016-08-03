using UnityEngine;
using System.Collections;

public class csKnockBack_Tile : MonoBehaviour
{
    public GameObject Bknock_Back;
    public GameObject Fknock_Back;
    public Vector3 Original_KnockBack_Transform;

    public float Tile_Timer;

    int knock_back_choice;
    bool Trigger = false;

    void Start()
    {
        Trigger = true;
    }

    void Update()
    {
        WHAT_KNOCKBACKS();
        Tile_Timer = Tile_Timer + Time.deltaTime;
    }

    void WHAT_KNOCKBACKS()
    {
        if (Trigger == true)
        {
            //Trigger = false;
            knock_back_choice = Random.Range(0, 2);

            if (knock_back_choice == 0 && Tile_Timer >= 3)
            {
                GameObject temp = Instantiate(Bknock_Back, Original_KnockBack_Transform, Quaternion.identity) as GameObject;
                temp.GetComponent<csDestroyTile>().KnockBack = gameObject;
                
                Trigger = true;
                Tile_Timer = 0;
            }
            else if (knock_back_choice == 1 && Tile_Timer >= 3)
            {
                GameObject temp = Instantiate(Fknock_Back, Original_KnockBack_Transform, Quaternion.identity) as GameObject;
                temp.GetComponent<csDestroyTile>().KnockBack = gameObject;
                Trigger = true;
                Tile_Timer = 0;
            }
        }
    }

    //IEnumerator WHAT_KNOCKBACK()
    //{
    //    if (Trigger == true)
    //    {
    //        Trigger = false;
    //        yield return new WaitForSeconds(3.0f);
    //        knock_back_choice = Random.Range(0, 2);

    //        if (knock_back_choice == 0)
    //        {
    //            Instantiate(Bknock_Back, Original_KnockBack_Transform, Quaternion.identity);
    //            Trigger = true;
    //        }
    //        else if (knock_back_choice == 1)
    //        {
    //            Instantiate(Fknock_Back, Original_KnockBack_Transform, Quaternion.identity);
    //            Trigger = true;
    //        }
    //    }
    //}
}
