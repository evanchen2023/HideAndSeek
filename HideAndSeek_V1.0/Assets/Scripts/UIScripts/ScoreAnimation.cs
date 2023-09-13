using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAnimation : MonoBehaviour
{
    // Play Scale Animation
    public float scaleSpeed;
    
    public void PlayAnimation()
    {
        LeanTween.cancel(gameObject);

        transform.localScale = Vector2.one;

        LeanTween.scale(gameObject, new Vector2(1.3f,1.3f), scaleSpeed)
            .setEasePunch();
    }
}
