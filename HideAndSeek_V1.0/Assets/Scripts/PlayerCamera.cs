using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;

public class PlayerCamera : NetworkTransform
{
    [SerializeField] private Transform follow;
    [SerializeField] private Vector3 standOffset = new(0.3f, 1.6f, -3f);
    [SerializeField] private Vector3 aimOffset = new(1.3f, 1.6f, -1.2f);
    [SerializeField] private float distance = 0.1f;
    [SerializeField] private float turnRate = 510f;

    private const float MAX_PITCH = 89f;
    private Vector3 back;
    private Vector3 right;
    private Matrix4x4 originTransform;
    private Quaternion rotY;
    private Quaternion rotX;
    private NetworkObject player;
    private Player playerInput;

    [SerializeField] private bool initialized = false;
    private Camera localCamera;
    
    //Camera Collision
    protected override void Awake()
    {
        //Get Local Player Input Script
        localCamera = GetComponent<Camera>();
    }

    // public override void FixedUpdateNetwork()
    // {
    //     
    // }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            //Enable Camera
            var playerList = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < playerList.Length; i++)
            {
                if (playerList[i].GetComponent<NetworkObject>().HasInputAuthority)
                {
                    player = playerList[i].GetComponent<NetworkObject>();
                    Debug.Log("Player Found : " + player);
                }
            }

            if (player)
            {
                follow = player.transform;
                playerInput = player.GetComponent<Player>();
        
                //Set Follow
                originTransform = follow.localToWorldMatrix;
                back = originTransform.MultiplyVector(Vector3.back);
                right = originTransform.MultiplyVector(Vector3.right);
        
                playerInput.SetLocalCamera(Object);
        
                Debug.Log("Camera : Added Follow");
                initialized = true;
            }
            
            localCamera.enabled = true;
            localCamera.GetComponent<AudioListener>().enabled = true;
        }
    }

    // Update is called once per frame
    public virtual void CameraUpdate(bool aimButton)
    {
        if (initialized)
        {
            var yaw = playerInput.LookEuler.y;
            var pitch = Mathf.Clamp(playerInput.LookEuler.x, -MAX_PITCH, MAX_PITCH);
            rotY = Quaternion.RotateTowards(
                rotY, Quaternion.AngleAxis(yaw, Vector3.up),
                Runner.DeltaTime * turnRate);
            rotX = Quaternion.RotateTowards(
                rotX, Quaternion.AngleAxis(pitch, right),
                Runner.DeltaTime * turnRate);

            var offset = aimButton ? aimOffset : standOffset;

            var shoulderOffset = rotY * originTransform.MultiplyVector(offset);
            var armOffset = rotY * (rotX * (distance * back));

            var shoulderPosition = follow.position + shoulderOffset;
            transform.position = shoulderPosition + armOffset;
            transform.LookAt(shoulderPosition);
        }
    }

    public virtual void SetFollow(NetworkObject networkPlayer)
    {
        //Set Player Variables
        player = networkPlayer;
        follow = player.transform;
        playerInput = player.GetComponent<Player>();
        
        //Set Follow
        originTransform = follow.localToWorldMatrix;
        back = originTransform.MultiplyVector(Vector3.back);
        right = originTransform.MultiplyVector(Vector3.right);
        
        playerInput.SetLocalCamera(Object);
        
        Debug.Log("Camera : Added Follow");
        initialized = true;
    }
    
}
