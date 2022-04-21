using UnityEngine;
using System.Collections;
using System.Linq;
using Werewolf.StatusIndicators.Services;

namespace Werewolf.StatusIndicators.Components
{
    public class StatusIndicator: Splat
    {
        public int ProgressSteps;

        public override void OnProgressChanged(float changedValue)
        {
            if (ProgressSteps == 0)
            {
                SetShaderFloat("_Fill", changedValue);
            }
            else
            {
                SetShaderFloat("_Fill", StepProgress());
            }
        }

        /// <summary>
        /// For a staggered fill, such as dotted circles.
        /// </summary>
        private float StepProgress()
        {
            float stepSize = 1.0f / ProgressSteps;
            int currentStep = Mathf.RoundToInt(progress / stepSize);
            return (currentStep * stepSize) - (stepSize / 15);
        }
    }
}