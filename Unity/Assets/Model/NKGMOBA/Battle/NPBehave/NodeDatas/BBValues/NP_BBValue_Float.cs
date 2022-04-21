//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月23日 15:38:54
//------------------------------------------------------------

using System;
using ProtoBuf;
using Sirenix.OdinInspector;

namespace ET
{
    [HideLabel]
    [HideReferenceObjectPicker]
    [ProtoContract]
    public class NP_BBValue_Float: NP_BBValueBase<float>, IEquatable<NP_BBValue_Float>
    {
        public override Type NP_BBValueType
        {
            get
            {
                return typeof (float);
            }
        }

        #region 对比函数

        public bool Equals(NP_BBValue_Float other)
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
            return Math.Abs(this.Value - other.GetValue()) < 0.0001f;
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

            return Equals((NP_BBValue_Float) obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static bool operator ==(NP_BBValue_Float lhs, NP_BBValue_Float rhs)
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

        public static bool operator !=(NP_BBValue_Float lhs, NP_BBValue_Float rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >(NP_BBValue_Float lhs, NP_BBValue_Float rhs)
        {
            return lhs.GetValue() > rhs.GetValue();
        }

        public static bool operator <(NP_BBValue_Float lhs, NP_BBValue_Float rhs)
        {
            return lhs.GetValue() < rhs.GetValue();
        }

        public static bool operator >=(NP_BBValue_Float lhs, NP_BBValue_Float rhs)
        {
            return lhs.GetValue() >= rhs.GetValue();
        }

        public static bool operator <=(NP_BBValue_Float lhs, NP_BBValue_Float rhs)
        {
            return lhs.GetValue() <= rhs.GetValue();
        }

        #endregion

        #region proto序列化支持

        [ProtoMember(1)] private float ValueForProtoSerilize;

        [ProtoBeforeSerialization]
        private void HandleBeforSerilize()
        {
            ValueForProtoSerilize = Value;
        }

        [ProtoAfterDeserialization]
        private void HandleAfterSerilize()
        {
            Value = (float)ValueForProtoSerilize;
        }

        #endregion
    }
}