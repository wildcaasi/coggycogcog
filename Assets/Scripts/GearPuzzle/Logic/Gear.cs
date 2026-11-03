using System;

namespace GearPuzzle
{
    public readonly struct Gear : IEquatable<Gear>
    {
        public Cell Cell { get; }
        public GearSize Size { get; }

        public Gear(Cell cell, GearSize size)
        {
            Cell = cell;
            Size = size;
        }

        public bool Equals(Gear other) => Cell.Equals(other.Cell) && Size == other.Size;
        public override bool Equals(object obj) => obj is Gear other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Cell, Size);
        public override string ToString() => $"{Size}{Cell}";
    }
}
