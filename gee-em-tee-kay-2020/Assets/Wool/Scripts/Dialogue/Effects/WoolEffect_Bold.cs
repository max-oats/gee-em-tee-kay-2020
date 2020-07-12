using UnityEngine;
using UnityEngine.UI;

namespace Wool
{
    [DialogueTag("bold", "b")]
    public class WoolEffect_Bold : WoolDialogueEffectBase
    {
        public override void Init(Letter letter)
        {
            letter.fontStyle = letter.fontStyle == FontStyle.Italic ? FontStyle.BoldAndItalic : FontStyle.Bold;

            waitingForRemove = true;
        }
    }
}