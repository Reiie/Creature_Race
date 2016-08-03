using UnityEngine;
using System.Collections;

public class csDestroy_Original : MonoBehaviour
{
    public bool Destroy_Object = false;

    void Update()
    {
        if (Destroy_Object == true)
        {
            Destroy(gameObject);
        }
    }
}
