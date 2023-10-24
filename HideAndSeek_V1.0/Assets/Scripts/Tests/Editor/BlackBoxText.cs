using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BlackBoxText
{
    [UnityTest]
    public IEnumerator EnemyIsDestroyedWhenHealthIsZero()
    {
        // Arrange
        GameObject enemyGameObject = new GameObject("Enemy");
        BasicEnemyDone enemy = enemyGameObject.AddComponent<BasicEnemyDone>();
        enemy.health = 0;

        // Act
        enemy.TakeDamage(10); // Try to inflict damage, health is already 0

        // Yield to skip a frame
        yield return null;

        // Assert
        Assert.IsTrue(enemyGameObject == null, "Enemy should be destroyed when health is zero or negative.");
    }

    [UnityTest]
    public IEnumerator EnemyIsDestroyedWhenHealthIsNegative()
    {
        // Arrange
        GameObject enemyGameObject = new GameObject("Enemy");
        BasicEnemyDone enemy = enemyGameObject.AddComponent<BasicEnemyDone>();
        enemy.health = -1;

        // Act
        enemy.TakeDamage(10); // Try to inflict damage, health is already negative

        // Yield to skip a frame
        yield return null;

        // Assert
        Assert.IsTrue(enemyGameObject == null, "Enemy should be destroyed when health is zero or negative.");
    }
}
