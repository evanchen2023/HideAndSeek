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
    public float rotationSpeed;
    private Vector3 moveVelocity;
    
    //Camera
    private Vector2 viewInput;
    private float camRotateX;
    private float camRotateY;
    private Camera localCamera;
    private CinemachineFreeLook cmv;
    
    void Awake()
    {
        //Character Controller
        nccp = gameObject.GetComponent<NetworkCharacterControllerPrototype>();
        playerModel = gameObject.transform.Find("Model");
        
        //Camera
        localCamera = gameObject.GetComponentInChildren<Camera>();
        cameraPosition = gameObject.transform.Find("CamPos");
        playerModelRotation = gameObject.GetComponentInChildren<PlayerModelRotation>();
        cmv = gameObject.GetComponentInChildren<CinemachineFreeLook>();
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
            //Camera Transform
            Transform cameraTransform = localCamera.transform;

            //Get Relative Movement
            Vector3 cameraRelativeMovement = PlayerRelativeMovement(cameraTransform, data);

            //Set Rotation
            PlayerRotate(cameraTransform, data);
            
            //Jump
            if ((data.buttons & NetworkInputData.JUMPBUTTON) != 0)
            {
                nccp.Jump();
            }

            //Move
            nccp.Move(playerSpeed*cameraRelativeMovement * Runner.DeltaTime);
        }
    }

    private void PlayerRotate(Transform cameraTransform, NetworkInputData data)
    {
            float verticalInput = data.direction.z;
            float horizontalInput = data.direction.x;
            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

            movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) *
                                movementDirection;
            movementDirection.Normalize();

            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Runner.DeltaTime);
            }
    }

    private Vector3 PlayerRelativeMovement(Transform cameraTransform, NetworkInputData data)
    {
            //Movement
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            
            //Get Relative Movement Vectors
            Vector3 forwardRelativeVerticalInput = data.direction.z * forward;
            Vector3 rightRelativeVerticalInput = data.direction.x * right;
            
            //Get Movement
            return forwardRelativeVerticalInput + rightRelativeVerticalInput;
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
            //Disable Other Cameras
            localCamera.enabled = false;
            cmv.enabled = false;
            
            //Only 1 Audio Listener - When Implement
            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;

            //Remove CinemachineBrain
            CinemachineBrain cmb = localCamera.GetComponent<CinemachineBrain>();
            cmb.enabled = false;

            nccp.enabled = false;
            
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
