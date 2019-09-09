// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// Base class for <see cref="AnimancerState"/>s which blend other states together.
    /// </summary>
    public abstract partial class MixerState : AnimancerState
    {
        /************************************************************************************************************************/
        #region Properties
        /************************************************************************************************************************/

        /// <summary>The number of input ports in the <see cref="Playable"/>.</summary>
        public abstract int PortCount { get; }

        /************************************************************************************************************************/

        /// <summary>Mixers should keep child playables connected to the graph at all times.</summary>
        public override bool KeepChildrenConnected { get { return true; } }

        /// <summary>An <see cref="MixerState"/> has no <see cref="AnimationClip"/>.</summary>
        public override AnimationClip Clip { get { return null; } }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the collection of states connected to this mixer. Note that some elements may be null.
        /// <para></para>
        /// Getting an enumerator that automatically skips over null states is slower and creates garbage, so
        /// internally we use this property and perform null checks manually even though it increases the code
        /// complexity a bit.
        /// </summary>
        public abstract IList<AnimancerState> ChildStates { get; }

        /// <summary>
        /// Returns an enumerator through each state connected to this mixer.
        /// </summary>
        public override IEnumerator<AnimancerState> GetEnumerator()
        {
            var childStates = ChildStates;
            if (childStates == null)
                yield break;

            var count = childStates.Count;
            for (int i = 0; i < count; i++)
            {
                var state = childStates[i];
                if (state == null)
                    continue;

                yield return state;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Determines whether the clips in this mixer are playing.
        /// </summary>
        public override bool IsPlaying
        {
            get { return base.IsPlaying; }
            set
            {
                base.IsPlaying = value;

                var childStates = ChildStates;
                if (childStates == null)
                    return;

                var count = childStates.Count;
                for (int i = 0; i < count; i++)
                {
                    var state = childStates[i];
                    if (state == null)
                        continue;

                    state.IsPlaying = value;
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// The current local time of this state in seconds.
        /// Setting this property passes the value onto all <see cref="ChildStates"/>.
        /// </summary>
        public override float Time
        {
            set
            {
                base.Time = value;

                var childStates = ChildStates;
                if (childStates == null)
                    return;

                var count = childStates.Count;
                for (int i = 0; i < count; i++)
                {
                    var state = childStates[i];
                    if (state == null)
                        continue;

                    state.Time = value;
                }
            }
        }

        /************************************************************************************************************************/

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member.
        /// <summary>
        /// <see cref="MixerState"/>s don't track the progress of a single animation so this event isn't
        /// used and will be ignored.
        /// </summary>
        [Obsolete("MixerState don't track the progress of a single animation so this event isn't used and will be ignored.")]
        public override Action OnEnd { get { return null; } set { } }
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member.

        /************************************************************************************************************************/

        /// <summary>
        /// Adds the 'callback' to the <see cref="AnimancerState.OnEnd"/> events of all child states.
        /// </summary>
        public void AddOnEndEventToChildren(Action callback)
        {
            var childStates = ChildStates;
            if (childStates == null)
                return;

            var count = childStates.Count;
            for (int i = 0; i < count; i++)
            {
                var state = childStates[i];
                if (state == null)
                    continue;

                state.OnEnd += callback;
            }
        }

        /// <summary>
        /// Removes the 'callback' from the <see cref="AnimancerState.OnEnd"/> events of all child states.
        /// </summary>
        public void RemoveOnEndEventFromChildren(Action callback)
        {
            var childStates = ChildStates;
            if (childStates == null)
                return;

            var count = childStates.Count;
            for (int i = 0; i < count; i++)
            {
                var state = childStates[i];
                if (state == null)
                    continue;

                state.OnEnd -= callback;
            }
        }

        /************************************************************************************************************************/

        /// <summary>Indicates whether the weights of all child states should be recalculated.</summary>
        public bool AreWeightsDirty { get; protected set; }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Initialisation
        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="MixerState"/> without connecting it to the <see cref="PlayableGraph"/>.
        /// You must call <see cref="AnimancerState.SetParent(AnimancerLayer)"/> or it won't actually do anything.
        /// </summary>
        public MixerState(AnimancerPlayable root)
            : base(root)
        {
            _Playable = AnimationMixerPlayable.Create(root._Graph, 0, true);
        }

        /// <summary>
        /// Constructs a new <see cref="MixerState"/> and connects it to the 'layer's
        /// <see cref="IAnimationMixer.Playable"/> using a spare port if there are any from previously destroyed
        /// states, or by adding a new port.
        /// </summary>
        public MixerState(AnimancerLayer layer)
            : this(layer.Root)
        {
            layer.AddChild(this);
        }

        /// <summary>
        /// Constructs a new <see cref="MixerState"/> and connects it to the 'parent's
        /// <see cref="IAnimationMixer.Playable"/> at the specified 'portIndex'.
        /// </summary>
        public MixerState(AnimancerNode parent, int portIndex)
            : this(parent.Root)
        {
            SetParent(parent, portIndex);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Creates and returns a new <see cref="ClipState"/> to play the 'clip' with this
        /// <see cref="MixerState"/> as its parent.
        /// </summary>
        public virtual ClipState CreateState(int portIndex, AnimationClip clip)
        {
            return new ClipState(this, portIndex, clip)
            {
                IsPlaying = IsPlaying,
                Time = Time
            };
        }

        /************************************************************************************************************************/

        /// <summary>The number of states using this mixer as their <see cref="AnimancerState.Parent"/>.</summary>
        public override int ChildCount { get { return ChildStates.Count; } }

        /// <summary>
        /// Returns the state connected to the specified 'portIndex' as a child of this mixer.
        /// </summary>
        public override AnimancerState GetChild(int portIndex)
        {
            return ChildStates[portIndex];
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Updates
        /************************************************************************************************************************/

        /// <summary>
        /// Updates the time of this mixer and all of its child states.
        /// </summary>
        protected override void Update(ref bool needsMoreUpdates)
        {
            base.Update(ref needsMoreUpdates);

            if (!AreWeightsDirty || Weight == 0)
                return;

            RecalculateWeights();

            if (AreWeightsDirty)
                Debug.LogWarning("AnimationMixer.AreWeightsDirty has not been set to false by RecalculateWeights().");

            // We need to apply the child weights immediately to ensure they are all in sync. Otherwise some of the
            // child states might have already been updated before the mixer and would not apply it until next frame.
            var childStates = ChildStates;
            if (childStates != null)
            {
                var count = childStates.Count;
                for (int i = 0; i < count; i++)
                {
                    var state = childStates[i];
                    if (state == null)
                        continue;

                    state.ApplyWeight();
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Recalculates the weights of all <see cref="States"/> based on the current value of the
        /// <see cref="Parameter"/> and the <see cref="Thresholds"/>.
        /// <para></para>
        /// Overrides of this method must set <see cref="AreWeightsDirty"/> = false.
        /// </summary>
        public abstract void RecalculateWeights();

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Inverse Kinematics
        /************************************************************************************************************************/

        /// <summary>
        /// Determines whether <c>OnAnimatorIK(int layerIndex)</c> will be called on the animated object for any
        /// <see cref="ChildStates"/>.
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
            get
            {
                var childStates = ChildStates;
                if (childStates == null)
                    return false;

                var count = childStates.Count;
                for (int i = 0; i < count; i++)
                {
                    var state = childStates[i];
                    if (state == null)
                        continue;

                    if (state.ApplyAnimatorIK)
                        return true;
                }

                return false;
            }
            set
            {
                var childStates = ChildStates;
                if (childStates == null)
                    return;

                var count = childStates.Count;
                for (int i = 0; i < count; i++)
                {
                    var state = childStates[i];
                    if (state == null)
                        continue;

                    state.ApplyAnimatorIK = value;
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Indicates whether any of the <see cref="ChildStates"/> in this mixer are applying IK to the character's feet.
        /// The initial value is determined by <see cref="AnimancerLayer.DefaultApplyFootIK"/>.
        /// <para></para>
        /// This is equivalent to the "Foot IK" toggle in Animator Controller states.
        /// </summary>
        public override bool ApplyFootIK
        {
            get
            {
                var childStates = ChildStates;
                if (childStates == null)
                    return false;

                var count = childStates.Count;
                for (int i = 0; i < count; i++)
                {
                    var state = childStates[i];
                    if (state == null)
                        continue;

                    if (state.ApplyFootIK)
                        return true;
                }

                return false;
            }
            set
            {
                var childStates = ChildStates;
                if (childStates == null)
                    return;

                var count = childStates.Count;
                for (int i = 0; i < count; i++)
                {
                    var state = childStates[i];
                    if (state == null)
                        continue;

                    state.ApplyFootIK = value;
                }
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Other Methods
        /************************************************************************************************************************/

        /// <summary>The average velocity of the root motion caused by this state.</summary>
        public override Vector3 AverageVelocity
        {
            get
            {
                var velocity = Vector3.zero;

                var childStates = ChildStates;
                if (childStates != null)
                {
                    var count = childStates.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var state = childStates[i];
                        if (state == null)
                            continue;

                        velocity += state.AverageVelocity * state.Weight;
                    }
                }

                return velocity;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Recalculates the <see cref="AnimancerState.Duration"/> of all child states so that they add up to 1.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if there are any states with no <see cref="Clip"/>.</exception>
        public void NormalizeDurations()
        {
            var childStates = ChildStates;
            if (childStates == null)
                return;

            int divideBy = 0;
            float totalDuration = 0f;

            // Count the number of states that exist and their total duration.
            var count = childStates.Count;
            for (int i = 0; i < count; i++)
            {
                var state = childStates[i];
                if (state == null)
                    continue;

                divideBy++;
                totalDuration += state.Duration;
            }

            // Calculate the average duration.
            totalDuration /= divideBy;

            // Set all states to that duration.
            for (int i = 0; i < count; i++)
            {
                var state = childStates[i];
                if (state == null)
                    continue;

                state.Duration = totalDuration;
            }
        }

        /************************************************************************************************************************/

        /// <summary>Checks if any of this mixer's children has an animation event with the specified 'functionName'.</summary>
        public override bool HasEvent(string functionName)
        {
            var childStates = ChildStates;
            if (childStates == null)
                return false;

            var count = childStates.Count;
            for (int i = 0; i < count; i++)
            {
                var state = childStates[i];
                if (state == null)
                    continue;

                if (state.HasEvent(functionName))
                    return true;
            }

            return false;
        }

        /// <summary>Gets a user-friendly key to identify the 'state' in the inspector.</summary>
        public abstract string GetDisplayKey(AnimancerState state);

        /************************************************************************************************************************/

        /// <summary>
        /// Returns a string describing the type of this mixer and the name of <see cref="Clip"/>s connected to it.
        /// </summary>
        public override string ToString()
        {
            var name = new StringBuilder();
            name.Append(GetType().Name);
            name.Append(" (");
            bool first = true;

            var childStates = ChildStates;
            if (childStates == null)
            {
                name.Append("null");
            }
            else
            {
                var count = childStates.Count;
                for (int i = 0; i < count; i++)
                {
                    var state = childStates[i];
                    if (state == null)
                        continue;

                    if (first)
                        first = false;
                    else
                        name.Append(", ");

                    if (state.Clip != null)
                        name.Append(state.Clip.name);
                    else
                        name.Append(state);
                }
            }
            name.Append(")");
            return name.ToString();
        }

        /************************************************************************************************************************/

        /// <summary>[<see cref="IAnimationClipSource"/>]
        /// Gathers all the animations associated with this state.
        /// </summary>
        public override void GetAnimationClips(List<AnimationClip> clips)
        {
            var childStates = ChildStates;
            if (childStates == null)
                return;

            var count = childStates.Count;
            for (int i = 0; i < count; i++)
            {
                var state = childStates[i];
                if (state == null)
                    continue;

                state.GetAnimationClips(clips);
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Inspector
        /************************************************************************************************************************/

        /// <summary>The number of parameters being managed by this state.</summary>
        protected virtual int ParameterCount { get { return 0; } }

        /// <summary>Returns the name of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected virtual string GetParameterName(int index) { throw new NotSupportedException(); }

        /// <summary>Returns the type of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected virtual UnityEngine.AnimatorControllerParameterType GetParameterType(int index) { throw new NotSupportedException(); }

        /// <summary>Returns the value of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected virtual object GetParameterValue(int index) { throw new NotSupportedException(); }

        /// <summary>Sets the value of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected virtual void SetParameterValue(int index, object value) { throw new NotSupportedException(); }

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
        public sealed class Drawer : Editor.ParametizedAnimancerStateDrawer<MixerState>
        {
            /************************************************************************************************************************/

            /// <summary>
            /// Constructs a new <see cref="Drawer"/> to manage the inspector GUI for the 'state'.
            /// </summary>
            public Drawer(MixerState state) : base(state) { }

            /************************************************************************************************************************/

            /// <summary>The number of parameters being managed by the target state.</summary>
            public override int ParameterCount { get { return State.ParameterCount; } }

            /// <summary>Returns the name of a parameter being managed by the target state.</summary>
            /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
            public override string GetParameterName(int index) { return State.GetParameterName(index); }

            /// <summary>Returns the type of a parameter being managed by the target state.</summary>
            /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
            public override UnityEngine.AnimatorControllerParameterType GetParameterType(int index) { return State.GetParameterType(index); }

            /// <summary>Returns the value of a parameter being managed by the target state.</summary>
            /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
            public override object GetParameterValue(int index) { return State.GetParameterValue(index); }

            /// <summary>Sets the value of a parameter being managed by the target state.</summary>
            /// <exception cref="NotSupportedException">Thrown if the target state doesn't manage any parameters.</exception>
            public override void SetParameterValue(int index, object value) { State.SetParameterValue(index, value); }

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
        /// Base class for serializable objects which can create a particular type of
        /// <see cref="MixerState{TParameter}"/> when passed into <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        /// <remarks>
        /// Even though it has the <see cref="SerializableAttribute"/>, this class won't actually get serialized
        /// by Unity because it's generic and abstract. Each child class still needs to include the attribute.
        /// </remarks>
        [Serializable]
        public abstract class Serializable<TMixer, TParameter> : ManualMixerState.Serializable<TMixer>
            where TMixer : MixerState<TParameter>
        {
            /************************************************************************************************************************/

            [SerializeField, HideInInspector]
            private TParameter[] _Thresholds;

            /// <summary>The parameter values at which each of the states are used and blended.</summary>
            public TParameter[] Thresholds
            {
                get { return _Thresholds; }
                set { _Thresholds = value; }
            }

            /************************************************************************************************************************/

            [SerializeField]
            private TParameter _DefaultParameter;

            /// <summary>The initial parameter value to give the mixer when it is first created.</summary>
            public TParameter DefaultParameter
            {
                get { return _DefaultParameter; }
                set { _DefaultParameter = value; }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Initialises the <see cref="AnimancerState.Serializable{TState}.State"/> immediately after it is created.
            /// </summary>
            public override void InitialiseState()
            {
                base.InitialiseState();

                State.SetThresholds(_Thresholds);
                State.Parameter = _DefaultParameter;
            }

            /************************************************************************************************************************/
            #region Drawer
#if UNITY_EDITOR
            /************************************************************************************************************************/

            /// <summary>[Editor-Only] Draws the inspector GUI for a <see cref="Serializable{TMixer, TParameter}"/>.</summary>
            [CustomPropertyDrawer(typeof(Serializable<,>), true)]
            public class Drawer : ManualMixerState.Serializable.Drawer
            {
                /************************************************************************************************************************/

                /// <summary>
                /// The number of horizontal pixels the threshold label occupies.
                /// </summary>
                private readonly float ThresholdWidth;

                private static float _ThresholdLabelWidth;

                /// <summary>
                /// The number of horizontal pixels the word "Threshold" label occupies when drawn with the standard
                /// <see cref="GUISkin.label"/> style.
                /// </summary>
                protected static float ThresholdLabelWidth
                {
                    get
                    {
                        if (_ThresholdLabelWidth == 0)
                            _ThresholdLabelWidth = Editor.AnimancerEditorUtilities.CalculateWidth(GUI.skin.label, "Threshold");
                        return _ThresholdLabelWidth;
                    }
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Constructs a new <see cref="Drawer"/> using the default <see cref="ThresholdLabelWidth"/>.
                /// </summary>
                public Drawer() : this(ThresholdLabelWidth) { }

                /// <summary>
                /// Constructs a new <see cref="Drawer"/> using a custom width for its threshold labels.
                /// </summary>
                protected Drawer(float thresholdWidth)
                {
                    ThresholdWidth = thresholdWidth;
                }

                /************************************************************************************************************************/

                /// <summary>
                /// The serialized <see cref="Thresholds"/> of the
                /// <see cref="ManualMixerState.Serializable.Drawer.CurrentProperty"/>.
                /// </summary>
                protected static SerializedProperty CurrentThresholds { get; private set; }

                /************************************************************************************************************************/

                /// <summary>
                /// Called every time a 'property' is drawn to find the relevant child properties and store them to be
                /// used in <see cref="GetPropertyHeight"/> and <see cref="OnGUI"/>.
                /// </summary>
                protected override void GatherSubProperties(SerializedProperty property)
                {
                    base.GatherSubProperties(property);

                    CurrentThresholds = property.FindPropertyRelative("_Thresholds");

                    var count = Mathf.Max(CurrentClips.arraySize, CurrentThresholds.arraySize);
                    CurrentClips.arraySize = count;
                    CurrentThresholds.arraySize = count;
                    if (CurrentSpeeds.arraySize != 0)
                        CurrentSpeeds.arraySize = count;
                }

                /************************************************************************************************************************/

                /// <summary>Splits the specified 'area' into separate sections.</summary>
                protected void SplitListRect(Rect area, out Rect state, out Rect threshold, out Rect speed)
                {
                    area.width += 2;

                    state = threshold = speed = area;

                    speed.xMin = speed.xMax - 50;

                    var xMax = threshold.xMax = speed.xMin - EditorGUIUtility.standardVerticalSpacing;
                    var xMin = threshold.xMin = Mathf.Max(
                        EditorGUIUtility.labelWidth + Editor.AnimancerEditorUtilities.IndentSize,
                        xMax - ThresholdWidth);

                    state.xMax = xMin;
                }

                /************************************************************************************************************************/

                /// <summary>Draws the headdings of the state list.</summary>
                protected override void DoStateListHeaderGUI(Rect area)
                {
                    Rect stateArea, thresholdArea, speedArea;
                    SplitListRect(area, out stateArea, out thresholdArea, out speedArea);

                    DoAnimationLabelGUI(stateArea);

                    GUI.Label(thresholdArea, Editor.AnimancerEditorUtilities.TempContent("Threshold",
                        "The parameter values at which each sub-state will be fully active"));

                    DoSpeedLabelGUI(speedArea);
                }

                /************************************************************************************************************************/

                /// <summary>Draws the GUI of the state at the specified 'index'.</summary>
                protected override void DoElementGUI(Rect area, int index,
                    SerializedProperty clip, SerializedProperty speed)
                {
                    var threshold = CurrentThresholds.GetArrayElementAtIndex(index);

                    Rect stateArea, thresholdArea, speedArea;
                    SplitListRect(area, out stateArea, out thresholdArea, out speedArea);

                    DoElementGUI(stateArea, speedArea, index, clip, speed);

                    EditorGUI.PropertyField(thresholdArea, threshold, GUIContent.none);
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Called when adding a new state to the list to ensure that any other relevant arrays have new
                /// elements added as well.
                /// </summary>
                protected override void OnAddElement(ReorderableList list)
                {
                    var index = CurrentClips.arraySize;
                    base.OnAddElement(list);
                    CurrentThresholds.InsertArrayElementAtIndex(index);
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Called when removing a state from the list to ensure that any other relevant arrays have elements
                /// removed as well.
                /// </summary>
                protected override void OnRemoveElement(ReorderableList list)
                {
                    base.OnRemoveElement(list);
                    RemoveArrayElement(CurrentThresholds, list.index);
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Called when reordering states in the list to ensure that any other relevant arrays have their
                /// corresponding elements reordered as well.
                /// </summary>
                protected override void OnReorderList(ReorderableList list, int oldIndex, int newIndex)
                {
                    base.OnReorderList(list, oldIndex, newIndex);
                    CurrentThresholds.MoveArrayElement(oldIndex, newIndex);
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
        #region Serializable 2D
        /************************************************************************************************************************/

        /// <summary>
        /// A serializable object which can create a <see cref="CartesianMixerState"/> or
        /// <see cref="DirectionalMixerState"/> when passed into <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        [Serializable]
        public class Serializable2D : Serializable<MixerState<Vector2>, Vector2>
        {
            /************************************************************************************************************************/

            /// <summary>
            /// A type of <see cref="MixerState"/> that can be created by a <see cref="Serializable2D"/>.
            /// </summary>
            public enum MixerType
            {
                /// <summary><see cref="CartesianMixerState"/></summary>
                Cartesian,

                /// <summary><see cref="DirectionalMixerState"/></summary>
                Directional,
            }

            [SerializeField]
            private MixerType _Type;

            /// <summary>
            /// The type of <see cref="MixerState"/> that this serializable will create.
            /// </summary>
            public MixerType Type
            {
                get { return _Type; }
                set { _Type = value; }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Creates and returns a new <see cref="CartesianMixerState"/> or <see cref="DirectionalMixerState"/>
            /// depending on the <see cref="Type"/> connected to the 'layer'.
            /// <para></para>
            /// This method also assigns it as the <see cref="AnimancerState.Serializable{TState}.State"/>.
            /// </summary>
            public override MixerState<Vector2> CreateState(AnimancerLayer layer)
            {
                switch (_Type)
                {
                    case MixerType.Cartesian: State = new CartesianMixerState(layer); break;
                    case MixerType.Directional: State = new DirectionalMixerState(layer); break;
                    default: throw new ArgumentOutOfRangeException("_Type");
                }
                InitialiseState();
                return State;
            }

            /************************************************************************************************************************/
            #region Drawer
#if UNITY_EDITOR
            /************************************************************************************************************************/

            /// <summary>[Editor-Only]
            /// Draws the inspector GUI for a <see cref="Vector2"/> <see cref="Serializable{TMixer, TParameter}"/>.
            /// </summary>
            [CustomPropertyDrawer(typeof(Serializable2D), true)]
            public new class Drawer : Serializable<MixerState<Vector2>, Vector2>.Drawer
            {
                /************************************************************************************************************************/

                /// <summary>
                /// Constructs a new <see cref="Drawer"/> using the a wider 'thresholdWidth' than usual to accomodate
                /// both the X and Y values.
                /// </summary>
                public Drawer() : base(ThresholdLabelWidth * 2 + 20) { }

                /************************************************************************************************************************/

                /// <summary>
                /// Fills the 'menu' with functions relevant to the 'rootProperty'.
                /// </summary>
                protected override void BuildContextMenu(GenericMenu menu, SerializedProperty rootProperty)
                {
                    base.BuildContextMenu(menu, rootProperty);

                    AddPropertyModifierFunction(menu, "Initialise Standard 4 Directions", InitialiseStandard4Directions);

                    AddCalculateThresholdsFunction(menu, "Calculate Thresholds From Velocity XZ", (clip, threshold) =>
                    {
                        var velocity = clip.averageSpeed;
                        return new Vector2(velocity.x, velocity.z);
                    });

                    AddCalculateThresholdsFunctionPerAxis(menu, "From Speed",
                        (clip, threshold) => clip.apparentSpeed);
                    AddCalculateThresholdsFunctionPerAxis(menu, "From Velocity X",
                        (clip, threshold) => clip.averageSpeed.x);
                    AddCalculateThresholdsFunctionPerAxis(menu, "From Velocity Y",
                        (clip, threshold) => clip.averageSpeed.z);
                    AddCalculateThresholdsFunctionPerAxis(menu, "From Velocity Z",
                        (clip, threshold) => clip.averageSpeed.z);
                    AddCalculateThresholdsFunctionPerAxis(menu, "From Angular Speed (Rad)",
                        (clip, threshold) => clip.averageAngularSpeed * Mathf.Deg2Rad);
                    AddCalculateThresholdsFunctionPerAxis(menu, "From Angular Speed (Deg)",
                        (clip, threshold) => clip.averageAngularSpeed);
                }

                /************************************************************************************************************************/

                private void InitialiseStandard4Directions(SerializedProperty property)
                {
                    var oldSpeedCount = CurrentSpeeds.arraySize;

                    CurrentClips.arraySize = CurrentThresholds.arraySize = CurrentSpeeds.arraySize = 5;
                    CurrentThresholds.GetArrayElementAtIndex(0).vector2Value = Vector2.zero;
                    CurrentThresholds.GetArrayElementAtIndex(1).vector2Value = Vector2.up;
                    CurrentThresholds.GetArrayElementAtIndex(2).vector2Value = Vector2.right;
                    CurrentThresholds.GetArrayElementAtIndex(3).vector2Value = Vector2.down;
                    CurrentThresholds.GetArrayElementAtIndex(4).vector2Value = Vector2.left;

                    InitialiseSpeeds(oldSpeedCount);

                    var type = property.FindPropertyRelative("_Type");
                    type.enumValueIndex = (int)MixerType.Directional;
                }

                /************************************************************************************************************************/

                private void AddCalculateThresholdsFunction(GenericMenu menu, string label,
                    Func<AnimationClip, Vector2, Vector2> calculateThreshold)
                {
                    AddPropertyModifierFunction(menu, label, (property) =>
                    {
                        var count = CurrentClips.arraySize;
                        for (int i = 0; i < count; i++)
                        {
                            var clip = CurrentClips.GetArrayElementAtIndex(i).objectReferenceValue as AnimationClip;
                            if (clip == null)
                                continue;

                            var threshold = CurrentThresholds.GetArrayElementAtIndex(i);

                            threshold.vector2Value = calculateThreshold(clip, threshold.vector2Value);
                        }
                    });
                }

                /************************************************************************************************************************/

                private void AddCalculateThresholdsFunctionPerAxis(GenericMenu menu, string label,
                    Func<AnimationClip, float, float> calculateThreshold)
                {
                    AddCalculateThresholdsFunction(menu, "Calculate Thresholds X/" + label, 0, calculateThreshold);
                    AddCalculateThresholdsFunction(menu, "Calculate Thresholds Y/" + label, 1, calculateThreshold);
                }

                private void AddCalculateThresholdsFunction(GenericMenu menu, string label, int axis,
                    Func<AnimationClip, float, float> calculateThreshold)
                {
                    AddPropertyModifierFunction(menu, label, (property) =>
                    {
                        var count = CurrentClips.arraySize;
                        for (int i = 0; i < count; i++)
                        {
                            var clip = CurrentClips.GetArrayElementAtIndex(i).objectReferenceValue as AnimationClip;
                            if (clip == null)
                                continue;

                            var threshold = CurrentThresholds.GetArrayElementAtIndex(i);

                            var value = threshold.vector2Value;
                            var newValue = calculateThreshold(clip, value[axis]);
                            if (!float.IsNaN(newValue))
                                value[axis] = newValue;
                            threshold.vector2Value = value;
                        }
                    });
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
