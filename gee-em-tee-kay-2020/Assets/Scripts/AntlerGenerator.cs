using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcMesh;
using static UnityEngine.ParticleSystem;
using UnityEditor;

public class AntlerGenerator : ProcMeshGenBase
{
    [Socks.Field(category="Growth")]
    public float initialChanceOfGrowth = 1.5f;

    [Socks.Field(category="Growth")]
    public float growthChanceReduction = 0.1f;
    
    [Socks.Field(category="Growth")]
    public int maximumGrowths = 5;

    [Socks.Field(category="Growth")]
    public int minimumInitialGrowths = 2;
    
    [Socks.Field(category="Size")]
    public MinMaxCurve radius;
    
    [Socks.Field(category="Size")]
    public float sizeReduction = 0.1f;
    
    [Socks.Field(category="Distance")]
    public MinMaxCurve distanceFromParent;
    
    [Socks.Field(category="Distance")]
    public float distanceReduction = 0.03f;

    [Socks.Field(category="Height")]
    public MinMaxCurve heightFromParent;

    [Socks.Field(category="Height")]
    public float heightReduction = 0.1f;

    [Socks.Field(category="Quality")]
    public int lengthSegments = 10;
    
    [Socks.Field(category="Quality")]
    public int radialSegments = 10;

    [Socks.Field(category="Positioning")]
    public bool showPosition = false;

    [Socks.Field(category="Positioning")]
    public Vector3 position;

    [Socks.Field(category="Debug")]
    public bool visualizeTree;

    [Socks.Field(category="Mesh")]
    public GameObject leftMesh;
    
    [Socks.Field(category="Mesh")]
    public GameObject rightMesh;

    void Awake()
    {
        if (leftMesh.GetComponent<MeshFilter>().sharedMesh == null)
        {
            BeginMeshGen();
        }
    }

    void Reset()
    {
        BeginMeshGen();
    }

    public override void BeginMeshGen()
    {
        // todo: error handling?
        base.BeginMeshGen();

        //Debug.LogFormat("Creating a new procmesh. No. of tree nodes: {0}", tree.nodes.Count);
        leftMesh.GetComponent<MeshFilter>().sharedMesh = builder.CreateMesh();
        leftMesh.GetComponent<MeshRenderer>().sharedMaterials = builder.materials.ToArray();
        
        rightMesh.GetComponent<MeshFilter>().sharedMesh = builder.CreateMesh();
        rightMesh.GetComponent<MeshRenderer>().sharedMaterials = builder.materials.ToArray();
    }

    public override void GenerateNode(MeshBuilder builder, string rootNodeID, Vector3 position, Quaternion rotation, int depth, int submeshDepth)
    {
        /** Grab the number of generations */
        float numberOf = Random.Range(Mathf.Clamp(minimumInitialGrowths - (depth), 0, 1000), maximumGrowths);

        for (int i = 0; i < numberOf; ++i)
        {
            float t = (float)i/(float)numberOf;
            if ((initialChanceOfGrowth - (depth*growthChanceReduction)) > Random.Range(0f, 1f))
            {
                float distMagnitude = distanceFromParent.Evaluate(t, Random.value) - (depth * distanceReduction);

                Vector3 dist = new Vector3(distMagnitude, 0f, 0f);
                if (depth > 0)
                {
                    if (Random.value > 0.7f)
                        dist = new Vector3(-distMagnitude, 0f, 0f);
                }

                Vector3 heightDir = Vector3.up;
                float heightMagnitude = Mathf.Clamp((heightFromParent.Evaluate(t, Random.value) - (depth * heightReduction)), 0f, 1000f);
                Vector3 height = heightDir * heightMagnitude;

                Vector3 pos = position;
                pos += dist; 
                pos += height;

                Vector3 distBetweenNewNodeAndParent = pos - position;
                BezierPoint p0 = new BezierPoint(position, new Vector3(dist.x, 0f, dist.z));
                BezierPoint p1 = new BezierPoint(pos, new Vector3(Random.Range(-0.1f, 0.1f), dist.y, Random.Range(-0.1f, 0.1f)));
                BezierSpline spline = new BezierSpline();
                spline.Clear();
                spline.SetPoints(new List<BezierPoint>{p0, p1});

                /** Set up cylinder data! */
                ProcMesh.CurvedCylinderData cylinder = new ProcMesh.CurvedCylinderData();
                cylinder.curve = spline;
                cylinder.lengthSegments = lengthSegments;
                cylinder.radialSegments = radialSegments;
                cylinder.radius = radius;

                vertex[] vertices = null;
                /** Build tube */
                vertices = MeshUtils.BuildCylinderAlongBezier(builder, builder.GetSubmesh(material), cylinder);

                /** Attach new node to tree*/
                string newNodeID = tree.AttachNewNode(rootNodeID);

                Vector3 newPos = spline.GetPointFromTAlongSpline(Random.Range(0.3f, 0.8f));
                tree.Get(newNodeID).position = newPos;
                /** Generate next node */
                GenerateNode(builder, newNodeID, newPos, rotation, depth+1, submeshDepth);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showPosition)
        {
            Gizmos.DrawSphere(transform.position + position, 0.1f);
            Gizmos.DrawSphere(transform.position + new Vector3(-position.x, position.y, position.z), 0.1f);
        }
    }

}
