using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    //Time Variables
    public float remainingTime;
    private bool timeLeft;

    //Timer Text
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
                //Negate Delta Time for Each Update
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

    //Update Timer Text
    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
