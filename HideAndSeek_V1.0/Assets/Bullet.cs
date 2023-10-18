using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        Health health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(20);
        }
    }

    void FixedUpdateNetwork()
    {
        //Bullet Movement
    }
}