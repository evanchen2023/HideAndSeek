using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{

    private bool controlsActive;
    public int propsToCollect;
    public GameObject gameEndScreen;
    
    //External
    private GameObject player;
    private PlayerInventory playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        controlsActive = true;
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
    }

    public void SetPlayerInventory(PlayerInventory playerInventory)
    {
        this.playerInventory = playerInventory;
    }

    public void GameEnd()
    {
        Instantiate(gameEndScreen, GameObject.FindGameObjectWithTag("PlayerUI").transform);
        Cursor.lockState = CursorLockMode.None;
        controlsActive = false;
    }

    public bool CheckWinCondition()
    {
        if (playerInventory.NumberOfProps < propsToCollect)
        {
            return false;
        }
        return true;
    }

    public bool GetControlsActive()
    {
        return controlsActive;
    }
}
