using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeInSeconds;
    private float currentTimeInSeconds;
    private bool countDown;


    private void Start()
    {
        countDown = true;
        currentTimeInSeconds = timeInSeconds;
    }

    private void Update()
    {
        SetTimer();
    }

    private void SetTimer()
    {
        if (countDown)
        {
            currentTimeInSeconds -= Time.deltaTime;
            
            if (currentTimeInSeconds <= 0)
            {
                countDown = false;
                currentTimeInSeconds = 0;
            }
        }
    }

    public bool GetCountDown()
    {
        if (currentTimeInSeconds > 0 && countDown)
        {
            return true;
        }
        return false;
    }

    public float GetCurrentTime()
    {
        if (currentTimeInSeconds > 0)
        {
            return currentTimeInSeconds;
        }

        return 0;
    }
    
}