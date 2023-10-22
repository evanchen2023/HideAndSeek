using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public const byte JUMPBUTTON = 0x01;
    public const byte SHOOTBUTTON = 0x02;

    public byte buttons;
    public byte shootButtons;
    public bool sprintButton;
    public bool aimButton;
    public bool shootButton;
    
    public Vector3 direction;
    public Vector2 rotationDir;
}