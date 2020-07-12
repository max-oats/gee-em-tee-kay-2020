using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Wool
{
    public class TextObject : MonoBehaviour
    {
        /** Associated wool textblock object */
        [SerializeField, Socks.Field]
        public Wool.TextBlock text = null;
        
        /** Letter prefab */
        [SerializeField, Socks.Field(category="Prefabs")]
        private GameObject _letterObject = null;

        /** Letter object list */
        protected List<LetterObject> letterObjects = new List<LetterObject>();

        void Awake()
        {
            text.onTextUpdated += OnTextUpdated;
        }

        void OnTextUpdated()
        {
            DestroyLetterObjects();

            foreach (Letter letter in text.GetLetters())
            {
                LetterObject obj = CreateLetter(letter);

                letterObjects.Add(obj);
            }
        }

        public virtual LetterObject CreateLetter(Letter letter)
        {
            GameObject go = Instantiate(_letterObject, transform);
            LetterObject obj = go.GetComponent<LetterObject>();
            obj.ResetState();
            
            obj.letter.SetColour(text.color);

            obj.SetLetter(letter);

            return obj;
        }

        public virtual void DestroyLetterObjects()
        {
            // Destroy all existing letter objects
            foreach (LetterObject letterObject in letterObjects)
            {
                if (letterObject != null)
                {
                    Destroy(letterObject.gameObject);
                }      
            }

            foreach (Transform child in transform)
            {
                if (child.GetComponent<LetterObject>() != null)
                {
                    Destroy(child.gameObject);
                }
            }

            letterObjects.Clear();
        }
        
        public List<LetterObject> GetLetterObjects()
        {
            return letterObjects;
        }

        public void SetPosition(Vector2 newPos)
        {
            GetComponent<RectTransform>().anchoredPosition = newPos;
        }
    }
}