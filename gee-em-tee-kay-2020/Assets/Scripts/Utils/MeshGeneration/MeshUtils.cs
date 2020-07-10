using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.ParticleSystem;
using ProcMesh;

namespace ProcMesh
{
    public struct vertex
    {
        public vertex(Vector3 index, Vector3 normal, Vector2 uv)
        {
            this.index = index;
            this.normal = normal;
            this.uv = uv;
        }

        public Vector3 index;
        public Vector3 normal;
        public Vector3 uv;
    }

    public class SphereData
    {
        public float radius;
        public MinMaxCurve radiusOffset;

        public int heightSegments;
        public int radialSegments;

        public MinMaxCurve randomOffset;
    }

    public class CurvedCylinderData
    {
        public BezierSpline curve;

        public int radialSegments;
        public int lengthSegments;
        
        public MinMaxCurve radius;
    }

    public class CylinderData
    {
        public int radialSegments;
        public int lengthSegments;
        
        public MinMaxCurve radius;

        public float height;
    }
}

public static class MeshUtils
{
    public static vertex[] BuildTubeAlongBezier(MeshBuilder builder, int submesh, ProcMesh.CurvedCylinderData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Tried to build tube along curve but data is null!");
            return null;
        }

        if (data.curve == null || data.curve.GetLength() == 0f)
        {
            Debug.LogWarning("Tried to build cylinder along curve- curve length = 0.");
            return null;
        }

        List<vertex> vertices = new List<vertex>();
        float lengthOfCurve = data.curve.GetLength();
        
        for (int i = 0; i <= data.lengthSegments; ++i)
        {
            float t = (float)i/(float)data.lengthSegments;

            // Grab the distance from t
            Vector3 pos = data.curve.GetPointFromTAlongSpline(t);

            /** Grab v (i.e. uv) */
            float v = t / 1.0f;
            BuildRing(builder, submesh, 
                    pos, Quaternion.FromToRotation(Vector3.up, data.curve.GetDirectionFromTAlongSpline(t)), 
                    data.radialSegments, data.radius.Evaluate(t, Random.value), 
                    v, t > 0f);
        }

        for (int i = data.lengthSegments; i >= 1; --i)
        {
            float t = (float)i/(float)data.lengthSegments;

            // Grab the distance from t
            Vector3 pos = data.curve.GetPointFromTAlongSpline(t);

            /** Grab v (i.e. uv) */
            float v = t / 1.0f;
            vertices.AddRange(BuildRing(builder, submesh, 
                    pos, Quaternion.FromToRotation(Vector3.up, data.curve.GetDirectionFromTAlongSpline(t)), 
                    data.radialSegments, data.radius.Evaluate(t, Random.value)*0.8f, 
                    v, t > 0f, true));
        }

        Vector3 bottomCapNormal = data.curve.GetDirectionFromTAlongSpline(0f);
        vertices.AddRange(BuildCap(builder, submesh, 
                data.curve.GetPointFromTAlongSpline(0f), Quaternion.FromToRotation(Vector3.up, bottomCapNormal),
                data.radialSegments, bottomCapNormal, 
                data.radius.Evaluate(0f, Random.value),
                 false));

        return vertices.ToArray();
    }

    public static vertex[] BuildCylinderAlongBezier(MeshBuilder builder, int submesh, ProcMesh.CurvedCylinderData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Tried to build tube along curve but data is null!");
            return null;
        }

        if (data.curve == null || data.curve.GetLength() == 0f)
        {
            Debug.LogWarning("Tried to build cylinder along curve- curve length = 0.");
            return null;
        }

        List<vertex> vertices = new List<vertex>();
        float lengthOfCurve = data.curve.GetLength();
        
        for (int i = 0; i <= data.lengthSegments; ++i)
        {
            float t = (float)i/(float)data.lengthSegments;

            // Grab the distance from t
            Vector3 pos = data.curve.GetPointFromTAlongSpline(t);

            /** Grab v (i.e. uv) */
            float v = t / 1.0f;
            vertices.AddRange(BuildRing(builder, submesh, 
                    pos, Quaternion.FromToRotation(Vector3.up, data.curve.GetDirectionFromTAlongSpline(t)),
                    data.radialSegments, data.radius.Evaluate(t, Random.value), 
                    v, t > 0f));
        }
        
        Vector3 bottomCapNormal = data.curve.GetDirectionFromTAlongSpline(0f);
        vertices.AddRange(BuildCap(builder, submesh,
                data.curve.GetPointFromTAlongSpline(0f), Quaternion.FromToRotation(Vector3.up, bottomCapNormal),
                data.radialSegments, bottomCapNormal, 
                data.radius.Evaluate(0f, Random.value),
                false));

        Vector3 topCapNormal = data.curve.GetDirectionFromTAlongSpline(1f);
        vertices.AddRange(BuildCap(builder, submesh, 
                data.curve.GetPointFromTAlongSpline(1f), Quaternion.FromToRotation(Vector3.up, topCapNormal),
                data.radialSegments, topCapNormal, 
                data.radius.Evaluate(1f, Random.value),
                true));

        return vertices.ToArray();
    }

    public static vertex[] BuildSphere(MeshBuilder builder, int submesh, Vector3 position, Quaternion rotation, ProcMesh.SphereData data)
    {
        List<vertex> vertices = new List<vertex>();
        float angleBetweenSegments = Mathf.PI / (float)data.heightSegments;
        
        for (int i = 0; i <= data.heightSegments; ++i)
        {
            float v = (float)i / data.heightSegments;

            Vector3 finalCentre = position;
            finalCentre.y += -Mathf.Cos(angleBetweenSegments * (float)i) * (data.radius) * (data.radiusOffset.Evaluate(v, Random.value)+1f);
            float finalRadius = Mathf.Sin(angleBetweenSegments * (float)i) * (data.radius) * (data.radiusOffset.Evaluate(v, Random.value)+1f);

            vertices.AddRange(BuildSphereRing(builder, submesh, finalCentre, rotation, data.radialSegments, finalRadius, v, data.randomOffset, i > 0));
        }

        return vertices.ToArray();
    }

    public static vertex[] BuildCylinder(MeshBuilder builder, int submesh, Vector3 position, Quaternion rotation, ProcMesh.CylinderData data)
    {
        List<vertex> vertices = new List<vertex>();
        float heightPerSegment = data.height / data.lengthSegments;
        for (int i = 0; i <= data.lengthSegments; ++i)
        {
            Vector3 ringCentre = position + (((rotation)*Vector3.up) * heightPerSegment * i);
            float v = (float)i / data.lengthSegments;

            vertices.AddRange(BuildRing(builder, submesh, ringCentre, rotation, data.radialSegments, data.radius.Evaluate(v, Random.value), v, i > 0));
        }

        Vector3 bottomCapNormal = rotation * Vector3.up;
        vertices.AddRange(BuildCap(builder, submesh,
                position, Quaternion.FromToRotation(Vector3.up, bottomCapNormal),
                data.radialSegments, bottomCapNormal, 
                data.radius.Evaluate(0f, Random.value),
                false));

        Vector3 topCapNormal = rotation * Vector3.down;
        vertices.AddRange(BuildCap(builder, submesh, 
                position + ((rotation)*Vector3.up)*data.height, Quaternion.FromToRotation(Vector3.up, topCapNormal),
                data.radialSegments, topCapNormal, 
                data.radius.Evaluate(1f, Random.value),
                true));

        return vertices.ToArray();
    }

    public static vertex[] BuildSphereRing(MeshBuilder builder, int submesh, 
                                Vector3 position, Quaternion rotation, 
                                int segments, float radius, float v,
                                MinMaxCurve vertexOffset,
                                bool buildTriangles = true, bool flipNormals = false)
    {
        List<vertex> vertices = new List<vertex>();
        /** grab the angle between each segment connection */
        float angleBetweenSegments = (Mathf.PI * 2.0f) / segments;

        for (int i = 0; i <= segments; ++i)
        {
            float angleFromCenter = angleBetweenSegments * i;

            Vector3 normal = rotation * new Vector3(Mathf.Cos(angleFromCenter), 0f, Mathf.Sin(angleFromCenter));
            Vector3 index = position + (normal * (radius));
            index += Random.insideUnitSphere.normalized * vertexOffset.Evaluate((float)i/(float)segments, Random.value);
            Vector2 uv = new Vector2((float)i / segments, v);

            if (flipNormals)
            {
                normal = normal * -1f;
            }

            /** Add the new point */
            vertices.Add(builder.AddPoint(index, normal, uv));

            /** if we want to build triangles! */
            if (i > 0 && buildTriangles)
            {
                /** grab the vertex index */
                int baseIndex = builder.vertices.Count - 1;

                /** the amount of vertices in each row (i.e. amount of lines + 1) */
                int verticesInRing = segments + 1;

                int i0 = baseIndex;
                int i1 = baseIndex - 1;
                int i2 = baseIndex - verticesInRing;
                int i3 = baseIndex - verticesInRing - 1;

                /** add triangles! */
                builder.AddTriangle(submesh, i0, i2, i1);
                builder.AddTriangle(submesh, i2, i3, i1);
            }
        }

        return vertices.ToArray();
    }
    
    public static vertex[] BuildRing(MeshBuilder builder, int submesh, Vector3 position, Quaternion rotation, int segments, float radius, float v, bool buildTriangles = true, bool flipNormals = false)
    {
        List<vertex> vertices = new List<vertex>();
        /** grab the angle between each segment connection */
        float angleBetweenSegments = (Mathf.PI * 2.0f) / segments;

        for (int i = 0; i <= segments; ++i)
        {
            float angleFromCenter = angleBetweenSegments * i;

            Vector3 normal = rotation * new Vector3(Mathf.Cos(angleFromCenter), 0f, Mathf.Sin(angleFromCenter));
            Vector3 index = position + (normal * radius);
            Vector2 uv = new Vector2((float)i / segments, v);

            if (flipNormals)
            {
                normal = normal * -1f;
            }

            /** Add the new point */
            vertices.Add(builder.AddPoint(index, normal, uv));

            /** if we want to build triangles! */
            if (i > 0 && buildTriangles)
            {
                /** grab the vertex index */
                int baseIndex = builder.vertices.Count - 1;

                /** the amount of vertices in each row (i.e. amount of lines + 1) */
                int verticesInRing = segments + 1;

                int i0 = baseIndex;
                int i1 = baseIndex - 1;
                int i2 = baseIndex - verticesInRing;
                int i3 = baseIndex - verticesInRing - 1;

                /** add triangles! */
                builder.AddTriangle(submesh, i0, i2, i1);
                builder.AddTriangle(submesh, i2, i3, i1);
            }
        }

        return vertices.ToArray();
    }

    public static vertex[] BuildCap(MeshBuilder builder, int submesh, Vector3 position, Quaternion rotation, int segments, Vector3 normal, float radius, bool reverseDirection, bool buildTriangles = true)
    {
        List<vertex> vertices = new List<vertex>();
        normal = reverseDirection ? normal : normal*-1;

        vertices.Add(builder.AddPoint(position, normal, new Vector2(0.5f, 0.5f)));

        int centreVertexIndex = builder.vertices.Count - 1;

        //vertices around the edge:
        float angleBetweenSegments = (Mathf.PI * 2.0f) / segments;

        for (int i = 0; i <= segments; i++)
        {
            float angleFromCenter = angleBetweenSegments * i;

            Vector3 normalizedVertex = new Vector3(Mathf.Cos(angleFromCenter), 0f, Mathf.Sin(angleFromCenter));
            /** Apply rotation */
            normalizedVertex = rotation * normalizedVertex;
            Vector3 vertex = (position + (normalizedVertex * radius));
            Vector2 uv = new Vector2(normalizedVertex.x + 1.0f, normalizedVertex.z + 1.0f) * 0.5f;

            vertices.Add(builder.AddPoint(vertex, normalizedVertex, uv));

            //build a triangle:
            if (i > 0)
            {
                int baseIndex = builder.vertices.Count - 1;

                if (reverseDirection)
                    builder.AddTriangle(submesh, centreVertexIndex, baseIndex - 1, 
                        baseIndex);
                else
                    builder.AddTriangle(submesh, centreVertexIndex, baseIndex, 
                        baseIndex - 1);
            }
        }

        return vertices.ToArray();
    }
}