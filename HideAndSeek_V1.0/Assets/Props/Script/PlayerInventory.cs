using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfProps { get; private set; }

    public UnityEvent<PlayerInventory> OnPropsCollected;

    public void PropsCollected()
    {
        NumberOfProps++;
        OnPropsCollected.Invoke(this);
    }
}
