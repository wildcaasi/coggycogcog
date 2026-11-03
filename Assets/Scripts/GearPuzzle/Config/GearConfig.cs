using UnityEngine;

namespace GearPuzzle
{
    [CreateAssetMenu(fileName = "GearConfig", menuName = "GearPuzzle/Gear Config")]
    public sealed class GearConfig : ScriptableObject
    {
        [SerializeField] private float cellSpacing = 150f;
        [SerializeField] private float smallRadius = 75f;
        [SerializeField] private float largeRadius = 150f;
        [SerializeField, Min(13f)] private float margin = 15f;
        [SerializeField, Min(3)] private int rows = 12;
        [SerializeField, Min(1)] private int columns = 6;
        [SerializeField, Min(0.1f)] private float doubleTapSeconds = 0.3f;
        [SerializeField, Min(0.0001f)] private float worldScale = 0.01f;

        public float CellSpacing => cellSpacing;
        public float SmallRadius => smallRadius;
        public float LargeRadius => largeRadius;
        public float Margin => margin;
        public int Rows => rows;
        public int Columns => columns;
        public float DoubleTapSeconds => doubleTapSeconds;
        public float WorldScale => worldScale;
    }
}
