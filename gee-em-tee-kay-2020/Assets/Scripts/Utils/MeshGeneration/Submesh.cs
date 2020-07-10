using static UnityEngine.ParticleSystem;

namespace ProcMesh
{
    public enum SubmeshStartPoints
    {
        RootOfNode,
        AllOverVertices,
        EndOfBranches,
    }

    [System.Serializable]
    public class Submesh
    {
        public SubmeshStartPoints startPoint;

        public MinMaxCurve numberOfMeshes;
        
        public MinMaxCurve chanceOfGenerating;

        public ProcMeshGenBase mesh;
    }
}