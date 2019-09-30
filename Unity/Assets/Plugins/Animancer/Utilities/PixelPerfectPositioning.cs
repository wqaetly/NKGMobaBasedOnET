// Animancer // Copyright 2019 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// Adjusts the <see cref="Transform.localPosition"/> every frame to keep this object aligned to a grid with a size
    /// determined by the <see cref="Renderer"/> while wrapping the value to keep it as close to 0 as possible.
    /// </summary>
    [AddComponentMenu("Animancer/Pixel Perfect Positioning")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + "/Pixel Perfect Positioning")]
    public sealed class PixelPerfectPositioning : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private SpriteRenderer _Renderer;

        /// <summary>
        /// The <see cref="SpriteRenderer"/> that will have its position adjusted.
        /// </summary>
        public SpriteRenderer Renderer
        {
            get { return _Renderer; }
            set { _Renderer = value; }
        }

        /************************************************************************************************************************/

#if UNITY_EDITOR
        private void Reset()
        {
            _Renderer = Editor.AnimancerEditorUtilities.GetComponentInHierarchy<SpriteRenderer>(gameObject);
        }
#endif

        /************************************************************************************************************************/

        private void Update()
        {
            var transform = _Renderer.transform;
            var position = transform.position;

            // Snap the position to the pixel grid.
            var pixelsPerUnit = _Renderer.sprite.pixelsPerUnit;
            transform.position = new Vector3(
                Mathf.Round(position.x / pixelsPerUnit) * pixelsPerUnit,
                Mathf.Round(position.y / pixelsPerUnit) * pixelsPerUnit,
                Mathf.Round(position.z / pixelsPerUnit) * pixelsPerUnit);

            // Keep the local position as small as possible while staying on the grid.
            var maxLocalPosition = 0.5f / pixelsPerUnit;
            position = transform.localPosition;
            WrapValue(ref position.x, maxLocalPosition);
            WrapValue(ref position.y, maxLocalPosition);
            WrapValue(ref position.z, maxLocalPosition);
            transform.localPosition = position;
        }

        /************************************************************************************************************************/

        private void WrapValue(ref float value, float max)
        {
            value %= max * 2;

            if (value > max) value -= max * 2;
            else if (value < -max) value += max * 2;
        }

        /************************************************************************************************************************/
    }
}
