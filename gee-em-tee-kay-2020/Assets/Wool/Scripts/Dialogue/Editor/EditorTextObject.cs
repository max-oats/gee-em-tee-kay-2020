using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Wool
{
    [System.Serializable]
    public class EditorTextObject
    {
        public List<EditorLine> lines = new List<EditorLine>();
        
        private Color chevronClr = new Color(0.6f, 0.7f, 0.8f, 1f);
        private Color commentClr = new Color(0.4f, 0.4f, 0.4f, 1f);
        private Color objClr = new Color(0.5f, 0.2f, 0.2f, 1f);
        private Color cmdClr = new Color(0.6f, 0.35f, 0.2f, 1f);
        private Color prmClr = new Color(0.6f, 0.5f, 0.2f, 1f);

        private const string openChevronString = "<<";
        private const string closeChevronString = ">>";
        private const string commentString = "//";

        private const string shortBranchString = "->";
        private const string shortBranchReplacement = " [b]⇒[/b] ";

        private const string fullBranchOpenString = "[[";
        private const string fullBranchCloseString = "]]";

        private WoolTagContainer woolTags;

        public EditorTextObject()
        {
            lines = new List<EditorLine>();
            woolTags = new WoolTagContainer();
        }

        public void InsertNewLine(int index)
        {
            TextBlock newLine = new TextBlock();
            newLine.parseText = true;
            newLine.lineLength = 1000;
            newLine.lineSpacing = 2f;
        }

        public void RemoveLine(int index)
        {
            
        }

        public void UpdateLine(int index)
        {

        }

        public void ParseText(string text)
        {
            lines.Clear();
            string[] textLines = text.Split(new char[]{'\n', '\r'});

            /** Set up text line-by-line! */
            foreach (string line in textLines)
            {
                lines.Add(ParseLine(line));
            }
        }

        public EditorLine ParseLine(string line)
        {
            EditorLine lineObj = new EditorLine();
            lineObj.text = new TextBlock();
            lineObj.text.lineLength = 1000;
            lineObj.text.lineSpacing = 2f;

            string trimmedLine = line.Trim();
            if (trimmedLine.StartsWith(openChevronString) && trimmedLine.EndsWith(closeChevronString))
            {
                ParseCommand(lineObj, line);
            }
            else if (trimmedLine.StartsWith(commentString))
            {
                ParseComment(lineObj, line);
            }
            else if (trimmedLine.StartsWith(shortBranchString))
            {
                ParseBranch(lineObj, line);
            }
            else
            {
                ParseDialogue(lineObj, line);
            }

            lineObj.text.CalculateLetterPositions(0, lines.Count);

            return lineObj;
        }

        void ParseBranch(EditorLine lineObj, string line)
        {
            lineObj.text.parseText = true;
            SetText(lineObj, line.Replace(shortBranchString, shortBranchReplacement));
        }

        void SetText(EditorLine lineObj, string newText)
        {
            lineObj.text.SetFont(GUI.skin.label.font);
            lineObj.text.SetFontSize(GUI.skin.label.fontSize);

            lineObj.text.SetText(newText, woolTags);

            foreach (Letter letter in lineObj.text.GetLetters())
            {
                EditorLetter editorLetter = new EditorLetter();
                editorLetter.SetLetter(letter);
                lineObj.letters.Add(editorLetter);
            }
        }

        public void ParseDialogue(EditorLine lineObj, string dialogue)
        {
            lineObj.text.parseText = true;
            SetText(lineObj, dialogue);
        }

        public void ParseCommand(EditorLine lineObj, string line)
        {
            lineObj.text.parseText = false;
            SetText(lineObj, line);

            int indexOfOpeningChevrons = line.IndexOf(openChevronString);
            int indexOfClosingChevrons = line.IndexOf(closeChevronString);

            lineObj.letters[indexOfOpeningChevrons].style.normal.textColor = (chevronClr);
            lineObj.letters[indexOfOpeningChevrons+1].style.normal.textColor = (chevronClr);
            lineObj.letters[indexOfClosingChevrons].style.normal.textColor = (chevronClr);
            lineObj.letters[indexOfClosingChevrons+1].style.normal.textColor = (chevronClr);
            bool obj = true;
            bool cmd = false;
            bool prm = false;
            for (int i = indexOfOpeningChevrons+2; i < indexOfClosingChevrons; ++i)
            {
                if (i==2 && char.IsLower(char.Parse(lineObj.letters[i].letter.character)))
                {
                    obj = false;
                    cmd = true;
                }

                if (obj)
                {
                    lineObj.letters[i].style.normal.textColor = (objClr);
                    if (lineObj.letters[i].letter.character==" ")
                    {
                        obj = false;
                        cmd = true;
                    }
                }
                else if (cmd)
                {
                    lineObj.letters[i].style.normal.textColor = (cmdClr);
                    if (lineObj.letters[i].letter.character==" ")
                    {
                        cmd = false;
                        prm = true;
                    }
                }
                else if (prm)
                {
                    lineObj.letters[i].style.normal.textColor = (prmClr);
                }
            }
        }

        public void ParseComment(EditorLine lineObj, string comment)
        {
            lineObj.text.parseText = false;
            SetText(lineObj, comment);

            foreach (EditorLetter letter in lineObj.letters)
            {
                letter.style.fontStyle = FontStyle.Italic;
                letter.style.normal.textColor = commentClr;
            }
        }

        public void SetBounds(float width, float height)
        {
            foreach (EditorLine line in lines)
            {
                line.text.SetBounds(new Vector2(width, height));
            }
        }

        public void UpdateContainer(float width, float height)
        {
            SetBounds(width, height);
        }

        public void DrawLineAndBatch(EditorLine line, float rightPadding, Rect drawRect)
        {
            if (line.letters.Count == 0)
            {
                // early out
                return;
            }

            if ((drawRect.y+drawRect.height) < line.letters[0].rect.y || (drawRect.y-20f > line.letters[0].rect.y))
            {
                // occlusion cull
                return;
            }

            EditorLetter batchedLetter = null;
            bool finishedBatching = false;
            string batched = "";
            for (int i = 0; i < line.letters.Count; ++i)
            {
                if (!line.letters[i].letter.shouldBatch)
                {
                    finishedBatching = true;

                    line.letters[i].Draw(rightPadding);
                    continue;
                }

                if (finishedBatching)
                {
                    line.letters[i].Draw(rightPadding);
                    continue;
                }

                if (batched.Length == 0)
                {
                    batchedLetter = line.letters[i];
                }

                batched += line.letters[i].letter.character;
            }

            if (batchedLetter != null)
            {
                Rect rect = new Rect(batchedLetter.rect);
                rect.x += rightPadding;
                rect.width = rect.width * batched.Length;
                GUI.Label(rect, batched, batchedLetter.style);
            }
        }
    }
}