using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype nccp;

    public float playerSpeed;
    void Awake()
    {
        nccp = gameObject.GetComponent<NetworkCharacterControllerPrototype>();
    }
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            nccp.Move(playerSpeed*data.direction * Runner.DeltaTime);
        }
    }
}
