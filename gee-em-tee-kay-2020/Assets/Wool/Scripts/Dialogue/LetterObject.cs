using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace Wool
{
    /** Connects abstract letter stuff to unity UI system! */
    public class LetterObject : MonoBehaviour
    {
        [SerializeField]
        public Text ui_text;

        [SerializeField, Socks.Field]
        public Wool.Letter letter = null;

        [SerializeField, Socks.Field(category="Transform")]
        public RectTransform parentRect = null;

        [SerializeField, Socks.Field(category="Transform")]
        public RectTransform childRect = null;

        void Awake()
        {
            ResetState();
        }
        
        public void ResetState()
        {
            ui_text.enabled = false;
            enabled = false;
;
            parentRect.localScale = Vector3.one;
            parentRect.anchoredPosition = Vector2.zero;
            parentRect.localEulerAngles = Vector3.zero;
        }

        public void SetLetter(Wool.Letter letter)
        {
            this.letter = letter;

            ui_text.text = letter.ToString();

            ui_text.font = letter.font;
            ui_text.fontStyle = letter.fontStyle;
            ui_text.fontSize = letter.fontSize;
            ui_text.color = letter.color;

            parentRect.anchoredPosition = letter.basePosition;
            parentRect.localEulerAngles = new Vector3(parentRect.localEulerAngles.x, parentRect.localEulerAngles.y, letter.baseRotation);
            parentRect.localScale = letter.baseScale;
        }

        public void LateUpdate()
        {
            letter.UpdateTime(Time.time, Time.deltaTime);
            letter.EvaluateEffects();

            childRect.anchoredPosition = (letter.transformedPosition / (float)letter.fontSize);
            childRect.localEulerAngles = new Vector3(parentRect.localEulerAngles.x, parentRect.localEulerAngles.y, letter.transformedRotation);
            childRect.localScale = letter.transformedScale;
        }

        public virtual void Show()
        {
            ui_text.enabled = true;

            /** If we have any effects, enable updates. If not, call it there :D */
            if (letter.HasEffects)
            {
                enabled = true;
            }

            letter.Show();
            
            parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, letter.fontSize);
            parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, letter.fontSize);
        }
    }
}