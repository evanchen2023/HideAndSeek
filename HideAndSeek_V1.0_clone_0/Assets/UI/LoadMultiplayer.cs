using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMultiplayer : MonoBehaviour
{
    private Button btn;
    void Start () {
        Button btn = GetComponent<Button>();
        btn.enabled = false;
        //btn.onClick.AddListener(TaskOnClick);
    }
    
    /*void TaskOnClick(){
        
    }*/
}
