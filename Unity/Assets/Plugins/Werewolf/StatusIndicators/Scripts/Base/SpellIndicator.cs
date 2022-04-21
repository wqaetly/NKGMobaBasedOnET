using UnityEngine;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using Werewolf.StatusIndicators.Services;

namespace Werewolf.StatusIndicators.Components
{
    public class SpellIndicator : Splat
    {
        // Fields

        /// <summary>
        /// Special indicator for displaying range, unselectable.
        /// 当技能指示器出现时，用于额外显示一个圆形的范围指示器
        /// </summary>
        [SerializeField] public RangeIndicator RangeIndicator;

        /// <summary>
        /// Set the size of the Range Indicator and bounds of Spell Cursor.
        /// </summary>
        [SerializeField] [OnValueChanged("OnRangeChanged")]
        protected float range = 1f;

        /// <summary>
        /// Set the size of the Range Indicator and bounds of Spell Cursor.
        /// </summary>
        public float Range
        {
            get { return range; }
            set
            {
                range = value;
                SetRange();
            }
        }

#if UNITY_EDITOR
        public void OnRangeChanged()
        {
            Range = range;
        }
#endif

        /// <summary>
        /// Set the size of the Range Indicator and bounds of Spell Cursor.
        /// </summary>
        public void SetRange()
        {
            if (RangeIndicator != null)
            {
                RangeIndicator.Width = range;
                RangeIndicator.Length = range;
            }
        }

        /// <summary>
        /// Get the vector that is on the same y position as the subject to get a more accurate angle.
        /// TODO 设置基准为指定英雄
        /// </summary>
        /// <param name="target">The target point which we are trying to adjust against</param>
        protected Vector3 FlattenVector(Vector3 target)
        {
            return new Vector3(target.x, 0, target.z);
        }

        protected Quaternion CalculateQuaWithAlineHeight(Vector3 hitPoint, Vector3 selfPostion, float standardHeight)
        {
            return Quaternion.LookRotation(new Vector3(hitPoint.x, standardHeight, hitPoint.z) -
                                           new Vector3(selfPostion.x, standardHeight, selfPostion.z));
        }
    }
}