using UnityEngine;
using UnityEngine.UI;

namespace Wool
{
    [DialogueTag("beetle")]
    public class WoolEffect_Beetle : WoolDialogueEffectBase
    {
        private const float defaultBeetleStrength = 0.05f;
        private const float defaultBeetleAngleRange = 1f;
        private const float beetleAngleRange = 60f;

        private const float timeLowerBound = 0.3f;
        private const float timeUpperBound = 1.0f;

        private float timeCounter = 0f;
        private float currentLerpTime = 0f;

        private Vector2 desiredPosition = Vector2.zero;
        private float desiredRotation = 0f;

        private Vector2 positionAtStartOfLerp = Vector2.zero;
        private float rotationAtStartOfLerp = 0f;
        
        public override void Init(Letter letter)
        {
            desiredPosition = Random.insideUnitCircle * defaultBeetleStrength * strength;
            desiredRotation = Random.Range(-beetleAngleRange * defaultBeetleAngleRange * strength, beetleAngleRange * defaultBeetleAngleRange * strength);
            currentLerpTime = Random.Range(timeLowerBound, timeUpperBound);
        }

        public override void Evaluate(Letter letter, float deltaTime, float fullTime)
        {
            timeCounter += deltaTime;

            // do some big lerping
            letter.transformedPosition += Vector2.Lerp(positionAtStartOfLerp, desiredPosition, timeCounter / currentLerpTime);
            letter.transformedRotation += Mathf.Lerp(rotationAtStartOfLerp, desiredRotation, timeCounter / currentLerpTime);

            /** wait! */
            if (timeCounter < (2 * currentLerpTime))
            {
                return;
            }

            timeCounter = 0f;
            currentLerpTime = Random.Range(timeLowerBound, timeUpperBound);

            desiredPosition = Random.insideUnitCircle * defaultBeetleStrength * strength;
            desiredRotation = Random.Range(-beetleAngleRange * defaultBeetleAngleRange * strength, beetleAngleRange * defaultBeetleAngleRange * strength);

            positionAtStartOfLerp = letter.transformedPosition;
            rotationAtStartOfLerp = letter.transformedRotation;
        }
    }
}