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

    private WinCondition winCondition;
    public TextMeshProUGUI timerText; 
    public Color warningColor = Color.red;
    

    private void Start()
    {
        winCondition = GetComponent<WinCondition>();
        
        countDown = true;
        currentTimeInSeconds = timeInSeconds;
    }

    private void Update()
    {
        SetTimer();
        UpdateTimerText();
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
                SetEndGame();
            }
        }
    }

    private void SetEndGame()
    {
        winCondition.GameEnd();
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            currentTimeInSeconds -= Time.deltaTime;
            currentTimeInSeconds = Mathf.Max(currentTimeInSeconds, 0); // Ensure it doesn't go negative

            // Calculate minutes and seconds
            int minutes = Mathf.FloorToInt(currentTimeInSeconds / 60);
            int seconds = Mathf.FloorToInt(currentTimeInSeconds % 60);

            // Convert to a formatted string
            string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Change the text color during the last 10 seconds
            if (currentTimeInSeconds <= 10)
            {
                timerText.color = Color.red; // Change the text color to red
            }
            else
            {
                // Reset the text color to its default
                timerText.color = Color.white; // Set to your desired default color
            }

            timerText.text = timeText; // Display the time in the "00:00" format
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