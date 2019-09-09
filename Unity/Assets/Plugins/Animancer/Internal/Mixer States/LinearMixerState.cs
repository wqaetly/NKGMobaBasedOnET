// Animancer // Copyright 2019 Kybernetik //

using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// An <see cref="AnimancerState"/> which blends an array of other states together using linear interpolation
    /// between the specified thresholds.
    /// <para></para>
    /// This mixer type is similar to the 1D Blend Type in Mecanim Blend Trees.
    /// </summary>
    public sealed class LinearMixerState : MixerState<float>
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="LinearMixerState"/> without connecting it to the <see cref="PlayableGraph"/>.
        /// You must call <see cref="AnimancerState.SetParent(AnimancerLayer)"/> or it won't actually do anything.
        /// </summary>
        private LinearMixerState(AnimancerPlayable root) : base(root) { }

        /// <summary>
        /// Constructs a new <see cref="LinearMixerState"/> and connects it to the 'layer's
        /// <see cref="IAnimationMixer.Playable"/> using a spare port if there are any from previously destroyed
        /// states, or by adding a new port.
        /// </summary>
        public LinearMixerState(AnimancerLayer layer) : base(layer) { }

        /// <summary>
        /// Constructs a new <see cref="LinearMixerState"/> and connects it to the 'parent's
        /// <see cref="IAnimationMixer.Playable"/> at the specified 'portIndex'.
        /// </summary>
        public LinearMixerState(AnimancerNode parent, int portIndex) : base(parent, portIndex) { }

        /************************************************************************************************************************/

        /// <summary>
        /// Initialises the <see cref="Mixer"/> and <see cref="States"/> with one state per clip and assigns thresholds
        /// evenly spaced between the specified min and max (inclusive).
        /// </summary>
        public void Initialise(AnimationClip[] clips, float minThreshold = 0, float maxThreshold = 1)
        {
            Initialise(clips);
            AssignLinearThresholds(minThreshold, maxThreshold);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Initialises the <see cref="Mixer"/> with two ports and connects two states to them for the specified clips
        /// at the specified thresholds (default 0 and 1).
        /// </summary>
        public void Initialise(AnimationClip clip0, AnimationClip clip1, float threshold0 = 0, float threshold1 = 1)
        {
            _Playable.SetInputCount(2);

            States = new AnimancerState[2];
            new ClipState(this, 0, clip0);
            new ClipState(this, 1, clip1);

            SetThresholds(new float[]
            {
                threshold0,
                threshold1,
            });
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Initialises the <see cref="Mixer"/> with three ports and connects three states to them for the specified
        /// clips at the specified thresholds (default -1, 0, and 1).
        /// </summary>
        public void Initialise(AnimationClip clip0, AnimationClip clip1, AnimationClip clip2, float threshold0 = -1, float threshold1 = 0, float threshold2 = 1)
        {
            _Playable.SetInputCount(3);

            States = new AnimancerState[3];
            new ClipState(this, 0, clip0);
            new ClipState(this, 1, clip1);
            new ClipState(this, 2, clip2);

            SetThresholds(new float[]
            {
                threshold0,
                threshold1,
                threshold2,
            });
        }

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>
        /// Called whenever the thresholds are changed. In the Unity Editor this method calls
        /// <see cref="AssertThresholdsSorted"/> then <see cref="RecalculateWeights"/> while at runtime it only calls
        /// the latter.
        /// </summary>
        public override void OnThresholdsChanged()
        {
            AssertThresholdsSorted();

            base.OnThresholdsChanged();
        }
#endif

        /************************************************************************************************************************/

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> unless the thresholds are sorted from lowest to highest.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public void AssertThresholdsSorted()
        {
            if (!HasThresholds())
                throw new InvalidOperationException("LinearAnimationMixer: no Thresholds have been assigned");

            var previous = float.NegativeInfinity;

            int count = States.Length;
            for (int i = 0; i < count; i++)
            {
                var state = States[i];
                if (state == null)
                    continue;

                float next = GetThreshold(i);
                if (next > previous)
                    previous = next;
                else
                    throw new ArgumentException("LinearAnimationMixer: Thresholds are out of order. They must be sorted from lowest to highest with no equal values.");
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Recalculates the weights of all <see cref="States"/> based on the current value of the
        /// <see cref="Parameter"/> and the thresholds.
        /// </summary>
        public override void RecalculateWeights()
        {
            AreWeightsDirty = false;

            // Go through all states, figure out how much weight to give those with thresholds adjacent to the
            // current parameter value using linear interpolation, and set all others to 0 weight.

            int index = 0;
            var previousState = GetNextState(ref index);
            if (previousState == null)
                return;

            float previousThreshold = GetThreshold(index);

            if (Parameter <= previousThreshold)
            {
                previousState.Weight = 1;
                DisableRemainingStates(index);
                return;
            }

            var count = States.Length;
            while (++index < count)
            {
                var nextState = GetNextState(ref index);
                if (nextState == null)
                    break;

                float nextThreshold = GetThreshold(index);

                if (Parameter > previousThreshold && Parameter <= nextThreshold)
                {
                    float t = (Parameter - previousThreshold) / (nextThreshold - previousThreshold);
                    previousState.Weight = 1 - t;
                    nextState.Weight = t;
                    DisableRemainingStates(index);
                    return;
                }
                else
                {
                    previousState.Weight = 0;
                }

                previousState = nextState;
                previousThreshold = nextThreshold;
            }

            previousState.Weight = Parameter > previousThreshold ? 1 : 0;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Assigns the <see cref="Thresholds"/> to be evenly spaced between the specified min and max (inclusive).
        /// </summary>
        public void AssignLinearThresholds(float min = 0, float max = 1)
        {
            int count = States.Length;

            var thresholds = new float[count];

            float increment = (max - min) / (count - 1);

            for (int i = 0; i < count; i++)
            {
                thresholds[i] =
                    i < count - 1 ?
                    min + i * increment :// Assign each threshold linearly spaced between the min and max.
                    max;// and ensure that the last one is exactly at the max (to avoid floating-point error).
            }

            SetThresholds(thresholds);
        }

        /************************************************************************************************************************/
        #region Inspector
        /************************************************************************************************************************/

        /// <summary>The number of parameters being managed by this state.</summary>
        protected override int ParameterCount { get { return 1; } }

        /// <summary>Returns the name of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected override string GetParameterName(int index) { return "Parameter"; }

        /// <summary>Returns the type of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected override AnimatorControllerParameterType GetParameterType(int index) { return AnimatorControllerParameterType.Float; }

        /// <summary>Returns the value of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected override object GetParameterValue(int index) { return Parameter; }

        /// <summary>Sets the value of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected override void SetParameterValue(int index, object value) { Parameter = (float)value; }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Serializable
        /************************************************************************************************************************/

        /// <summary>
        /// A serializable object which can create a <see cref="LinearMixerState"/> when passed into
        /// <see cref="AnimancerPlayable.Transition"/>.
        /// </summary>
        [Serializable]
        public new class Serializable : Serializable<LinearMixerState, float>
        {
            /************************************************************************************************************************/

            /// <summary>
            /// Creates and returns a new <see cref="LinearMixerState"/> connected to the 'layer'.
            /// <para></para>
            /// This method also assigns it as the <see cref="AnimancerState.Serializable{TState}.State"/>.
            /// </summary>
            public override LinearMixerState CreateState(AnimancerLayer layer)
            {
                State = new LinearMixerState(layer);
                InitialiseState();
                return State;
            }

            /************************************************************************************************************************/
            #region Drawer
#if UNITY_EDITOR
            /************************************************************************************************************************/

            /// <summary>[Editor-Only] Draws the inspector GUI for a <see cref="Serializable"/>.</summary>
            [UnityEditor.CustomPropertyDrawer(typeof(Serializable), true)]
            public new class Drawer : Serializable<LinearMixerState, float>.Drawer
            {
                /************************************************************************************************************************/

                /// <summary>
                /// Fills the 'menu' with functions relevant to the 'rootProperty'.
                /// </summary>
                protected override void BuildContextMenu(UnityEditor.GenericMenu menu, UnityEditor.SerializedProperty rootProperty)
                {
                    base.BuildContextMenu(menu, rootProperty);

                    AddPropertyModifierFunction(menu, "Calculate Thresholds/Evenly Spaced", (property) =>
                    {
                        var count = CurrentThresholds.arraySize;
                        if (count <= 1)
                            return;

                        var first = CurrentThresholds.GetArrayElementAtIndex(0).floatValue;
                        var last = CurrentThresholds.GetArrayElementAtIndex(count - 1).floatValue;
                        for (int i = 0; i < count; i++)
                        {
                            CurrentThresholds.GetArrayElementAtIndex(i).floatValue = Mathf.Lerp(first, last, i / (float)(count - 1));
                        }
                    });

                    AddCalculateThresholdsFunction(menu, "Calculate Thresholds/From Speed",
                        (clip, threshold) => clip.apparentSpeed);
                    AddCalculateThresholdsFunction(menu, "Calculate Thresholds/From Velocity X",
                        (clip, threshold) => clip.averageSpeed.x);
                    AddCalculateThresholdsFunction(menu, "Calculate Thresholds/From Velocity Y",
                        (clip, threshold) => clip.averageSpeed.z);
                    AddCalculateThresholdsFunction(menu, "Calculate Thresholds/From Velocity Z",
                        (clip, threshold) => clip.averageSpeed.z);
                    AddCalculateThresholdsFunction(menu, "Calculate Thresholds/From Angular Speed (Rad)",
                        (clip, threshold) => clip.averageAngularSpeed * Mathf.Deg2Rad);
                    AddCalculateThresholdsFunction(menu, "Calculate Thresholds/From Angular Speed (Deg)",
                        (clip, threshold) => clip.averageAngularSpeed);
                }

                /************************************************************************************************************************/

                private void AddCalculateThresholdsFunction(UnityEditor.GenericMenu menu, string label,
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

                            threshold.floatValue = calculateThreshold(clip, threshold.floatValue);
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
