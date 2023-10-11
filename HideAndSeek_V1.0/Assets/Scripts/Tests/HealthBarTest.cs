using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class HealthBarTests
{
    [Test]
    public void SetMaxHealth()
    {
        GameObject go = new GameObject();
        HealthBar healthBar = go.AddComponent<HealthBar>();
        Assert.Throws<NullReferenceException>(() => healthBar.SetMaxHealth(100));
    }

    [Test]
    public void SetMaxHealth_SetsSliderMaxValueAndFillColorToMax()
    {
        GameObject go = new GameObject();
        HealthBar healthBar = go.AddComponent<HealthBar>();
        healthBar.slider = go.AddComponent<Slider>();
        healthBar.fill = new GameObject().AddComponent<Image>();
        healthBar.gradient = new Gradient();

        int maxHealth = 100;
        healthBar.SetMaxHealth(maxHealth);

        Assert.AreEqual(maxHealth, healthBar.slider.maxValue);
        Assert.AreEqual(maxHealth, healthBar.slider.value);
        Assert.AreEqual(healthBar.gradient.Evaluate(1f), healthBar.fill.color);
    }

}