using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTarget : MonoBehaviour
{
    private Camera localCamera;
    // Start is called before the first frame update
    void Start()
    {
        localCamera = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = localCamera.transform.rotation;
    }
}
