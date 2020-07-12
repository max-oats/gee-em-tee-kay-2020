using UnityEngine;
using UnityEditor;

namespace Wool
{
    [System.Serializable]
    public class EditorLetter
    {
        public Letter letter;
        public Rect rect;
        public GUIStyle style;

        private float prevTime = 0f;
        private float deltaTime = 0f;
        private int noOfHits = 0;

        void OnEnable()
        {
            prevTime = Time.realtimeSinceStartup;
        }

        public void Draw(float rightPadding = 0f)
        {
            if (letter.transformedRotation != 0f)
            {
                GUIUtility.RotateAroundPivot(letter.transformedRotation, new Vector2(rect.x + (letter.GetAdvance()/2f), rect.y + (letter.fontSize/2f)));
            }
            GUI.Label(new Rect(rect.x+rightPadding, rect.y, rect.width, rect.height), letter.character, style);
            if (letter.transformedRotation != 0f)
            {
                GUIUtility.RotateAroundPivot(-letter.transformedRotation, new Vector2(rect.x + (letter.GetAdvance()/2f), rect.y + (letter.fontSize/2f)));
            }
        }

        public void SetLetter(Letter letter)
        {
            this.letter = letter;

            style = new GUIStyle(EditorStyles.label);
            style.font = letter.font;
            style.fontSize = letter.fontSize;
            style.fontStyle = letter.fontStyle;
            style.normal.textColor = letter.color;
        }

        public void Update()
        {
            noOfHits++;
            if (noOfHits == 10)
            {
                noOfHits = 0;
                deltaTime = (Time.realtimeSinceStartup - prevTime);
                prevTime = Time.realtimeSinceStartup;
            }

            letter.UpdateTime(Time.realtimeSinceStartup, deltaTime);
            letter.EvaluateEffects();

            rect.x = letter.basePosition.x + (letter.transformedPosition.x * (float)letter.fontSize);
            rect.y = (-letter.basePosition.y) + (letter.transformedPosition.y  * (float)letter.fontSize);

            rect.width = letter.fontSize*2f;
            rect.height = letter.fontSize*2f;
        }
    }
}