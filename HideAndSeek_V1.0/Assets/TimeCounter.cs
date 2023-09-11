using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    private float timeLeft;
    public float time;

    public bool timerActive;
    // Start is called before the first frame update
    void Start()
    {
        timerActive = true;
        timeLeft = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Timer : 0");
                timeLeft = 0;
                timerActive = false;
            }
        }
    }

    public bool GetTimerActive()
    {
        return timerActive;
    }

    public float GetTimeLeft()
    {
        return timeLeft;
    }
}
