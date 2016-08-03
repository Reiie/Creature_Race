using UnityEngine;
using System.Collections;

public class csRecoveryTest : MonoBehaviour
{
    Transform Base_transform;
    GameObject Car;
    GameObject Body;
    GameObject Target;

    int RecoveryLimit = 0;
    bool boolRecovery = false;

    void Start()
    {
        Car = GameObject.Find("Car");
        Body = GameObject.Find("Body");
    }

    void Update()
    {
        if (boolRecovery == false)
        {
            Body.SetActive(true);
            Car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Untagged")
        {
            boolRecovery = true;
            StartCoroutine(Recovery1());
            Base_transform = GameObject.Find("Target").GetComponent<Transform>();
        }
    }

    IEnumerator Recovery1()
    {
        if (boolRecovery == true)
        {
            Debug.Log("Recovery1");
            yield return new WaitForSeconds(2.5f);
            Car.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            Body.SetActive(false);
            StartCoroutine(Recovery2());
        }
    }

    IEnumerator Recovery2()
    {
        if (boolRecovery == true)
        {
            yield return new WaitForSeconds(0.1f);

            if (RecoveryLimit < 4)
            {
                RecoveryLimit++;
                Body.SetActive(true);
                StartCoroutine(Recovery3());
            }
            if (RecoveryLimit == 4)
            {
                RecoveryLimit = 0;
                boolRecovery = false;
                Body.SetActive(true);  
                Car.GetComponent<Transform>().position = Base_transform.position;
                Car.GetComponent<Transform>().rotation = Base_transform.rotation;

                FixedRecovery();
            }
        }
    }

    IEnumerator Recovery3()
    {
        if (boolRecovery == true)
        {
            Debug.Log("Recovery2");
            yield return new WaitForSeconds(0.2f);
            Body.SetActive(false);
            StartCoroutine(Recovery2());
        }
    }

    void FixedRecovery()
    {
        Body.SetActive(true);
        Debug.Log("11111111111");
    }
}
