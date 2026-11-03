namespace GearPuzzle
{
    public sealed class TapClassifier
    {
        private readonly float doubleTapSeconds;
        private Cell lastCell;
        private float lastTime;
        private bool hasLast;

        public TapClassifier(float doubleTapSeconds)
        {
            this.doubleTapSeconds = doubleTapSeconds;
        }

        public TapKind Classify(Cell cell, float time)
        {
            bool isDouble = hasLast && lastCell.Equals(cell) && time - lastTime <= doubleTapSeconds;

            if (isDouble)
            {
                hasLast = false;
                return TapKind.Double;
            }

            lastCell = cell;
            lastTime = time;
            hasLast = true;
            return TapKind.Single;
        }
    }
}
