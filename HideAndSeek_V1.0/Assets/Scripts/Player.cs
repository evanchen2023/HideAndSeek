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

    //Camera
    private Vector2 viewInput;
    private Camera localCamera;
    private Transform localCameraTransform;
    private const int MAX_ANGLE = 360;
    private Vector3 lookEuler;
    [SerializeField] private float lookSensitivity = 135f;

    private NetworkObject networkCamera;
    private PlayerCamera cameraInput;
    [SerializeField] private bool initialized = false;

    //Shooting
    public NetworkPrefabRef bulletPrefab; 
    public Transform bulletSpawnPoint;
    
    //Animator
    Animator animator;
    private bool jumping = false;
    private bool throwing = false;
    private float inputMagnitude = 0;
    
    //Team
    public bool seeker;
    
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
        //Animation Controller
        animator = gameObject.GetComponent<NetworkMecanimAnimator>().Animator;
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
        if (Runner.IsForward == false)
            return;
        
        if (initialized)
        {
            if (GetInput(out NetworkInputData data))
            {
                //Input
                float verticalInput = data.direction.z;
                float horizontalInput = data.direction.x;
                Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
                Vector3 movementMagnitude = movementDirection;
                
                //Set Direction if Aiming
                if (data.aimButton)
                {
                    movementDirection = new Vector3(0, 0, 1);
                }
                
                //Update Camera and Movement Rotation
                cameraInput.CameraUpdate(data.aimButton);
                movementDirection = Quaternion.AngleAxis(localCameraTransform.rotation.eulerAngles.y, Vector3.up) *
                                    movementDirection;
                movementDirection.Normalize();

                if (movementDirection != Vector3.zero)
                {
                    nccp.Rotate(movementDirection);
                }

                //Update for Camera
                lookEuler += Runner.DeltaTime * lookSensitivity *
                             new Vector3(data.rotationDir.y, data.rotationDir.x, 0);
            
                //Jump
                if ((data.buttons & NetworkInputData.JUMPBUTTON) != 0)
                {
                    nccp.Jump();
                    jumping = true;
                }

                //Move
                //Relative Movement
                Vector3 cameraRelativeMovement = PlayerRelativeMovement(data);
                nccp.Move(cameraRelativeMovement * Runner.DeltaTime, data.sprintButton);

                //Movement Animation
                inputMagnitude = Mathf.Clamp01(movementMagnitude.magnitude);
                if (!data.sprintButton)
                {
                    inputMagnitude /= 2;
                }
                
                //Shooting - Can Only Shoot if Seeker
                if (seeker)
                {
                    if (data.shootButton)
                    {
                        Fire();
                    }
                }
            }
        }
        
        //Animations - Only Seeker Needs to be Animated
        if (seeker)
        {
            //Jump Animation
            animator.SetBool("IsJump", jumping);
            jumping = false;
     
            //Shooting Animation
            animator.SetBool("IsThrow", throwing);
            throwing = false;
        
            //Running Animation
            animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Runner.DeltaTime);
        }
    }
    
    void Fire()
    {
        //Spawn Bullet using Runner
        Runner.Spawn(bulletPrefab, bulletSpawnPoint.position, localCameraTransform.rotation);
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
