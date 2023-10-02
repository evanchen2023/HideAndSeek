using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerModelRotation : NetworkTransform
{
    private Player player;
    private Camera playerCamera;
    public float turningSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
        playerCamera = player.GetCamera();
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        if (playerCamera)
        {
            if (playerCamera.enabled)
            {
                Transform camTransform = playerCamera.transform;
                //transform.rotation = playerCamera.transform.rotation;
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,
                    camTransform.rotation, turningSpeed * Runner.DeltaTime);
            }
        }
    }
}