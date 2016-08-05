using UnityEngine;
using System.Collections;

public class csBtnScale : MonoBehaviour {

    // Use this for initialization

    Vector3 orignal_Size;
	void Awake () {

    }

    void OnEnable()
    {
    }

    void OnClick()
    {
        gameObject.GetComponent<TweenScale>().PlayForward();   
        StartCoroutine(reverse());
    }

    IEnumerator reverse()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<TweenScale>().PlayReverse();
    }

 
}
