// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Base class for the various states a <see cref="PlatformerCreature"/> can be in and actions they can perform.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "CreatureState", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/PlatformerCreatureState")]
    public abstract class PlatformerCreatureState : FSM.StateBehaviour<PlatformerCreatureState>,
        FSM.IOwnedState<PlatformerCreatureState>, IAnimancerClipSource
    {
        /************************************************************************************************************************/

        [SerializeField]
        private PlatformerCreature _Creature;

        /// <summary>The <see cref="Examples.PlatformerCreature"/> that owns this state.</summary>
        public PlatformerCreature Creature { get { return _Creature; } }

        /// <summary>
        /// Sets the <see cref="Creature"/>.
        /// </summary>
        /// <remarks>
        /// This isn't a property setter because you shouldn't be casually changing the owner of a state. Usually this
        /// would only be used when adding a state to a creature using a script.
        /// </remarks>
        public void SetCreature(PlatformerCreature creature)
        {
            _Creature = creature;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// The current speed at which this state allows the creature to move.
        /// </summary>
        /// <remarks>
        /// This value is always 0 unless overridden by a child class.
        /// </remarks>
        public virtual float MovementSpeed { get { return 0; } }

        /************************************************************************************************************************/

        /// <summary>
        /// The <see cref="AnimancerComponent"/> of the <see cref="Creature"/>.
        /// </summary>
        public AnimancerComponent Animancer { get { return _Creature.Animancer; } }

        /************************************************************************************************************************/

        public FSM.StateMachine<PlatformerCreatureState> OwnerStateMachine { get { return _Creature.StateMachine; } }

        /************************************************************************************************************************/
    }
}
