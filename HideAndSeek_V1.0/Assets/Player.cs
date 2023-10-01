using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype nccp;
    
    //Control Variables
    public float playerSpeed;
    public float jumpSpeed;
    private Vector3 moveVelocity;
    private float gravity;

    void Awake()
    {
        nccp = gameObject.GetComponent<NetworkCharacterControllerPrototype>();
    }

    void Start()
    {
        gravity = nccp.gravity * -1;
        Debug.Log("Player Gravity : " + gravity);
    }
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //Movement
            data.direction.Normalize();
            //nccp.Move(playerSpeed*data.direction * Runner.DeltaTime);

            //Jump
            if ((data.buttons & NetworkInputData.JUMPBUTTON) != 0)
            {
                moveVelocity.y = jumpSpeed;
            }

            //Gravity
            moveVelocity.y += gravity * Runner.DeltaTime;
            data.direction.y = moveVelocity.y;

            //Move
            nccp.Move(playerSpeed*data.direction * Runner.DeltaTime);
        }
    }

    private void PlayerJump()
    {
        
    }
}
