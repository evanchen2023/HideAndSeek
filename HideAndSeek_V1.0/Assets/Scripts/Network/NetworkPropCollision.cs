using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkPropCollision : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
        if (other.CompareTag("Player"))
        {
            if (Runner.IsServer)
            {
                Runner.Despawn(Object);
            }
        }
    }
}
