// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for Layers in Animancer Lite).
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Demonstrates how to use layers to play multiple animations at once on different body parts.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Layer Example")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/LayerExample")]
    public sealed class LayerExample : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _BasicAnimancer;

        [SerializeField]
        private AnimancerComponent _LayeredAnimancer;

        [SerializeField]
        private AnimationClip _Idle;

        [SerializeField]
        private AnimationClip _Run;

        [SerializeField]
        private AnimationClip _Action;

        [SerializeField]
        private AvatarMask _ActionMask;

        /************************************************************************************************************************/

        private const int BaseLayer = 0;
        private const int ActionLayer = 1;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            // Idle on default layer 0.
            _BasicAnimancer.Play(_Idle);
            _LayeredAnimancer.Play(_Idle);

            // Set the mask for layer 1 (this automatically creates the layer).
            _LayeredAnimancer.SetLayerMask(ActionLayer, _ActionMask);
        }

        /************************************************************************************************************************/

        private bool _IsRunning;

        public void ToggleRunning()
        {
            _IsRunning = !_IsRunning;

            if (_IsRunning)
            {
                _BasicAnimancer.CrossFade(_Run);
                _LayeredAnimancer.CrossFade(_Run);
            }
            else
            {
                _BasicAnimancer.CrossFade(_Idle);
                _LayeredAnimancer.CrossFade(_Idle);
            }
        }

        /************************************************************************************************************************/

        public void PerformAction()
        {
            var state = _BasicAnimancer.CrossFade(_Action);
            state.OnEnd = () => _BasicAnimancer.CrossFade(_IsRunning ? _Run : _Idle);

            const float FadeDuration = AnimancerPlayable.DefaultFadeDuration;

            // When running, perform the action on the ActionLayer (1) then fade that layer back out.
            if (_IsRunning)
            {
                state = _LayeredAnimancer.CrossFade(_Action, FadeDuration, ActionLayer);
                state.OnEnd = () => _LayeredAnimancer.GetLayer(ActionLayer).StartFade(0);
            }
            else// Otherwise perform the action on the BaseLayer (0) then return to idle.
            {
                state = _LayeredAnimancer.CrossFade(_Action, FadeDuration, BaseLayer);
                state.OnEnd = () => _LayeredAnimancer.CrossFade(_Idle);
            }
        }

        /************************************************************************************************************************/
    }
}
