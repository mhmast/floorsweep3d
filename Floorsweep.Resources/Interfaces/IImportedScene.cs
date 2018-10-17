using System.Collections.Generic;

namespace Floorsweep.Resources.Interfaces
{
    public interface IImportedScene
    {
        IEnumerable<IImportedMesh> Meshes { get; }
        IEnumerable<IImportedMaterial> Materials { get; }
    }
}
