using NUnit.Framework;
using UnityEngine;

namespace GearPuzzle.Tests
{
    public sealed class PuzzleSessionTests
    {
        private readonly Cell cell = new Cell(5, 2);
        private GearConfig config;
        private PuzzleSession session;
        private PuzzleSnapshot last;

        [SetUp]
        public void SetUp()
        {
            config = ScriptableObject.CreateInstance<GearConfig>();
            var geometry = new GearGeometry(config);
            session = new PuzzleSession(
                new GridBoard(config.Rows, config.Columns),
                new PlacementValidator(geometry),
                new MeshBuilder(geometry),
                new TransmissionSolver(),
                new PuzzleGenerator(new System.Random(1), config));
            session.Updated += snapshot => last = snapshot;
            session.NewPuzzle();
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(config);
        }

        private bool Has(Cell at, GearSize size) => last.States.ContainsKey(new Gear(at, size));

        [Test]
        public void Tap_OnEmptyCell_PlacesSmallGear()
        {
            session.Tap(cell);

            Assert.IsTrue(Has(cell, GearSize.Small));
        }

        [Test]
        public void Tap_OnOccupiedCell_RemovesGear()
        {
            session.Tap(cell);
            session.Tap(cell);

            Assert.IsFalse(Has(cell, GearSize.Small));
            Assert.IsFalse(Has(cell, GearSize.Large));
        }

        [Test]
        public void DoubleTap_AfterSingleTap_UpgradesToLarge()
        {
            session.Tap(cell);
            session.DoubleTap(cell);

            Assert.IsTrue(Has(cell, GearSize.Large));
            Assert.IsFalse(Has(cell, GearSize.Small));
        }

        [Test]
        public void DoubleTap_OnEmptyCell_PlacesLarge()
        {
            session.DoubleTap(cell);

            Assert.IsTrue(Has(cell, GearSize.Large));
        }

        [Test]
        public void DoubleTap_BlockedByNeighbour_KeepsSmall()
        {
            session.Tap(new Cell(5, 3));
            session.Tap(cell);
            session.DoubleTap(cell);

            Assert.IsTrue(Has(cell, GearSize.Small));
            Assert.IsFalse(Has(cell, GearSize.Large));
        }

        [Test]
        public void Tap_OnDriverRow_IsIgnored()
        {
            PuzzleSnapshot before = last;

            session.Tap(new Cell(0, 0));
            session.DoubleTap(new Cell(config.Rows - 1, 0));

            Assert.AreSame(before, last);
        }
    }
}
