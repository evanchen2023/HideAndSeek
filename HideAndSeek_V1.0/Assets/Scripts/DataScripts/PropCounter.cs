using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PropCounter : MonoBehaviour
{
    private GameObject[] propList;
    private int propCount;
    private int propCollected;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize Prop Count
        propList = GameObject.FindGameObjectsWithTag("Prop");
        propCount = propList.Length;
        propCollected = 0;
        //Debug.Log("Prop Count : " + propCount);
    }

    public int GetPropCount()
    {
        return propCount;
    }
    public int GetPropCollected()
    {
        return propCollected;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
