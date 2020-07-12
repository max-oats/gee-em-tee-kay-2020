using UnityEngine;
using UnityEngine.UI;

namespace Wool
{
    [DialogueTag("wavey", "w")]
    public class WoolEffect_Wavey : WoolDialogueEffectBase
    {
        private const float defaultStrength = 0.3f;
        private const float defaultSpeed = 10f;

        public override void Evaluate(Letter letter, float deltaTime, float fullTime)
        {
            letter.transformedPosition += new Vector2(0f, Mathf.Sin(fullTime*defaultSpeed) * defaultStrength * strength);
        }
    }
}