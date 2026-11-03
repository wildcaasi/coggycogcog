using UnityEngine;

namespace GearPuzzle
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private GearConfig config;
        [SerializeField] private GridInput gridInput;
        [SerializeField] private GearViewSpawner spawner;
        [SerializeField] private PuzzleHud hud;
        [SerializeField] private int seed;
        [SerializeField] private CellMarkerSpawner markers;

        private PuzzleSession session;

        private void Awake()
        {
            if (!AllReferencesAssigned())
            {
                enabled = false;
                return;
            }

            var random = seed == 0 ? new System.Random() : new System.Random(seed);
            var geometry = new GearGeometry(config);
            var board = new GridBoard(config.Rows, config.Columns);
            var validator = new PlacementValidator(geometry);
            var meshBuilder = new MeshBuilder(geometry);
            var solver = new TransmissionSolver();
            var generator = new PuzzleGenerator(random, config);
            session = new PuzzleSession(board, validator, meshBuilder, solver, generator);

            var coordinates = new GridCoordinates(config);
            var classifier = new TapClassifier(config.DoubleTapSeconds);

            spawner.Initialize(session, coordinates, geometry);
            hud.Initialize(session);
            gridInput.Initialize(coordinates, classifier);
            gridInput.CellTapped += session.Tap;
            gridInput.CellDoubleTapped += session.DoubleTap;
            markers.Initialize(session, coordinates, config);
        }

        private bool AllReferencesAssigned()
        {
            if (config == null || gridInput == null || spawner == null || hud == null)
            {
                Debug.LogError("GameBootstrap: assign Config, Grid Input, Spawner and Hud in the Inspector.", this);
                return false;
            }

            bool inputReady = gridInput.HasRequiredReferences();
            bool spawnerReady = spawner.HasRequiredReferences();
            bool hudReady = hud.HasRequiredReferences();
            return inputReady && spawnerReady && hudReady;
        }

        private void Start()
        {
            if (session == null) return;
            session.NewPuzzle();
        }

        private void OnDestroy()
        {
            if (session == null || gridInput == null) return;
            gridInput.CellTapped -= session.Tap;
            gridInput.CellDoubleTapped -= session.DoubleTap;
        }
    }
}
