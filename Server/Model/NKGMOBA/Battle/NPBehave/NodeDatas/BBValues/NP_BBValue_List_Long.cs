//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月18日 20:37:53
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ETModel.BBValues
{
    public class NP_BBValue_List_Long: NP_BBValueBase<List<long>>, IEquatable<NP_BBValue_List_Long>
    {
        public override Type NP_BBValueType
        {
            get
            {
                return typeof (List<long>);
            }
        }

        #region 对比函数

        public bool Equals(NP_BBValue_List_Long other)
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

            return Equals((NP_BBValue_List_Long) obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static bool operator ==(NP_BBValue_List_Long lhs, NP_BBValue_List_Long rhs)
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
    }
}