using UnityEngine;
using System.Collections;

public class csHeadCollisionCheck : MonoBehaviour 
{


    csCarState car_state;


    // Use this for initialization
    void Start () {
        car_state = gameObject.transform.parent.GetComponent<csCarState>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Untagged")
        {
            car_state.reverseTime = 0;
            //car_state.carState = csCarState.CARSTATE.RECOVERY;


			if (car_state.boolRecovery == true) 
			{
				car_state.boolRecovery = false;
				car_state.reverseTime = 0;
				car_state.HeadToBlinkEffect ();
			}

        }

		if(other.tag == "Floor")
		{
			car_state.reverseTime = 0;
			//car_state.carState = csCarState.CARSTATE.RECOVERY;


			if (car_state.boolRecovery == true) 
			{
				car_state.boolRecovery = false;
				car_state.reverseTime = 0;
				car_state.HeadToBlinkEffect ();
			}

		}

		if(other.tag == "Water")
		{
			car_state.reverseTime = 0;
			//car_state.carState = csCarState.CARSTATE.RECOVERY;


			if (car_state.boolRecovery == true) 
			{
				car_state.boolRecovery = false;
				car_state.reverseTime = 0;
				car_state.HeadToBlinkEffect ();
			}

		}

        if (other.tag == "Stage2_Water")
        {
            car_state.reverseTime = 0;
            

			if (car_state.boolRecovery == true) 
			{
				car_state.boolRecovery = false;
				car_state.reverseTime = 0;
				car_state.HeadToBlinkEffect ();
			}
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Untagged")
        {
            car_state.reverseTime = 0;
            car_state.carState = csCarState.CARSTATE.RUN;
        }
		if (other.tag == "Stage2_Water")
		{
			car_state.reverseTime = 0;
			car_state.carState = csCarState.CARSTATE.WATER_Sixty;
		}
		if (other.tag == "Floor")
		{
			car_state.reverseTime = 0;
			car_state.carState = csCarState.CARSTATE.RUN;
		}
		if (other.tag == "Water")
		{
			car_state.reverseTime = 0;
			car_state.carState = csCarState.CARSTATE.WATER;
		}

    }
}
