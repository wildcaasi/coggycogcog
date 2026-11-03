using System;

namespace GearPuzzle
{
    public readonly struct Cell : IEquatable<Cell>
    {
        public int Row { get; }
        public int Column { get; }

        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool Equals(Cell other) => Row == other.Row && Column == other.Column;
        public override bool Equals(object obj) => obj is Cell other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Row, Column);
        public override string ToString() => $"({Row},{Column})";
    }
}
