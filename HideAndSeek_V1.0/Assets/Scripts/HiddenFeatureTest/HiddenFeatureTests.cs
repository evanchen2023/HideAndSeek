using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class HiddenFeatureTests
{
    private GameObject go;
    public HiddenFeature hiddenFeature;

    [SetUp]
    public void Setup()
    {
        go = new GameObject();
        hiddenFeature = go.AddComponent<HiddenFeature>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(go);
    }

    [Test]
    public void Test_DisguiseActivation()
    {
        hiddenFeature.isInRange = true;
        hiddenFeature.isCooldown = false;
        InputSimulator.SimulateKeyDown(KeyCode.E);
        hiddenFeature.Update();
        Assert.IsNotNull(hiddenFeature.currentDisguise);
    }

    [UnityTest]
    public IEnumerator Test_DisguiseDuration()
    {
        hiddenFeature.isInRange = true;
        hiddenFeature.isCooldown = false;
        InputSimulator.SimulateKeyDown(hiddenFeature.interactKey);
        hiddenFeature.Update();
        yield return new WaitForSeconds(20f);
        Assert.IsNull(hiddenFeature.currentDisguise);
    }

    [UnityTest]
    public IEnumerator Test_DisguiseFlicker()
    {
        hiddenFeature.isInRange = true;
        hiddenFeature.isCooldown = false;
        InputSimulator.SimulateKeyDown(hiddenFeature.interactKey);
        hiddenFeature.Update();
        yield return new WaitForSeconds(17f);
        bool isFlickering = hiddenFeature.currentDisguise.activeSelf;
        yield return new WaitForSeconds(0.5f);
        Assert.AreNotEqual(isFlickering, hiddenFeature.currentDisguise.activeSelf);
    }

    [UnityTest]
    public IEnumerator Test_CooldownPeriod()
    {
        hiddenFeature.isInRange = true;
        hiddenFeature.isCooldown = false;
        InputSimulator.SimulateKeyDown(hiddenFeature.interactKey);
        hiddenFeature.Update();
        yield return new WaitForSeconds(20f);
        hiddenFeature.isInRange = true;  // Assume player is still in range
        InputSimulator.SimulateKeyDown(hiddenFeature.interactKey);
        hiddenFeature.Update();
        Assert.IsNull(hiddenFeature.currentDisguise);  // No disguise should be toggled on during cooldown
    }

    [Test]
    public void Test_TriggerEnter()
    {
        var collider = new GameObject().AddComponent<BoxCollider>();
        collider.gameObject.tag = "Player";
        hiddenFeature.OnTriggerEnter(collider);
        Assert.IsTrue(hiddenFeature.isInRange);
    }

    [Test]
    public void Test_TriggerExit()
    {
        var collider = new GameObject().AddComponent<BoxCollider>();
        collider.gameObject.tag = "Player";
        hiddenFeature.OnTriggerExit(collider);
        Assert.IsFalse(hiddenFeature.isInRange);
    }
}
