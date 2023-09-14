using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.PropsCollected();
            gameObject.SetActive(false);
        }
    }
}
