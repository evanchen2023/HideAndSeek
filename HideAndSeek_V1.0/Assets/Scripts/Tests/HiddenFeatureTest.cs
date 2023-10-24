using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class HiddenFeatureTests
{
    private GameObject testObject;
    public HiddenFeature hiddenFeature;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject();
        hiddenFeature = testObject.AddComponent<HiddenFeature>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(testObject);
    }

    //Test if the hidden state is removed after a duration of 20 seconds
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

    // Test if a cooldown period is enforced before being able to hide again
    [UnityTest]
    public IEnumerator Test_CooldownPeriod()
    {
        hiddenFeature.isInRange = true;
        hiddenFeature.isCooldown = false;
        InputSimulator.SimulateKeyDown(hiddenFeature.interactKey);
        hiddenFeature.Update();
        yield return new WaitForSeconds(20f);
        hiddenFeature.isInRange = true;  
        InputSimulator.SimulateKeyDown(hiddenFeature.interactKey);
        hiddenFeature.Update();
        Assert.IsNull(hiddenFeature.currentDisguise); 
    }

    // Test if the isInRange flag is set to true when player enters the trigger area
    [Test]
    public void Test_TriggerEnter()
    {
        var collider = new GameObject().AddComponent<BoxCollider>();
        collider.gameObject.tag = "Player";
        hiddenFeature.OnTriggerEnter(collider);
        Assert.IsTrue(hiddenFeature.isInRange);
    }

    // Test if the isInRange flag is set to false when player exits the trigger area
    [Test]
    public void Test_TriggerExit()
    {
        var collider = new GameObject().AddComponent<BoxCollider>();
        collider.gameObject.tag = "Player";
        hiddenFeature.OnTriggerExit(collider);
        Assert.IsFalse(hiddenFeature.isInRange);
    }
}
