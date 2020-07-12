using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Wool
{
    /** Database encapsulates all yarn files */
    public class WoolDatabase : ScriptableObject
    {
        public List<YarnNode> nodes = new List<YarnNode>();
        public List<GameObject> prefabs = new List<GameObject>();

        private List<CommandCacher> caches = new List<CommandCacher>();

        public void Init()
        {
            foreach (GameObject prefab in prefabs)
            {
                caches.Add(prefab.GetComponent<CommandCacher>());
            }
        }

        public void LoadYarnNodes()
        {
            nodes = YarnUtils.GetAllValidYarnNodes();
        }

        public YarnNode AddNode()
        {
            List<YarnNode> untitledNodes = nodes.FindAll(x => x.title.StartsWith("NewNode"));
            return AddNode(string.Format("NewNode.{0}", untitledNodes.Count));
        }

        public YarnNode AddNode(string nodeName)
        {
            YarnNode node = new YarnNode(nodeName);
            node.fullString = "[w=0.2]type yr dialogue here!";

            nodes.Add(node);

            return node;
        }

        public CommandCacher GetCacherFromString(string objName)
        {
            CommandCacher cacher = caches.Find(x => x.name.ToLower() == objName.ToLower());
            if (cacher != null)
            {
                return cacher;
            }

            return null;
        }

        public void Save()
        {
            YarnUtils.ClearAllYarnFiles();
            foreach (YarnNode node in nodes)
            {
                YarnUtils.SaveNodeToFile(node);
            }
        }
    }
}