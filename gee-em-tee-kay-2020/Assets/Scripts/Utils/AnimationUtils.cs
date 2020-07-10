using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnimationUtils : MonoBehaviour 
{
    public static IEnumerator LerpAnimatorVariable(Animator animator, string variable, float newValue)
    {
        float timeCounter = 0f;
        float lerpTime = 0.1f;

        float current = animator.GetFloat(variable);

        while (timeCounter < lerpTime)
        {
            timeCounter += Time.deltaTime;

            animator.SetFloat(variable, Mathf.Lerp(current, newValue, timeCounter/lerpTime));

            yield return null;
        }
    }

    public static IEnumerator UpdateLayerWeightOverTime(Animator animator, int layer, float initialWeight, float desiredWeight, float time)
    {
        float timeCounter = 0f;

        while (timeCounter < time)
        {
            timeCounter += Time.deltaTime;

            animator.SetLayerWeight(layer, Mathf.Lerp(initialWeight, desiredWeight, timeCounter/time));

            yield return null;
        }
    }
}