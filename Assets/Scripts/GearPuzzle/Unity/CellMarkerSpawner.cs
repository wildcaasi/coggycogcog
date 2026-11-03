using UnityEngine;

namespace GearPuzzle
{
    public sealed class CellMarkerSpawner : MonoBehaviour
    {
        [SerializeField] private Sprite markerSprite;
        [SerializeField] private Color markerColor = Color.black;
        [SerializeField] private float markerDiameter = 24f;
        [SerializeField] private int sortingOrder = -10;

        public void Initialize(PuzzleSession session, GridCoordinates coordinates, GearConfig config)
        {
            Transform root = new GameObject("Cell Markers").transform;
            float worldDiameter = coordinates.ToWorldLength(markerDiameter);
            float scale = worldDiameter / markerSprite.bounds.size.x;

            for (int row = 0; row < config.Rows; row++)
            {
                for (int column = 0; column < config.Columns; column++)
                {
                    var cell = new Cell(row, column);
                    if (!session.IsPlayable(cell)) continue;

                    var marker = new GameObject($"Marker {cell}");
                    marker.transform.SetParent(root, false);
                    marker.transform.position = coordinates.CellToWorld(cell);
                    marker.transform.localScale = Vector3.one * scale;

                    var spriteRenderer = marker.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = markerSprite;
                    spriteRenderer.color = markerColor;
                    spriteRenderer.sortingOrder = sortingOrder;
                }
            }
        }
    }
}
