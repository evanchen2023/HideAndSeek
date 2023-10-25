using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TestWinCondition
{
    [Test]
    public void Test_Win()
    {
        GameObject newGameObject = new GameObject();
        newGameObject.AddComponent<WinCondition>();
        newGameObject.AddComponent<PlayerInventory>();

        PlayerInventory playerInventory = newGameObject.GetComponent<PlayerInventory>();
        WinCondition winCondition = newGameObject.GetComponent<WinCondition>();
        winCondition.SetPlayerInventory(playerInventory);

        playerInventory.NumberOfProps = 2;
        winCondition.propsToCollect = 10;
        
        Assert.IsTrue(winCondition.CheckWinCondition());
    }

    [Test]
    public void Test_Lose()
    {
        GameObject newGameObject = new GameObject();
        newGameObject.AddComponent<WinCondition>();
        newGameObject.AddComponent<PlayerInventory>();

        PlayerInventory playerInventory = newGameObject.GetComponent<PlayerInventory>();
        WinCondition winCondition = newGameObject.GetComponent<WinCondition>();
        winCondition.SetPlayerInventory(playerInventory);

        playerInventory.NumberOfProps = 90;
        winCondition.propsToCollect = 10;
        
        Assert.IsFalse(winCondition.CheckWinCondition());
    }
}
