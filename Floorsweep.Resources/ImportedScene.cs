using System.Collections.Generic;
using System.Linq;
using Assimp;
using Floorsweep.Resources.Interfaces;

namespace Floorsweep.Resources
{
    public class ImportedScene : IImportedScene
    {
        private readonly Scene _scene;
        private readonly IList<ImportedMaterial> _materials;

        public ImportedScene(Scene scene)
        {
            _scene = scene;
            _materials = scene.Materials.Select((m,i) => new ImportedMaterial(m, i)).ToList();
        }

        public IEnumerable<IImportedMesh> Meshes => _scene.Meshes.Select(e => new ImportedMesh(e, _materials[e.MaterialIndex]));
        public IEnumerable<IImportedMaterial> Materials => _materials;
    }
}