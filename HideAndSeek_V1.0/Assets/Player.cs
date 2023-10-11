using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Object = System.Object;

public class Player : NetworkBehaviour, IPlayerLeft
{
    //Player Variables
    public static Player Local { get; set; }
    private NetworkCharacterControllerPrototype nccp;
    
    //Control Variables
    public float playerSpeed;
    private Vector3 moveVelocity;
    
    //Camera
    private Vector2 viewInput;
    private float camRotateX;
    private float camRotateY;
    private Camera localCamera;
    private Transform localCameraTransform;
    private const int MAX_ANGLE = 360;
    private Vector3 lookEuler;
    [SerializeField] private float lookSensitivity = 135f;

    private NetworkObject networkCamera;
    private PlayerCamera cameraInput;
    [SerializeField] private bool initialized = false;
    
    public Vector3 LookEuler
    {
        get => lookEuler;
        private set
        {
            lookEuler = value;
            lookEuler.y %= MAX_ANGLE;
            lookEuler.x %= MAX_ANGLE;
        }
    }
    
    void Awake()
    {
        //Character Controller
        nccp = gameObject.GetComponent<NetworkCharacterControllerPrototype>();
    }

    private Vector3 PlayerRelativeMovement(NetworkInputData data)
    {
        //Movement
        Vector3 forward = localCameraTransform.forward;
        Vector3 right = localCameraTransform.right;
            
        //Get Relative Movement Vectors
        Vector3 forwardRelativeVerticalInput = data.direction.z * forward;
        Vector3 rightRelativeVerticalInput = data.direction.x * right;
            
        //Get Movement
        return forwardRelativeVerticalInput + rightRelativeVerticalInput;
    }

    public override void FixedUpdateNetwork()
    {
        if (!initialized)
            return;

        if (GetInput(out NetworkInputData data))
        {
            //Input
            float verticalInput = data.direction.z;
            float horizontalInput = data.direction.x;
            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            
            cameraInput.CameraUpdate();
            movementDirection = Quaternion.AngleAxis(localCameraTransform.rotation.eulerAngles.y, Vector3.up) *
                                movementDirection;
            movementDirection.Normalize();
            
            if (movementDirection != Vector3.zero)
            {
                nccp.Rotate(movementDirection);
            }
            
            //Camera
            lookEuler += Runner.DeltaTime * lookSensitivity *
                         new Vector3(data.rotationDir.y, data.rotationDir.x, 0);
            
            //Jump
            if ((data.buttons & NetworkInputData.JUMPBUTTON) != 0)
            {
                nccp.Jump();
            }

            //Move
            //Relative Movement
            Vector3 cameraRelativeMovement = PlayerRelativeMovement(data);
            nccp.Move(playerSpeed*cameraRelativeMovement * Runner.DeltaTime);
        }
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    public void SetLocalCamera(NetworkObject networkObject)
    {
        networkCamera = networkObject;
        localCamera = networkCamera.GetComponent<Camera>();
        localCameraTransform = localCamera.transform;
        cameraInput = networkCamera.GetComponent<PlayerCamera>();

        Debug.Log("Player : Set Local Camera");
        initialized = true;
    }
}