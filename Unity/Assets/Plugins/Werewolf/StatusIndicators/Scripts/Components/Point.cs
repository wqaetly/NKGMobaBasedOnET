using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Werewolf.StatusIndicators.Services;

namespace Werewolf.StatusIndicators.Components
{
    public class Point: SpellIndicator
    {
        /// <summary>
        /// Determine if you want the Splat to be restricted to the Range Indicator bounds. Applies to "Point" Splats only.
        /// </summary>
        [SerializeField]
        public bool IsRestrictCursorToRange = false;

        /// <summary>
        /// 跟随鼠标的Transforms
        /// </summary>
        public List<Transform> FollowCursorTransforms = new List<Transform>();

        /// <summary>
        /// Restrict splat position bound to range from player
        /// </summary>
        private void RestrictCursorToRange(Vector3 cursorWorldPos)
        {
            var centerPos = this.SplatBelongToUnit.position;

            foreach (var followCursorTransform in FollowCursorTransforms)
            {
                followCursorTransform.position = centerPos +
                        Vector3.ClampMagnitude(cursorWorldPos - centerPos, this.range);
            }
        }

        public override void Update()
        {
            if (IsRestrictCursorToRange)
            {
                RestrictCursorToRange(SplatManager.Get3DMousePosition());
            }
        }
    }
}