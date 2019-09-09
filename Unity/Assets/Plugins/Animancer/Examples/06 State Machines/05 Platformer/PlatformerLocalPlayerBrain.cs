// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A brain for creatures controlled by local input (keyboard and mouse).
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "LocalPlayerBrain", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Local Player Brain 2D")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/LocalPlayerBrain 2D")]
    public sealed class PlatformerLocalPlayerBrain : PlatformerCreatureBrain
    {
        /************************************************************************************************************************/

        [SerializeField]
        private PlatformerCreatureState _Jump;

        [SerializeField]
        private PlatformerCreatureState _Attack;

        /************************************************************************************************************************/

        private void Update()
        {
            if (Input.GetButtonUp("Fire1"))// Left Click by default.
                Creature.StateMachine.TrySetState(_Attack);

            if (Input.GetButtonDown("Jump"))// Space by default.
                Creature.StateMachine.TrySetState(_Jump);

            // GetAxisRaw rather than GetAxis because we don't want any smoothing.
            MovementDirection = Input.GetAxisRaw("Horizontal");// A and D or Arrow Keys by default.

            IsRunning = Input.GetButton("Fire3");// Left Shift by default.
        }

        /************************************************************************************************************************/
    }
}
