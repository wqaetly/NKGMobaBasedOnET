//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月18日 20:37:53
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class NP_BBValue_List_Long : NP_BBValueBase<List<long>>, IEquatable<NP_BBValue_List_Long>
    {
        public override Type NP_BBValueType
        {
            get { return typeof(List<long>); }
        }

        protected override void SetValueFrom(INP_BBValue<List<long>> bbValue)
        {
            //因为List是引用类型，所以这里要做一下特殊处理，如果要设置的值为0元素的List，就Clear一下，而且这个东西也不会用来做为黑板条件，因为它没办法用来对比
            //否则就拷贝全部元素
            this.Value.Clear();
            foreach (var item in bbValue.GetValue())
            {
                this.Value.Add(item);
            }
        }

        public override void SetValueFrom(List<long> bbValue)
        {
            //因为List是引用类型，所以这里要做一下特殊处理，如果要设置的值为0元素的List，就Clear一下，而且这个东西也不会用来做为黑板条件，因为它没办法用来对比
            //否则就拷贝全部元素
            this.Value.Clear();
            foreach (var item in bbValue)
            {
                this.Value.Add(item);
            }
        }

        #region 对比函数

        public bool Equals(NP_BBValue_List_Long other)
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

            if (this.Value.Count != other.GetValue().Count)
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            for (int i = 0; i < this.Value.Count; i++)
            {
                if (this.Value[i] != other.GetValue()[i])
                {
                    return false;
                }
            }

            return true;
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

            return Equals((NP_BBValue_List_Long) obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static bool operator ==(NP_BBValue_List_Long lhs, NP_BBValue_List_Long rhs)
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

        public static bool operator !=(NP_BBValue_List_Long lhs, NP_BBValue_List_Long rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator >(NP_BBValue_List_Long lhs, NP_BBValue_List_Long rhs)
        {
            Log.Error("你他妈确定对比两个List<long>大小关系？这是人能干出来的事？想对比自己写");
            return false;
        }

        public static bool operator <(NP_BBValue_List_Long lhs, NP_BBValue_List_Long rhs)
        {
            Log.Error("你他妈确定对比两个List<long>大小关系？这是人能干出来的事？想对比自己写");
            return false;
        }

        public static bool operator >=(NP_BBValue_List_Long lhs, NP_BBValue_List_Long rhs)
        {
            Log.Error("你他妈确定对比两个List<long>大小关系？这是人能干出来的事？想对比自己写");
            return false;
        }

        public static bool operator <=(NP_BBValue_List_Long lhs, NP_BBValue_List_Long rhs)
        {
            Log.Error("你他妈确定对比两个List<long>大小关系？这是人能干出来的事？想对比自己写");
            return false;
        }

        #endregion

        #region proto序列化支持

        [ProtoMember(1)] private List<long> ValueForProtoSerilize = new List<long>();

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