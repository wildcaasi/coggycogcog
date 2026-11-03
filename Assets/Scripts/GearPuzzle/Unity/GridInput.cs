using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GearPuzzle
{
    public sealed class GridInput : MonoBehaviour
    {
        [SerializeField] private Camera worldCamera;
        [SerializeField] private InputActionReference pointAction;
        [SerializeField] private InputActionReference tapAction;

        private readonly List<RaycastResult> uiHits = new();
        private GridCoordinates coordinates;
        private TapClassifier classifier;

        public event Action<Cell> CellTapped;
        public event Action<Cell> CellDoubleTapped;

        private int lastHandledFrame = -1;

        public void Initialize(GridCoordinates coordinates, TapClassifier classifier)
        {
            this.coordinates = coordinates;
            this.classifier = classifier;

            pointAction.action.Enable();
            tapAction.action.Enable();
            tapAction.action.performed += OnTap;
        }

        private void Update()
        {
            if (Touchscreen.current == null) return;
            int active = 0;
            foreach (var touch in Touchscreen.current.touches)
                if (touch.isInProgress) active++;

            if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
                Debug.Log($"Raw press activeTouches={active} handled={lastHandledFrame == Time.frameCount}");
        }

        public bool HasRequiredReferences()
        {
            bool valid = true;
            if (worldCamera == null) { Debug.LogError("GridInput: Camera is not assigned.", this); valid = false; }
            if (pointAction == null) { Debug.LogError("GridInput: Point Action is not assigned.", this); valid = false; }
            if (tapAction == null) { Debug.LogError("GridInput: Tap Action is not assigned.", this); valid = false; }
            return valid;
        }

        private void OnDestroy()
        {
            if (classifier == null) return;
            tapAction.action.performed -= OnTap;
        }

        private void OnTap(InputAction.CallbackContext context)
        {
            if (!context.ReadValueAsButton()) return;

            lastHandledFrame = Time.frameCount;

            Vector2 screenPosition = pointAction.action.ReadValue<Vector2>();

            //if (IsOverUi(screenPosition)) return;
            Debug.Log("OnTap fired");
            if (IsOverUi(screenPosition))
            {
                Debug.Log($"Blocked by UI: {uiHits[0].gameObject.name}");
                return;
            }

            float depth = -worldCamera.transform.position.z;
            Vector3 world = worldCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, depth));
            if (!coordinates.TryWorldToCell(world, out Cell cell)) return;

            switch (classifier.Classify(cell, Time.unscaledTime))
            {
                case TapKind.Single:
                    CellTapped?.Invoke(cell);
                    break;
                case TapKind.Double:
                    CellDoubleTapped?.Invoke(cell);
                    break;
            }
        }


        private bool IsOverUi(Vector2 screenPosition)
        {
            if (EventSystem.current == null) return false;

            var pointer = new PointerEventData(EventSystem.current) { position = screenPosition };
            uiHits.Clear();
            EventSystem.current.RaycastAll(pointer, uiHits);
            return uiHits.Count > 0;
        }
    }
}
