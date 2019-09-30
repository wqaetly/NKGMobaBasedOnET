// Animancer // Copyright 2019 Kybernetik //

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// An <see cref="AnimancerState"/> which blends multiple child states. Unlike other mixers, this class does not
    /// perform any automatic weight calculations, it simple allows you to control the weight of all states manually.
    /// <para></para>
    /// This mixer type is similar to the Direct Blend Type in Mecanim Blend Trees.
    /// </summary>
    public class ManualMixerState : MixerState
    {
        /************************************************************************************************************************/

        /// <summary>The states managed by this mixer.</summary>
        public AnimancerState[] States { get; protected set; }

        /// <summary>The number of input ports in the <see cref="Mixer"/>.</summary>
        public override int PortCount { get { return States.Length; } }

        /************************************************************************************************************************/

        /// <summary>Returns the <see cref="States"/> array.</summary>
        public override IList<AnimancerState> ChildStates { get { return States; } }

        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="LinearMixerState"/> without connecting it to the <see cref="PlayableGraph"/>.
        /// You must call <see cref="AnimancerState.SetParent(AnimancerLayer)"/> or it won't actually do anything.
        /// </summary>
        protected ManualMixerState(AnimancerPlayable root) : base(root) { }

        /// <summary>
        /// Constructs a new <see cref="LinearMixerState"/> and connects it to the 'layer's
        /// <see cref="IAnimationMixer.Playable"/> using a spare port if there are any from previously destroyed
        /// states, or by adding a new port.
        /// </summary>
        public ManualMixerState(AnimancerLayer layer) : base(layer) { }

        /// <summary>
        /// Constructs a new <see cref="LinearMixerState"/> and connects it to the 'parent's
        /// <see cref="IAnimationMixer.Playable"/> at the specified 'portIndex'.
        /// </summary>
        public ManualMixerState(AnimancerNode parent, int portIndex) : base(parent, portIndex) { }

        /************************************************************************************************************************/

        /// <summary>
        /// Initialises this mixer with the specified number of ports which can be filled individually by <see cref="CreateState"/>.
        /// </summary>
        public virtual void Initialise(int portCount)
        {
            if (portCount <= 1)
                Debug.LogWarning(GetType() + " is being initialised with capacity <= 1. The purpose of a mixer is to mix multiple clips.");

            _Playable.SetInputCount(portCount);
            States = new AnimancerState[portCount];
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Initialises this mixer with one state per clip.
        /// </summary>
        public void Initialise(params AnimationClip[] clips)
        {
            int count = clips.Length;
            _Playable.SetInputCount(count);

            if (count <= 1)
                Debug.LogWarning(GetType() + " is being initialised without multiple clips. The purpose of a mixer is to mix multiple clips.");

            States = new AnimancerState[count];

            for (int i = 0; i < count; i++)
            {
                var clip = clips[i];
                if (clip != null)
                    new ClipState(this, i, clip);
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Creates and returns a new <see cref="ClipState"/> to play the 'clip' with this
        /// <see cref="MixerState"/> as its parent.
        /// </summary>
        public override ClipState CreateState(int portIndex, AnimationClip clip)
        {
            var oldState = States[portIndex];
            if (oldState != null)
            {
                // If the old state has the specified 'clip', return it.
                if (oldState.Clip == clip)
                {
                    return oldState as ClipState;
                }
                else// Otherwise destroy and replace it.
                {
                    oldState.Dispose();
                }
            }

            return base.CreateState(portIndex, clip);
        }

        /************************************************************************************************************************/

        /// <summary>Connects the 'state' to this mixer at its <see cref="AnimancerNode.PortIndex"/>.</summary>
        protected internal override void OnAddChild(AnimancerState state)
        {
            OnAddChild(States, state);
        }

        /// <summary>Disconnects the 'state' from this mixer at its <see cref="AnimancerNode.PortIndex"/>.</summary>
        protected internal override void OnRemoveChild(AnimancerState state)
        {
            ValidateRemoveChild(States[state.PortIndex], state);
            States[state.PortIndex] = null;
            state.DisconnectFromGraph();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Destroys the <see cref="Playable"/> of this mixer and its <see cref="States"/>.
        /// </summary>
        public override void Dispose()
        {
            DestroyChildren();
            base.Dispose();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Destroys all <see cref="States"/> connected to this mixer. This operation cannot be undone.
        /// </summary>
        public void DestroyChildren()
        {
            if (States == null)
                return;

            var count = States.Length;
            for (int i = 0; i < count; i++)
            {
                var state = States[i];
                if (state != null)
                    state.Dispose();
            }

            States = null;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Does nothing. Manual mixers don't automatically recalculate their weights.
        /// </summary>
        public override void RecalculateWeights() { }

        /************************************************************************************************************************/

        /// <summary>
        /// Sets the weight of all states after the 'previousIndex' to 0.
        /// </summary>
        protected void DisableRemainingStates(int previousIndex)
        {
            var count = States.Length;
            while (++previousIndex < count)
            {
                var state = States[previousIndex];
                if (state == null)
                    continue;

                state.Weight = 0;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the state at the specified 'index' if it isn't null, otherwise increments the index and checks
        /// again. Returns null if no state is found by the end of the <see cref="States"/> array.
        /// </summary>
        protected AnimancerState GetNextState(ref int index)
        {
            while (index < States.Length)
            {
                var state = States[index];
                if (state != null)
                    return state;

                index++;
            }

            return null;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Divides the weight of all states by the 'totalWeight' so that they all add up to 1.
        /// </summary>
        protected void NormalizeWeights(float totalWeight)
        {
            if (totalWeight == 1)
                return;

            totalWeight = 1f / totalWeight;

            int count = States.Length;
            for (int i = 0; i < count; i++)
            {
                var state = States[i];
                if (state == null)
                    continue;

                state.Weight *= totalWeight;
            }
        }

        /************************************************************************************************************************/

        /// <summary>Gets a user-friendly key to identify the 'state' in the inspector.</summary>
        public override string GetDisplayKey(AnimancerState state)
        {
            return string.Concat("[", state.PortIndex.ToString(), "]");
        }

        /************************************************************************************************************************/
        #region Serializable
        /************************************************************************************************************************/

        /// <summary>
        /// Base class for serializable objects which can create a particular type of <see cref="ManualMixerState"/>
        /// when passed into <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        /// <remarks>
        /// Even though it has the <see cref="SerializableAttribute"/>, this class won't actually get serialized
        /// by Unity because it's generic and abstract. Each child class still needs to include the attribute.
        /// </remarks>
        [Serializable]
        public abstract new class Serializable<TMixer> : AnimancerState.Serializable<TMixer> where TMixer : ManualMixerState
        {
            /************************************************************************************************************************/

            [SerializeField, HideInInspector]
            private AnimationClip[] _Clips;

            /// <summary>
            /// The <see cref="ClipState.Clip"/> to use for each state in the mixer.
            /// </summary>
            public AnimationClip[] Clips
            {
                get { return _Clips; }
                set { _Clips = value; }
            }

            [SerializeField, HideInInspector]
            private float[] _Speeds;

            /// <summary>
            /// The <see cref="AnimancerState.Speed"/> to use for each state in the mixer.
            /// <para></para>
            /// If the size of this array doesn't match the <see cref="Clips"/>, it will be ignored.
            /// </summary>
            public float[] Speeds
            {
                get { return _Speeds; }
                set { _Speeds = value; }
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Initialises the <see cref="AnimancerState.Serializable{TState}.State"/> immediately after it is created.
            /// </summary>
            public virtual void InitialiseState()
            {
                State.Initialise(_Clips);

                if (_Speeds != null && _Speeds.Length == _Clips.Length)
                {
                    for (int i = 0; i < State.States.Length; i++)
                    {
                        State.States[i].Speed = _Speeds[i];
                    }
                }
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/

        /// <summary>
        /// A serializable object which can create a <see cref="ManualMixerState"/> when passed into
        /// <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        [Serializable]
        public class Serializable : Serializable<ManualMixerState>
        {
            /************************************************************************************************************************/

            /// <summary>
            /// Creates and returns a new <see cref="ManualMixerState"/> connected to the 'layer'.
            /// <para></para>
            /// This method also assigns it as the <see cref="AnimancerState.Serializable{TState}.State"/>.
            /// </summary>
            public override ManualMixerState CreateState(AnimancerLayer layer)
            {
                return State = new ManualMixerState(layer);
            }

            /************************************************************************************************************************/
            #region Drawer
#if UNITY_EDITOR
            /************************************************************************************************************************/

            /// <summary>[Editor-Only] Draws the inspector GUI for a <see cref="Serializable"/>.</summary>
            [CustomPropertyDrawer(typeof(Serializable), true)]
            public class Drawer : Editor.AnimancerStateSerializableDrawer
            {
                /************************************************************************************************************************/

                /// <summary>
                /// The property this drawer is currently drawing.
                /// <para></para>
                /// Normally each property has its own drawer, but arrays share a single drawer for all elements.
                /// </summary>
                protected static SerializedProperty CurrentProperty { get; private set; }

                /// <summary>The <see cref="Serializable{TState}.Clips"/> field.</summary>
                protected static SerializedProperty CurrentClips { get; private set; }

                /// <summary>The <see cref="Serializable{TState}.Speeds"/> field.</summary>
                protected static SerializedProperty CurrentSpeeds { get; private set; }

                private readonly Dictionary<string, ReorderableList>
                    PropertyPathToStates = new Dictionary<string, ReorderableList>();

                /// <summary>
                /// Gather the details of the 'property'.
                /// <para></para>
                /// This method gets called by every <see cref="GetPropertyHeight"/> and <see cref="OnGUI"/> call since
                /// Unity uses the same <see cref="PropertyDrawer"/> instance for each element in a collection, so it
                /// needs to gather the details associated with the current property.
                /// </summary>
                protected virtual ReorderableList GatherDetails(SerializedProperty property)
                {
                    CurrentProperty = property;
                    GatherSubProperties(property);

                    var propertyPath = property.propertyPath;

                    ReorderableList states;
                    if (!PropertyPathToStates.TryGetValue(propertyPath, out states))
                    {
                        states = new ReorderableList(CurrentClips.serializedObject, CurrentClips)
                        {
                            drawHeaderCallback = DoStateListHeaderGUI,
                            elementHeightCallback = GetElementHeight,
                            drawElementCallback = DoElementGUI,
                            onAddCallback = OnAddElement,
                            onRemoveCallback = OnRemoveElement,
#if UNITY_2018_1_OR_NEWER
                            onReorderCallbackWithDetails = OnReorderList,
#else
                            onReorderCallback = OnReorderList,
                            onSelectCallback = OnListSelectionChanged,
#endif
                        };

                        PropertyPathToStates.Add(propertyPath, states);
                    }

                    return states;
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Called every time a 'property' is drawn to find the relevant child properties and store them to be
                /// used in <see cref="GetPropertyHeight"/> and <see cref="OnGUI"/>.
                /// </summary>
                protected virtual void GatherSubProperties(SerializedProperty property)
                {
                    CurrentClips = property.FindPropertyRelative("_Clips");
                    CurrentSpeeds = property.FindPropertyRelative("_Speeds");

                    if (CurrentSpeeds.arraySize != 0)
                        CurrentSpeeds.arraySize = CurrentClips.arraySize;
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Calculates the number of vertical pixels the 'property' will occupy when it is drawn.
                /// </summary>
                public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
                {
                    var height = EditorGUI.GetPropertyHeight(property, label);

                    if (property.isExpanded)
                    {
                        var states = GatherDetails(property);
                        height += EditorGUIUtility.standardVerticalSpacing + states.GetHeight();
                    }

                    return height;
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Draws the root 'property' GUI and calls <see cref="DoPropertyGUI"/> for each of its children.
                /// </summary>
                public override void OnGUI(Rect area, SerializedProperty property, GUIContent label)
                {
                    var originalProperty = property.Copy();

                    base.OnGUI(area, property, label);

                    if (!originalProperty.isExpanded)
                        return;

                    var states = GatherDetails(originalProperty);

                    var indentLevel = EditorGUI.indentLevel;

                    area.yMin = area.yMax - states.GetHeight();

                    EditorGUI.indentLevel++;
                    area = EditorGUI.IndentedRect(area);

                    EditorGUI.indentLevel = 0;
                    states.DoList(area);

                    EditorGUI.indentLevel = indentLevel;

                    TryCollapseSpeeds();
                }

                /************************************************************************************************************************/

                /// <summary>Splits the specified 'area' into separate sections.</summary>
                protected void SplitListRect(Rect area, out Rect state, out Rect speed)
                {
                    area.width += 2;

                    state = speed = area;

                    state.xMax = speed.xMin = speed.xMax - 50;
                }

                /************************************************************************************************************************/

                /// <summary>Draws the headdings of the state list.</summary>
                protected virtual void DoStateListHeaderGUI(Rect area)
                {
                    Rect stateArea, speedArea;
                    SplitListRect(area, out stateArea, out speedArea);

                    DoAnimationLabelGUI(stateArea);
                    DoSpeedLabelGUI(speedArea);
                }

                /************************************************************************************************************************/

                /// <summary>Draws an "Animation" label.</summary>
                protected static void DoAnimationLabelGUI(Rect area)
                {
                    GUI.Label(area, Editor.AnimancerEditorUtilities.TempContent("Animation",
                        "The animations that will be used for each sub-state"));
                }

                /// <summary>Draws a "Speed" label.</summary>
                protected static void DoSpeedLabelGUI(Rect area)
                {
                    GUI.Label(area, Editor.AnimancerEditorUtilities.TempContent("Speed",
                        "Determines how fast each sub-state plays (Default = 1)"));
                }

                /************************************************************************************************************************/

                /// <summary>Calculates the height of the state at the specified 'index'.</summary>
                protected virtual float GetElementHeight(int index)
                {
                    return EditorGUIUtility.singleLineHeight;
                }

                /************************************************************************************************************************/

                /// <summary>Draws the GUI of the state at the specified 'index'.</summary>
                private void DoElementGUI(Rect area, int index, bool isActive, bool isFocused)
                {
                    if (index < 0 || index > CurrentClips.arraySize)
                        return;

                    var clip = CurrentClips.GetArrayElementAtIndex(index);
                    var speed = CurrentSpeeds.arraySize > 0 ? CurrentSpeeds.GetArrayElementAtIndex(index) : null;
                    DoElementGUI(area, index, clip, speed);
                }

                /************************************************************************************************************************/

                /// <summary>Draws the GUI of the state at the specified 'index'.</summary>
                protected virtual void DoElementGUI(Rect area, int index,
                    SerializedProperty clip, SerializedProperty speed)
                {
                    Rect stateArea, speedArea;
                    SplitListRect(area, out stateArea, out speedArea);

                    DoElementGUI(stateArea, speedArea, index, clip, speed);
                }

                /// <summary>Draws the GUI of the state at the specified 'index'.</summary>
                protected void DoElementGUI(Rect stateArea, Rect speedArea, int index,
                    SerializedProperty clip, SerializedProperty speed)
                {
                    EditorGUI.PropertyField(stateArea, clip, GUIContent.none);

                    if (speed != null)
                    {
                        EditorGUI.PropertyField(speedArea, speed, GUIContent.none);
                    }
                    else
                    {
                        var value = EditorGUI.FloatField(speedArea, 1);
                        if (value != 1)
                        {
                            CurrentSpeeds.InsertArrayElementAtIndex(0);
                            CurrentSpeeds.GetArrayElementAtIndex(0).floatValue = 1;
                            CurrentSpeeds.arraySize = CurrentClips.arraySize;
                            CurrentSpeeds.GetArrayElementAtIndex(index).floatValue = value;
                        }
                    }
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Called when adding a new state to the list to ensure that any other relevant arrays have new
                /// elements added as well.
                /// </summary>
                protected virtual void OnAddElement(ReorderableList list)
                {
                    var index = CurrentClips.arraySize;
                    CurrentClips.InsertArrayElementAtIndex(index);

                    if (CurrentSpeeds.arraySize > 0)
                        CurrentSpeeds.InsertArrayElementAtIndex(index);
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Called when removing a state from the list to ensure that any other relevant arrays have elements
                /// removed as well.
                /// </summary>
                protected virtual void OnRemoveElement(ReorderableList list)
                {
                    var index = list.index;

                    RemoveArrayElement(CurrentClips, index);

                    if (CurrentSpeeds.arraySize > 0)
                        RemoveArrayElement(CurrentSpeeds, index);
                }

                /// <summary>
                /// Removes the specified array element from the 'property'.
                /// <para></para>
                /// If the element isn't at its default value, the first call to
                /// <see cref="SerializedProperty.DeleteArrayElementAtIndex"/> will only reset it, so this method will
                /// call it again if necessary to ensure that it actually gets removed.
                /// </summary>
                protected static void RemoveArrayElement(SerializedProperty property, int index)
                {
                    var count = property.arraySize;
                    property.DeleteArrayElementAtIndex(index);
                    if (property.arraySize == count)
                        property.DeleteArrayElementAtIndex(index);
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Called when reordering states in the list to ensure that any other relevant arrays have their
                /// corresponding elements reordered as well.
                /// </summary>
                protected virtual void OnReorderList(ReorderableList list, int oldIndex, int newIndex)
                {
                    CurrentSpeeds.MoveArrayElement(oldIndex, newIndex);
                }

#if !UNITY_2018_1_OR_NEWER
                private int _SelectedIndex;

                private void OnListSelectionChanged(ReorderableList list)
                {
                    _SelectedIndex = list.index;
                }

                private void OnReorderList(ReorderableList list)
                {
                    OnReorderList(list, _SelectedIndex, list.index);
                }
#endif

                /************************************************************************************************************************/
                #region Speeds
                /************************************************************************************************************************/

                /// <summary>
                /// Initialises every element in the <see cref="CurrentSpeeds"/> array from the 'start' to the end of
                /// the array to contain a value of 1.
                /// </summary>
                protected static void InitialiseSpeeds(int start)
                {
                    var count = CurrentSpeeds.arraySize;
                    while (start < count)
                        CurrentSpeeds.GetArrayElementAtIndex(start++).floatValue = 1;

                }

                /************************************************************************************************************************/

                /// <summary>
                /// If every element in the <see cref="CurrentSpeeds"/> array is 1, this method sets the array size to 0.
                /// </summary>
                protected void TryCollapseSpeeds()
                {
                    var speedCount = CurrentSpeeds.arraySize;
                    if (speedCount <= 0)
                        return;

                    for (int i = 0; i < speedCount; i++)
                    {
                        if (CurrentSpeeds.GetArrayElementAtIndex(i).floatValue != 1)
                            return;
                    }

                    CurrentSpeeds.arraySize = 0;
                }

                /************************************************************************************************************************/
                #endregion
                /************************************************************************************************************************/
                #region Context Menu
                /************************************************************************************************************************/

                /// <summary>
                /// Fills the 'menu' with functions relevant to the 'rootProperty'.
                /// </summary>
                protected override void BuildContextMenu(GenericMenu menu, SerializedProperty rootProperty)
                {
                    base.BuildContextMenu(menu, rootProperty);

                    AddPropertyModifierFunction(menu, "Reset Speeds", (property) => CurrentSpeeds.arraySize = 0);
                    AddPropertyModifierFunction(menu, "Normalize Durations", NormalizeDurations);
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Recalculates the <see cref="CurrentSpeeds"/> depending on the <see cref="AnimationClip.length"/> of
                /// their animations so that they all take the same amount of time to play fully.
                /// </summary>
                private void NormalizeDurations(SerializedProperty property)
                {
                    var speedCount = CurrentSpeeds.arraySize;

                    var lengths = new float[CurrentClips.arraySize];
                    if (lengths.Length <= 1)
                        return;

                    int nonZeroLengths = 0;
                    float totalLength = 0;
                    float totalSpeed = 0;
                    for (int i = 0; i < lengths.Length; i++)
                    {
                        var clip = CurrentClips.GetArrayElementAtIndex(i).objectReferenceValue as AnimationClip;
                        if (clip != null && clip.length > 0)
                        {
                            nonZeroLengths++;
                            totalLength += clip.length;
                            lengths[i] = clip.length;

                            if (speedCount > 0)
                                totalSpeed += CurrentSpeeds.GetArrayElementAtIndex(i).floatValue;
                        }
                    }

                    if (nonZeroLengths == 0)
                        return;

                    var averageLength = totalLength / nonZeroLengths;
                    var averageSpeed = speedCount > 0 ? totalSpeed / nonZeroLengths : 1;

                    CurrentSpeeds.arraySize = lengths.Length;
                    InitialiseSpeeds(speedCount);

                    for (int i = 0; i < lengths.Length; i++)
                    {
                        if (lengths[i] == 0)
                            continue;

                        CurrentSpeeds.GetArrayElementAtIndex(i).floatValue = averageSpeed * lengths[i] / averageLength;
                    }

                    TryCollapseSpeeds();
                }

                /************************************************************************************************************************/

                /// <summary>
                /// Adds a menu function that will call <see cref="GatherSubProperties"/> then the specified 'function'.
                /// </summary>
                protected void AddPropertyModifierFunction(GenericMenu menu, string label, Action<SerializedProperty> function)
                {
                    Editor.AnimancerEditorUtilities.AddPropertyModifierFunction(menu, CurrentProperty, label, (property) =>
                    {
                        GatherSubProperties(property);
                        function(property);
                    });
                }

                /************************************************************************************************************************/
                #endregion
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
