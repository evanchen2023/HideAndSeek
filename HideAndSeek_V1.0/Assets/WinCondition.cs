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
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        controlsActive = true;
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
        animator = player.GetComponent<Animator>();
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
            DefeatAnimation();
            return false;
        } else {
            VictoryAnimation();
            return true;
        }
    }

    private void VictoryAnimation()
    {
        animator.SetBool("IsWin", true);
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
    }

    private void DefeatAnimation(){
        animator.SetBool("IsLose", true);
    }

    public bool GetControlsActive()
    {
        return controlsActive;
    }
}
