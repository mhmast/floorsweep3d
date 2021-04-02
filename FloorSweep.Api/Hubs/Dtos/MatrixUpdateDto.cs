namespace FloorSweep.Api.Hubs.Dtos
{
    public class MatrixUpdateDto
    {

        public MatrixUpdateDto(string name, int row, int col, double value)
        {
            Name = name;
            Row = row;
            Col = col;
            Value = value;
        }
        public string Name { get; }
        public int Row { get; }
        public int Col { get; }
        public double Value { get; }
    }
}
