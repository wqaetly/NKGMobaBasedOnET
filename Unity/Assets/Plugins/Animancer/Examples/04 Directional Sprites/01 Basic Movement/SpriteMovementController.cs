// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Animates a character to either stand idle or walk using the animations defined in
    /// <see cref="DirectionalAnimationSet"/>s.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Sprite Movement Controller")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/SpriteMovementController")]
    public sealed class SpriteMovementController : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;

        [SerializeField]
        private DirectionalAnimationSet _Idles;

        [SerializeField]
        private DirectionalAnimationSet _Walks;

        /************************************************************************************************************************/

        private Vector2 _Facing = Vector2.down;

        /************************************************************************************************************************/

        private void Awake()
        {
            // Instead of only a single animation, we have a different one for each direction we can face.
            // So we get whichever is appropriate for that direction and play it.
            var clip = _Idles.GetClip(_Facing);
            _Animancer.Play(clip);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input != Vector2.zero)
            {
                _Facing = input;
                var state = _Animancer.Play(_Walks.GetClip(_Facing));

                // We can still set the Speed of the animation we are playing, exactly the same as we normally would.

                if (Input.GetButton("Fire3"))// Left Shift by default.
                    state.Speed = 2;
                else
                    state.Speed = 1;
            }
            else
            {
                // When we aren't moving, we still remember the direction we are facing so we can continue using the
                // correct idle animation for that direction.
                _Animancer.Play(_Idles.GetClip(_Facing));
            }
        }

        /************************************************************************************************************************/
    }
}
