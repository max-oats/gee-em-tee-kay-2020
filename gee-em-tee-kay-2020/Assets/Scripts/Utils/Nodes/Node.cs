using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NodeTree
{
    public List<string> ids = new List<string>();
    public List<Node> nodes = new List<Node>();

    private string baseNode;

    public NodeTree()
    {
        Reset();
    }

    public void Reset()
    {
        ids.Clear();
        nodes.Clear();

        /** Set up base node! */
        Node node = new Node();
        baseNode = node.id;
        Add(node);
    }

    public void Add(Node node)
    {
        nodes.Add(node);
        ids.Add(node.id);
    }

    public Node Get(string id)
    {
        int idx = ids.FindIndex(x => x == id);
        if (idx == -1)
        {
            Debug.LogWarningFormat("Attempted to get node with id <i>{0}</i> but failed to find in Dictionary. Returning null.", id.ToString());
            return null;
        }

        return nodes[idx];
    }

    public Node GetBaseNode()
    {
        Node node = Get(baseNode);
        if (node == null)
        {
            Reset();
            node = Get(baseNode);
        }
        return node;
    }

    public List<Node> GetOutputs(List<string> outputs)
    {
        List<Node> nodes = new List<Node>();
        foreach (string id in outputs)
        {
            nodes.Add(Get(id));
        }

        return nodes;
    }

    public string AttachNewNode(string id)
    {
        /** Add node to existing node tree */
        Node node = new Node();
        Add(node);

        /** Add node to parent node's outputs */
        Node parentNode = Get(id);
        parentNode.outputs.Add(node.id);

        node.position = parentNode.position;

        /** Return the ID of the new node */
        return node.id;
    }
}

[System.Serializable]
public class Node
{
    public Node()
    {
        id = System.Guid.NewGuid().ToString();
    }
    
    public string id;

    public Vector3 position = Vector3.zero;

    public List<string> outputs = new List<string>();
}