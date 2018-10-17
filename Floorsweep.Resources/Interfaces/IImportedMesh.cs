using System.Collections.Generic;
using System.Numerics;

namespace Floorsweep.Resources.Interfaces
{
    public interface IImportedMesh
    {
        IEnumerable<uint> Indices { get; }
        string Name { get; }
        IImportedMaterial Material { get; }
        IEnumerable<Vector3> Vertices { get; }
        IEnumerable<Vector3> Normals { get; }
    }
}
