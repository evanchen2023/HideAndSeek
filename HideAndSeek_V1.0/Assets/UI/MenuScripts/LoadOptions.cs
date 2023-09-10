using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadOptions : MonoBehaviour
{

    private Button btn;
    public GameObject optionsPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Instantiate(optionsPanel, this.transform.parent.transform.parent);
    }
}
