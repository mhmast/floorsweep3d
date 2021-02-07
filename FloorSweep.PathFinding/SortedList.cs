namespace FloorSweep.PathFinding
{
    internal class SortedList<T>
    {
        private DStarComparator dStarComparator;

        public SortedList(DStarComparator dStarComparator)
        {
            this.dStarComparator = dStarComparator;
        }
    }
}