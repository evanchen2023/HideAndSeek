using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{

    private TimeCounter timeCounter;
    private TextMeshProUGUI timerText;
        
    // Start is called before the first frame update
    void Start()
    {
        //Initialize Timer Components
        timeCounter = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<TimeCounter>();
        timerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer(timeCounter.GetTimeLeft());
    }
    
    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
