using UnityEngine;
using System.Collections;

public class csEffectFix : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.localPosition = new Vector3(0.5f, -0.5f, 0.5f);
    }
}
