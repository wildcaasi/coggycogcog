using System.Collections.Generic;

namespace GearPuzzle
{
    public sealed class PuzzleSnapshot
    {
        public IReadOnlyDictionary<Gear, GearState> States { get; }
        public Puzzle Puzzle { get; }
        public bool Solved { get; }

        public PuzzleSnapshot(IReadOnlyDictionary<Gear, GearState> states, Puzzle puzzle, bool solved)
        {
            States = states;
            Puzzle = puzzle;
            Solved = solved;
        }
    }
}
