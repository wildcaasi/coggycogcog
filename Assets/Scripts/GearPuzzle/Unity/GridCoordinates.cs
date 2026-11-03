using UnityEngine;

namespace GearPuzzle
{
    public sealed class GridCoordinates
    {
        private readonly GearConfig config;

        public GridCoordinates(GearConfig config)
        {
            this.config = config;
        }

        public float ToWorldLength(float configLength) => configLength * config.WorldScale;

        public Vector3 CellToWorld(Cell cell)
        {
            float size = ToWorldLength(config.CellSpacing);
            float x = (cell.Column - (config.Columns - 1) * 0.5f) * size;
            float y = ((config.Rows - 1) * 0.5f - cell.Row) * size;
            return new Vector3(x, y, 0f);
        }

        public bool TryWorldToCell(Vector3 world, out Cell cell)
        {
            float size = ToWorldLength(config.CellSpacing);
            int column = Mathf.RoundToInt(world.x / size + (config.Columns - 1) * 0.5f);
            int row = Mathf.RoundToInt((config.Rows - 1) * 0.5f - world.y / size);
            cell = new Cell(row, column);
            return row >= 0 && row < config.Rows && column >= 0 && column < config.Columns;
        }
    }
}
