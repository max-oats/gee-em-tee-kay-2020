using UnityEngine;
using UnityEngine.UI;

namespace Wool
{
    public class WoolDialogueEffectBase
    {
        /** The strength of the effect (default = 1) */
        public float strength = 1f;

        /** Is this effect finished and waiting to be removed? */
        public bool waitingForRemove = false;

        public virtual void SetParam(string param)
        {
            /** Set default parameter */
            if (float.TryParse(param, out float result))
            {
                strength = result;
            }
        }

        public virtual void Init(Letter letter)
        {
            /** init! */
        }

        public virtual void Evaluate(Letter letter, float deltaTime, float fullTime)
        {
            /** stub! */
        }
                
        public virtual void Show(Letter letter)
        {
            /** show! */
        }
    }
}