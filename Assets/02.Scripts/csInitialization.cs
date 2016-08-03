using UnityEngine;
using System.Collections;

public class csInitialization : MonoBehaviour {


    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<TweenPosition>().PlayForward();

        gameObject.SetActive(false);
    }

}
