// Animancer // Copyright 2019 Kybernetik //

using System;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// An <see cref="AnimancerState"/> which blends an array of other states together based on a two dimensional
    /// parameter and thresholds using Polar Gradient Band Interpolation.
    /// <para></para>
    /// This mixer type is similar to the 2D Freeform Directional Blend Type in Mecanim Blend Trees.
    /// </summary>
    public class DirectionalMixerState : MixerState<Vector2>
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Constructs a new <see cref="DirectionalMixerState"/> without connecting it to the <see cref="PlayableGraph"/>.
        /// You must call <see cref="AnimancerState.SetParent(AnimancerLayer)"/> or it won't actually do anything.
        /// </summary>
        protected DirectionalMixerState(AnimancerPlayable root) : base(root) { }

        /// <summary>
        /// Constructs a new <see cref="DirectionalMixerState"/> and connects it to the 'layer's
        /// <see cref="IAnimationMixer.Playable"/> using a spare port if there are any from previously destroyed
        /// states, or by adding a new port.
        /// </summary>
        public DirectionalMixerState(AnimancerLayer layer) : base(layer) { }

        /// <summary>
        /// Constructs a new <see cref="DirectionalMixerState"/> and connects it to the 'parent's
        /// <see cref="IAnimationMixer.Playable"/> at the specified 'portIndex'.
        /// </summary>
        public DirectionalMixerState(AnimancerNode parent, int portIndex) : base(parent, portIndex) { }

        /************************************************************************************************************************/

        /// <summary>Precalculated magnitudes of all thresholds to speed up the recalculation of weights.</summary>
        private float[] _ThresholdMagnitudes;

        /// <summary>Precalculated values to speed up the recalculation of weights.</summary>
        private Vector2[][] _BlendFactors;

        /// <summary>Indicates whether the <see cref="_BlendFactors"/> need to be recalculated.</summary>
        private bool _BlendFactorsDirty = true;

        /// <summary>The multiplier that controls how much an angle (in radians) is worth compared to normalized distance.</summary>
        private const float AngleFactor = 2;

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
        /// <see cref="Parameter"/> and the thresholds.
        /// </summary>
        public override void RecalculateWeights()
        {
            AreWeightsDirty = false;

            CalculateBlendFactors();

            float parameterMagnitude = Parameter.magnitude;
            float totalWeight = 0;

            int portCount = PortCount;
            for (int i = 0; i < portCount; i++)
            {
                var state = States[i];
                if (state == null)
                    continue;

                var blendFactors = _BlendFactors[i];

                var thresholdI = GetThreshold(i);
                float magnitudeI = _ThresholdMagnitudes[i];

                // Convert the threshold to polar coordinates (distance, angle) and interpolate the weight based on those.
                float differenceIToParameter = parameterMagnitude - magnitudeI;
                float angleIToParameter = SignedAngle(thresholdI, Parameter) * AngleFactor;

                float weight = 1;

                for (int j = 0; j < portCount; j++)
                {
                    if (j == i || States[j] == null)
                        continue;

                    float magnitudeJ = _ThresholdMagnitudes[j];
                    float averageMagnitude = (magnitudeJ + magnitudeI) * 0.5f;

                    var polarIToParameter = new Vector2(
                        differenceIToParameter / averageMagnitude,
                        angleIToParameter);

                    float newWeight = 1 - Vector2.Dot(polarIToParameter, blendFactors[j]);

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
                _ThresholdMagnitudes = new float[portCount];

                _BlendFactors = new Vector2[portCount][];
                for (int i = 0; i < portCount; i++)
                    _BlendFactors[i] = new Vector2[portCount];
            }

            // Calculate the magnitude of each threshold.
            for (int i = 0; i < portCount; i++)
            {
                _ThresholdMagnitudes[i] = GetThreshold(i).magnitude;
            }

            // Calculate the blend factors between each combination of thresholds.
            for (int i = 0; i < portCount; i++)
            {
                var blendFactors = _BlendFactors[i];

                var thresholdI = GetThreshold(i);
                float magnitudeI = _ThresholdMagnitudes[i];

                int j = 0;// i + 1;
                for (; j < portCount; j++)
                {
                    if (i == j)
                        continue;

                    var thresholdJ = GetThreshold(j);
                    float magnitudeJ = _ThresholdMagnitudes[j];

                    float averageMagnitude = (magnitudeI + magnitudeJ) * 0.5f;

                    // Convert the thresholds to polar coordinates (distance, angle) and interpolate the weight based on those.

                    float differenceIToJ = magnitudeJ - magnitudeI;
                    float angleIToJ = SignedAngle(thresholdI, thresholdJ);

                    var polarIToJ = new Vector2(
                        differenceIToJ / averageMagnitude,
                        angleIToJ * AngleFactor);

                    polarIToJ *= 1f / polarIToJ.sqrMagnitude;

                    // Each factor is used in [i][j] with it's opposite in [j][i].
                    blendFactors[j] = polarIToJ;
                    _BlendFactors[j][i] = -polarIToJ;
                }
            }
        }

        /************************************************************************************************************************/

        private static float SignedAngle(Vector2 a, Vector2 b)
        {
            // If either vector is exactly at the origin, the angle is 0.
            if ((a.x == 0 && a.y == 0) || (b.x == 0 && b.y == 0))
            {
                // Due to floating point error "Mathf.Atan2(0 * b.y - 0 * b.x, 0 * b.x + 0 * b.y);" is usually 0 but
                // sometimes Pi, which screws up our other calculations so we need it to always be 0 properly.
                return 0;
            }

            return Mathf.Atan2(
                a.x * b.y - a.y * b.x,
                a.x * b.x + a.y * b.y);
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
