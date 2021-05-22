﻿using System;
using System.Threading.Tasks;

namespace FloorSweep.Engine.EventHandlers
{

    internal class EventHandlerAdapter<T> : IEventHandler<T>
    {
        private readonly Func<T, Task> _updateFunc;
        private readonly Func<Task> _resetFunc;

        public EventHandlerAdapter(Func<T, Task> updateFunc, Func<Task> resetFunc)
        {
            _updateFunc = updateFunc;
            _resetFunc = resetFunc;
        }
        public Task OnStatusUpdatedAsync(T status)
        => _updateFunc(status);

        public Task ResetStatusAsync()
        => _resetFunc();
    }

}
