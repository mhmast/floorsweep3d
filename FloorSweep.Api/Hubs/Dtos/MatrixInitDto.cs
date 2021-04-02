using FloorSweep.Math;

namespace FloorSweep.Api.Hubs.Dtos
{
    public class MatrixInitDto
    {

        public MatrixInitDto(string name, Mat m, bool isBinary)
        {
            IsBinary = isBinary;
            Name = name;
            Data = m.Data;
            Width = m.Cols;
            Height = m.Rows;
        }
        public double[] Data { get; }
        public int Width { get; }
        public int Height { get; }
        public bool IsBinary { get; }
        public string Name { get; }
    }
}
