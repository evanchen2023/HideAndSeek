using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

enum PlayerInputs{

}

public struct NetworkInputData : INetworkInput
{
    public const byte JUMPBUTTON = 0x01;

    public byte buttons;
    
    public Vector3 direction;
    public Vector2 rotationDir;
}