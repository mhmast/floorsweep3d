using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Floorsweep.Engine;
using Floorsweep.UI.Rendering;
using Floorsweep.UI.Rendering.Interfaces;
using Veldrid;
using BufferUsage = Floorsweep.UI.Rendering.Core.BufferUsage;
using ResourceKind = Floorsweep.UI.Rendering.Core.ResourceKind;
using ShaderStages = Floorsweep.UI.Rendering.Core.ShaderStages;

namespace FloorSweep.Engine
{
    public class Camera : IDrawable
    {
        private readonly bool _inverted;
        private float _fov = 1f;
        private float _near = 1f;
        private float _far = 1000f;
        const string ProjectionBufferName = "Projection";

        const string ViewBufferName = "View";

        private Matrix4x4 _viewMatrix;
        private Matrix4x4 _projectionMatrix;

        private Vector3 _position = new Vector3(0, 3, 0);
        private Vector3 _lookDirection = new Vector3(0, -.3f, -1f);
        private float _moveSpeed = 10.0f;
    
        private float _yaw;
        private float _pitch;

        private Vector2 _previousMousePos;
        private float _windowWidth;
        private float _windowHeight;
        private IResourceSet _projectionViewSet;

        public event Action<Matrix4x4> ProjectionChanged;
        public event Action<Matrix4x4> ViewChanged;

        //public Camera(float width, float height)
        //{
        //    _windowWidth = width;
        //    _windowHeight = height;
        //    UpdatePerspectiveMatrix();
        //    UpdateViewMatrix();
        //}

        public Camera(IApplicationWindow window, bool inverted = true)
        {
            _inverted = inverted;
            _windowHeight = window.Height;
            _windowWidth = window.Width;
            UpdatePerspectiveMatrix();
            UpdateViewMatrix(GetLookDir());
            window.Resized += () => WindowResized(window.Width, window.Height);
        } 

        public void CreateResources(IResourceFactory factory)
        {
            _projectionViewSet = factory.BuildShaderResourceSet()
                .AddShaderParam(ProjectionBufferName, ResourceKind.UniformBuffer, ShaderStages.Vertex, (uint)Unsafe.SizeOf<Matrix4x4>(), BufferUsage.UniformBuffer | BufferUsage.Dynamic)
                .AddShaderParam(ViewBufferName, ResourceKind.UniformBuffer, ShaderStages.Vertex, (uint)Unsafe.SizeOf<Matrix4x4>(), BufferUsage.UniformBuffer | BufferUsage.Dynamic)
                .Build();
        }

        public Matrix4x4 ViewMatrix => _viewMatrix;
        public Matrix4x4 ProjectionMatrix => _projectionMatrix;

        public Vector3 Position { get => _position; set { _position = value; UpdateViewMatrix(GetLookDir()); } }

        public float FarDistance { get => _far; set { _far = value; UpdatePerspectiveMatrix(); } }
        public float FieldOfView => _fov;
        public float NearDistance { get => _near; set { _near = value; UpdatePerspectiveMatrix(); } }

        public float AspectRatio => _windowWidth / _windowHeight;

        public float Yaw { get => _yaw; set { _yaw = value; UpdateViewMatrix(GetLookDir()); } }
        public float Pitch { get => _pitch; set { _pitch = value; UpdateViewMatrix(GetLookDir()); } }

        public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
        public Vector3 Forward => GetLookDir();

        public void Draw(float deltaSeconds, IGraphicsPipeline pipeline)
        {
            var sprintFactor = InputTracker.GetKey(Key.ControlLeft)
                ? 0.1f
                : InputTracker.GetKey(Key.ShiftLeft)
                    ? 2.5f
                    : 1f;
            var motionDir = Vector3.Zero;
            if (InputTracker.GetKey(Key.A))
            {
                motionDir += -Vector3.UnitX;
            }
            if (InputTracker.GetKey(Key.D))
            {
                motionDir += Vector3.UnitX;
            }
            if (InputTracker.GetKey(Key.W))
            {
                motionDir += -Vector3.UnitZ;
            }
            if (InputTracker.GetKey(Key.S))
            {
                motionDir += Vector3.UnitZ;
            }
            if (InputTracker.GetKey(Key.Q))
            {
                motionDir += -Vector3.UnitY;
            }
            if (InputTracker.GetKey(Key.E))
            {
                motionDir += Vector3.UnitY;
            }

            if (motionDir != Vector3.Zero)
            {
                var lookRotation = Quaternion.CreateFromYawPitchRoll(Yaw, Pitch, 0f);
                motionDir = Vector3.Transform(motionDir, lookRotation);
                _position += motionDir * MoveSpeed * sprintFactor * deltaSeconds;
                UpdateViewMatrix(GetLookDir());
            }

            var mouseDelta = InputTracker.MousePosition - _previousMousePos;
            _previousMousePos = InputTracker.MousePosition;

            float factor = _inverted ? -1 : 1;
            if (InputTracker.GetMouseButton(MouseButton.Left) || InputTracker.GetMouseButton(MouseButton.Right))
            {
                Yaw += -mouseDelta.X * 0.01f;
                Pitch += -mouseDelta.Y * 0.01f * factor;
                Pitch = Clamp(Pitch, -1.55f, 1.55f);

                UpdateViewMatrix(GetLookDir());
            }

            _projectionViewSet[ViewBufferName].Update(ViewMatrix);
            _projectionViewSet[ProjectionBufferName].Update(ProjectionMatrix);
            _projectionViewSet.Load();
        }

        private float Clamp(float value, float min, float max)
        {
            return value > max
                ? max
                : value < min
                    ? min
                    : value;
        }

        public void WindowResized(float width, float height)
        {
            _windowWidth = width;
            _windowHeight = height;
            UpdatePerspectiveMatrix();
        }

        private void UpdatePerspectiveMatrix()
        {
            _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(_fov, _windowWidth / _windowHeight, _near, _far);
            ProjectionChanged?.Invoke(_projectionMatrix);
        }

        private void UpdateViewMatrix(Vector3 lookDirection)
        {
            _lookDirection = lookDirection;
            _viewMatrix = Matrix4x4.CreateLookAt(_position,  Position + _lookDirection, Vector3.UnitY);
            ViewChanged?.Invoke(_viewMatrix);
        }

        private Vector3 GetLookDir()
        {
            var lookRotation = Quaternion.CreateFromYawPitchRoll(Yaw, Pitch, 0f);
            var lookDir = Vector3.Transform(-Vector3.UnitZ, lookRotation);
            return lookDir;
        }

        //public CameraInfo GetCameraInfo() => new CameraInfo
        //{
        //    CameraPosition_WorldSpace = _position,
        //    CameraLookDirection = _lookDirection
        //};

        public void Dispose()
        {
            _projectionViewSet?.Dispose();
        }

        public void LookAt(GameObject obj)
        {
            var lookAt = Position - obj.Position;
            UpdateViewMatrix(Vector3.Normalize(lookAt));
            Pitch = (float)Math.Asin(lookAt.Y);
            Yaw = (float)Math.Atan2(lookAt.X, lookAt.Z);
        }
    }

    //[StructLayout(LayoutKind.Sequential)]
    //public struct CameraInfo
    //{
    //    public Vector3 CameraPosition_WorldSpace;
    //    private float _padding1;
    //    public Vector3 CameraLookDirection;
    //    private float _padding2;
    //}
}
