using System;
using UnityEngine;

namespace GearPuzzle
{
    public sealed class PuzzleSession
    {
        private readonly GridBoard board;
        private readonly PlacementValidator validator;
        private readonly MeshBuilder meshBuilder;
        private readonly TransmissionSolver solver;
        private readonly PuzzleGenerator generator;
        private Puzzle puzzle;

        public event Action<PuzzleSnapshot> Updated;

        public PuzzleSession(
            GridBoard board,
            PlacementValidator validator,
            MeshBuilder meshBuilder,
            TransmissionSolver solver,
            PuzzleGenerator generator)
        {
            this.board = board;
            this.validator = validator;
            this.meshBuilder = meshBuilder;
            this.solver = solver;
            this.generator = generator;
            ResetBoard();
        }

        public void NewPuzzle()
        {
            ResetBoard();
            Publish();
        }

        public void Tap(Cell cell)
        {
            if (!IsPlayable(cell)) return;

            if (board.TryGet(cell, out _)) board.Remove(cell);
            else TryPlace(new Gear(cell, GearSize.Small));

            Publish();
        }


        public void DoubleTap(Cell cell)
        {
            if (!IsPlayable(cell)) return;

            var large = new Gear(cell, GearSize.Large);

            if (board.TryGet(cell, out Gear existing))
            {
                if (existing.Size == GearSize.Small) Replace(existing, large);
            }
            else
            {
                TryPlace(large);
            }

            Publish();
        }

        public bool IsPlayable(Cell cell) =>
            board.InBounds(cell) && cell.Row > 0 && cell.Row < board.Rows - 1;

        private void TryPlace(Gear gear)
        {
            if (validator.CanPlace(board, gear)) board.TryPlace(gear);
        }

        private void Replace(Gear existing, Gear replacement)
        {
            board.Remove(existing.Cell);
            if (validator.CanPlace(board, replacement)) board.TryPlace(replacement);
            else board.TryPlace(existing);
        }

        private void ResetBoard()
        {
            board.Clear();
            puzzle = generator.Generate();
            board.TryPlace(puzzle.Driver);
            board.TryPlace(puzzle.Receiver);
        }

        //private void Publish()
        //{
        //    var graph = meshBuilder.Build(board);
        //    var states = solver.Solve(graph, puzzle.Driver, puzzle.DriverDirection);
        //    bool solved = states.TryGetValue(puzzle.Receiver, out GearState receiverState)
        //                  && receiverState == puzzle.TargetDirection;
        //    Updated?.Invoke(new PuzzleSnapshot(states, puzzle, solved));
        //}
        private void Publish()
        {
            var graph = meshBuilder.Build(board);
            var states = solver.Solve(graph, puzzle.Driver, puzzle.DriverDirection);
            bool solved = states.TryGetValue(puzzle.Receiver, out GearState receiverState)
                          && receiverState == puzzle.TargetDirection;
            Debug.Log($"Publish solved={solved} receiver={(states.TryGetValue(puzzle.Receiver, out var s) ? s.ToString() : "absent")} gears={board.All.Count}");
            try { Updated?.Invoke(new PuzzleSnapshot(states, puzzle, solved)); }
            catch (Exception e) { Debug.LogException(e); throw; }
        }
    }
}
