// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Base class for any kind of <see cref="PlatformerCreature"/> controller - local, network, AI, replay, etc.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "CreatureBrain", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/PlatformerCreatureBrain")]
    public abstract class PlatformerCreatureBrain : MonoBehaviour, IAnimancerClipSource
    {
        /************************************************************************************************************************/

        [SerializeField]
        private PlatformerCreature _Creature;
        public PlatformerCreature Creature { get { return _Creature; } }

        public AnimancerComponent Animancer { get { return _Creature.Animancer; } }

        /************************************************************************************************************************/

        public float MovementDirection { get; protected set; }

        public bool IsRunning { get; protected set; }

        /************************************************************************************************************************/
    }
}
