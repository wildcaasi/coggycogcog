namespace GearPuzzle
{
    public sealed class PlacementValidator
    {
        private readonly GearGeometry geometry;

        public PlacementValidator(GearGeometry geometry)
        {
            this.geometry = geometry;
        }

        public bool CanPlace(GridBoard board, Gear gear)
        {
            if (!board.InBounds(gear.Cell)) return false;

            foreach (Gear other in board.All)
            {
                if (geometry.Classify(gear, other) == GearContact.Overlap) return false;
            }

            return true;
        }
    }
}
