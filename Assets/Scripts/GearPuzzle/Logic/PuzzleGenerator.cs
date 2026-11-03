using System;

namespace GearPuzzle
{
    public sealed class PuzzleGenerator
    {
        private readonly Random random;
        private readonly GearConfig config;

        public PuzzleGenerator(Random random, GearConfig config)
        {
            this.random = random;
            this.config = config;
        }

        public Puzzle Generate()
        {
            var driverCell = new Cell(0, random.Next(config.Columns));
            var receiverCell = new Cell(config.Rows - 1, random.Next(config.Columns));
            return new Puzzle(driverCell, receiverCell, RandomDirection(), RandomDirection());
        }

        private GearState RandomDirection() =>
            random.Next(2) == 0 ? GearState.Clockwise : GearState.CounterClockwise;
    }
}
