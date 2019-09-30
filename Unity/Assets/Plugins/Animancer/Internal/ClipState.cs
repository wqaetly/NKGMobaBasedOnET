// Animancer // Copyright 2019 Kybernetik //

using System;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace Animancer
{
    /// <summary>
    /// An <see cref="AnimancerState"/> which plays an <see cref="UnityEngine.AnimationClip"/>.
    /// </summary>
    public sealed class ClipState : AnimancerState, IEarlyUpdate
    {
        /************************************************************************************************************************/
        #region Fields and Properties
        /************************************************************************************************************************/
        #region Clip
        /************************************************************************************************************************/

        /// <summary>The <see cref="AnimationClip"/> which this state plays.</summary>
        private AnimationClip _Clip;

        /// <summary>The <see cref="AnimationClip"/> which this state plays.</summary>
        public override AnimationClip Clip
        {
            get { return _Clip; }
            set
            {
                if (ReferenceEquals(_Clip, value))
                    return;

                if (ReferenceEquals(_Key, _Clip))
                    Key = value;

                if (_Playable.IsValid())
                    Root._Graph.DestroyPlayable(_Playable);

                CreatePlayable(value);
            }
        }

        /// <summary>The <see cref="AnimationClip"/> which this state plays.</summary>
        public override Object MainObject
        {
            get { return _Clip; }
            set { Clip = (AnimationClip)value; }
        }

        /************************************************************************************************************************/

        /// <summary>The average velocity of the root motion caused by this state.</summary>
        public override Vector3 AverageVelocity
        {
            get { return _Clip.averageSpeed; }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Timing
        /************************************************************************************************************************/
        // Time.
        /************************************************************************************************************************/

        /// <summary>
        /// The current time of the <see cref="Playable"/>, retrieved by <see cref="Time"/> whenever
        /// <see cref="_IsTimeDirty"/> is false.</summary>
        private float _Time;

        /// <summary>
        /// The <see cref="AnimancerPlayable.FrameID"/> from when the <see cref="Time"/> was last retrieved from the
        /// <see cref="Playable"/>.
        /// </summary>
        private uint _TimeFrameID;

        /// <summary>
        /// The estimated number of times the animation would have looped this frame which was calculated last frame.
        /// Only used for checking <see cref="OnEnd"/> events on looping animations.
        /// </summary>
        private float _EstimatedLoopCount = int.MinValue;

        /************************************************************************************************************************/

        /// <summary>
        /// The number of seconds that have passed since the start of this animation.
        /// <para></para>
        /// This value will continue increasing after the animation passes the end of its <see cref="Length"/> and it
        /// will either freeze in place or start again from the beginning according to whether it is looping or not.
        /// <para></para>
        /// Animancer Lite does not allow this value to be changed in a runtime build (except setting it to 0).
        /// </summary>
        /// <remarks>
        /// See <see cref="AnimancerState.Time"/> for more details.
        /// </remarks>
        public override float Time
        {
            get
            {
                var frameID = Root.FrameID;
                if (_TimeFrameID != frameID)
                {
                    _Time = (float)_Playable.GetTime();
                    _TimeFrameID = frameID;
                }

                return _Time;
            }
            set
            {
                var frameID = Root.FrameID;
                if (_TimeFrameID == frameID &&
                    _Time == value)
                    return;

                _Time = value;
                _TimeFrameID = frameID;
                _EstimatedLoopCount = int.MinValue;

                base.Time = value;
            }
        }

        /************************************************************************************************************************/
        // Length.
        /************************************************************************************************************************/

        /// <summary>
        /// The <see cref="AnimationClip.length"/>.
        /// </summary>
        public override float Length { get { return _Clip.length; } }

        /// <summary>
        /// Indicates whether this state will return a valid <see cref="Length"/> value.
        /// </summary>
        public override bool HasLength { get { return _Clip != null; } }

        /// <summary>
        /// Indicates whether this state will loop back to the start when it reaches the end.
        /// </summary>
        public override bool IsLooping { get { return _Clip.isLooping; } }

        /************************************************************************************************************************/

        private Action _OnEnd;

        /// <summary>
        /// A callback which is triggered when the animation reaches its end (not when the state is exited for
        /// whatever reason).
        /// <para></para>
        /// If the animation is looping, this callback will be triggered every time it passes the end. Otherwise it
        /// will be triggered every frame while it is past the end so if you want to ensure that your callback only
        /// occurs once, you will need to clear it as part of that callback.
        /// <para></para>
        /// This callback is automatically cleared by <see cref="Play"/>, <see cref="OnStartFade"/>, and
        /// <see cref="Stop"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="AnimancerState.OnEnd"/> for more details. Hopefully a future version of C# will implement the
        /// ability for documentation to be inherited so these comments don't need to be duplicated all the time.
        /// </remarks>
        public override Action OnEnd
        {
            get { return _OnEnd; }
            set
            {
                if (_OnEnd == null)
                    _EstimatedLoopCount = int.MinValue;

                _OnEnd = value;

                if (value != null)
                    RequireEarlyUpdate();
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Inverse Kinematics
        /************************************************************************************************************************/

        /// <summary>
        /// Determines whether <c>OnAnimatorIK(int layerIndex)</c> will be called on the animated object.
        /// The initial value is determined by <see cref="AnimancerLayer.DefaultApplyAnimatorIK"/>.
        /// <para></para>
        /// This is equivalent to the "IK Pass" toggle in Animator Controller layers.
        /// <para></para>
        /// It requires Unity 2018.1 or newer, however 2018.3 or newer is recommended because a bug in earlier versions
        /// of the Playables API caused this value to only take effect while a state was at
        /// <see cref="AnimancerNode.Weight"/> == 1 which meant that IK would not work while fading between animations.
        /// </summary>
        public override bool ApplyAnimatorIK
        {
#if UNITY_2018_1_OR_NEWER
            get { return ((AnimationClipPlayable)_Playable).GetApplyPlayableIK(); }
            set { ((AnimationClipPlayable)_Playable).SetApplyPlayableIK(value); }
#else
            get
            {
                throw new NotSupportedException(
                    "ApplyAnimatorIK is not supported by this version of Unity. Please upgrade to Unity 2018.1 or newer.");
            }
            set
            {
                throw new NotSupportedException(
                    "ApplyAnimatorIK is not supported by this version of Unity. Please upgrade to Unity 2018.1 or newer.");
            }
#endif
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Indicates whether this state is applying IK to the character's feet.
        /// The initial value is determined by <see cref="AnimancerLayer.DefaultApplyFootIK"/>.
        /// <para></para>
        /// This is equivalent to the "Foot IK" toggle in Animator Controller states.
        /// </summary>
        public override bool ApplyFootIK
        {
            get { return ((AnimationClipPlayable)_Playable).GetApplyFootIK(); }
            set { ((AnimationClipPlayable)_Playable).SetApplyFootIK(value); }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Applies the default IK flags from the specified 'layer'.
        /// </summary>
        private void InitialiseIKDefaults(AnimancerLayer layer)
        {
            // Foot IK is actually enabled by default so we disable it if necessary.
            if (!layer.DefaultApplyFootIK)
                ApplyFootIK = false;

#if UNITY_2018_1_OR_NEWER
            if (layer.DefaultApplyAnimatorIK)
                ApplyAnimatorIK = true;
#endif
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Methods
        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="ClipState"/> to play the 'clip' without connecting it to the
        /// <see cref="PlayableGraph"/>. You must call <see cref="AnimancerState.SetParent(AnimancerNode, int)"/> or it
        /// won't actually do anything.
        /// </summary>
        public ClipState(AnimancerPlayable root, AnimationClip clip)
            : base(root)
        {
            CreatePlayable(clip);
        }

        /// <summary>
        /// Constructs a new <see cref="ClipState"/> to play the 'clip' and connects it to a new port on the 'layer's
        /// <see cref="Playable"/>.
        /// </summary>
        public ClipState(AnimancerLayer layer, AnimationClip clip)
            : this(layer.Root, clip)
        {
            layer.AddChild(this);
            InitialiseIKDefaults(layer);
        }

        /// <summary>
        /// Constructs a new <see cref="ClipState"/> to play the 'clip' and connects it to the 'parent's
        /// <see cref="Playable"/> at the specified 'portIndex'.
        /// </summary>
        public ClipState(AnimancerNode parent, int portIndex, AnimationClip clip)
            : this(parent.Root, clip)
        {
            SetParent(parent, portIndex);
            InitialiseIKDefaults(parent.Layer);
        }

        /************************************************************************************************************************/

        private void CreatePlayable(AnimationClip clip)
        {
            if (clip == null)
                throw new ArgumentNullException("clip");

            ValidateClip(clip);

            _Clip = clip;
            _Playable = AnimationClipPlayable.Create(Root._Graph, clip);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the 'clip' is marked as <see cref="AnimationClip.legacy"/>.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static void ValidateClip(AnimationClip clip)
        {
            if (clip.legacy)
                throw new ArgumentException("Legacy clip '" + clip + "' cannot be used by Animancer." +
                    " Set the legacy property to false before using this clip." +
                    " If it was imported as part of a model then the model's Rig type must be changed to Humanoid or Generic." +
                    " Otherwise you can use the 'Toggle Legacy' function in the clip's context menu" +
                    " (via the cog icon in the top right of its Inspector).");
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Indicates whether this has been added to the list of nodes that need to be updated early.
        /// </summary>
        internal bool _IsEarlyUpdating;

        /// <summary>
        /// Adds this to the list of nodes that need to be updated early if it wasn't there already.
        /// </summary>
        private void RequireEarlyUpdate()
        {
            if (_IsEarlyUpdating)
                return;

            Root.RequireEarlyUpdate(this);
            _IsEarlyUpdating = true;
        }

        /************************************************************************************************************************/

        /// <summary>[Internal]
        /// Checks if the animation has reached its end in order to trigger the <see cref="OnEnd"/> event.
        /// </summary>
        void IEarlyUpdate.EarlyUpdate(out bool needsMoreUpdates)
        {
            if (IsPlaying && _OnEnd != null)
            {
                if (_Clip.isLooping)
                {
                    // Compare the number of times the clip has looped this frame and next frame.
                    // If the numbers are different, trigger the event.

                    var length = _Clip.length;
                    var time = Time;

                    var previousLoopCount = _EstimatedLoopCount != int.MinValue ?
                        _EstimatedLoopCount :
                        Mathf.FloorToInt(time / length);

                    _EstimatedLoopCount = Mathf.FloorToInt((time + Speed * AnimancerPlayable.DeltaTime) / length);

                    if (previousLoopCount != _EstimatedLoopCount)
                        _OnEnd();
                }
                else
                {
                    // If the time is passed the end of the clip, trigger the event.

                    // In earlier versions this checked _PreviousTime < length as well to only trigger the event once.

                    // But it's safer to continue doing it every frame while over the limit in case the user only
                    // registers their callback after the animation has ended but is relying on it getting called.

                    // If they want it to only get called once, they can unregister it during the call.

                    var speed = Speed;
                    var nextTime = Time + speed * AnimancerPlayable.DeltaTime;

                    if (speed >= 0)
                    {
                        if (nextTime >= _Clip.length)
                            _OnEnd();
                    }
                    else
                    {
                        if (nextTime <= 0)
                            _OnEnd();
                    }
                }
            }

            needsMoreUpdates = _IsEarlyUpdating = _OnEnd != null;
        }

        /************************************************************************************************************************/

        /// <summary>Checks if the <see cref="Clip"/> has an animation event with the specified 'functionName'.</summary>
        public override bool HasEvent(string functionName)
        {
            return HasEvent(_Clip, functionName);
        }

        /// <summary>Checks if the 'clip' has an animation event with the specified 'functionName'.</summary>
        public static bool HasEvent(AnimationClip clip, string functionName)
        {
            var events = clip.events;
            var count = events.Length;
            for (int i = 0; i < count; i++)
            {
                if (events[i].functionName == functionName)
                    return true;
            }

            return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns a string describing the type of this state and the name of the <see cref="Clip"/>.
        /// </summary>
        public override string ToString()
        {
            return string.Concat(base.ToString(), " (", _Clip.name, ")");
        }

        /************************************************************************************************************************/

        /// <summary>Appends a detailed descrption of the current details of this state.</summary>
        public override void AppendDescription(StringBuilder description, bool includeClip, bool includeChildStates = true, string delimiter = "\n")
        {
            base.AppendDescription(description, includeClip, includeChildStates, delimiter);
            AppendDelegate(description, _OnEnd, "OnClipEnd", delimiter);
        }

        /************************************************************************************************************************/

        /// <summary>Appends the <see cref="Delegate.Target"/> and <see cref="Delegate.Method"/> of 'del'.</summary>
        public static void AppendDelegate(StringBuilder description, Delegate del, string name, string delimiter = "\n")
        {
            if (del != null)
            {
                description.Append(delimiter).Append(name).Append(".Target=").Append(del.Target);
                description.Append(delimiter).Append(name).Append(".Method=").Append(del.Method);
            }
        }

        /************************************************************************************************************************/

        /// <summary>Destroys the <see cref="Playable"/>.</summary>
        public override void Dispose()
        {
            _Clip = null;
            base.Dispose();
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Inspector
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Returns an <see cref="Drawer"/> for this state.</summary>
        protected internal override Editor.IAnimancerStateDrawer GetDrawer()
        {
            return new Drawer(this);
        }

        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Draws the inspector GUI for a <see cref="ClipState"/>.</summary>
        public sealed class Drawer : Editor.AnimancerStateDrawer<ClipState>
        {
            /************************************************************************************************************************/

            /// <summary>Indicates whether the animation has an event called "End".</summary>
            private bool _HasEndEvent;

            /************************************************************************************************************************/

            /// <summary>
            /// Constructs a new <see cref="Drawer"/> to manage the inspector GUI for the 'state'.
            /// </summary>
            public Drawer(ClipState state) : base(state)
            {
                var events = state._Clip.events;
                for (int i = events.Length - 1; i >= 0; i--)
                {
                    if (events[i].functionName == "End")
                    {
                        _HasEndEvent = true;
                        break;
                    }
                }
            }

            /************************************************************************************************************************/

            /// <summary> Draws the details of the target state in the GUI.</summary>
            protected override void DoDetailsGUI(IAnimancerComponent owner)
            {
                base.DoDetailsGUI(owner);
                DoAnimationTypeWarningGUI(owner);
                DoEndEventWarningGUI();
            }

            /************************************************************************************************************************/

            private string _AnimationTypeWarning;
            private Animator _AnimationTypeWarningOwner;

            /// <summary>
            /// Validates the <see cref="Clip"/> type compared to the owner's <see cref="Animator"/> type.
            /// </summary>
            private void DoAnimationTypeWarningGUI(IAnimancerComponent owner)
            {
                // Validate the clip type compared to the owner.
                if (owner.Animator == null)
                {
                    _AnimationTypeWarning = null;
                    return;
                }

                if (_AnimationTypeWarningOwner != owner.Animator)
                {
                    _AnimationTypeWarning = null;
                    _AnimationTypeWarningOwner = owner.Animator;
                }

                if (_AnimationTypeWarning == null)
                {
                    var ownerAnimationType = Editor.AnimancerEditorUtilities.GetAnimationType(_AnimationTypeWarningOwner);
                    var clipAnimationType = Editor.AnimancerEditorUtilities.GetAnimationType(State._Clip);

                    if (ownerAnimationType == clipAnimationType)
                    {
                        _AnimationTypeWarning = "";
                    }
                    else
                    {
                        var text = new StringBuilder()
                            .Append("Possible animation type mismatch:\n - Animator type is ")
                            .Append(ownerAnimationType)
                            .Append("\n - AnimationClip type is ")
                            .Append(clipAnimationType)
                            .Append("\nThis means that the clip may not work correctly," +
                                " however this check is not totally accurate. Click here for more info.");

                        _AnimationTypeWarning = text.ToString();
                    }
                }

                if (_AnimationTypeWarning != "")
                {
                    UnityEditor.EditorGUILayout.HelpBox(_AnimationTypeWarning, UnityEditor.MessageType.Warning);

                    if (Editor.AnimancerEditorUtilities.TryUseClickInLastRect())
                        UnityEditor.EditorUtility.OpenWithDefaultApp(
                            AnimancerPlayable.APIDocumentationURL + "/docs/manual/inspector/#animation-types");
                }
            }

            /************************************************************************************************************************/

            private void DoEndEventWarningGUI()
            {
                if (_HasEndEvent && State._OnEnd == null)
                {
                    UnityEditor.EditorGUILayout.HelpBox("The animation has an event called 'End'" +
                        " but no 'OnEnd' delegate is currently registered for this state. Click here for more info.", UnityEditor.MessageType.Warning);

                    if (Editor.AnimancerEditorUtilities.TryUseClickInLastRect())
                        UnityEditor.EditorUtility.OpenWithDefaultApp(
                            AnimancerPlayable.APIDocumentationURL + "/docs/manual/animation-events/#end-events");
                }
            }

            /************************************************************************************************************************/

            /// <summary>Adds the details of this state to the menu.</summary>
            protected override void AddContextMenuFunctions(UnityEditor.GenericMenu menu)
            {
                menu.AddDisabledItem(new GUIContent(DetailsPrefix + "Animation Type: " +
                    Editor.AnimancerEditorUtilities.GetAnimationType(State._Clip)));

                base.AddContextMenuFunctions(menu);

                menu.AddItem(new GUIContent("Inverse Kinematics/Apply Animator IK"),
                    State.ApplyAnimatorIK,
                    () => State.ApplyAnimatorIK = !State.ApplyAnimatorIK);
                menu.AddItem(new GUIContent("Inverse Kinematics/Apply Foot IK"),
                    State.ApplyFootIK,
                    () => State.ApplyFootIK = !State.ApplyFootIK);
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
#endif
        #endregion
        /************************************************************************************************************************/
        #region Serializable
        /************************************************************************************************************************/

        /// <summary>
        /// A serializable object which can create a <see cref="ClipState"/> when passed into
        /// <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        [Serializable]
        public class Serializable : Serializable<ClipState>
        {
            /************************************************************************************************************************/

            [Tooltip("The animation to play")]
            [SerializeField]
            private AnimationClip _Clip;

            /// <summary>The animation to play.</summary>
            public AnimationClip Clip
            {
                get { return _Clip; }
                set
                {
                    if (value != null)
                        ValidateClip(value);

                    _Clip = value;
                }
            }

            /// <summary>
            /// The <see cref="Clip"/> will be used as the <see cref="AnimancerState.Key"/> for the created state to be
            /// registered with.
            /// </summary>
            public override object Key { get { return _Clip; } }

            /************************************************************************************************************************/

            [Tooltip("Determines how fast the animation plays (default = 1)")]
            [SerializeField]
            private float _Speed = 1;

            /// <summary>Determines how fast the animation plays (default = 1).</summary>
            public float Speed
            {
                get { return _Speed; }
                set { _Speed = value; }
            }

            /************************************************************************************************************************/

            [Tooltip("Determines what time to start the animation at if the toggle is enabled")]
            [SerializeField]
            private float _StartTime = float.NaN;

            /// <summary>
            /// Determines what time to start the animation at (either the raw <see cref="AnimancerState.Time"/> or
            /// the <see cref="AnimancerState.NormalizedTime"/> based on <see cref="StartTimeIsNormalized"/>).
            /// <para></para>
            /// The default value is <see cref="float.NaN"/> which indicates that this value isn't used so the
            /// animation will continue from its current time.
            /// </summary>
            public float StartTime
            {
                get { return _StartTime; }
                set { _StartTime = value; }
            }

            /// <summary>
            /// If this transition will set the <see cref="StartTime"/>, then it needs to use
            /// <see cref="AnimancerPlayable.CrossFadeFromStart"/>.
            /// </summary>
            public override bool CrossFadeFromStart { get { return !float.IsNaN(_StartTime); } }

            /************************************************************************************************************************/

            [Tooltip("Determines whether the to set the animation's Time or Normalized Time")]
            [SerializeField, HideInInspector]
            private bool _StartTimeIsNormalized = true;

            /// <summary>
            /// Determines whether the <see cref="StartTime"/> sets the <see cref="AnimancerState.Time"/> or
            /// <see cref="AnimancerState.NormalizedTime"/>.
            /// </summary>
            public bool StartTimeIsNormalized
            {
                get { return _StartTimeIsNormalized; }
                set { _StartTimeIsNormalized = value; }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// A callback which is triggered when the animation reaches its end (not when the state is exited for
            /// whatever reason).
            /// <para></para>
            /// If the animation is looping, this callback will be triggered every time it passes the end. Otherwise it
            /// will be triggered every frame while it is past the end so if you want to ensure that your callback only
            /// occurs once, you will need to clear it as part of that callback.
            /// <para></para>
            /// See <see cref="AnimancerState.OnEnd"/> for more details.
            /// </summary>
            /// <remarks>
            /// This property is not serialized, but <see cref="SerializableWithEndEvent"/> adds a
            /// <see cref="UnityEvent"/> that is serialized and can be edited in the Inspector.
            /// </remarks>
            public Action OnEnd { get; set; }

            /************************************************************************************************************************/

            /// <summary>
            /// Creates and returns a new <see cref="ClipState"/> connected to the 'layer'.
            /// <para></para>
            /// This method also assigns it as the <see cref="AnimancerState.Serializable{TState}.State"/>.
            /// </summary>
            public override ClipState CreateState(AnimancerLayer layer)
            {
                return State = new ClipState(layer, _Clip);
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Called by <see cref="AnimancerPlayable.Transition"/> to apply the <see cref="Speed"/> and
            /// <see cref="StartTime"/>.
            /// </summary>
            public override void Apply(AnimancerState state)
            {
                state.OnEnd = OnEnd;
                state.Speed = _Speed;

                if (!float.IsNaN(_StartTime))
                {
                    if (_StartTimeIsNormalized)
                        state.NormalizedTime = _StartTime;
                    else
                        state.Time = _StartTime;
                }
            }

            /************************************************************************************************************************/
#if UNITY_EDITOR
            /************************************************************************************************************************/

            /// <summary>[Editor-Only] Draws the inspector GUI for a <see cref="Serializable"/>.</summary>
            [UnityEditor.CustomPropertyDrawer(typeof(Serializable), true)]
            public class Drawer : Editor.AnimancerStateSerializableDrawer
            {
                /************************************************************************************************************************/

                /// <summary>Constructs a new <see cref="Drawer"/>.</summary>
                public Drawer() : base("_Clip") { }

                /************************************************************************************************************************/

                private static float _NormalizedWidth;

                /// <summary>
                /// Draws the 'property' GUI in relation to the 'rootProperty' which was passed into
                /// <see cref="Editor.AnimancerStateSerializableDrawer.OnGUI"/>.
                /// </summary>
                protected override void DoPropertyGUI(ref Rect area,
                    UnityEditor.SerializedProperty baseProperty, UnityEditor.SerializedProperty property, GUIContent label)
                {
                    if (property.propertyPath.EndsWith("._StartTime"))
                    {
                        const float ToggleIndent = 13;
                        var spacing = UnityEditor.EditorGUIUtility.standardVerticalSpacing;

                        area.height = UnityEditor.EditorGUIUtility.singleLineHeight;

                        var timeArea = area;
                        var toggleArea = new Rect(
                            UnityEditor.EditorGUIUtility.labelWidth + ToggleIndent,
                            timeArea.y,
                            area.height,
                            timeArea.height);

                        label.text = "Normalized";
                        if (_NormalizedWidth == 0)
                            _NormalizedWidth = Editor.AnimancerEditorUtilities.CalculateWidth(GUI.skin.toggle, label);

                        var normalizedArea = Editor.AnimancerEditorUtilities.StealWidth(ref timeArea, _NormalizedWidth);
                        normalizedArea.x += spacing;

                        var enabled = GUI.enabled;

                        // Set Start Time Toggle.

                        var wasUsingStartTime = !float.IsNaN(property.floatValue);

                        UnityEditor.EditorGUI.BeginProperty(toggleArea, GUIContent.none, property);
                        var useStartTime = GUI.Toggle(toggleArea, wasUsingStartTime, GUIContent.none);
                        UnityEditor.EditorGUI.EndProperty();

                        GUI.enabled = useStartTime;
                        float displayValue;
                        if (useStartTime)
                        {
                            if (!wasUsingStartTime)
                                property.floatValue = 0;

                            displayValue = property.floatValue;
                        }
                        else
                        {
                            if (wasUsingStartTime)
                                property.floatValue = float.NaN;

                            displayValue = 0;
                        }

                        // Time Field.

                        var labelWidth = UnityEditor.EditorGUIUtility.labelWidth;
                        UnityEditor.EditorGUIUtility.labelWidth += ToggleIndent + spacing;

                        label.text = "Start Time";
                        label.tooltip = "If enabled, the animation's time will start at the following value";

                        UnityEditor.EditorGUI.BeginChangeCheck();
                        label = UnityEditor.EditorGUI.BeginProperty(timeArea, label, property);
                        displayValue = UnityEditor.EditorGUI.FloatField(timeArea, label, displayValue);
                        UnityEditor.EditorGUI.EndProperty();
                        if (UnityEditor.EditorGUI.EndChangeCheck())
                            property.floatValue = displayValue;

                        UnityEditor.EditorGUIUtility.labelWidth = labelWidth;

                        // Normalized Toggle.

                        timeArea.x = timeArea.xMax;
                        timeArea.xMax = area.xMax;

                        var normalizedProperty = baseProperty.FindPropertyRelative("_StartTimeIsNormalized");

                        UnityEditor.EditorGUI.BeginChangeCheck();

                        label.text = "Normalized";
                        label = UnityEditor.EditorGUI.BeginProperty(normalizedArea, label, normalizedProperty);
                        var normalized = GUI.Toggle(normalizedArea, normalizedProperty.boolValue, label);
                        UnityEditor.EditorGUI.EndProperty();

                        if (UnityEditor.EditorGUI.EndChangeCheck())
                            normalizedProperty.boolValue = normalized;

                        GUI.enabled = enabled;

                        return;
                    }

                    base.DoPropertyGUI(ref area, baseProperty, property, label);
                }

                /************************************************************************************************************************/
            }

            /************************************************************************************************************************/
#endif
            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/

        /// <summary>
        /// A serializable object which can create a <see cref="ClipState"/> when passed into
        /// <see cref="AnimancerPlayable.Transition"/>. It also has a <see cref="UnityEvent"/> which is triggered
        /// by the <see cref="AnimancerState.OnEnd"/> callback.
        /// </summary>
        [Serializable]
        public class SerializableWithEndEvent : Serializable, ISerializationCallbackReceiver
        {
            /************************************************************************************************************************/

            [SerializeField]
            private UnityEvent _OnEnd;

            /// <summary>
            /// A callback which is triggered when the animation reaches its end (not when the state is exited for
            /// whatever reason).
            /// <para></para>
            /// If the animation is looping, this callback will be triggered every time it passes the end. Otherwise it
            /// will be triggered every frame while it is past the end so if you want to ensure that your callback only
            /// occurs once, you will need to clear it as part of that callback.
            /// <para></para>
            /// See <see cref="AnimancerState.OnEnd"/> for more details.
            /// </summary>
            public new UnityEvent OnEnd
            {
                get { return _OnEnd; }
                set
                {
                    _OnEnd = value;
                    base.OnEnd = value.Invoke;
                }
            }

            /************************************************************************************************************************/

            /// <summary>[<see cref="ISerializationCallbackReceiver"/>]
            /// Called in the Unity Editor before this object is serialized. Does nothing unless overridden.
            /// </summary>
            public virtual void OnBeforeSerialize() { }

            /// <summary>[<see cref="ISerializationCallbackReceiver"/>]
            /// Called after this object is deserialized to ensure that the <see cref="UnityEvent.Invoke"/> method is
            /// assigned to the <see cref="Serializable.OnEnd"/> callback.
            /// </summary>
            public virtual void OnAfterDeserialize()
            {
                base.OnEnd = _OnEnd.Invoke;
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
