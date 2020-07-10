using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcMesh;
using static UnityEngine.ParticleSystem;
using UnityEditor;

public class SimpleCylinderGenerator : ProcMeshGenBase
{
    [Socks.Field(category="Size")]
    public MinMaxCurve radius;
   
    [Socks.Field(category="Height")]
    public MinMaxCurve height;

    [Socks.Field(category="Quality")]
    public int lengthSegments = 10;
    
    [Socks.Field(category="Quality")]
    public int radialSegments = 10;

    void Reset()
    {
        BeginMeshGen();
    }

    public override void BeginMeshGen()
    {
        // todo: error handling?
        base.BeginMeshGen();

        //Debug.LogFormat("Creating a new procmesh. No. of tree nodes: {0}", tree.nodes.Count);
        meshFilter.sharedMesh = builder.CreateMesh();
        meshRenderer.sharedMaterials = builder.materials.ToArray();
    }

    public override void GenerateNode(MeshBuilder builder, string rootNodeID, Vector3 position, Quaternion rotation, int depth, int submeshDepth)
    {
        if (depth == 0)
        {
            foreach (Submesh submesh in subMeshes.FindAll(x => x.startPoint == SubmeshStartPoints.RootOfNode))
            {
                GenerateSubMesh(submesh, builder, tree.GetBaseNode().id, position, rotation, submeshDepth+1);
            }
        }

        /** Attach new node to tree*/
        string newNodeID = tree.AttachNewNode(rootNodeID);

        /** Set up cylinder data! */
        ProcMesh.CylinderData cylinder = new ProcMesh.CylinderData();
        cylinder.lengthSegments = lengthSegments;
        cylinder.radialSegments = radialSegments;
        cylinder.radius = radius;
        cylinder.height = height.Evaluate(0f, Random.value);

        /** Build tube */
        vertex[] vertices = MeshUtils.BuildCylinder(builder, builder.GetSubmesh(material), position, rotation, cylinder);

        foreach (ProcMesh.Submesh submesh in subMeshes.FindAll(x => x.startPoint == SubmeshStartPoints.AllOverVertices))
        {
            for (int j = 0; j < vertices.Length; ++j)
            {
                if (Random.value > 0.5f)
                {
                    GenerateSubMesh(submesh, builder, newNodeID, vertices[j].index, Quaternion.Euler(vertices[j].normal), submeshDepth+1);
                }
            }
        }
    }
}