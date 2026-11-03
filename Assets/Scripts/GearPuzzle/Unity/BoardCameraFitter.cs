using UnityEngine;

namespace GearPuzzle
{
    public sealed class BoardCameraFitter : MonoBehaviour
    {
        [SerializeField] private GearConfig config;
        [SerializeField] private Camera targetCamera;
        [SerializeField] private float padding = 1.5f;
        [SerializeField] private bool drawGrid = true;

        private float lastAspect;

        private void Reset()
        {
            targetCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (Mathf.Approximately(lastAspect, targetCamera.aspect)) return;
            Fit();
        }

        private void Fit()
        {
            float cellSize = config.CellSpacing * config.WorldScale;
            float boardWidth = cellSize * config.Columns;
            float boardHeight = cellSize * config.Rows;
            float aspect = targetCamera.aspect;

            float sizeForHeight = boardHeight * 0.5f + padding;
            float sizeForWidth = (boardWidth * 0.5f + padding) / aspect;

            targetCamera.orthographic = true;
            targetCamera.orthographicSize = Mathf.Max(sizeForHeight, sizeForWidth);
            targetCamera.transform.position = new Vector3(0f, 0f, -10f);
            lastAspect = aspect;
        }

        private void OnDrawGizmos()
        {
            if (!drawGrid || config == null) return;

            float cellSize = config.CellSpacing * config.WorldScale;
            float width = cellSize * config.Columns;
            float height = cellSize * config.Rows;
            Gizmos.color = new Color(1f, 1f, 1f, 0.35f);

            for (int column = 0; column <= config.Columns; column++)
            {
                float x = -width * 0.5f + column * cellSize;
                Gizmos.DrawLine(new Vector3(x, -height * 0.5f, 0f), new Vector3(x, height * 0.5f, 0f));
            }

            for (int row = 0; row <= config.Rows; row++)
            {
                float y = -height * 0.5f + row * cellSize;
                Gizmos.DrawLine(new Vector3(-width * 0.5f, y, 0f), new Vector3(width * 0.5f, y, 0f));
            }
        }
    }
}
