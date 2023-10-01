using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour, IPlayerLeft
{
    //Player Variables
    public static Player Local { get; set; }
    private NetworkCharacterControllerPrototype nccp;
    
    //Control Variables
    public float playerSpeed;
    public float jumpSpeed;
    private Vector3 moveVelocity;
    
    //Camera
    private Vector2 viewInput;
    private float camRotateX;
    private Camera localCamera;
    
    //Camera
    public bool is3rdPersonCamera { get; set; }

    void Awake()
    {
        nccp = gameObject.GetComponent<NetworkCharacterControllerPrototype>();
        localCamera = gameObject.GetComponentInChildren<Camera>();
    }

    private void LateUpdate()
    {
        //Camera X Rotation
        camRotateX += viewInput.y * Time.deltaTime;
        camRotateX = Mathf.Clamp(camRotateX, -90, 90);
        
        localCamera.transform.localRotation = Quaternion.Euler(camRotateX, 0, 0);
    }
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //Movement
            var cameraTransform = localCamera.transform;
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            
            //Get Relative Movement Vectors
            Vector3 forwardRelativeVerticalInput = data.direction.z * forward;
            Vector3 rightRelativeVerticalInput = data.direction.x * right;
            
            //Get Movement
            Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;

            nccp.Rotate(data.rotationDir.x);
            
            //Jump
            if ((data.buttons & NetworkInputData.JUMPBUTTON) != 0)
            {
                nccp.Jump();
            }
            //Move
            nccp.Move(playerSpeed*cameraRelativeMovement * Runner.DeltaTime);
        }
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            
            //Disable Main Camera
            Camera.main.gameObject.SetActive(false);
            
            //Set Cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("Spawned Local Player");
        }
        else
        {
            Camera playerCamera = GetComponentInChildren<Camera>();
            playerCamera.enabled = false;
            
            //Only 1 Audio Listener - When Implement
            //AudioListener audioListener = GetComponentInChildren<AudioListener>();
            //audioListener.enabled = false;
            
            Debug.Log("Spawned Remote Player");
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }
}
