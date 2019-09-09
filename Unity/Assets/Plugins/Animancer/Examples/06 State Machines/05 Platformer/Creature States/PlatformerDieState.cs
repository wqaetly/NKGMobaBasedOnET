// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A <see cref="PlatformerCreatureState"/> that plays a die animation.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "DieState", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Platformer Die State")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/PlatformerDieState")]
    public sealed class PlatformerDieState : PlatformerCreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _Animation;

        /************************************************************************************************************************/

        private void Awake()
        {
            Creature.Health.OnHealthChanged += () =>
            {
                if (Creature.Health.CurrentHealth <= 0)
                    Creature.StateMachine.ForceSetState(this);
                else if (enabled)
                    Creature.Idle.ForceEnterState();
            };
        }

        /************************************************************************************************************************/

        private void OnEnable()
        {
            Creature.Animancer.Play(_Animation);
        }

        /************************************************************************************************************************/

        public override bool CanExitState(PlatformerCreatureState nextState)
        {
            return Creature.Health.CurrentHealth > 0;
        }

        /************************************************************************************************************************/
    }
}
