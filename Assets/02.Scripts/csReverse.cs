using UnityEngine;
using System.Collections;

public class csReverse : MonoBehaviour
{
    public bool isJumpToOverTurn = false;
    bool isOverTurn = false;

    void Update()
    {
        StartCoroutine(Reverse_Area());
    }

    IEnumerator Reverse_Area()
    {
        if (isJumpToOverTurn == true)
        {
            GameObject.Find("Car").GetComponent<Rigidbody>().AddForce(Vector3.up * 10000.0f);

            isJumpToOverTurn = false;

            yield return new WaitForSeconds(0.2f);

            isOverTurn = true;
        }

        if (isOverTurn == true)
        {
            GameObject.Find("Car").GetComponent<Transform>().transform.Rotate(230.0f * Time.deltaTime * Vector3.forward);   ///230

            yield return new WaitForSeconds(3.0f);

            isOverTurn = false;
        }
    }


}
