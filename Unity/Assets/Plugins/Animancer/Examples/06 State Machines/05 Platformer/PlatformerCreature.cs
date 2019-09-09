// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

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
    [AddComponentMenu("Animancer/Examples/Platformer Creature")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/PlatformerCreature")]
    public sealed class PlatformerCreature : MonoBehaviour, IAnimancerClipSource
    {
        /************************************************************************************************************************/

        [SerializeField]
        private AnimancerComponent _Animancer;
        public AnimancerComponent Animancer { get { return _Animancer; } }

        [SerializeField]
        private SpriteRenderer _Renderer;
        public SpriteRenderer Renderer { get { return _Renderer; } }

        [SerializeField]
        private PlatformerCreatureBrain _Brain;
        public PlatformerCreatureBrain Brain { get { return _Brain; } }

        [SerializeField]
        private Rigidbody2D _Rigidbody;
        public Rigidbody2D Rigidbody { get { return _Rigidbody; } }

        [SerializeField]
        private GroundDetector2D _GroundDetector;
        public GroundDetector2D GroundDetector { get { return _GroundDetector; } }

        [SerializeField]
        private Health _Health;
        public Health Health { get { return _Health; } }

        [SerializeField]
        private PlatformerCreatureState _Idle;
        public PlatformerCreatureState Idle { get { return _Idle; } }

        // Stats.
        // Mana.
        // Pathfinding.
        // Etc.
        // Anything common to most creatures.

        /************************************************************************************************************************/

        public FSM.StateMachine<PlatformerCreatureState> StateMachine { get; private set; }

        /// <summary>
        /// Forces the <see cref="StateMachine"/> to return to the <see cref="Idle"/> state.
        /// </summary>
        public Action ForceEnterIdleState { get; private set; }

        /************************************************************************************************************************/

        private void Awake()
        {
            ForceEnterIdleState = () => StateMachine.ForceSetState(_Idle);

            StateMachine = new FSM.StateMachine<PlatformerCreatureState>(_Idle);
        }

        /************************************************************************************************************************/

        private void FixedUpdate()
        {
            var speed = StateMachine.CurrentState.MovementSpeed * _Brain.MovementDirection;
            _Rigidbody.velocity = new Vector2(speed, _Rigidbody.velocity.y);

            // The sprites face right by default, so flip the X axis when moving left.
            if (speed != 0)
                _Renderer.flipX = _Brain.MovementDirection < 0;
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only]
        /// Displays the current state at the bottom of the inspector.
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
                UnityEditor.EditorGUILayout.ObjectField("Current State", StateMachine.CurrentState, typeof(PlatformerCreatureState), true);
            }
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}
