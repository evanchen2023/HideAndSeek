using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public int damage;
    public float lifespan = 1f; // Time in seconds before the projectile disappears

    private Rigidbody rb;

    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Destroy the projectile after a certain duration
        Destroy(gameObject, lifespan);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // make sure only to stick to the first target you hit
        //if (targetHit)
        //    return;
        //else
        //    targetHit = true;

        // check if you hit an enemy
        if(collision.gameObject.GetComponent<BasicEnemyDone>() != null)
        {
            BasicEnemyDone enemy = collision.gameObject.GetComponent<BasicEnemyDone>();

            enemy.TakeDamage(damage);

            // destroy projectile
            Destroy(gameObject);
        }

        // make sure projectile sticks to surface
        //rb.isKinematic = true;

        // make sure projectile moves with target
        transform.SetParent(collision.transform);
    }
}