using System.Collections.Generic;

namespace GearPuzzle
{
    public sealed class GridBoard
    {
        private readonly Dictionary<Cell, Gear> gears = new();

        public int Rows { get; }
        public int Columns { get; }
        public IReadOnlyCollection<Gear> All => gears.Values;

        public GridBoard(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        public bool InBounds(Cell cell) =>
            cell.Row >= 0 && cell.Row < Rows && cell.Column >= 0 && cell.Column < Columns;

        public bool TryPlace(Gear gear)
        {
            if (!InBounds(gear.Cell)) return false;
            return gears.TryAdd(gear.Cell, gear);
        }

        public bool Remove(Cell cell) => gears.Remove(cell);

        public bool TryGet(Cell cell, out Gear gear) => gears.TryGetValue(cell, out gear);

        public void Clear() => gears.Clear();
    }
}
