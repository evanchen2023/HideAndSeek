using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public const byte JUMPBUTTON = 0x01;

    public byte buttons;
    public bool sprintButton;
    
    public Vector3 direction;
    public Vector2 rotationDir;
}