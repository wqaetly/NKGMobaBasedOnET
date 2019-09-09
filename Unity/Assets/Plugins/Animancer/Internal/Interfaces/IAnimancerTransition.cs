// Animancer // Copyright 2019 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// An object that can create an <see cref="AnimancerState"/> and manage the details of how it should be played.
    /// </summary>
    public interface IAnimancerTransition : IHasKey
    {
        /************************************************************************************************************************/

        /// <summary>Creates and returns a new <see cref="AnimancerState"/> connected to the 'layer'.</summary>
        AnimancerState CreateState(AnimancerLayer layer);

        /// <summary>
        /// When a transition is passed into <see cref="AnimancerPlayable.Transition"/>, this property
        /// determines whether it needs to fade in from the start of the animation.
        /// </summary>
        bool CrossFadeFromStart { get; }

        /// <summary>The amount of time the transition should take (in seconds).</summary>
        float FadeDuration { get; }

        /// <summary>
        /// Called by <see cref="AnimancerPlayable.Transition"/> to apply any modifications to the 'state'.
        /// </summary>
        void Apply(AnimancerState state);

        /************************************************************************************************************************/
    }
}
