using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TestLoadMultiplayer
{
    [Test]
    public void Test_AddLoadMultiplayerComponent()
    {
        GameObject newGameObject = new GameObject();
        newGameObject.AddComponent<LoadMultiplayer>();

        bool hasComponent = newGameObject.GetComponent<LoadMultiplayer>();
        Assert.IsTrue(hasComponent);
    }

    [Test]
    public void Test_SetSceneName()
    {
        GameObject newGameObject = new GameObject();
        newGameObject.AddComponent<LoadMultiplayer>();
        LoadMultiplayer loadMultiplayer = newGameObject.GetComponent<LoadMultiplayer>();
        String testName = "NewTest";
        loadMultiplayer.sceneName = testName;
        Assert.IsTrue(testName.Equals(loadMultiplayer.sceneName, StringComparison.Ordinal));
    }
}
