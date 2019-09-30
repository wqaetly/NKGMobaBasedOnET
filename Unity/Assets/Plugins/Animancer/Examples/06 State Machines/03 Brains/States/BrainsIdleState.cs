// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A <see cref="BrainsCreatureState"/> which keeps the creature standing still.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "IdleState", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Brains Idle State")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/BrainsIdleState")]
    public sealed class BrainsIdleState : BrainsCreatureState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimationClip _Animation;

        /************************************************************************************************************************/

        private void OnEnable()
        {
            Creature.Animancer.CrossFade(_Animation);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Constantly clears the <see cref="Rigidbody.velocity"/> to ensure that the creature doesn't slide or get
        /// pushed around too easily.
        /// </summary>
        /// <remarks>
        /// This method is kept simple for the sake of demonstrating the animation system in this example.
        /// In a real game you would usually not set the velocity directly, but would instead use
        /// <see cref="Rigidbody.AddForce"/> to avoid interfering with collisions and other forces.
        /// </remarks>
        private void FixedUpdate()
        {
            Creature.Rigidbody.velocity = Vector3.zero;
        }

        /************************************************************************************************************************/
    }
}
