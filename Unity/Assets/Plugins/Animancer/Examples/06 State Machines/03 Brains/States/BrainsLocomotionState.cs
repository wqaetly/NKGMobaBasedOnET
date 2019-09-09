// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A <see cref="BrainsCreatureState"/> which moves the creature according to their
    /// <see cref="BrainsCreatureBrain.MovementDirection"/>.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "LocomotionState", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Brains Locomotion State")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/BrainsLocomotionState")]
    public sealed class BrainsLocomotionState : BrainsCreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _Walk;

        [SerializeField]
        private AnimationClip _Run;

        /************************************************************************************************************************/

        /// <summary>
        /// This method is basically identical to the "PlayMove" method in the <see cref="IdleAndWalkAndRun"/> script,
        /// but instead of checking <see cref="Input"/> to determine whether or not to run we are checking if the
        /// <see cref="BrainsCreature.Brain"/> says it wants to run.
        /// </summary>
        private void Update()
        {
            UpdateAnimation();
            UpdateTurning();
        }

        /************************************************************************************************************************/

        private void UpdateAnimation()
        {
            // We will play either the Walk or Run animation.

            // We need to know which animation we are trying to play and which is the other one.
            AnimationClip playAnimation, otherAnimation;

            if (Creature.Brain.IsRunning)
            {
                playAnimation = _Run;
                otherAnimation = _Walk;
            }
            else
            {
                playAnimation = _Walk;
                otherAnimation = _Run;
            }

            // Play the one we want.
            var playState = Animancer.CrossFade(playAnimation);

            // If the brain wants to move slowly, slow down the animation.
            var speed = Mathf.Min(Creature.Brain.MovementDirection.magnitude, 1);
            playState.Speed = speed;

            // If the other one is still fading out, align their NormalizedTime to ensure they stay at the same
            // relative progress through their walk cycle.
            var otherState = Animancer.GetState(otherAnimation);
            if (otherState != null && otherState.IsPlaying)
            {
                playState.NormalizedTime = otherState.NormalizedTime;
                otherState.Speed = speed;
            }
        }

        /************************************************************************************************************************/

        private void UpdateTurning()
        {
            var targetAngle = Mathf.Atan2(Creature.Brain.MovementDirection.x, Creature.Brain.MovementDirection.z) * Mathf.Rad2Deg;
            var turnDelta = Creature.Stats.TurnSpeed * Time.deltaTime;

            var transform = Creature.Animancer.transform;
            var eulerAngles = transform.eulerAngles;
            eulerAngles.y = Mathf.MoveTowardsAngle(eulerAngles.y, targetAngle, turnDelta);
            transform.eulerAngles = eulerAngles;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Constantly moves the creature according to the <see cref="BrainsCreatureBrain.MovementDirection"/>.
        /// </summary>
        /// <remarks>
        /// This method is kept simple for the sake of demonstrating the animation system in this example.
        /// In a real game you would usually not set the velocity directly, but would instead use
        /// <see cref="Rigidbody.AddForce"/> to avoid interfering with collisions and other forces.
        /// </remarks>
        private void FixedUpdate()
        {
            // Get the desired direction, remove any vertical component, and limit the magnitude to 1 or less.
            // Otherwise a brain could make the creature travel at any speed by setting a longer vector.
            var direction = Creature.Brain.MovementDirection;
            direction.y = 0;
            direction = Vector3.ClampMagnitude(direction, 1);

            var speed = Creature.Stats.GetMoveSpeed(Creature.Brain.IsRunning);

            Creature.Rigidbody.velocity = direction * speed;
        }

        /************************************************************************************************************************/
    }
}
