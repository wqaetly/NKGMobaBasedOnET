using System;
using ET;

namespace NPBehave
{
    public class FrameAction : IReference, IEquatable<FrameAction>
    {
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Action != null ? Action.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) IntervalFrame;
                hashCode = (hashCode * 397) ^ RepeatTime;
                hashCode = (hashCode * 397) ^ (int) TargetTickFrame;
                return hashCode;
            }
        }

        public long Id;
        public System.Action Action;
        
        /// <summary>
        /// 间隔帧，为0代表每帧都执行
        /// </summary>
        public uint IntervalFrame = 0;
        
        /// <summary>
        /// 重复次数，默认为1，只执行一次，如果为-1代表无限执行，直到手动移除
        /// </summary>
        public int RepeatTime = 1;

        /// <summary>
        /// 目标触发帧，将在这一帧进行callBack
        /// </summary>
        public uint TargetTickFrame = 0;

        public void Clear()
        {
            this.Id = 0;
            this.IntervalFrame = 0;
            this.Action = null;
            this.RepeatTime = 1;
            this.TargetTickFrame = 0;
        }

        #region 对比函数

        public bool Equals(FrameAction other)
        {
            // If parameter is null, return false.
            if (System.Object.ReferenceEquals(other, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (System.Object.ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            return this.Id == other.Id && IntervalFrame == other.IntervalFrame && this.RepeatTime == other.RepeatTime;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((FrameAction) obj);
        }

        public static bool operator ==(FrameAction lhs, FrameAction rhs)
        {
            // Check for null on left side.
            if (System.Object.ReferenceEquals(lhs, null))
            {
                if (System.Object.ReferenceEquals(rhs, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }

            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(FrameAction lhs, FrameAction rhs)
        {
            return !(lhs == rhs);
        }

        #endregion
    }
}