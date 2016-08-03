using UnityEngine;
using System.Collections;

public class csDestroyTile : MonoBehaviour
{

    public GameObject KnockBack;
    
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    public void Destroy2()
    {
        Destroy(gameObject);
    }

}
