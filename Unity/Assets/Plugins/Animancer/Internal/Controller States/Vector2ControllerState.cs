// Animancer // Copyright 2019 Kybernetik //

using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// An <see cref="ControllerState"/> which manages two float parameters.
    /// </summary>
    public sealed class Vector2ControllerState : ControllerState
    {
        /************************************************************************************************************************/

        private Parameter _ParameterX;

        /// <summary>
        /// The name of the parameter which <see cref="ParameterX"/> will get and set.
        /// This will be null if the <see cref="ParameterHashX"/> was assigned directly.
        /// </summary>
        public string ParameterNameX
        {
            get { return _ParameterX.Name; }
            set
            {
                _ParameterX.Name = value;
                _ParameterX.Validate(Controller, AnimatorControllerParameterType.Float);
            }
        }

        /// <summary>
        /// The name hash of the parameter which <see cref="ParameterX"/> will get and set.
        /// </summary>
        public int ParameterHashX
        {
            get { return _ParameterX.Hash; }
            set
            {
                _ParameterX.Hash = value;
                _ParameterX.Validate(Controller, AnimatorControllerParameterType.Float);
            }
        }

        /// <summary>
        /// Gets and sets a float parameter in the <see cref="ControllerState.Controller"/> using the
        /// <see cref="ParameterHashX"/> as the id.
        /// </summary>
        public float ParameterX
        {
            get { return Playable.GetFloat(_ParameterX); }
            set { Playable.SetFloat(_ParameterX, value); }
        }

        /************************************************************************************************************************/

        private Parameter _ParameterY;

        /// <summary>
        /// The name of the parameter which <see cref="ParameterY"/> will get and set.
        /// This will be null if the <see cref="ParameterHashY"/> was assigned directly.
        /// </summary>
        public string ParameterNameY
        {
            get { return _ParameterY.Name; }
            set
            {
                _ParameterY.Name = value;
                _ParameterY.Validate(Controller, AnimatorControllerParameterType.Float);
            }
        }

        /// <summary>
        /// The name hash of the parameter which <see cref="ParameterY"/> will get and set.
        /// </summary>
        public int ParameterHashY
        {
            get { return _ParameterY.Hash; }
            set
            {
                _ParameterY.Hash = value;
                _ParameterY.Validate(Controller, AnimatorControllerParameterType.Float);
            }
        }

        /// <summary>
        /// Gets and sets a float parameter in the <see cref="ControllerState.Controller"/> using the
        /// <see cref="ParameterHashY"/> as the id.
        /// </summary>
        public float ParameterY
        {
            get { return Playable.GetFloat(_ParameterY); }
            set { Playable.SetFloat(_ParameterY, value); }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Gets and sets <see cref="ParameterX"/> and <see cref="ParameterY"/>.
        /// </summary>
        public new Vector2 Parameter
        {
            get
            {
                return new Vector2(ParameterX, ParameterY);
            }
            set
            {
                ParameterX = value.x;
                ParameterY = value.y;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="Vector2ControllerState"/> to play the 'controller' without connecting
        /// it to the <see cref="PlayableGraph"/>. You must call <see cref="AnimancerState.SetParent(AnimancerLayer)"/>
        /// or it won't actually do anything.
        /// </summary>
        private Vector2ControllerState(AnimancerPlayable root, RuntimeAnimatorController controller,
            Parameter parameterX, Parameter parameterY)
            : base(root, controller)
        {
            _ParameterX = parameterX;
            _ParameterX.Validate(Controller, AnimatorControllerParameterType.Float);

            _ParameterY = parameterY;
            _ParameterY.Validate(Controller, AnimatorControllerParameterType.Float);
        }

        /// <summary>
        /// Constructs a new <see cref="Vector2ControllerState"/> to play the 'controller' and
        /// connects it to the 'layer's <see cref="IAnimationMixer.Playable"/> using a spare port if there are any from
        /// previously destroyed states, or by adding a new port.
        /// </summary>
        public Vector2ControllerState(AnimancerLayer layer, RuntimeAnimatorController controller,
            Parameter parameterX, Parameter parameterY)
            : this(layer.Root, controller, parameterX, parameterY)
        {
            layer.AddChild(this);
        }

        /// <summary>
        /// Constructs a new <see cref="Vector2ControllerState"/> to play the 'controller' and
        /// connects it to the 'parent's <see cref="IAnimationMixer.Playable"/> at the specified 'portIndex'.
        /// </summary>
        public Vector2ControllerState(AnimancerNode parent, int portIndex, RuntimeAnimatorController controller,
            Parameter parameterX, Parameter parameterY)
            : this(parent.Root, controller, parameterX, parameterY)
        {
            SetParent(parent, portIndex);
        }

        /************************************************************************************************************************/

        /// <summary>The number of parameters being wrapped by this state.</summary>
        public override int ParameterCount { get { return 2; } }

        /// <summary>Returns the hash of a parameter being wrapped by this state.</summary>
        public override int GetParameterHash(int index)
        {
            switch (index)
            {
                case 0: return ParameterHashX;
                case 1: return ParameterHashY;
                default: throw new ArgumentOutOfRangeException("index");
            };
        }

        /************************************************************************************************************************/
        #region Serializable
        /************************************************************************************************************************/

        /// <summary>
        /// A serializable object which can create a <see cref="Vector2ControllerState"/> when passed into
        /// <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        [Serializable]
        public new class Serializable : Serializable<Vector2ControllerState>
        {
            /************************************************************************************************************************/

            [SerializeField]
            private string _ParameterNameX;

            /// <summary>
            /// The <see cref="Vector2ControllerState.ParameterNameX"/> that will be used for the created state.
            /// </summary>
            public string ParameterNameX
            {
                get { return _ParameterNameX; }
                set { _ParameterNameX = value; }
            }

            /************************************************************************************************************************/

            [SerializeField]
            private string _ParameterNameY;

            /// <summary>
            /// The <see cref="Vector2ControllerState.ParameterNameY"/> that will be used for the created state.
            /// </summary>
            public string ParameterNameY
            {
                get { return _ParameterNameY; }
                set { _ParameterNameY = value; }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Creates and returns a new <see cref="Vector2ControllerState"/> connected to the 'layer'.
            /// <para></para>
            /// This method also assigns it as the <see cref="AnimancerState.Serializable{TState}.State"/>.
            /// </summary>
            public override Vector2ControllerState CreateState(AnimancerLayer layer)
            {
                return State = new Vector2ControllerState(layer, Controller, _ParameterNameX, _ParameterNameY);
            }

            /************************************************************************************************************************/
            #region Drawer
#if UNITY_EDITOR
            /************************************************************************************************************************/

            /// <summary>[Editor-Only] Draws the inspector GUI for a <see cref="Serializable"/>.</summary>
            [UnityEditor.CustomPropertyDrawer(typeof(Serializable), true)]
            public class Drawer : ControllerState.Serializable.Drawer
            {
                /************************************************************************************************************************/

                /// <summary>Constructs a new <see cref="Drawer"/> and sets the <see cref="Parameters"/>.</summary>
                public Drawer() : base("_ParameterNameX", "_ParameterNameY") { }

                /************************************************************************************************************************/
            }

            /************************************************************************************************************************/
#endif
            #endregion
            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
