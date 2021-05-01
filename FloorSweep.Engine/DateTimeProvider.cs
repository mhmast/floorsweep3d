﻿using FloorSweep.Engine.Interfaces;
using System;

namespace FloorSweep.Engine
{
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}