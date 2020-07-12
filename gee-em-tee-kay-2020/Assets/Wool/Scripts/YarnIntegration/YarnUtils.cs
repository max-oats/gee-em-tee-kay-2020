using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Wool
{
    public static class YarnUtils
    {
        private const string titleTag = "title:";
        private const string tagsTag = "tags:";
        private const string nodeStartString = "---";
        private const string nodeEndString = "===";

        private const string assetSearch = "t:YarnProgram";

        public static List<YarnNode> GetAllValidYarnNodes()
        {
            List<YarnNode> nodes = new List<YarnNode>();

            string dir = Application.dataPath + "/";
            string[] filePaths = Directory.GetFiles(dir, "*.yarn", SearchOption.AllDirectories);
            Debug.LogFormat("Loaded {0} <i>.yarn</i> files from directory '{1}'", filePaths.Length, dir);

            /** For each file path */
            foreach (string filePath in filePaths)
            {
                nodes.AddRange(GetNodesFromFile(filePath));
            }

            /** Success! */
            Debug.LogFormat("Yarn files loaded successfully. <i>Loaded {0} nodes.</i>", nodes.Count);
            return nodes;
        }

        public static void ClearAllYarnFiles()
        {
            string dir = Application.dataPath + "/";
            string[] filePaths = Directory.GetFiles(dir, "*.yarn", SearchOption.AllDirectories);

            foreach (string filePath in filePaths)
            {
                File.WriteAllText(filePath, "");
            }
        }

        public static void SaveNodeToFile(YarnNode node)
        {
            /** Load streamreader */
            StreamWriter writer = new StreamWriter(node.filename, true);
            writer.WriteLine(string.Format("title: {0}", node.title));
            writer.WriteLine("---");
            writer.WriteLine(node.fullString);
            writer.WriteLine("===");
            writer.WriteLine("");

            writer.Close();
        }

        public static List<YarnNode> GetNodesFromFile(string filename)
        {
            List<YarnNode> nodesInFile = new List<YarnNode>();

            /** Load streamreader */
            StreamReader reader = new StreamReader(filename);

            /** Set the current node that we're loading to null.!-- While its null, we're looking for a title */
            YarnNode currentNode = null;
            bool parsingNodeText = false;

            /** Load first line! */
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                /** If we don't currently have a node, look for a "title:" tag */
                if (currentNode == null)
                {
                    if (line.StartsWith(titleTag))
                    {
                        if (line.Length <= titleTag.Length)
                        {
                            Debug.LogWarningFormat("Node has no title.");
                            continue;
                        }

                        string title = line.Substring(titleTag.Length).Trim();
                        if (title.Length == 0)
                        {
                            Debug.LogWarningFormat("Node has no title.");
                            continue;
                        }

                        /** We've found our title- lets move on */
                        currentNode = new YarnNode(title);
                    }
                    
                    /** If its not a title, move on til we find one */
                    continue;
                }

                /** if we're this far, currentnode must be not null. Now we're looking for a '---' */
                /** todo: add tag support! */
                if (!parsingNodeText)
                {
                    if (line.StartsWith(nodeStartString))
                    {
                        parsingNodeText = true;
                    }

                    continue;
                }

                /** Now we're just grabbing lines */
                if (line.StartsWith(nodeEndString))
                {
                    currentNode.filename = filename;
                    currentNode.fullString = currentNode.fullString.Remove(currentNode.fullString.Length-1);
                    nodesInFile.Add(currentNode);

                    currentNode = null;
                    parsingNodeText = false;

                    continue;
                }

                currentNode.lines.Add(line);
                currentNode.fullString += line + "\n";
            }


            if (currentNode != null)
            {
                Debug.LogWarningFormat("Node {0} not closed properly.", currentNode.title);
            }

            reader.Close();

            return nodesInFile;
        }
    }
}