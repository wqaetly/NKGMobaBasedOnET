// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A <see cref="PlatformerCreatureState"/> that performs an attack animation then returns to idle.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "AttackState", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Platformer Attack State")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/PlatformerAttackState")]
    public sealed class PlatformerAttackState : PlatformerCreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _GroundAnimation;

        [SerializeField]
        private AnimationClip _AirAnimation;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            var animation = Creature.GroundDetector.IsGrounded ? _GroundAnimation : _AirAnimation;
            var state = Creature.Animancer.Play(animation);
            state.OnEnd = Creature.ForceEnterIdleState;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns false so that nothing can interript an attack until it is done.
        /// </summary>
        /// <remarks>
        /// If we had a Flinch state that gets triggered when the creature gets hit by an enemy, we would probably want
        /// it to be able to interrupt an attack. To do this, we could use an int or an enum like in the
        /// <see cref="InterruptableCreatureState.Priority"/> example.
        /// <para></para>
        /// We could just enter the Flinch state using <see cref="FSM.StateMachine{TState}.ForceSetState(TState)"/>,
        /// but then we would never be able to prevent it from triggering. For example, interacting with an object or
        /// doing something in a cutscene might use a state that should never be interrupted.
        /// </remarks>
        public override bool CanExitState(PlatformerCreatureState nextState)
        {
            return false;
        }

        /************************************************************************************************************************/
    }
}
