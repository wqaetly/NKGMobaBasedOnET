// Animancer // Copyright 2019 Kybernetik //

using System;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// Base class for <see cref="MixerState"/>s which blend an array of <see cref="States"/> together based on a
    /// <see cref="Parameter"/>.
    /// </summary>
    public abstract class MixerState<TParameter> : ManualMixerState
    {
        /************************************************************************************************************************/
        #region Properties
        /************************************************************************************************************************/

        /// <summary>The parameter values at which each of the <see cref="States"/> are used and blended.</summary>
        private TParameter[] _Thresholds;

        /************************************************************************************************************************/

        private TParameter _Parameter;

        /// <summary>The value used to calculate the weights of the <see cref="States"/>.</summary>
        public TParameter Parameter
        {
            get { return _Parameter; }
            set
            {
                _Parameter = value;
                AreWeightsDirty = true;
                RequireUpdate();
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Thresholds
        /************************************************************************************************************************/

        /// <summary>
        /// Returns true if the thresholds array is not null.
        /// </summary>
        public bool HasThresholds()
        {
            return _Thresholds != null;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the value of the threshold associated with the specified portIndex.
        /// </summary>
        public TParameter GetThreshold(int portIndex)
        {
            return _Thresholds[portIndex];
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Sets the value of the threshold associated with the specified portIndex.
        /// </summary>
        public void SetThreshold(int portIndex, TParameter threshold)
        {
            _Thresholds[portIndex] = threshold;
            OnThresholdsChanged();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Assigns the specified array as the thresholds to use for blending.
        /// <para></para>
        /// WARNING: if you keep a reference to the 'thresholds' array you must call <see cref="OnThresholdsChanged"/>
        /// whenever any changes are made to it, otherwise this mixer may not blend correctly.
        /// </summary>
        public void SetThresholds(TParameter[] thresholds)
        {
            if (thresholds.Length != States.Length)
                throw new ArgumentOutOfRangeException("thresholds", "Incorrect threshold count. There are " + States.Length +
                    " states, but the specified thresholds array contains " + thresholds.Length + " elements.");

            _Thresholds = thresholds;
            OnThresholdsChanged();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// If the <see cref="_Thresholds"/> don't have the same <see cref="Array.Length"/> as the
        /// <see cref="States"/>, this method allocates and assigns a new array of that size.
        /// </summary>
        public bool ValidateThresholdCount()
        {
            if (States == null)
                return false;

            if (_Thresholds == null || _Thresholds.Length != States.Length)
            {
                _Thresholds = new TParameter[States.Length];
                return true;
            }

            return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called whenever the thresholds are changed. By default this method simply indicates that the blend weights
        /// need recalculating but it can be overridden by child classes to perform validation checks or optimisations.
        /// </summary>
        public virtual void OnThresholdsChanged()
        {
            AreWeightsDirty = true;
            RequireUpdate();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls 'calculate' for each of the <see cref="States"/> and stores the returned value as the threshold for
        /// that state.
        /// </summary>
        public void CalculateThresholds(Func<AnimancerState, TParameter> calculate)
        {
            ValidateThresholdCount();

            var count = States.Length;
            for (int i = 0; i < count; i++)
            {
                var state = States[i];
                if (state == null)
                    continue;

                _Thresholds[i] = calculate(state);
            }

            OnThresholdsChanged();
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Initialisation
        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="MixerState{T}"/> without connecting it to the <see cref="PlayableGraph"/>.
        /// You must call <see cref="AnimancerState.SetParent(AnimancerLayer)"/> or it won't actually doanything.
        /// </summary>
        protected MixerState(AnimancerPlayable root) : base(root) { }

        /// <summary>
        /// Constructs a new <see cref="MixerState{T}"/> and connects it to the 'layer's
        /// <see cref="IAnimationMixer.Playable"/> using a spare port if there are any from previously destroyed
        /// states, or by adding a new port.
        /// </summary>
        public MixerState(AnimancerLayer layer) : base(layer) { }

        /// <summary>
        /// Constructs a new <see cref="MixerState{T}"/> and connects it to the 'parent's
        /// <see cref="IAnimationMixer.Playable"/> at the specified 'portIndex'.
        /// </summary>
        public MixerState(AnimancerNode parent, int portIndex) : base(parent, portIndex) { }

        /************************************************************************************************************************/

        /// <summary>
        /// Initialises this mixer with the specified number of ports which can be filled individually by <see cref="CreateState"/>.
        /// </summary>
        public override void Initialise(int portCount)
        {
            base.Initialise(portCount);
            _Thresholds = new TParameter[portCount];
            OnThresholdsChanged();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Initialises the <see cref="Mixer"/> and <see cref="States"/> with one state per clip and assigns the
        /// 'thresholds'.
        /// <para></para>
        /// WARNING: if the caller keeps a reference to the 'thresholds' array, it must call
        /// <see cref="OnThresholdsChanged"/> whenever any changes are made to it, otherwise this mixer may not blend
        /// correctly.
        /// </summary>
        public void Initialise(AnimationClip[] clips, TParameter[] thresholds)
        {
            Initialise(clips);
            _Thresholds = thresholds;
            OnThresholdsChanged();
        }

        /// <summary>
        /// Initialises the <see cref="Mixer"/> and <see cref="States"/> with one state per clip and assigns the
        /// thresholds by calling 'calculateThreshold' for each state.
        /// </summary>
        public void Initialise(AnimationClip[] clips, Func<AnimancerState, TParameter> calculateThreshold)
        {
            Initialise(clips);
            CalculateThresholds(calculateThreshold);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Creates and returns a new <see cref="ClipState"/> to play the 'clip' with this
        /// <see cref="MixerState"/> as its parent, connects it to the specified 'portIndex', and assigns the
        /// 'threshold' for it.
        /// </summary>
        public ClipState CreateState(int portIndex, AnimationClip clip, TParameter threshold)
        {
            SetThreshold(portIndex, threshold);
            return CreateState(portIndex, clip);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Descriptions
        /************************************************************************************************************************/

        /// <summary>Gets a user-friendly key to identify the 'state' in the inspector.</summary>
        public override string GetDisplayKey(AnimancerState state)
        {
            return string.Concat("[", state.PortIndex.ToString(), "] ", _Thresholds[state.PortIndex].ToString());
        }

        /************************************************************************************************************************/

        /// <summary>Appends a detailed descrption of the current details of this state.</summary>
        public override void AppendDescription(StringBuilder description, bool includeClip, bool includeChildStates = true, string delimiter = "\n")
        {
            description.Append("Parameter: ");
            AppendParameter(description);
            description.Append(delimiter);

            base.AppendDescription(description, includeClip, includeChildStates, delimiter);
        }

        /************************************************************************************************************************/

        /// <summary>Appends the current parameter value of this mixer.</summary>
        public virtual void AppendParameter(StringBuilder description)
        {
            description.Append(Parameter);
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
