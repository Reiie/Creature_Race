using UnityEngine;
using System.Collections;

public class csCarBodyCollision : MonoBehaviour
{
    public GameObject Collision_Effect;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Car_Body")
        {
            GameObject C_Particle = Instantiate(Collision_Effect) as GameObject;
            C_Particle.transform.position = collision.transform.position;
            Destroy(C_Particle, 0.5f);
        }
    }
}
