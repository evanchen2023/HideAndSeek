using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI propsText;
    //private ScoreAnimation scoreAnimation;
    
    void Start()
    {
        propsText = GetComponent<TextMeshProUGUI>();
        //scoreAnimation = GetComponent<ScoreAnimation>();
    }

    public void UpdatePropsText(PlayerInventory playerInventory)
    {
        propsText.text = playerInventory.NumberOfProps.ToString(); //get the number from PlayerInventory 
                                                                //and change it to the string
        //scoreAnimation.PlayAnimation();
    }
}
