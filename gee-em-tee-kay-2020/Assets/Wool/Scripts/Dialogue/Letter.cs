using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Wool
{
    [System.Serializable]
    public class Letter
    {
        public string character;
        public Font font;
        public Color color = Color.black;
        public int fontSize;
        public FontStyle fontStyle;
        public bool lineBreakAfter = false;
        public float delay = 0f;

        public float advance;

        public bool shouldBatch = false;

        public Vector2 basePosition;
        public float baseRotation;
        public Vector3 baseScale;

        public Vector2 transformedPosition;
        public float transformedRotation;
        public Vector3 transformedScale;

        public float fullTime = 0f;
        public float deltaTime = 0f;

        public float timeOffset = 0f;

        private List<WoolDialogueEffectBase> effects = new List<WoolDialogueEffectBase>();

        public float SetAdvance()
        {
            if (character == "\n" || character == "\r")
            {
                return 0;
            }

            if (ToString().Length == 0)
            {
                Debug.LogWarningFormat("Attempted to get the advance of a letter but failed: Text length = 0.");
                return 0;
            }

            font.RequestCharactersInTexture(ToString(), fontSize, fontStyle);
            if (!font.GetCharacterInfo(char.Parse(character), out var charInfo, fontSize, fontStyle))
            {
                Debug.LogWarningFormat("Attempted to get the advance of a letter but failed: Couldn't retrieve character info.");
                return 0;
            }

            return charInfo.advance;
        }

        public float GetAdvance()
        {
            return advance;
        }

        public Letter()
        {
            basePosition = Vector2.zero;
            baseRotation = 0f;
            baseScale = Vector3.one;
        }

        public bool HasEffects
        {
            get
            {
                return effects.Count > 0;
            }
        }

        public void ResetTransformations()
        {
            transformedPosition = Vector2.zero;
            transformedRotation = 0f;
            transformedScale = Vector3.one;
        }

        public void Init(string c)
        {
            character = c;
            
            advance = SetAdvance();
        }

        public void Init(string c, WoolDialogueEffectBase[] effects, float offset)
        {
            character = c;

            this.effects = new List<WoolDialogueEffectBase>(effects);

            if (effects.Length == 0)
            {
                shouldBatch = true;
            }

            timeOffset = offset;

            foreach (WoolDialogueEffectBase effect in effects)
            {
                effect.Init(this);
            }

            advance = SetAdvance();
        }

        public void SetColour(Color color)
        {
            this.color = color;
        }

        public void Show()
        {
            foreach (WoolDialogueEffectBase effect in effects)
            {
                effect.Show(this);
            }
        }

        public override string ToString()
        {
            return character.ToString();
        }

        public void UpdateTime(float fullTime, float deltaTime)
        {
            this.fullTime = timeOffset+fullTime;
            this.deltaTime = deltaTime;
        }

        public bool EvaluateEffects()
        {
            ResetTransformations();

            if (effects.Count == 0)
            {
                return false;
            }

            List<WoolDialogueEffectBase> effectsToRemove = new List<WoolDialogueEffectBase>();

            foreach (WoolDialogueEffectBase effect in effects)
            {
                if (effect.waitingForRemove)
                {
                    effectsToRemove.Add(effect);
                    continue;
                }

                effect.Evaluate(this, deltaTime, fullTime);
            }

            /** Remove if we're set to remove */
            foreach (WoolDialogueEffectBase effect in effectsToRemove)
            {
                effects.Remove(effect);
            }

            return true;
        }

        void AddEffect(WoolDialogueEffectBase effect)
        {
            effects.Add(effect);
        }
    }
}