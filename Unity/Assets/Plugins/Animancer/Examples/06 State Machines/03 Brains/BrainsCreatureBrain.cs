// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Base class for controlling the actions of a <see cref="BrainsCreature"/>.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "CreatureBrain", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Brains Creature Brain")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/BrainsCreatureBrain")]
    public abstract class BrainsCreatureBrain : MonoBehaviour, IAnimancerClipSource
    {
        /************************************************************************************************************************/

        [SerializeField]
        private BrainsCreature _Creature;
        public BrainsCreature Creature
        {
            get { return _Creature; }
            set
            {
                if (_Creature == value)
                    return;

                var oldCreature = _Creature;
                _Creature = value;

                // Make sure the old creature doesn't still reference this brain.
                if (oldCreature != null)
                    oldCreature.Brain = null;

                // Give the new creature a reference to this brain.
                if (value != null)
                    value.Brain = this;
            }
        }

        public AnimancerComponent Animancer { get { return _Creature.Animancer; } }

        /************************************************************************************************************************/

        public Vector3 MovementDirection { get; protected set; }

        public bool IsRunning { get; protected set; }

        /************************************************************************************************************************/
    }
}
