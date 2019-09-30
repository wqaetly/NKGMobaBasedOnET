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
    [AddComponentMenu("Animancer/Examples/Basic Creature")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/BasicCreature")]
    public sealed class BasicCreature : MonoBehaviour, IAnimancerClipSource
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;
        public AnimancerComponent Animancer { get { return _Animancer; } }

        [SerializeField]
        private BasicCreatureState _Idle;
        public BasicCreatureState Idle { get { return _Idle; } }

        // Rigidbody.
        // Ground Detector.
        // Stats.
        // Health and Mana.
        // Pathfinding.
        // Etc.
        // Anything common to most creatures.

        /************************************************************************************************************************/

        public StateMachine<BasicCreatureState> StateMachine { get; private set; }

        /// <summary>
        /// Forces the <see cref="StateMachine"/> to return to the <see cref="Idle"/> state.
        /// </summary>
        public Action ForceIdleState { get; private set; }

        /************************************************************************************************************************/

        private void Awake()
        {
            ForceIdleState = () => StateMachine.ForceSetState(_Idle);

            StateMachine = new StateMachine<BasicCreatureState>(_Idle);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="StateMachine{TState}.TrySetState"/>. Normally you would just access the
        /// <see cref="StateMachine"/> directly. This method only exists to be called by UI buttons.
        /// </summary>
        public void TrySetState(BasicCreatureState state)
        {
            StateMachine.TrySetState(state);
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
                UnityEditor.EditorGUILayout.ObjectField("Current State", StateMachine.CurrentState, typeof(BasicCreatureState), true);
            }
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
