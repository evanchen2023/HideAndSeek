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
    private Transform playerModel;
    private PlayerModelRotation playerModelRotation;
    private Transform cameraPosition;
    
    //Control Variables
    public float playerSpeed;
    public float jumpSpeed;
    public float viewSpeed;
    private Vector3 moveVelocity;
    
    //Camera
    private Vector2 viewInput;
    private float camRotateX;
    private float camRotateY;
    private Camera localCamera;
    private CinemachineVirtualCamera cmv;
    
    void Awake()
    {
        //Character Controller
        nccp = gameObject.GetComponent<NetworkCharacterControllerPrototype>();
        playerModel = gameObject.transform.Find("Model");
        
        //Camera
        localCamera = gameObject.GetComponentInChildren<Camera>();
        cameraPosition = gameObject.transform.Find("CamPos");
        playerModelRotation = gameObject.GetComponentInChildren<PlayerModelRotation>();
        cmv = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        
    }
    
    public override void FixedUpdateNetwork()
    {
        if (!localCamera.enabled)
            return;

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
            
            //Get View Input
            viewInput = data.rotationDir;
            
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
            localCamera.enabled = false;
            cmv.enabled = false;
            
            //Only 1 Audio Listener - When Implement
            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;
            
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

    public Camera GetCamera()
    {
        return localCamera;
    }
}
