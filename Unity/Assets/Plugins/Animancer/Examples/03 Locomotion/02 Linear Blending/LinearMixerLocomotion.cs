// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for MixerStates in Animancer Lite).
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// An example of how you can use a <see cref="LinearMixerState"/> to mix a set of animations based on a
    /// <see cref="Speed"/> parameter.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Linear Mixer Locomotion")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/LinearMixerLocomotion")]
    public sealed class LinearMixerLocomotion : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;

        [SerializeField]
        private LinearMixerState.Serializable _Mixer;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            _Animancer.Transition(_Mixer);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Set by a <see cref="UnityEngine.Events.UnityEvent"/> on a <see cref="UnityEngine.UI.Slider"/>.
        /// </summary>
        public float Speed
        {
            get { return _Mixer.State.Parameter; }
            set { _Mixer.State.Parameter = value; }
        }

        /************************************************************************************************************************/
    }
}
