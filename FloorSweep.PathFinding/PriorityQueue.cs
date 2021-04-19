using FloorSweep.PathFinding.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FloorSweep.PathFinding
{
    internal class PriorityQueue<T> : SortedSet<T> where T : IQueueKeyProvider 
    {
        public PriorityQueue() : base(new IQueueKeyProviderComparer())
        {

        }
        public void Queue(T item)
        {
            Add(item);
        }

        public T Dequeue()
        {
            var i = this.First();
            Remove(i);
            return i;
        }

        class IQueueKeyProviderComparer : IComparer<T>
        {
            public int Compare(T x, T y)
            => x.Key.CompareTo(y.Key);
        }
    }
}
