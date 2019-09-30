// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
#pragma warning disable IDE0018 // Inline variable declaration.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// An <see cref="AnimancerState"/> which plays a <see cref="RuntimeAnimatorController"/>.
    /// You can control this state very similarly to an <see cref="Animator"/> via its <see cref="Playable"/> field.
    /// </summary>
    public class ControllerState : AnimancerState
    {
        /************************************************************************************************************************/
        #region Fields and Auto-Properties
        /************************************************************************************************************************/

        private RuntimeAnimatorController _Controller;

        /// <summary>The <see cref="RuntimeAnimatorController"/> which this state plays.</summary>
        public RuntimeAnimatorController Controller
        {
            get { return _Controller; }
            set
            {
                if (ReferenceEquals(_Controller, value))
                    return;

                if (ReferenceEquals(_Key, value))
                    Key = value;

                if (_Playable.IsValid())
                    Root._Graph.DestroyPlayable(_Playable);

                CreatePlayable(value);
            }
        }

        /// <summary>The <see cref="RuntimeAnimatorController"/> which this state plays.</summary>
        public override Object MainObject
        {
            get { return Controller; }
            set { Controller = (RuntimeAnimatorController)value; }
        }

        /// <summary>The internal system which plays the <see cref="RuntimeAnimatorController"/>.</summary>
        public AnimatorControllerPlayable Playable { get; private set; }

        /************************************************************************************************************************/

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member.
        /// <summary>
        /// <see cref="ControllerState"/>s don't track the progress of a single animation so this event isn't
        /// used and will be ignored.
        /// </summary>
        [Obsolete("ControllerStates don't track the progress of a single animation so this event isn't used and will be ignored.")]
        public override Action OnEnd { get { return null; } set { } }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member.

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Public API
        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="ControllerState"/> to play the 'animatorController' without connecting
        /// it to the <see cref="PlayableGraph"/>. You must call <see cref="AnimancerState.SetParent"/> or it won't
        /// actually do anything.
        /// </summary>
        protected ControllerState(AnimancerPlayable root, RuntimeAnimatorController animatorController)
            : base(root)
        {

            CreatePlayable(animatorController);
        }

        /// <summary>
        /// Constructs a new <see cref="ControllerState"/> to play the 'animatorController' and connects it to
        /// the 'layer's <see cref="IAnimationMixer.Playable"/> using a spare port if there are any from previously
        /// destroyed states, or by adding a new port.
        /// </summary>
        public ControllerState(AnimancerLayer layer, RuntimeAnimatorController animatorController)
            : this(layer.Root, animatorController)
        {
            layer.AddChild(this);
        }

        /// <summary>
        /// Constructs a new <see cref="ControllerState"/> to play the 'animatorController' and connects it to
        /// the 'parent's <see cref="IAnimationMixer.Playable"/> at the specified 'portIndex'.
        /// </summary>
        public ControllerState(AnimancerNode parent, int portIndex, RuntimeAnimatorController animatorController)
            : this(parent.Root, animatorController)
        {
            SetParent(parent, portIndex);
        }

        /************************************************************************************************************************/

        private void CreatePlayable(RuntimeAnimatorController animatorController)
        {
            if (animatorController == null)
                throw new ArgumentNullException("animatorController");

            _Controller = animatorController;
            Playable = AnimatorControllerPlayable.Create(Root._Graph, animatorController);
            _Playable = Playable;
        }

        /************************************************************************************************************************/

        /// <summary>Checks if any clip in the <see cref="Controller"/> has an animation event with the specified 'functionName'.</summary>
        public override bool HasEvent(string functionName)
        {
            var clips = _Controller.animationClips;
            var count = clips.Length;
            for (int i = 0; i < count; i++)
            {
                if (ClipState.HasEvent(clips[i], functionName))
                    return true;
            }
            return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// The current <see cref="AnimatorStateInfo.length"/>.
        /// </summary>
        public override float Length { get { return Playable.GetCurrentAnimatorStateInfo(0).length; } }

        /// <summary>
        /// Indicates whether this state will return a valid <see cref="Length"/> value.
        /// </summary>
        public override bool HasLength { get { return Playable.IsValid(); } }

        /// <summary>
        /// Indicates whether the current state will loop back to the start when it reaches the end.
        /// </summary>
        public override bool IsLooping { get { return Playable.GetCurrentAnimatorStateInfo(0).loop; } }

        /************************************************************************************************************************/

        /// <summary>[Unsupported]
        /// Returns <see cref="Vector3.zero"/> since <see cref="AnimatorControllerPlayable"/> doesn't expose the
        /// average velocity.
        /// </summary>
        public override Vector3 AverageVelocity
        {
            get { return Vector3.zero; }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns a string describing the type of this state and the name of the <see cref="Controller"/>.
        /// </summary>
        public override string ToString()
        {
            if (_Controller != null)
                return string.Concat(base.ToString(), " (", _Controller.name, ")");
            else
                return base.ToString() + " (null)";
        }

        /************************************************************************************************************************/

        /// <summary>[<see cref="IAnimationClipSource"/>]
        /// Gathers all the animations associated with this state.
        /// </summary>
        public override void GetAnimationClips(List<AnimationClip> clips)
        {
            clips.AddRange(_Controller.animationClips);
        }

        /************************************************************************************************************************/

        /// <summary>Destroys the <see cref="Playable"/>.</summary>
        public override void Dispose()
        {
            _Controller = null;
            base.Dispose();
        }

        /************************************************************************************************************************/
        #region Inverse Kinematics
        /************************************************************************************************************************/

        /// <summary>
        /// Does nothing and returns false since <see cref="AnimatorControllerPlayable"/> doesn't expose this function.
        /// </summary>
        public override bool ApplyFootIK
        {
            get { return false; }
            set { }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Does nothing and returns false since <see cref="AnimatorControllerPlayable"/> doesn't expose this function.
        /// </summary>
        public override bool ApplyAnimatorIK
        {
            get { return false; }
            set { }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Parameters
        /************************************************************************************************************************/

        /// <summary>
        /// A wrapper for the name and hash of an <see cref="AnimatorControllerParameter"/> to allow easy access.
        /// </summary>
        public struct Parameter
        {
            /************************************************************************************************************************/

            private string _Name;
            private int _Hash;

            /************************************************************************************************************************/

            /// <summary>
            /// The name of the wrapped parameter. This will be null if the <see cref="Hash"/> was assigned directly.
            /// </summary>
            public string Name
            {
                get { return _Name; }
                set
                {
                    _Name = value;
                    _Hash = Animator.StringToHash(value);
                }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// The name hash of the wrapped parameter.
            /// </summary>
            public int Hash
            {
                get { return _Hash; }
                set
                {
                    _Name = null;
                    _Hash = value;
                }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Constructs a new <see cref="Parameter"/> with the specified <see cref="Name"/> and uses
            /// <see cref="Animator.StringToHash"/> to calculate the <see cref="Hash"/>.
            /// </summary>
            public Parameter(string name)
            {
                _Name = name;
                _Hash = Animator.StringToHash(name);
            }

            /// <summary>
            /// Constructs a new <see cref="Parameter"/> with the specified <see cref="Hash"/> and leaves the
            /// <see cref="Name"/> null.
            /// </summary>
            public Parameter(int hash)
            {
                _Name = null;
                _Hash = hash;
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Constructs a new <see cref="Parameter"/> with the specified <see cref="Name"/> and uses
            /// <see cref="Animator.StringToHash"/> to calculate the <see cref="Hash"/>.
            /// </summary>
            public static implicit operator Parameter(string name)
            {
                return new Parameter(name);
            }

            /// <summary>
            /// Constructs a new <see cref="Parameter"/> with the specified <see cref="Hash"/> and leaves the
            /// <see cref="Name"/> null.
            /// </summary>
            public static implicit operator Parameter(int hash)
            {
                return new Parameter(hash);
            }

            /************************************************************************************************************************/

            /// <summary>Returns the <see cref="Hash"/>.</summary>
            public static implicit operator int(Parameter parameter)
            {
                return parameter._Hash;
            }

            /************************************************************************************************************************/

#if UNITY_EDITOR
            private static Dictionary<RuntimeAnimatorController, Dictionary<int, AnimatorControllerParameterType>>
                _ControllerToParameterHashAndType;
#endif

            /// <summary>[Editor-Conditional]
            /// Throws an <see cref="ArgumentException"/> if the <see cref="Controller"/> doesn't have a parameter
            /// with the specified 'nameHash' and 'type'.
            /// </summary>
            /// <exception cref="ArgumentException"/>
            [System.Diagnostics.Conditional("UNITY_EDITOR")]
            public void Validate(RuntimeAnimatorController controller, AnimatorControllerParameterType type)
            {
#if UNITY_EDITOR
                if (_ControllerToParameterHashAndType == null)
                    _ControllerToParameterHashAndType = new Dictionary<RuntimeAnimatorController, Dictionary<int, AnimatorControllerParameterType>>();

                // Get the parameter details.
                Dictionary<int, AnimatorControllerParameterType> parameterDetails;
                if (!_ControllerToParameterHashAndType.TryGetValue(controller, out parameterDetails))
                {
                    parameterDetails = new Dictionary<int, AnimatorControllerParameterType>();

                    var animatorController = controller as AnimatorController;
                    var parameters = animatorController.parameters;
                    var count = parameters.Length;
                    for (int i = 0; i < count; i++)
                    {
                        var parameter = parameters[i];
                        parameterDetails.Add(parameter.nameHash, parameter.type);
                    }

                    _ControllerToParameterHashAndType.Add(controller, parameterDetails);
                }

                // Check that there is a parameter with the correct hash and type.

                AnimatorControllerParameterType parameterType;
                if (!parameterDetails.TryGetValue(_Hash, out parameterType))
                {
                    throw new ArgumentException(controller + " has no " + type + " parameter matching " + this);
                }

                if (type != parameterType)
                {
                    throw new ArgumentException(controller + " has a parameter matching " + this + ", but it is not a " + type);
                }
#endif
            }

            /************************************************************************************************************************/

            /// <summary>Returns a string containing the <see cref="Name"/> and <see cref="Hash"/>.</summary>
            public override string ToString()
            {
                return string.Concat(
                    "ControllerState.Parameter(Name:'",
                    _Name,
                    "', Hash:",
                    _Hash.ToString(),
                    ")");
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Inspector
        /************************************************************************************************************************/

        /// <summary>The number of parameters being wrapped by this state.</summary>
        public virtual int ParameterCount { get { return 0; } }

        /// <summary>Returns the hash of a parameter being wrapped by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't wrap any parameters.</exception>
        public virtual int GetParameterHash(int index) { throw new NotSupportedException(); }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Returns an <see cref="Drawer"/> for this state.</summary>
        protected internal override Editor.IAnimancerStateDrawer GetDrawer()
        {
            return new Drawer(this);
        }

        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Draws the inspector GUI for an <see cref="ControllerState"/>.</summary>
        public sealed class Drawer : Editor.ParametizedAnimancerStateDrawer<ControllerState>
        {
            /************************************************************************************************************************/

            /// <summary>
            /// Constructs a new <see cref="Drawer"/> to manage the inspector GUI for the 'state'.
            /// </summary>
            public Drawer(ControllerState state) : base(state) { }

            /************************************************************************************************************************/

            /// <summary> Draws the details of the target state in the GUI.</summary>
            protected override void DoDetailsGUI(IAnimancerComponent owner)
            {
                GatherParameters(owner.Animator);
                base.DoDetailsGUI(owner);
            }

            /************************************************************************************************************************/

            private Animator _CurrentAnimator;

            private readonly List<AnimatorControllerParameter>
                Parameters = new List<AnimatorControllerParameter>();

            private void GatherParameters(Animator animator)
            {
                _CurrentAnimator = animator;
                Parameters.Clear();

                if (animator == null)
                    return;

                var count = State.ParameterCount;
                if (count == 0)
                    return;

                for (int i = 0; i < count; i++)
                {
                    var hash = State.GetParameterHash(i);
                    Parameters.Add(GetParameter(animator, hash));
                }
            }

            /************************************************************************************************************************/

            private AnimatorControllerParameter GetParameter(Animator animator, int hash)
            {
                var parameters = animator.parameters;
                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    if (parameter.nameHash == hash)
                        return parameter;
                }

                return null;
            }

            /************************************************************************************************************************/

            /// <summary>The number of parameters being managed by the target state.</summary>
            public override int ParameterCount { get { return Parameters.Count; } }

            /// <summary>Returns the name of a parameter being managed by the target state.</summary>
            /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
            public override string GetParameterName(int index) { return Parameters[index].name; }

            /// <summary>Returns the type of a parameter being managed by the target state.</summary>
            /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
            public override AnimatorControllerParameterType GetParameterType(int index) { return Parameters[index].type; }

            /// <summary>Returns the value of a parameter being managed by the target state.</summary>
            /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
            public override object GetParameterValue(int index)
            {
                return Editor.AnimancerEditorUtilities.GetParameterValue(_CurrentAnimator, Parameters[index]);
            }

            /// <summary>Sets the value of a parameter being managed by the target state.</summary>
            /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
            public override void SetParameterValue(int index, object value)
            {
                Editor.AnimancerEditorUtilities.SetParameterValue(_CurrentAnimator, Parameters[index], value);
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Serializable
        /************************************************************************************************************************/

        /// <summary>
        /// Base class for serializable objects which can create a particular type of <see cref="ControllerState"/>
        /// when passed into <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        /// <remarks>
        /// Even though it has the <see cref="SerializableAttribute"/>, this class won't actually get serialized
        /// by Unity because it's generic and abstract. Each child class still needs to include the attribute.
        /// </remarks>
        [Serializable]
        public abstract new class Serializable<TState> : AnimancerState.Serializable<TState>
            where TState : ControllerState
        {
            /************************************************************************************************************************/

            [SerializeField]
            private RuntimeAnimatorController _Controller;

            /// <summary>
            /// The <see cref="ControllerState.Controller"/> that will be used for the created state.
            /// </summary>
            public RuntimeAnimatorController Controller
            {
                get { return _Controller; }
                set { _Controller = value; }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// The <see cref="Controller"/> will be used as the <see cref="AnimancerState.Key"/> for the created state to be
            /// registered with.
            /// </summary>
            public override object Key { get { return _Controller; } }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/

        /// <summary>
        /// A serializable object which can create a <see cref="ControllerState"/> when passed into
        /// <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        [Serializable]
        public class Serializable : Serializable<ControllerState>
        {
            /************************************************************************************************************************/

            /// <summary>
            /// Creates and returns a new <see cref="ControllerState"/> connected to the 'layer'.
            /// <para></para>
            /// This method also assigns it as the <see cref="AnimancerState.Serializable{TState}.State"/>.
            /// </summary>
            public override ControllerState CreateState(AnimancerLayer layer)
            {
                return State = new ControllerState(layer, Controller);
            }

            /************************************************************************************************************************/
            #region Drawer
#if UNITY_EDITOR
            /************************************************************************************************************************/

            /// <summary>
            /// [Editor-Only] Draws the inspector GUI for a <see cref="Serializable{TState}"/> or
            /// <see cref="Serializable"/>.
            /// </summary>
            [CustomPropertyDrawer(typeof(Serializable<>), true)]
            [CustomPropertyDrawer(typeof(Serializable), true)]
            public class Drawer : Editor.AnimancerStateSerializableDrawer
            {
                /************************************************************************************************************************/

                private readonly string[] Parameters;
                private readonly string[] ParameterPrefixes;

                /************************************************************************************************************************/

                /// <summary>Constructs a new <see cref="Drawer"/> without any parameters.</summary>
                public Drawer() : this(null) { }

                /// <summary>Constructs a new <see cref="Drawer"/> and sets the <see cref="Parameters"/>.</summary>
                public Drawer(params string[] parameters) : base("_Controller")
                {
                    Parameters = parameters;
                    if (parameters == null)
                        return;

                    ParameterPrefixes = new string[parameters.Length];

                    for (int i = 0; i < ParameterPrefixes.Length; i++)
                    {
                        ParameterPrefixes[i] = "." + parameters[i];
                    }
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Draws the 'property' GUI in relation to the 'rootProperty' which was passed into
                /// <see cref="Editor.AnimancerStateSerializableDrawer.OnGUI"/>.
                /// </summary>
                protected override void DoPropertyGUI(ref Rect area, SerializedProperty rootProperty, SerializedProperty property, GUIContent label)
                {
                    if (ParameterPrefixes != null)
                    {
                        var controllerProperty = rootProperty.FindPropertyRelative(MainPropertyName);
                        var controller = controllerProperty.objectReferenceValue as AnimatorController;
                        if (controller != null)
                        {
                            var path = property.propertyPath;

                            for (int i = 0; i < ParameterPrefixes.Length; i++)
                            {
                                if (path.EndsWith(ParameterPrefixes[i]))
                                {
                                    area.height = EditorGUIUtility.singleLineHeight;
                                    DoParameterGUI(area, controller, property);
                                    return;
                                }
                            }
                        }
                    }

                    EditorGUI.BeginChangeCheck();

                    base.DoPropertyGUI(ref area, rootProperty, property, label);

                    // When the controller changes, validate all parameters.
                    if (EditorGUI.EndChangeCheck() &&
                        Parameters != null &&
                        property.propertyPath.EndsWith(MainPropertyPathSuffix))
                    {
                        var controller = property.objectReferenceValue as AnimatorController;
                        if (controller != null)
                        {
                            for (int i = 0; i < Parameters.Length; i++)
                            {
                                property = rootProperty.FindPropertyRelative(Parameters[i]);
                                var parameterName = property.stringValue;
                                if (!HasFloatParameter(controller, parameterName))
                                {
                                    parameterName = GetFirstFloatParameterName(controller);
                                    if (!string.IsNullOrEmpty(parameterName))
                                        property.stringValue = parameterName;
                                }
                            }
                        }
                    }
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Draws a dropdown menu to select the name of a parameter in the 'controller'.
                /// </summary>
                protected void DoParameterGUI(Rect area, AnimatorController controller, SerializedProperty property)
                {
                    var parameterName = property.stringValue;
                    var parameters = controller.parameters;

                    var label = Editor.AnimancerEditorUtilities.TempContent(property.displayName, property.tooltip);
                    label = EditorGUI.BeginProperty(area, label, property);

                    var xMax = area.xMax;
                    area.width = EditorGUIUtility.labelWidth;
                    EditorGUI.PrefixLabel(area, label);

                    area.x += area.width;
                    area.xMax = xMax;

                    var color = GUI.color;
                    if (!HasFloatParameter(controller, parameterName))
                        GUI.color = Editor.AnimancerEditorUtilities.ErrorFieldColor;

                    var content = Editor.AnimancerEditorUtilities.TempContent(parameterName);
                    if (EditorGUI.DropdownButton(area, content, FocusType.Passive))
                    {
                        property = property.Copy();

                        var menu = new GenericMenu();

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var parameter = parameters[i];
                            Editor.AnimancerEditorUtilities.AddMenuItem(menu, parameter.name,
                                parameter.type == AnimatorControllerParameterType.Float, () =>
                                {
                                    Editor.AnimancerEditorUtilities.ForEachTarget(property, (targetProperty) =>
                                    {
                                        targetProperty.stringValue = parameter.name;
                                    });
                                });
                        }

                        if (menu.GetItemCount() == 0)
                            menu.AddDisabledItem(new GUIContent("No Parameters"));

                        menu.ShowAsContext();
                    }

                    GUI.color = color;

                    EditorGUI.EndProperty();
                }

                /************************************************************************************************************************/

                private static bool HasFloatParameter(AnimatorController controller, string name)
                {
                    if (string.IsNullOrEmpty(name))
                        return false;

                    var parameters = controller.parameters;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters[i];
                        if (parameter.type == AnimatorControllerParameterType.Float && name == parameters[i].name)
                        {
                            return true;
                        }
                    }

                    return false;
                }

                /************************************************************************************************************************/

                private static string GetFirstFloatParameterName(AnimatorController controller)
                {
                    var parameters = controller.parameters;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters[i];
                        if (parameter.type == AnimatorControllerParameterType.Float)
                        {
                            return parameter.name;
                        }
                    }

                    return "";
                }

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
