using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadQuit : MonoBehaviour
{
    private Button btn;
    void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick(){
        Application.Quit();
        Debug.Log("Game Quit.");
    }
}
