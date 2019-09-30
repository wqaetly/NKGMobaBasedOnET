// Animancer // Copyright 2019 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// A <see cref="ScriptableObject"/> which defines various details about how to play an animation so they can be
    /// configured in the Unity Editor instead of hard coded.
    /// <para></para>
    /// To trigger a transition, simply pass it as a parameter into
    /// <see cref="AnimancerComponent.Play(IAnimancerTransition, int)"/> or
    /// <see cref="AnimancerComponent.CrossFade(IAnimancerTransition, int)"/>.
    /// </summary>
    public class AnimancerTransition<TSerializable, TState> : ScriptableObject, IAnimancerTransition
        where TSerializable : AnimancerState.Serializable<TState>
        where TState : AnimancerState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private TSerializable _Animation;

        /// <summary>The details of the state that this transition will create.</summary>
        public TSerializable Animation
        {
            get { return _Animation; }
            set { _Animation = value; }
        }

        /************************************************************************************************************************/

        /// <summary>The amount of time the transition should take (in seconds).</summary>
        public float FadeDuration
        {
            get { return _Animation.FadeDuration; }
            set { _Animation.FadeDuration = value; }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// The <see cref="AnimancerState.Key"/> which the created state will be registered with.
        /// </summary>
        public object Key { get { return _Animation.Key; } }

        /// <summary>
        /// When a serializable is passed into <see cref="AnimancerPlayable.Transition"/>, this property
        /// determines whether it needs to fade in from the start of the animation.
        /// </summary>
        public bool CrossFadeFromStart { get { return _Animation.CrossFadeFromStart; } }

        /// <summary>
        /// Creates and returns a new <typeparamref name="TState"/> connected to the 'layer'.
        /// </summary>
        public AnimancerState CreateState(AnimancerLayer layer)
        {
            return _Animation.CreateState(layer);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Applies the additional parameters of this transition to the 'state'. By default this is just the
        /// <see cref="Speed"/>, but this class could be inherited to add more parameters.
        /// </summary>
        public virtual void Apply(AnimancerState state)
        {
            _Animation.Apply(state);
        }

        /************************************************************************************************************************/
    }

    /************************************************************************************************************************/

    /// <summary>
    /// A <see cref="ScriptableObject"/> which defines various details about how to play an animation so they can be
    /// configured in the Unity Editor instead of hard coded.
    /// <para></para>
    /// To trigger a transition, simply pass it as a parameter into
    /// <see cref="AnimancerComponent.Play(IAnimancerTransition, int)"/> or
    /// <see cref="AnimancerComponent.CrossFade(IAnimancerTransition, int)"/>.
    /// </summary>
    [CreateAssetMenu(menuName = "Animancer/Transition", order = AnimancerComponent.AssetMenuOrder)]
    public sealed class AnimancerTransition : AnimancerTransition<ClipState.Serializable, ClipState>
    {
        /************************************************************************************************************************/

        /// <summary>The animation to play.</summary>
        public AnimationClip Clip
        {
            get { return Animation.Clip; }
            set { Animation.Clip = value; }
        }

        /************************************************************************************************************************/

        /// <summary>Determines how fast the animation plays (default = 1).</summary>
        public float Speed
        {
            get { return Animation.Speed; }
            set { Animation.Speed = value; }
        }

        /************************************************************************************************************************/
    }
}
