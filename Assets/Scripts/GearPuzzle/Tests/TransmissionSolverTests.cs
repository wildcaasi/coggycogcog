using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace GearPuzzle.Tests
{
    public sealed class TransmissionSolverTests
    {
        private GearConfig config;
        private TransmissionSolver solver;

        [SetUp]
        public void SetUp()
        {
            config = ScriptableObject.CreateInstance<GearConfig>();
            solver = new TransmissionSolver();
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(config);
        }

        private Gear Small(int row, int column) => new Gear(new Cell(row, column), GearSize.Small);
        private Gear Large(int row, int column) => new Gear(new Cell(row, column), GearSize.Large);

        private Dictionary<Gear, GearState> Run(Gear driver, GearState direction, params Gear[] others)
        {
            var board = new GridBoard(config.Rows, config.Columns);
            board.TryPlace(driver);
            foreach (Gear gear in others) board.TryPlace(gear);

            var graph = new MeshBuilder(new GearGeometry(config)).Build(board);
            return solver.Solve(graph, driver, direction);
        }

        [Test]
        public void StraightChain_AlternatesDirection()
        {
            Gear a = Small(1, 2), b = Small(2, 2), c = Small(3, 2);

            var states = Run(a, GearState.Clockwise, b, c);

            Assert.AreEqual(GearState.Clockwise, states[a]);
            Assert.AreEqual(GearState.CounterClockwise, states[b]);
            Assert.AreEqual(GearState.Clockwise, states[c]);
        }

        [Test]
        public void ParityFlip_EvenChainEndsOpposite()
        {
            Gear a = Small(1, 2), b = Small(2, 2), c = Small(3, 2), d = Small(4, 2);

            var states = Run(a, GearState.CounterClockwise, b, c, d);

            Assert.AreEqual(GearState.Clockwise, states[d]);
        }

        [Test]
        public void OddLoop_JamsWholeComponent()
        {
            Gear driver = Small(4, 1);
            Gear leftLarge = Large(3, 0), rightLarge = Large(3, 2);
            Gear tail = Small(5, 1);

            var states = Run(driver, GearState.Clockwise, leftLarge, rightLarge, tail);

            Assert.AreEqual(GearState.NotMoving, states[driver]);
            Assert.AreEqual(GearState.NotMoving, states[leftLarge]);
            Assert.AreEqual(GearState.NotMoving, states[rightLarge]);
            Assert.AreEqual(GearState.NotMoving, states[tail]);
        }

        [Test]
        public void DisconnectedGear_StaysNotMoving()
        {
            Gear driver = Small(1, 2), neighbour = Small(2, 2), lone = Small(6, 4);

            var states = Run(driver, GearState.Clockwise, neighbour, lone);

            Assert.AreEqual(GearState.CounterClockwise, states[neighbour]);
            Assert.AreEqual(GearState.NotMoving, states[lone]);
        }

        [Test]
        public void MixedSizes_MeshDiagonallyAndTwoCellsApart()
        {
            Gear driver = Small(1, 1);
            Gear largeA = Large(2, 2);
            Gear largeB = Large(2, 4);

            var states = Run(driver, GearState.Clockwise, largeA, largeB);

            Assert.AreEqual(GearState.CounterClockwise, states[largeA]);
            Assert.AreEqual(GearState.Clockwise, states[largeB]);
        }
    }
}
