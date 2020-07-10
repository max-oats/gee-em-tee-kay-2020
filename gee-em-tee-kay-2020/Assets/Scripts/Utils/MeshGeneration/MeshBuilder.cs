using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcMesh;

public class MeshBuilder
{
    public List<Material> materials = new List<Material>();

    public List<Vector3> vertices = new List<Vector3>();

    public List<Vector3> normals = new List<Vector3>();

    public List<Vector2> uvs = new List<Vector2>();

    public List<List<int>> indices = new List<List<int>>();

    public void AddTriangle(int submesh, int i0, int i1, int i2)
    {
        if (submesh >= indices.Count)
        {
            indices.Add(new List<int>());
        }

        indices[submesh].Add(i0);
        indices[submesh].Add(i1);
        indices[submesh].Add(i2);
    }

    public int GetSubmesh(Material material)
    {
        int findIndex = materials.FindIndex(x => x.Equals(material));
        if (findIndex == -1)
        {
            findIndex = materials.Count;
            materials.Add(material);
        }

        return findIndex;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.subMeshCount = materials.Count;

        mesh.vertices = vertices.ToArray();
        int indicesCount = 0;
        for (int i = 0; i < indices.Count; ++i)
        {
            mesh.SetTriangles(indices[i].ToArray(), i);
            indicesCount += indices[i].Count;
        }

        if (normals.Count == vertices.Count)
        {
            mesh.normals = normals.ToArray();
        }

        if (uvs.Count == vertices.Count)
        {
            mesh.uv = uvs.ToArray();
        }
        //Debug.LogFormat("Creating a new procmesh. Vertices: {0}, Normals: {1}, UVs: {2}, Indices {3}", vertices.Count, normals.Count, uvs.Count, indicesCount);

        mesh.RecalculateBounds();

        return mesh;
    }

    public vertex AddPoint(Vector3 index, Vector3 normal, Vector3 uv)
    {
        vertices.Add(index);
        normals.Add(normal);
        uvs.Add(uv);

        return new vertex(index, normal, uv);
    }
}
