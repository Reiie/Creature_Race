using UnityEngine;
using System.Collections;

public class csSortActive : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActiveTrue()
    {
        gameObject.SetActive(true);
    }

    public void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
