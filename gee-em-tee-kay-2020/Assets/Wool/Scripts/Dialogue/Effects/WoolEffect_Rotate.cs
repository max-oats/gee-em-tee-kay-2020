using UnityEngine;
using UnityEngine.UI;

namespace Wool
{
    [DialogueTag("rotate")]
    public class WoolEffect_Rotate : WoolDialogueEffectBase
    {
        public float defaultStrength = 30f;

        public override void Evaluate(Letter letter, float deltaTime, float fullTime)
        {
            letter.transformedRotation += (fullTime * defaultStrength * strength);
        }
    }
}