//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月23日 14:03:00
//------------------------------------------------------------

using System;
using ProtoBuf;
using Sirenix.OdinInspector;

namespace ET
{
    [HideLabel]
    [HideReferenceObjectPicker]
    [ProtoContract]
    public class NP_BBValue_Int: NP_BBValueBase<int>, IEquatable<NP_BBValue_Int>
    {
        public override Type NP_BBValueType
        {
            get
            {
                return typeof (int);
            }
        }

        #region 对比函数

        public bool Equals(NP_BBValue_Int other)
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

            return Equals((NP_BBValue_Int) obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static bool operator ==(NP_BBValue_Int lhs, NP_BBValue_Int rhs)
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

        public static bool operator !=(NP_BBValue_Int lhs, NP_BBValue_Int rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >(NP_BBValue_Int lhs, NP_BBValue_Int rhs)
        {
            return lhs.GetValue() > rhs.GetValue();
        }

        public static bool operator <(NP_BBValue_Int lhs, NP_BBValue_Int rhs)
        {
            return lhs.GetValue() < rhs.GetValue();
        }

        public static bool operator >=(NP_BBValue_Int lhs, NP_BBValue_Int rhs)
        {
            return lhs.GetValue() >= rhs.GetValue();
        }

        public static bool operator <=(NP_BBValue_Int lhs, NP_BBValue_Int rhs)
        {
            return lhs.GetValue() <= rhs.GetValue();
        }

        #endregion
        #region proto序列化支持

        [ProtoMember(1)] private int ValueForProtoSerilize;

        [ProtoBeforeSerialization]
        private void HandleBeforSerilize()
        {
            ValueForProtoSerilize = Value;
        }

        [ProtoAfterDeserialization]
        private void HandleAfterSerilize()
        {
            Value = ValueForProtoSerilize;
        }

        #endregion
        
    }
}