// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.FSM;
using System;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// A centralised group of references to the common parts of a creature and a state machine for their actions.
    /// </summary>
    /// <remarks>
    /// This class would normally just be called "Creature", but this name was chosen to avoid conflict with the
    /// other examples.
    /// </remarks>
    [AddComponentMenu("Animancer/Examples/Brains Creature")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/BrainsCreature")]
    public sealed class BrainsCreature : MonoBehaviour, IAnimancerClipSource
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;
        public AnimancerComponent Animancer { get { return _Animancer; } }

        [SerializeField]
        private BrainsCreatureState _Idle;
        public BrainsCreatureState Idle { get { return _Idle; } }

        [SerializeField]
        private Rigidbody _Rigidbody;
        public Rigidbody Rigidbody { get { return _Rigidbody; } }

        [SerializeField]
        private BrainsCreatureBrain _Brain;
        public BrainsCreatureBrain Brain
        {
            get { return _Brain; }
            set
            {
                if (_Brain == value)
                    return;

                var oldBrain = _Brain;
                _Brain = value;

                // Make sure the old brain doesn't still reference this creature.
                if (oldBrain != null)
                    oldBrain.Creature = null;

                // Give the new brain a reference to this creature.
                if (value != null)
                    value.Creature = this;
            }
        }

        [SerializeField]
        private BrainsCreatureStats _Stats;
        public BrainsCreatureStats Stats { get { return _Stats; } }

        // Ground Detector.
        // Health and Mana.
        // Pathfinding.
        // Etc.
        // Anything common to most creatures.

        /************************************************************************************************************************/

        public StateMachine<BrainsCreatureState> StateMachine { get; private set; }

        /// <summary>
        /// Forces the <see cref="StateMachine"/> to return to the <see cref="Idle"/> state.
        /// </summary>
        public Action ForceEnterIdleState { get; private set; }

        /************************************************************************************************************************/

        private void Awake()
        {
            ForceEnterIdleState = () => StateMachine.ForceSetState(_Idle);

            StateMachine = new StateMachine<BrainsCreatureState>(_Idle);
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Inspector Gadgets Pro calls this method after drawing the regular Inspector GUI, allowing this script to
        /// display its current state in Play Mode.
        /// </summary>
        /// <remarks>
        /// Inspector Gadgets Pro allows you to easily customise the inspector without writing a full custom inspector
        /// class by simply adding a method with this name. Without Inspector Gadgets, this method will do nothing.
        /// It can be purchased from https://assetstore.unity.com/packages/tools/gui/inspector-gadgets-pro-83013
        /// </remarks>
        private void AfterInspectorGUI()
        {
            if (UnityEditor.EditorApplication.isPlaying)
            {
                GUI.enabled = false;
                UnityEditor.EditorGUILayout.ObjectField("Current State", StateMachine.CurrentState, typeof(BrainsCreatureState), true);
            }
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
