using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Wool
{
    public class EditorLine
    {
        public string lineString = "";

        public TextBlock text = new TextBlock();
        public List<EditorLetter> letters = new List<EditorLetter>();
    }
}