using System.Collections.Generic;
using System.Linq;
using Floorsweep.UI.Rendering.Interfaces;
using Veldrid;

namespace Floorsweep.UI.Rendering.Core
{
    internal class ShaderResourceSetBuilder : IShaderResourceSetBuilder
    {
        private readonly uint _slot;
        private readonly CommandList _cl;
        private readonly ResourceFactory _factory;
        private readonly List<ResourceLayoutElementDescription> _shaderParameters = new List<ResourceLayoutElementDescription>();
        private readonly List<DeviceBuffer> _shaderBuffers = new List<DeviceBuffer>();
        internal ResourceLayout Layout { get; private set; }

        public ShaderResourceSetBuilder(uint slot, CommandList cl, ResourceFactory factory)
        {
            _slot = slot;
            _cl = cl;
            _factory = factory;
        }

        public IShaderResourceSetBuilder AddShaderParam(string name, ResourceKind resourceKind, ShaderStages shaderStages,
            uint size, BufferUsage usage)
        {
            _shaderParameters.Add(new ResourceLayoutElementDescription(name, (Veldrid.ResourceKind)resourceKind,
                (Veldrid.ShaderStages)shaderStages));
            var deviceBuffer = _factory.CreateBuffer(new BufferDescription(size, (Veldrid.BufferUsage)usage));
            deviceBuffer.Name = name;
            _shaderBuffers.Add(deviceBuffer);
            return this;
        }

        public IResourceSet Build()
        {
            Layout = _factory.CreateResourceLayout(new ResourceLayoutDescription(_shaderParameters.ToArray()));
            var set = _factory.CreateResourceSet(new ResourceSetDescription(Layout, _shaderBuffers.Cast<BindableResource>().ToArray()));
            return new ResourceSet(_cl, _slot, Layout, set, _shaderBuffers.Select(b => new ShaderParameterBuffer(_cl, b)));
        }
    }
}