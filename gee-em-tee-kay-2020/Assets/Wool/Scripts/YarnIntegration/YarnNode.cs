using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Wool
{
    [System.Serializable]
    public class YarnNode
    {
        public YarnNode(string nodeName)
        {
            this.title = nodeName;
        }

        public Vector2 pos = Vector2.zero;

        public string filename = "";
        public string title = "";
        public List<string> lines = new List<string>();
        public string fullString = "";
        public int hash;
    }
}