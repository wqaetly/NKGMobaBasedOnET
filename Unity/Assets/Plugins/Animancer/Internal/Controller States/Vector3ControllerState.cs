// Animancer // Copyright 2019 Kybernetik //

using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// An <see cref="ControllerState"/> which manages three float parameters.
    /// </summary>
    public sealed class Vector3ControllerState : ControllerState
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

        private Parameter _ParameterZ;

        /// <summary>
        /// The name of the parameter which <see cref="ParameterZ"/> will get and set.
        /// This will be null if the <see cref="ParameterHashZ"/> was assigned directly.
        /// </summary>
        public string ParameterNameZ
        {
            get { return _ParameterZ.Name; }
            set
            {
                _ParameterZ.Name = value;
                _ParameterZ.Validate(Controller, AnimatorControllerParameterType.Float);
            }
        }

        /// <summary>
        /// The name hash of the parameter which <see cref="ParameterZ"/> will get and set.
        /// </summary>
        public int ParameterHashZ
        {
            get { return _ParameterZ.Hash; }
            set
            {
                _ParameterZ.Hash = value;
                _ParameterZ.Validate(Controller, AnimatorControllerParameterType.Float);
            }
        }

        /// <summary>
        /// Gets and sets a float parameter in the <see cref="ControllerState.Controller"/> using the
        /// <see cref="ParameterHashZ"/> as the id.
        /// </summary>
        public float ParameterZ
        {
            get { return Playable.GetFloat(_ParameterZ); }
            set { Playable.SetFloat(_ParameterZ, value); }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Gets and sets <see cref="ParameterX"/>, <see cref="ParameterY"/>, and <see cref="ParameterZ"/>.
        /// </summary>
        public new Vector3 Parameter
        {
            get
            {
                return new Vector3(ParameterX, ParameterY, ParameterZ);
            }
            set
            {
                ParameterX = value.x;
                ParameterY = value.y;
                ParameterZ = value.z;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="Vector3ControllerState"/> to play the 'controller' without connecting
        /// it to the <see cref="PlayableGraph"/>. You must call <see cref="AnimancerState.SetParent(AnimancerLayer)"/>
        /// or it won't actually do anything.
        /// </summary>
        private Vector3ControllerState(AnimancerPlayable root, RuntimeAnimatorController controller,
            Parameter parameterX, Parameter parameterY, Parameter parameterZ)
            : base(root, controller)
        {
            _ParameterX = parameterX;
            _ParameterX.Validate(Controller, AnimatorControllerParameterType.Float);

            _ParameterY = parameterY;
            _ParameterY.Validate(Controller, AnimatorControllerParameterType.Float);

            _ParameterZ = parameterZ;
            _ParameterZ.Validate(Controller, AnimatorControllerParameterType.Float);
        }

        /// <summary>
        /// Constructs a new <see cref="Vector3ControllerState"/> to play the 'controller' and
        /// connects it to the 'layer's <see cref="IAnimationMixer.Playable"/> using a spare port if there are any from
        /// previously destroyed states, or by adding a new port.
        /// </summary>
        public Vector3ControllerState(AnimancerLayer layer, RuntimeAnimatorController controller,
            Parameter parameterX, Parameter parameterY, Parameter parameterZ)
            : this(layer.Root, controller, parameterX, parameterY, parameterZ)
        {
            layer.AddChild(this);
        }

        /// <summary>
        /// Constructs a new <see cref="Vector3ControllerState"/> to play the 'controller' and
        /// connects it to the 'parent's <see cref="IAnimationMixer.Playable"/> at the specified 'portIndex'.
        /// </summary>
        public Vector3ControllerState(AnimancerNode parent, int portIndex, RuntimeAnimatorController controller,
            Parameter parameterX, Parameter parameterY, Parameter parameterZ)
            : this(parent.Root, controller, parameterX, parameterY, parameterZ)
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
                case 2: return ParameterHashZ;
                default: throw new ArgumentOutOfRangeException("index");
            };
        }

        /************************************************************************************************************************/
        #region Serializable
        /************************************************************************************************************************/

        /// <summary>
        /// A serializable object which can create a <see cref="Vector3ControllerState"/> when passed into
        /// <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        [Serializable]
        public new class Serializable : Serializable<Vector3ControllerState>
        {
            /************************************************************************************************************************/

            [SerializeField]
            private string _ParameterNameX;

            /// <summary>
            /// The <see cref="Vector3ControllerState.ParameterNameX"/> that will be used for the created state.
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
            /// The <see cref="Vector3ControllerState.ParameterNameY"/> that will be used for the created state.
            /// </summary>
            public string ParameterNameY
            {
                get { return _ParameterNameY; }
                set { _ParameterNameY = value; }
            }

            /************************************************************************************************************************/

            [SerializeField]
            private string _ParameterNameZ;

            /// <summary>
            /// The <see cref="Vector3ControllerState.ParameterNameZ"/> that will be used for the created state.
            /// </summary>
            public string ParameterNameZ
            {
                get { return _ParameterNameZ; }
                set { _ParameterNameZ = value; }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Creates and returns a new <see cref="Vector3ControllerState"/> connected to the 'layer'.
            /// <para></para>
            /// This method also assigns it as the <see cref="AnimancerState.Serializable{TState}.State"/>.
            /// </summary>
            public override Vector3ControllerState CreateState(AnimancerLayer layer)
            {
                return State = new Vector3ControllerState(layer, Controller, _ParameterNameX, _ParameterNameY, _ParameterNameZ);
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
                public Drawer() : base("_ParameterNameX", "_ParameterNameY", "_ParameterNameZ") { }

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
