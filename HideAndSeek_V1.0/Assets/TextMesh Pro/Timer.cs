using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float currentTimeInSeconds; 
    public float timerLimitInSeconds; 
    public bool countDown;
    public bool hasLimit;
    public bool hasFormat;
    public TimerFormats format;
    private Dictionary<TimerFormats, string> timeFormats = new Dictionary<TimerFormats, string>();
    

    private void Start()
    {
        timeFormats.Add(TimerFormats.Seconds, "0");
        timeFormats.Add(TimerFormats.Minutes, "0:00");
        timeFormats.Add(TimerFormats.TenthDecimal, "0.0");
        timeFormats.Add(TimerFormats.HundredthsDecimal, "0.00");

        if (!countDown)
        {
            currentTimeInSeconds = 0f;
            SetTimerText();
        }
    }

    private void Update()
    {
        if (countDown)
        {
            currentTimeInSeconds -= Time.deltaTime;

           
            if (currentTimeInSeconds <= 0)
            {
                currentTimeInSeconds = 0;
                SetTimerText();
                timerText.color = Color.red;
                enabled = false;

                // 加载结束界面的场景（假设结束界面的场景名称为 "EndScene"）
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
        {
            currentTimeInSeconds += Time.deltaTime;
        }

        SetTimerText();
    }

    private void SetTimerText()
    {
        string timeText = "";
        if (hasFormat)
        {
            switch (format)
            {
                case TimerFormats.Seconds:
                    timeText = Mathf.RoundToInt(currentTimeInSeconds).ToString("0");
                    break;
                case TimerFormats.Minutes:
                    float minutes = Mathf.Floor(currentTimeInSeconds / 60);
                    float seconds = Mathf.RoundToInt(currentTimeInSeconds % 60);
                    timeText = minutes.ToString("0") + ":" + seconds.ToString("00");
                    break;
                case TimerFormats.TenthDecimal:
                    timeText = currentTimeInSeconds.ToString("0.0");
                    break;
                case TimerFormats.HundredthsDecimal:
                    timeText = currentTimeInSeconds.ToString("0.00");
                    break;
            }
        }
        else
        {
            timeText = Mathf.RoundToInt(currentTimeInSeconds).ToString("0");
        }

        timerText.text = timeText;
    }

    public enum TimerFormats
    {
        Seconds,
        Minutes,
        TenthDecimal,
        HundredthsDecimal
    }
}