// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Starts with an idle animation and performs an action when the user clicks the mouse, then returns to idle.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Play Animation On Click")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/PlayAnimationOnClick")]
    public sealed class PlayAnimationOnClick : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;

        [SerializeField]
        private AnimationClip _Idle;

        [SerializeField]
        private AnimationClip _Action;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            // Play the idle on startup.
            _Animancer.Play(_Idle);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Play the action animation and register a callback for when it finishes.
                var state = _Animancer.CrossFade(_Action);
                state.OnEnd = OnActionEnd;
            }
        }

        /************************************************************************************************************************/

        private void OnActionEnd()
        {
            // Now that the action is done, go back to idle.
            _Animancer.CrossFade(_Idle);
        }

        /************************************************************************************************************************/
    }
}
