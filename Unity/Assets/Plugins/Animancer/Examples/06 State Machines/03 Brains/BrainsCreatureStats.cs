// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// The details of a <see cref="BrainsCreature"/>.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "CreatureStats", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [Serializable]
    public sealed class BrainsCreatureStats
    {
        /************************************************************************************************************************/

        [SerializeField]
        private float _WalkSpeed = 2;
        public float WalkSpeed { get { return _WalkSpeed; } }

        [SerializeField]
        private float _RunSpeed = 4;
        public float RunSpeed { get { return _RunSpeed; } }

        public float GetMoveSpeed(bool isRunning)
        {
            return isRunning ? _RunSpeed : _WalkSpeed;
        }

        /************************************************************************************************************************/

        [SerializeField]
        private float _TurnSpeed = 360;
        public float TurnSpeed { get { return _TurnSpeed; } }

        /************************************************************************************************************************/

        // Max health.
        // Strength, dexterity, intelligence.
        // Carrying capacity.
        // Etc.

        /************************************************************************************************************************/
    }
}
