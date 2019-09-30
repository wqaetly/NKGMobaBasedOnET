// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Base class for the various states a <see cref="BrainsCreature"/> can be in and actions they can perform.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "CreatureState", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Brains Creature State")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/BrainsCreatureState")]
    public abstract class BrainsCreatureState : StateBehaviour<BrainsCreatureState>,
        IOwnedState<BrainsCreatureState>, IAnimancerClipSource
    {
        /************************************************************************************************************************/

        [SerializeField]
        private BrainsCreature _Creature;

        /// <summary>The <see cref="BrainsCreature"/> that owns this state.</summary>
        public BrainsCreature Creature { get { return _Creature; } }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            _Creature = Editor.AnimancerEditorUtilities.GetComponentInHierarchy<BrainsCreature>(gameObject);
        }
#endif

        /************************************************************************************************************************/

        public AnimancerComponent Animancer { get { return _Creature.Animancer; } }

        /************************************************************************************************************************/

        public StateMachine<BrainsCreatureState> OwnerStateMachine { get { return _Creature.StateMachine; } }

        /************************************************************************************************************************/
    }
}
