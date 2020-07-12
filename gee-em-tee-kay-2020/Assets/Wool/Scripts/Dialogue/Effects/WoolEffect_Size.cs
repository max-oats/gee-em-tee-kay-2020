using UnityEngine;
using UnityEngine.UI;

namespace Wool
{
    [DialogueTag("size")]
    public class WoolEffect_Size : WoolDialogueEffectBase
    {
        private const float localScaleMultiplier = 1f;

        public override void Init(Letter letter)
        {
            letter.fontSize = (int)((float)letter.fontSize * strength);
        }
    }
}