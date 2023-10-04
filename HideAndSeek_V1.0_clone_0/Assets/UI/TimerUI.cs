using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    //Timer Text
    private TextMeshProUGUI timerText;
    private GameObject GameManager;
    private Timer timerData;
    
    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        GameManager = GameObject.FindGameObjectWithTag("Manager");
        
        //Timer Variables
        timerData = GameManager.GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerData.GetCountDown())
        {
            UpdateTimer(timerData.GetCurrentTime());
        }
        else
        {
            UpdateTimer(-1);
        }
    }

    //Update Timer Text
    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
