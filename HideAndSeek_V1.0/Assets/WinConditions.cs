using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditions : MonoBehaviour
{
    private PropCounter propCounter;
    private TimeCounter timeCounter;
    
    public GameObject endScreen;

    private bool win;
    private bool gameEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        propCounter = GetComponent<PropCounter>();
        timeCounter = GetComponent<TimeCounter>();
        gameEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnd)
        {
            if (!timeCounter.GetTimerActive())
            {
                WinCondition();
                Instantiate(endScreen);
                gameEnd = true;
            }
        }
    }

    void WinCondition()
    {
        if (propCounter.GetPropCount() > propCounter.GetPropCollected())
        {
            win = false;
        }
        else
        {
            win = true;
        }
    }

    public bool GetWin()
    {
        return win;
    }
}
