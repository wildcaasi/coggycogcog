using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GearPuzzle
{
    public sealed class PuzzleHud : MonoBehaviour
    {
        [SerializeField] private TMP_Text targetLabel;
        [SerializeField] private GameObject winBanner;
        [SerializeField] private Button newPuzzleButton;

        private PuzzleSession session;

        public void Initialize(PuzzleSession session)
        {
            this.session = session;
            session.Updated += OnUpdated;
            newPuzzleButton.onClick.AddListener(session.NewPuzzle);
        }

        public bool HasRequiredReferences()
        {
            bool valid = true;
            if (targetLabel == null) { Debug.LogError("PuzzleHud: Target Label is not assigned.", this); valid = false; }
            if (winBanner == null) { Debug.LogError("PuzzleHud: Win Banner is not assigned.", this); valid = false; }
            if (newPuzzleButton == null) { Debug.LogError("PuzzleHud: New Puzzle Button is not assigned.", this); valid = false; }
            return valid;
        }

        private void OnDestroy()
        {
            if (session == null) return;
            session.Updated -= OnUpdated;
            if (newPuzzleButton != null) newPuzzleButton.onClick.RemoveListener(session.NewPuzzle);
        }

        private void OnUpdated(PuzzleSnapshot snapshot)
        {
            Debug.Log($"HUD got solved={snapshot.Solved}");

            targetLabel.text = LabelFor(snapshot.Puzzle.TargetDirection);
            winBanner.SetActive(snapshot.Solved);
        }

        private string LabelFor(GearState target)
        {
            switch (target)
            {
                case GearState.Clockwise: return "Target: Clockwise";
                case GearState.CounterClockwise: return "Target: Counter-clockwise";
                default: return string.Empty;
            }
        }
    }
}
