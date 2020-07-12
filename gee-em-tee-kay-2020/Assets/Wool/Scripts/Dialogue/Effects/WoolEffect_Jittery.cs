using UnityEngine;
using UnityEngine.UI;

namespace Wool
{
    [DialogueTag("jitter", "jittery", "j")]
    public class WoolEffect_Jittery : WoolDialogueEffectBase
    {
        private const float defaultStrength = 0.2f;

        private float timeCounter = 0f;
        private float prevFullTime = 0f;
        private float everyTime = 0.15f;

        private Vector2 pos = Vector2.zero;

        public override void Evaluate(Letter letter, float deltaTime, float fullTime)
        {
            float dt = fullTime - prevFullTime;
            timeCounter += dt;
            prevFullTime = fullTime;

            letter.transformedPosition += pos;

            if (timeCounter > everyTime)
            {
                pos = (Random.insideUnitCircle) * defaultStrength * strength;
                timeCounter = (Random.Range(0f, 0.1f));
            }
        }
    }
}