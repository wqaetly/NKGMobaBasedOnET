// Animancer // Copyright 2019 Kybernetik //

using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// An <see cref="ControllerState"/> which manages a float parameter.
    /// </summary>
    public sealed class FloatControllerState : ControllerState
    {
        /************************************************************************************************************************/

        private Parameter _Parameter;

        /// <summary>
        /// The name of the parameter which <see cref="Parameter"/> will get and set.
        /// This will be null if the <see cref="ParameterHash"/> was assigned directly.
        /// </summary>
        public string ParameterName
        {
            get { return _Parameter.Name; }
            set
            {
                _Parameter.Name = value;
                _Parameter.Validate(Controller, AnimatorControllerParameterType.Float);
            }
        }

        /// <summary>
        /// The name hash of the parameter which <see cref="Parameter"/> will get and set.
        /// </summary>
        public int ParameterHash
        {
            get { return _Parameter.Hash; }
            set
            {
                _Parameter.Hash = value;
                _Parameter.Validate(Controller, AnimatorControllerParameterType.Float);
            }
        }

        /// <summary>
        /// Gets and sets a float parameter in the <see cref="ControllerState.Controller"/> using the
        /// <see cref="ParameterHash"/> as the id.
        /// </summary>
        public new float Parameter
        {
            get { return Playable.GetFloat(_Parameter); }
            set { Playable.SetFloat(_Parameter, value); }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="FloatControllerState"/> to play the 'controller' without connecting
        /// it to the <see cref="PlayableGraph"/>. You must call <see cref="AnimancerState.SetParent(AnimancerLayer)"/>
        /// or it won't actually do anything.
        /// </summary>
        private FloatControllerState(AnimancerPlayable root, RuntimeAnimatorController controller, Parameter parameter)
            : base(root, controller)
        {
            _Parameter = parameter;
            _Parameter.Validate(controller, AnimatorControllerParameterType.Float);
        }

        /// <summary>
        /// Constructs a new <see cref="FloatControllerState"/> to play the 'controller' and connects
        /// it to the 'layer's <see cref="IAnimationMixer.Playable"/> using a spare port if there are any from
        /// previously destroyed states, or by adding a new port.
        /// </summary>
        public FloatControllerState(AnimancerLayer layer, RuntimeAnimatorController controller, Parameter parameter)
            : this(layer.Root, controller, parameter)
        {
            layer.AddChild(this);
        }

        /// <summary>
        /// Constructs a new <see cref="FloatControllerState"/> to play the 'controller' and connects
        /// it to the 'parent's <see cref="IAnimationMixer.Playable"/> at the specified 'portIndex'.
        /// </summary>
        public FloatControllerState(AnimancerNode parent, int portIndex, RuntimeAnimatorController controller, Parameter parameter)
            : this(parent.Root, controller, parameter)
        {
            SetParent(parent, portIndex);
        }

        /************************************************************************************************************************/

        /// <summary>The number of parameters being wrapped by this state.</summary>
        public override int ParameterCount { get { return 1; } }

        /// <summary>Returns the hash of a parameter being wrapped by this state.</summary>
        public override int GetParameterHash(int index) { return ParameterHash; }

        /************************************************************************************************************************/
        #region Serializable
        /************************************************************************************************************************/

        /// <summary>
        /// A serializable object which can create a <see cref="FloatControllerState"/> when passed into
        /// <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        [Serializable]
        public new class Serializable : Serializable<FloatControllerState>
        {
            /************************************************************************************************************************/

            [SerializeField]
            private string _ParameterName;

            /// <summary>
            /// The <see cref="FloatControllerState.ParameterName"/> that will be used for the created state.
            /// </summary>
            public string ParameterName
            {
                get { return _ParameterName; }
                set { _ParameterName = value; }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Creates and returns a new <see cref="FloatControllerState"/> connected to the 'layer'.
            /// <para></para>
            /// This method also assigns it as the <see cref="AnimancerState.Serializable{TState}.State"/>.
            /// </summary>
            public override FloatControllerState CreateState(AnimancerLayer layer)
            {
                return State = new FloatControllerState(layer, Controller, _ParameterName);
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
                public Drawer() : base("_ParameterName") { }

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
