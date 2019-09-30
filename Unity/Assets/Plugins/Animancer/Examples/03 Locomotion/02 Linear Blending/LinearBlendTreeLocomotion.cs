// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0618 // Type or member is obsolete (for ControllerStates in Animancer Lite).
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// An example of how you can wrap a <see cref="RuntimeAnimatorController"/> containing a single blend tree in a
    /// <see cref="FloatControllerState"/> to easily control its parameter.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Linear Blend Tree Locomotion")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/LinearBlendTreeLocomotion")]
    public sealed class LinearBlendTreeLocomotion : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;

        [SerializeField]
        private FloatControllerState.Serializable _Controller;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            _Animancer.Transition(_Controller);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Set by a <see cref="UnityEngine.Events.UnityEvent"/> on a <see cref="UnityEngine.UI.Slider"/>.
        /// </summary>
        public float Speed
        {
            get { return _Controller.State.Parameter; }
            set { _Controller.State.Parameter = value; }
        }

        /************************************************************************************************************************/
    }
}
