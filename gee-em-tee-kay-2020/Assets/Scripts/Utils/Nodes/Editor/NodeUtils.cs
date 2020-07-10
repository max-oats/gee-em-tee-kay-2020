using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NodeUtils
{
    /** Put in DuringSceneGUI! */
    public static void DrawTree(NodeTree tree, Vector3 rootPoint, Quaternion rotation)
    {
        Node initialNode = tree.GetBaseNode();

        DrawNode(tree, initialNode, rootPoint, rotation);
    }

    static void DrawNode(NodeTree tree, Node node, Vector3 rootPoint, Quaternion rotation)
    {
        Handles.SphereHandleCap(GUIUtility.GetControlID(FocusType.Passive), 
                                Socks.Utils.TransformPoint(node.position, rootPoint, rotation), rotation, 0.1f, EventType.Repaint);

        foreach (string id in node.outputs)
        {
            Node node2 = tree.Get(id);
            DrawNode(tree, tree.Get(id), rootPoint, rotation);
            DrawBetween(node, node2, rootPoint, rotation);
        }
    }

    static void DrawBetween(Node n1, Node n2, Vector3 rootPoint, Quaternion rotation)
    {
        Vector3 dist = n2.position-n1.position;
        Handles.DrawBezier(Socks.Utils.TransformPoint(n1.position, rootPoint, rotation)
                        , Socks.Utils.TransformPoint(n2.position, rootPoint, rotation)
                        , Socks.Utils.TransformPoint(n1.position + new Vector3(dist.x, 0f, dist.z), rootPoint, rotation)
                        , Socks.Utils.TransformPoint(n2.position - new Vector3(0f, dist.y, 0f), rootPoint, rotation)
                        , Color.white, null, 2f);
    }
}