using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public float remainingTime;
    private bool timeLeft;

    public TextMeshProUGUI timerText;
    
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimer(remainingTime);
            }
            else
            {
                remainingTime = 0;
                timeLeft = false;
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerAnimation()
    {
        
    }
}
