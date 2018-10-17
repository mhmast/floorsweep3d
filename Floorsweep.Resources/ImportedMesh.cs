using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Assimp;
using Floorsweep.Resources.Extensions;
using Floorsweep.Resources.Interfaces;

namespace Floorsweep.Resources
{
    public class ImportedMesh : IImportedMesh
    {
        private readonly Mesh _mesh;

        public ImportedMesh(Mesh mesh, IImportedMaterial material)
        {
            _mesh = mesh;
            Material = material;
        }

        public IEnumerable<uint> Indices => _mesh.GetUnsignedIndices();

        public string Name => _mesh.Name;

        public IImportedMaterial  Material { get; }

        public IEnumerable<Vector3> Vertices => _mesh.Vertices.Select(c=>c.ToVector3()).ToArray();

        public IEnumerable<Vector3> Normals => _mesh.Normals.Select(n=>n.ToVector3()).ToArray();
      
    }
}