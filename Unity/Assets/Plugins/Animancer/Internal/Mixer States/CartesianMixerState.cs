// Animancer // Copyright 2019 Kybernetik //

using System;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// An <see cref="AnimancerState"/> which blends an array of other states together based on a two dimensional
    /// parameter and thresholds using Gradient Band Interpolation.
    /// <para></para>
    /// This mixer type is similar to the 2D Freeform Cartesian Blend Type in Mecanim Blend Trees.
    /// </summary>
    public class CartesianMixerState : MixerState<Vector2>
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="CartesianMixerState"/> without connecting it to the <see cref="PlayableGraph"/>.
        /// You must call <see cref="AnimancerState.SetParent(AnimancerLayer)"/> or it won't actually do anything.
        /// </summary>
        protected CartesianMixerState(AnimancerPlayable root) : base(root) { }

        /// <summary>
        /// Constructs a new <see cref="CartesianMixerState"/> and connects it to the 'layer's
        /// <see cref="IAnimationMixer.Playable"/> using a spare port if there are any from previously destroyed
        /// states, or by adding a new port.
        /// </summary>
        public CartesianMixerState(AnimancerLayer layer) : base(layer) { }

        /// <summary>
        /// Constructs a new <see cref="CartesianMixerState"/> and connects it to the 'parent's
        /// <see cref="IAnimationMixer.Playable"/> at the specified 'portIndex'.
        /// </summary>
        public CartesianMixerState(AnimancerNode parent, int portIndex) : base(parent, portIndex) { }

        /************************************************************************************************************************/

        /// <summary>Precalculated values to speed up the recalculation of weights.</summary>
        private Vector2[][] _BlendFactors;

        /// <summary>Indicates whether the <see cref="_BlendFactors"/> need to be recalculated.</summary>
        private bool _BlendFactorsDirty = true;

        /************************************************************************************************************************/

        /// <summary>Gets or sets Parameter.x.</summary>
        public float ParameterX
        {
            get { return Parameter.x; }
            set { Parameter = new Vector2(value, Parameter.y); }
        }

        /// <summary>Gets or sets Parameter.y.</summary>
        public float ParameterY
        {
            get { return Parameter.y; }
            set { Parameter = new Vector2(Parameter.x, value); }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Called whenever the thresholds are changed. Indicates that the internal blend factors need to be
        /// recalculated and calls <see cref="RecalculateWeights"/>.
        /// </summary>
        public override void OnThresholdsChanged()
        {
            _BlendFactorsDirty = true;
            base.OnThresholdsChanged();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Recalculates the weights of all <see cref="States"/> based on the current value of the
        /// <see cref="Parameter"/> and the <see cref="Thresholds"/>.
        /// </summary>
        public override void RecalculateWeights()
        {
            AreWeightsDirty = false;

            CalculateBlendFactors();

            int portCount = PortCount;

            float totalWeight = 0;

            for (int i = 0; i < portCount; i++)
            {
                var state = States[i];
                if (state == null)
                    continue;

                var blendFactors = _BlendFactors[i];

                var threshold = GetThreshold(i);
                var thresholdToParameter = Parameter - threshold;

                float weight = 1;

                for (int j = 0; j < portCount; j++)
                {
                    if (j == i || States[j] == null)
                        continue;

                    float newWeight = 1 - Vector2.Dot(thresholdToParameter, blendFactors[j]);

                    if (weight > newWeight)
                        weight = newWeight;
                }

                if (weight < 0)
                    weight = 0;

                state.Weight = weight;
                totalWeight += weight;
            }

            NormalizeWeights(totalWeight);
        }

        /************************************************************************************************************************/

        private void CalculateBlendFactors()
        {
            if (!_BlendFactorsDirty)
                return;

            _BlendFactorsDirty = false;

            int portCount = PortCount;
            if (PortCount <= 1)
                return;

            // Resize the precalculated values.
            if (_BlendFactors == null || _BlendFactors.Length != portCount)
            {
                _BlendFactors = new Vector2[portCount][];
                for (int i = 0; i < portCount; i++)
                    _BlendFactors[i] = new Vector2[portCount];
            }

            // Calculate the blend factors between each combination of thresholds.
            for (int i = 0; i < portCount; i++)
            {
                var blendFactors = _BlendFactors[i];

                var thresholdI = GetThreshold(i);

                int j = i + 1;
                for (; j < portCount; j++)
                {
                    var thresholdIToJ = GetThreshold(j) - thresholdI;

                    thresholdIToJ *= 1f / thresholdIToJ.sqrMagnitude;

                    // Each factor is used in [i][j] with it's opposite in [j][i].
                    blendFactors[j] = thresholdIToJ;
                    _BlendFactors[j][i] = -thresholdIToJ;
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>Appends the current parameter values of this mixer.</summary>
        public override void AppendParameter(StringBuilder description)
        {
            description.Append(ParameterX);
            description.Append(", ");
            description.Append(ParameterY);
        }

        /************************************************************************************************************************/
        #region Inspector
        /************************************************************************************************************************/

        /// <summary>The number of parameters being managed by this state.</summary>
        protected override int ParameterCount { get { return 2; } }

        /// <summary>Returns the name of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected override string GetParameterName(int index)
        {
            switch (index)
            {
                case 0: return "Parameter X";
                case 1: return "Parameter Y";
                default: throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>Returns the type of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected override AnimatorControllerParameterType GetParameterType(int index) { return AnimatorControllerParameterType.Float; }

        /// <summary>Returns the value of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected override object GetParameterValue(int index)
        {
            switch (index)
            {
                case 0: return ParameterX;
                case 1: return ParameterY;
                default: throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>Sets the value of a parameter being managed by this state.</summary>
        /// <exception cref="NotSupportedException">Thrown if this state doesn't manage any parameters.</exception>
        protected override void SetParameterValue(int index, object value)
        {
            switch (index)
            {
                case 0: ParameterX = (float)value; break;
                case 1: ParameterY = (float)value; break;
                default: throw new ArgumentOutOfRangeException("index");
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
