using System;

namespace GearPuzzle
{
    public sealed class GearGeometry
    {
        private readonly GearConfig config;

        public GearGeometry(GearConfig config)
        {
            this.config = config;
        }

        public float RadiusOf(GearSize size)
        {
            switch (size)
            {
                case GearSize.Small: return config.SmallRadius;
                case GearSize.Large: return config.LargeRadius;
                default: throw new ArgumentOutOfRangeException(nameof(size));
            }
        }

        public float Distance(Cell a, Cell b)
        {
            float rowOffset = (a.Row - b.Row) * config.CellSpacing;
            float columnOffset = (a.Column - b.Column) * config.CellSpacing;
            return MathF.Sqrt(rowOffset * rowOffset + columnOffset * columnOffset);
        }

        public GearContact Classify(Gear a, Gear b)
        {
            float distance = Distance(a.Cell, b.Cell);
            float target = RadiusOf(a.Size) + RadiusOf(b.Size);

            if (distance < target - config.Margin) return GearContact.Overlap;
            if (MathF.Abs(distance - target) <= config.Margin) return GearContact.Meshed;
            return GearContact.Apart;
        }
    }
}
