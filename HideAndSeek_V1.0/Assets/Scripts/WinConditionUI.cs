using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinConditionUI : MonoBehaviour
{

    private TextMeshProUGUI winText;
    
    private WinCondition winCondition;
    
    // Start is called before the first frame update
    void Start()
    {
        winText = GetComponent<TextMeshProUGUI>();
        winCondition = GameObject.FindGameObjectWithTag("Manager").GetComponent<WinCondition>();
        
        SetWinText();
    }

    private void SetWinText()
    {
        if (winCondition.CheckWinCondition())
        {
            winText.text = "VICTORY";
        }
        else
        {
            winText.text = "DEFEAT";
        }
    }
}
