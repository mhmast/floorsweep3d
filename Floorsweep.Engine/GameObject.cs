using System;
using System.Numerics;
using FloorSweep.Engine.Interfaces;

namespace Floorsweep.Engine
{
    public class GameObject : IDisposable
    {
        private Vector3 _position;

        public GameObject()
        {
            WorldPositionMatrix = Matrix4x4.Identity;
        }
        public Matrix4x4 WorldPositionMatrix { get; private set; }
        public IRenderMesh RenderMesh { get; set; }

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateWorldMatrix();
            }
        }

        private void UpdateWorldMatrix()
        {
            WorldPositionMatrix = Matrix4x4.CreateTranslation(Position);
        }

        public void Dispose()
        {
            RenderMesh?.Dispose();
        }
    }
}