using System.Collections.Generic;
using UnityEngine;

namespace GearPuzzle
{
    public sealed class GearViewSpawner : MonoBehaviour
    {
        [SerializeField] private GearView gearPrefab;
        [SerializeField] private Transform container;
        [SerializeField] private Color playerColor = Color.white;
        [SerializeField] private Color driverColor = new Color(0.35f, 0.65f, 1f);
        [SerializeField] private Color receiverColor = new Color(0.45f, 0.9f, 0.5f);

        private readonly Dictionary<Gear, GearView> views = new();
        private readonly List<Gear> staleGears = new();
        private PuzzleSession session;
        private GridCoordinates coordinates;
        private GearGeometry geometry;

        public void Initialize(PuzzleSession session, GridCoordinates coordinates, GearGeometry geometry)
        {
            this.session = session;
            this.coordinates = coordinates;
            this.geometry = geometry;
            if (container == null) container = new GameObject("Gears").transform;
            session.Updated += OnUpdated;
        }

        public bool HasRequiredReferences()
        {
            if (gearPrefab != null) return true;
            Debug.LogError("GearViewSpawner: Gear Prefab is not assigned. Drag the GearView prefab from the Project window.", this);
            return false;
        }

        private void OnDestroy()
        {
            if (session != null) session.Updated -= OnUpdated;
        }

        private void OnUpdated(PuzzleSnapshot snapshot)
        {
            RemoveStaleViews(snapshot);

            foreach (KeyValuePair<Gear, GearState> pair in snapshot.States)
            {
                if (!views.TryGetValue(pair.Key, out GearView view))
                {
                    view = Instantiate(gearPrefab, coordinates.CellToWorld(pair.Key.Cell), Quaternion.identity, container);
                    view.Initialize(pair.Key, geometry, coordinates);
                    views[pair.Key] = view;
                }

                view.Apply(pair.Value, TintFor(pair.Key, pair.Value, snapshot.Puzzle));
            }
        }

        private void RemoveStaleViews(PuzzleSnapshot snapshot)
        {
            staleGears.Clear();
            foreach (Gear gear in views.Keys)
            {
                if (!snapshot.States.ContainsKey(gear)) staleGears.Add(gear);
            }

            foreach (Gear gear in staleGears)
            {
                Destroy(views[gear].gameObject);
                views.Remove(gear);
            }
        }

        private Color TintFor(Gear gear, GearState state, Puzzle puzzle)
        {
            Color color = playerColor;
            if (gear.Cell.Equals(puzzle.DriverCell)) color = driverColor;
            else if (gear.Cell.Equals(puzzle.ReceiverCell)) color = receiverColor;

            if (state == GearState.NotMoving) color = Color.Lerp(color, Color.gray, 0.6f);
            return color;
        }
    }
}
