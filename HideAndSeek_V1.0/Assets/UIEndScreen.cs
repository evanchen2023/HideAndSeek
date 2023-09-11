using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIEndScreen : MonoBehaviour
{

    private TextMeshProUGUI endText;

    private WinConditions winConditions;
    
    // Start is called before the first frame update
    void Start()
    {
        endText = GetComponent<TextMeshProUGUI>();
        winConditions = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<WinConditions>();
        SetEndText();
    }

    void SetEndText()
    {
        if (winConditions.GetWin())
        {
            endText.text = "Victory";
        }
        else
        {
            endText.text = "Defeat";
        }
    }
}
