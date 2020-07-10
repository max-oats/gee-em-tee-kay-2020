using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProcMeshGenBase : MonoBehaviour
{
    [SerializeField, Socks.Field(category="Sub-Meshes")] 
    public List<ProcMesh.Submesh> subMeshes = new List<ProcMesh.Submesh>();

    [SerializeField, Socks.Field(category="Materials")]
    public Material material = null;

    /** The actual node tree that creates the coral */
    public NodeTree tree = new NodeTree();
    protected MeshBuilder builder = new MeshBuilder();
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public virtual void GenerateNode(MeshBuilder builder, 
                                        string rootNodeID, 
                                        Vector3 position, 
                                        Quaternion rotation, 
                                        int depth, 
                                        int submeshDepth) {}
    
    public virtual void GenerateSubMesh(ProcMesh.Submesh submesh, 
                                        MeshBuilder builder, 
                                        string rootNodeID, 
                                        Vector3 position, 
                                        Quaternion rotation, 
                                        int submeshDepth) 
    {
        int numberOfMeshes = (int)(submesh.numberOfMeshes.Evaluate(0f, Random.value));
        
        for (int i = 0; i < numberOfMeshes; ++i)
        {
            if (Random.value <= submesh.chanceOfGenerating.Evaluate(0f, Random.value))
            {
                submesh.mesh.GenerateNode(builder, submesh.mesh.tree.GetBaseNode().id, position, rotation, 0, submeshDepth);
            }
        }
    }

    public void Init()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public virtual void BeginMeshGen()
    {
        builder = new MeshBuilder();
        tree = new NodeTree();

        tree.Reset();

        /** Grab base node */
        Node baseNode = tree.GetBaseNode();

        GenerateNode(builder, baseNode.id, Vector3.zero, Quaternion.identity, 0, 0);
    }
}