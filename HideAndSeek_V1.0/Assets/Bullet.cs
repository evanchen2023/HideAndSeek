using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{

    public int distance = 10;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 6.0f;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject hit = collision.gameObject;
        Health health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(20);
        }
    }

    public override void FixedUpdateNetwork()
    {

    }
}