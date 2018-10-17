using System;
using System.Diagnostics;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Veldrid.Utilities;

namespace Floorsweep.UI.Rendering
{
    public class VeldridStartupWindow : IApplicationWindow
    {
        private readonly Sdl2Window _window;
        private GraphicsDevice _gd;
        private DisposeCollectorResourceFactory _factory;
        private bool _windowResized = true;

        public event Action<float> Rendering;
        public event Action<GraphicsDevice, ResourceFactory, Swapchain> GraphicsDeviceCreated;
        public event Action GraphicsDeviceDestroyed;
        public event Action Resized;
        public event Action<KeyEvent> KeyPressed;

        public uint Width => (uint)_window.Width;
        public uint Height => (uint)_window.Height;

     
        public VeldridStartupWindow(string title)
        {
            var wci = new WindowCreateInfo
            {
                X = 100,
                Y = 100,
                WindowWidth = 1280,
                WindowHeight = 720,
                WindowTitle = title,
            };
            _window = VeldridStartup.CreateWindow(ref wci);
            _window.Resized += () =>
            {
                _windowResized = true;
            };
            _window.KeyDown += OnKeyDown;
        }

        public void Run()
        {
            var options = new GraphicsDeviceOptions(
                false,
                PixelFormat.R16_UNorm,
                true,
                ResourceBindingModel.Improved) {Debug = true};

            _gd = VeldridStartup.CreateGraphicsDevice(_window, options);
            _factory = new DisposeCollectorResourceFactory(_gd.ResourceFactory);
            GraphicsDeviceCreated?.Invoke(_gd, _factory, _gd.MainSwapchain);

            var sw = Stopwatch.StartNew();
            var previousElapsed = sw.Elapsed.TotalSeconds;

            while (_window.Exists)
            {
                var newElapsed = sw.Elapsed.TotalSeconds;
                var deltaSeconds = (float)(newElapsed - previousElapsed);

                var inputSnapshot = _window.PumpEvents();
                InputTracker.UpdateFrameInput(inputSnapshot);

                if (_window.Exists)
                {
                    previousElapsed = newElapsed;
                    if (_windowResized)
                    {
                        _windowResized = false;
                        _gd.ResizeMainWindow((uint)_window.Width, (uint)_window.Height);
                        Resized?.Invoke();
                    }

                    Rendering?.Invoke(deltaSeconds);
                }
            }

            _gd.WaitForIdle();
            _factory.DisposeCollector.DisposeAll();
            _gd.Dispose();
            GraphicsDeviceDestroyed?.Invoke();
        }

        private void OnKeyDown(KeyEvent keyEvent)
        {
            KeyPressed?.Invoke(keyEvent);
        }
    }
}
