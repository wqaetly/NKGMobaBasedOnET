//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月23日 14:06:59
//------------------------------------------------------------

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ETModel.BBValues
{
    [HideLabel]
    [HideReferenceObjectPicker]
    public class NP_BBValue_Vector3: NP_BBValueBase<Vector3>, IEquatable<NP_BBValue_Vector3>
    {
        public override Type NP_BBValueType
        {
            get
            {
                return typeof (Vector3);
            }
        }

        #region 对比函数

        public bool Equals(NP_BBValue_Vector3 other)
        {
            // If parameter is null, return false.
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return this.Value == other.GetValue();
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

            return Equals((NP_BBValue_Vector3) obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static bool operator ==(NP_BBValue_Vector3 lhs, NP_BBValue_Vector3 rhs)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
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

        public static bool operator !=(NP_BBValue_Vector3 lhs, NP_BBValue_Vector3 rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >(NP_BBValue_Vector3 lhs, NP_BBValue_Vector3 rhs)
        {
            return false;
        }

        public static bool operator <(NP_BBValue_Vector3 lhs, NP_BBValue_Vector3 rhs)
        {
            return false;
        }

        public static bool operator >=(NP_BBValue_Vector3 lhs, NP_BBValue_Vector3 rhs)
        {
            return false;
        }

        public static bool operator <=(NP_BBValue_Vector3 lhs, NP_BBValue_Vector3 rhs)
        {
            return false;
        }

        #endregion
    }
}