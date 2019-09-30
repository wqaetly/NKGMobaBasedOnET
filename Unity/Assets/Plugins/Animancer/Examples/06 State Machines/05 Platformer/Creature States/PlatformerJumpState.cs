// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A <see cref="PlatformerCreatureState"/> that plays a jump animation and applies some upwards force.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "JumpState", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Platformer JumpState")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/PlatformerJumpState")]
    public sealed class PlatformerJumpState : PlatformerCreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _Animation;

        [SerializeField]
        private float _Force;

        private AnimancerState _AnimancerState;

        /************************************************************************************************************************/

        public override float MovementSpeed
        {
            get { return Creature.Idle.MovementSpeed; }
        }

        /************************************************************************************************************************/

        public override bool CanEnterState(PlatformerCreatureState previousState)
        {
            return Creature.GroundDetector.IsGrounded;
        }

        /************************************************************************************************************************/

        private void OnEnable()
        {
            Creature.Rigidbody.velocity += new Vector2(0, _Force);

            _AnimancerState = Creature.Animancer.Play(_Animation);
        }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            // Wait until we are grounded and the animation has finished, then return to idle.
            if (Creature.GroundDetector.IsGrounded && _AnimancerState.NormalizedTime > 1)
                Creature.Idle.ForceEnterState();
        }

        /************************************************************************************************************************/
    }
}
