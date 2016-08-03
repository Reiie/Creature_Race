using UnityEngine;
using System.Collections;

public class csSmart : MonoBehaviour {

    csCarState car_state;

    
    // Use this for initialization
    void Start () {

        car_state = GetComponent<csCarState>();
        car_state.isSmart = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
