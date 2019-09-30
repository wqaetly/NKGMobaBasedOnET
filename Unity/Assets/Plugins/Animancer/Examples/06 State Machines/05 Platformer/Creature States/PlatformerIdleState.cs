// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A <see cref="PlatformerCreatureState"/> that plays an idle animation.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "IdleState", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Platformer Idle State")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/PlatformerIdleState")]
    public sealed class PlatformerIdleState : PlatformerCreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _Idle;

        [SerializeField]
        private AnimationClip _Walk;

        [SerializeField]
        private AnimationClip _Run;

        [SerializeField]
        private float _WalkSpeed;

        [SerializeField]
        private float _RunSpeed;

        /************************************************************************************************************************/

        public override float MovementSpeed
        {
            get { return Creature.Brain.IsRunning ? _RunSpeed : _WalkSpeed; }
        }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            if (Creature.Brain.MovementDirection == 0)
            {
                Creature.Animancer.Play(_Idle);
            }
            else if (Creature.Brain.IsRunning)
            {
                Creature.Animancer.Play(_Run);
            }
            else
            {
                Creature.Animancer.Play(_Walk);
            }
        }

        /************************************************************************************************************************/
    }
}
