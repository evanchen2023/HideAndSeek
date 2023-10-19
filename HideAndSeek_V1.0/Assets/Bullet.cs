using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{

    public int distance = 10; //Seconds
    public float speed = 20f;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed * Runner.DeltaTime;
        Destroy(gameObject, distance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player"))
            {
                Health health = other.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(20);
                }
            }

            if (other.CompareTag("Bullet"))
            {
                if (Runner.IsServer)
                {
                    Runner.Despawn(Object);
                }
            }
        }
    }

    public override void FixedUpdateNetwork()
    {

    }
}