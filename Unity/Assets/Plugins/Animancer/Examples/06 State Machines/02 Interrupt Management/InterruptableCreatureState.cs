// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A state for an <see cref="InterruptableCreature"/> which plays an animation and uses a <see cref="Priority"/>
    /// enum to determine which other states can interrupt it.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "CreatureState", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Interruptable Creature State")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/InterruptableCreatureState")]
    public sealed class InterruptableCreatureState : StateBehaviour<InterruptableCreatureState>, IAnimancerClipSource
    {
        /************************************************************************************************************************/

        /// <summary>An indication of importance.</summary>
        public enum Priority
        {
            Low,
            Medium,
            High,
        }

        /************************************************************************************************************************/

        [SerializeField]
        private InterruptableCreature _Creature;

        [SerializeField]
        private AnimationClip _Animation;

        [SerializeField]
        private Priority _Priority;

        /************************************************************************************************************************/

        public AnimancerComponent Animancer { get { return _Creature.Animancer; } }

        /************************************************************************************************************************/

        private void OnEnable()
        {
            var state = _Creature.Animancer.CrossFade(_Animation);
            if (!_Animation.isLooping)
                state.OnEnd = _Creature.ForceIdleState;
        }

        /************************************************************************************************************************/

        public override bool CanExitState(InterruptableCreatureState nextState)
        {
            return nextState._Priority >= _Priority;
        }

        /************************************************************************************************************************/
    }
}
