using UnityEngine;

namespace GearPuzzle
{
    public sealed class GearView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float smallGearDegreesPerSecond = 90f;

        private GearState state = GearState.NotMoving;
        private float degreesPerSecond;

        private void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Initialize(Gear gear, GearGeometry geometry, GridCoordinates coordinates)
        {
            if (spriteRenderer == null || spriteRenderer.sprite == null)
            {
                Debug.LogError("GearView prefab needs a SpriteRenderer with a sprite assigned.", this);
                return;
            }

            float radius = geometry.RadiusOf(gear.Size);
            float smallRadius = geometry.RadiusOf(GearSize.Small);
            degreesPerSecond = smallGearDegreesPerSecond * smallRadius / radius;

            float worldDiameter = coordinates.ToWorldLength(radius * 2f);
            float spriteWidth = spriteRenderer.sprite.bounds.size.x;
            transform.localScale = Vector3.one * (worldDiameter / spriteWidth);
        }

        public void Apply(GearState newState, Color tint)
        {
            state = newState;
            spriteRenderer.color = tint;
        }

        private void Update()
        {
            float signedSpeed;
            switch (state)
            {
                case GearState.Clockwise:
                    signedSpeed = -degreesPerSecond;
                    break;
                case GearState.CounterClockwise:
                    signedSpeed = degreesPerSecond;
                    break;
                default:
                    return;
            }

            transform.Rotate(0f, 0f, signedSpeed * Time.deltaTime);
        }
    }
}
