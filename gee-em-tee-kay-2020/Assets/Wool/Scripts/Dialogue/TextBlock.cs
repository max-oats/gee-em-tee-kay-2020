using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

/** Text formats for automatically formatting text */
public enum TextFormat
{
    Standard,
    AllUpper,
    AllLower,
}

namespace Wool
{
    /**
    * TextObject
    * - Contains a List of subobjects of type "Wool Letters".
    * - Equivalent to the Unity "Text" UI class.
    */
    [System.Serializable]
    public class TextBlock
    {
        public delegate void OnTextUpdated();
        public OnTextUpdated onTextUpdated;

        [SerializeField, Socks.Field(category="Text"), TextArea]
        private string text = null;
        
        [SerializeField, Socks.Field(category="Effects")]
        private float offsetPerLetter = 0.1f;

        [SerializeField, Socks.Field(category="Formatting")]
        public Color color = Color.white;

        [SerializeField, Socks.Field(category="Formatting")]
        public bool parseText = true;

        [SerializeField, Socks.Field(category="Font")]
        private Font font = null;

        [SerializeField, Socks.Field(category="Font")]
        private int fontSize = 0;

        [SerializeField, Socks.Field(category="Character")]
        private float extraKerning = 0f;

        [SerializeField, Socks.Field(category="Paragraph")]
        public float lineSpacing = 0f;

        [SerializeField, Socks.Field(category="Paragraph")]
        public bool useBoundsForLineLength = false;
        
        [SerializeField, Socks.Field(category="Paragraph", dependOn="!useWidthAsLineLength")]
        public int lineLength = 15;

        [SerializeField, Socks.Field(category="Formatting")]
        private TextFormat textCaseFormatting = TextFormat.Standard;

        [SerializeField, Socks.Field(category="Display", dependOn="!visibleByDefault")]
        private float textSpeed = 0.05f;
        
        [SerializeField, Socks.Field(category="Display", dependOn="!visibleByDefault")]
        private Vector2 bounds = new Vector2();

        /** Privates */
        private float fontWidth = 0f;
        private float longestLine = 0f;
        private int noOfLines = 1; // The number of lines in the text object
        private string parsedString = null; // The parsed string, only used if parseText==true
        private List<Wool.Letter> letters = new List<Wool.Letter>(); // The letters, collected

        private List<(int, string)> inlineCmds = new List<(int, string)>();

        public void SetFont(Font font)
        {
            this.font = font;
        }

        public void SetFontSize(int newFontSize)
        {
            fontSize = newFontSize;
        }

        public void SetBounds(Vector2 newBounds)
        {
            bounds = newBounds;
        }

        /**
        * SetText
        * - Updates the existing text.
        */
        public void SetText(string newText, WoolTagContainer woolTags)
        {
            // Destroy existing letters
            ClearLetters();

            // Update the text to the new text
            text = newText;

            // Update text case
            if (textCaseFormatting == TextFormat.AllLower)
            {
                text = text.ToLower();
            }
            else if (textCaseFormatting == TextFormat.AllUpper)
            {
                text = text.ToUpper();
            }

            // Build new letters
            SetUpLettersFromText(woolTags);

            onTextUpdated?.Invoke();
        }

        void ClearLetters()
        {
            // Clear the letter objects
            letters.Clear();

            inlineCmds.Clear();
        }

        void SetUpLettersFromText(WoolTagContainer woolTags)
        {
            parsedString = ""; // Reset parsed string
            noOfLines = 1; // Reset number of lines
            float offset = 0f;
            
            // Listeners
            bool isListeningTag = false; // Used to check whether we are listening for the next character
            bool isListeningCommand = false;

            // todo: sort removing effects (tupes? mayb)
            List<string> activeDialogueEffects = new List<string>();
            List<string> oneshotDialogueEffects = new List<string>();

            if (woolTags == null)
            {
                woolTags = new WoolTagContainer();
            }

            if (parseText)
            {
                // Set up built tag
                string builtTag = "";

                // Iterate through the text string and create an array of characters
                foreach (char c in text)
                {
                    if (c == '\n' || c == '\r')
                    {
                        // if manual linebreak- reset everything
                        isListeningTag = false;
                        isListeningCommand = false;
                        oneshotDialogueEffects.Clear();
                        activeDialogueEffects.Clear();
                    }
                    
                    if (!isListeningTag && !isListeningCommand)
                    {
                        if (c == '[')
                        {
                            // Start listening for next character
                            isListeningTag = true;
                        }
                        else if (c == '{')
                        {
                            isListeningCommand = true;
                        }
                        else
                        {
                            // If we're not using any special character, we must be using the actual character
                            Wool.Letter letter = new Wool.Letter();

                            oneshotDialogueEffects.AddRange(activeDialogueEffects);

                            List<Wool.WoolDialogueEffectBase> effectBases = new List<WoolDialogueEffectBase>();
                            foreach (string de in oneshotDialogueEffects)
                            {
                                // Grab multiple strings
                                string[] tagWords = de.Replace(" ", "").Split(new char[]{'='}, 2);
                                
                                // Set tag name to this
                                string tagName = tagWords[0];

                                string param = null;
                                if (tagWords.Length > 1)
                                {
                                    param = tagWords[1];
                                }

                                var foundType = woolTags.GetTag(tagName);
                                var effect = (Wool.WoolDialogueEffectBase)Activator.CreateInstance(foundType);
                                effect.SetParam(param);

                                effectBases.Add(effect);
                            }

                            letter.fontSize = fontSize;
                            letter.font = font;
                            letter.Init(c.ToString(), effectBases.ToArray(), offset);
                            letters.Add(letter);

                            /** Clear dialogue effects */
                            oneshotDialogueEffects.Clear();

                            // Build up final string
                            parsedString += c;

                            offset += offsetPerLetter;
                        }
                    }
                    else
                    {
                        /** If we ARE listening for a tag */
                        if (isListeningTag && c == ']')
                        {
                            // Stop listening
                            isListeningTag = false;

                            string finalTagString = builtTag;
                            builtTag = "";

                            // Check tag and apply
                            if (finalTagString.Length == 0 
                                || (finalTagString[0] == '/' && finalTagString.Length == 1) 
                                || (finalTagString[0] == '!' && finalTagString.Length == 1))
                            {
                                // tag length = 0?
                                continue;
                            }

                            /** Decide whether we're creating a NEW effect or removing one */
                            bool setTagTo = true;
                            bool oneshot = false;
                            if (finalTagString[0] == '/')
                            {
                                setTagTo = false;
                                finalTagString = finalTagString.Remove(0, 1);
                            }
                            else if (finalTagString[0] == '!')
                            {
                                oneshot = true;
                                finalTagString = finalTagString.Remove(0, 1);
                            }

                            // Grab multiple strings
                            string[] tagWords = finalTagString.Replace(" ", "").Split(new char[]{'='}, 2);
                            
                            // Set tag name to this
                            string tagName = tagWords[0];

                            string param = null;
                            if (tagWords.Length > 1)
                            {
                                param = tagWords[1];
                            }

                            var foundType = woolTags.GetTag(tagName);
                            if (foundType == null)
                            {
                                continue;
                            }

                            if (setTagTo)
                            {
                                if (!oneshot)
                                {
                                    activeDialogueEffects.Add(finalTagString);
                                }
                                else
                                {
                                    oneshotDialogueEffects.Add(finalTagString);
                                }
                            }
                            else
                            {
                                activeDialogueEffects.Remove(activeDialogueEffects.Find(x => x.Split('=')[0] == tagName));
                            }
                        }
                        else if (isListeningCommand && c == '}')
                        {
                            // Stop listening
                            isListeningCommand = false;

                            string finalTagString = builtTag;
                            builtTag = "";

                            // Check tag and apply
                            if (finalTagString.Length == 0)
                            {
                                continue;
                            }

                            inlineCmds.Add((parsedString.Length, finalTagString));
                        }
                        else
                        {
                            // Set to lower for listening purposes
                            char listenedCharacter = char.ToLower(c);

                            // Add character to tag
                            builtTag += listenedCharacter;
                        }
                    }
                }
            }
            else
            {
                parsedString = text;

                foreach (char c in text)
                {
                    // Build character
                    Wool.Letter letter = new Wool.Letter();

                    letter.fontSize = fontSize;
                    letter.font = font;
                    letter.Init(c.ToString());
                    letters.Add(letter);
                }
            }

            CalculateLetterPositions(0, 0);
        }

        public void CalculateLetterPositions(int xOffset, int yOffset)
        {
            if (parsedString == null || parsedString.Length == 0)
            {
                return;
            }

            // Start by splitting all manual line breaks into different lines
            string[] manualLines = parsedString.Split(new char[]{'\n', '\r'});
            letters.RemoveAll(x => x.character == "\n" || x.character == "\r");

            int finalLineLength = lineLength;

            if (useBoundsForLineLength)
            {
                finalLineLength = (int)(bounds.x / fontWidth);
            }

            int totalLength = 0;

            for (int j = 0; j < manualLines.Length; ++j)
            {
                if (manualLines[j].Length == 0)
                {
                    continue;
                }

                // Step through the final string, grabbing the nearest spaces
                for (int i = finalLineLength-1; i < manualLines[j].Length; i += finalLineLength)
                {
                    int spaceIndex = manualLines[j].LastIndexOf(" ", i);

                    if (spaceIndex != -1 && spaceIndex > (i - finalLineLength))
                    {
                        // Found a space!
                        i = spaceIndex;

                        // Set to be a line break
                        letters[totalLength+i].lineBreakAfter = true;

                        // Increase number of lines
                        noOfLines++;
                    }
                    else
                    {
                        if (i != (manualLines[j].Length-1))
                        {
                            // If not found a space, just break on the line
                            letters[totalLength+i].lineBreakAfter = true;
                            noOfLines++;
                        }
                    }
                }
                
                totalLength += manualLines[j].Length;
                letters[totalLength-1].lineBreakAfter = true;
            }

            // Place letters
            Vector2 letterPlacement = Vector2.zero;
            letterPlacement.x = (xOffset + extraKerning);
            letterPlacement.y = -(yOffset*(fontSize+lineSpacing));
            longestLine = 0f;

            foreach (Wool.Letter letter in letters)
            {
                // Set new position
                letter.basePosition = letterPlacement;

                // Update X location (if real character)
                letterPlacement.x += (letter.GetAdvance() + extraKerning);
                
                if (letterPlacement.x > longestLine)
                {
                    longestLine = letterPlacement.x;
                }

                // Add linebreak if necessary
                if (letter.lineBreakAfter)
                {
                    // Update Y location
                    letterPlacement.y -= (fontSize + lineSpacing);
                    letterPlacement.x = 0f;
                }
            }
        }

        public List<string> GetInlineCommands(int index)
        {
            List<string> cmdsAtIndex = new List<string>();
            var list = inlineCmds.FindAll(x => x.Item1 == index);
            foreach (var tuple in list)
            {
                cmdsAtIndex.Add(tuple.Item2);
            }

            return cmdsAtIndex;
        }

        public Wool.Letter GetLetter(int index)
        {
            if (index < letters.Count && index >= 0)
            {
                return letters[index];
            }

            Debug.LogWarning("Attempted to get a letter with index: " + index + ", index out of range.");

            return null;
        }

        public List<Wool.Letter> GetLetters()
        {
            return letters;
        }

        public int GetLengthOfText()
        {
            return letters.Count;
        }

        public Vector2 GetSize()
        {
            return new Vector2(longestLine, noOfLines * (fontSize + lineSpacing));
        }

        public float GetTextSpeed()
        {
            return textSpeed;
        }
    }
}