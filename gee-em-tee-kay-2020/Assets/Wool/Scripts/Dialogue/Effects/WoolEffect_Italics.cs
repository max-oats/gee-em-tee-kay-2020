using UnityEngine;
using UnityEngine.UI;

namespace Wool
{
    [DialogueTag("italics", "i")]
    public class WoolEffect_Italics : WoolDialogueEffectBase
    {
        public override void Init(Letter letter)
        {
            letter.fontStyle = letter.fontStyle == FontStyle.Bold ? FontStyle.BoldAndItalic : FontStyle.Italic;
            
            waitingForRemove = true;
        }
    }
}