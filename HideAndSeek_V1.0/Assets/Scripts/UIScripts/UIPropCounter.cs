using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPropCounter : MonoBehaviour
{
    
    private int propCount;
    private TextMeshProUGUI propText;

    private int propCollected;

    private PropCounter propCounter;
    // Start is called before the first frame update
    void Start()
    {
        //Set Prop Text
        propText = GetComponent<TextMeshProUGUI>();
        
        //Get Prop Count
        propCounter = GameObject.FindWithTag("GameMaster").GetComponent<PropCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        propCount = propCounter.GetPropCount();
        propCollected = propCounter.GetPropCollected();
        propText.text = (propCollected + "/" + propCount.ToString("0"));
    }
}
