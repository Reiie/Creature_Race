using UnityEngine;
using System.Collections;

public class csJumper : MonoBehaviour {

    csCarState car_state;


    // Use this for initialization
    void Start()
    {
        car_state = GetComponent<csCarState>();
        car_state.isJumper = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
