using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    public class BuffSnapInfo : IReference, IEquatable<BuffSnapInfo>
    {
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = NP_SupportId.GetHashCode();
                hashCode = (hashCode * 397) ^ BuffNodeId.GetHashCode();
                hashCode = (hashCode * 397) ^ BuffLayer;
                hashCode = (hashCode * 397) ^ FromUnitId.GetHashCode();
                hashCode = (hashCode * 397) ^ BelongtoUnitId.GetHashCode();
                hashCode = (hashCode * 397) ^ BelongtoNP_RuntimeTreeId.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) BuffMaxLimitFrame;
                hashCode = (hashCode * 397) ^ (int) OperationType;
                return hashCode;
            }
        }

        public enum BuffOperationType
        {
            NONE,
            ADD,
            REMOVE,
            CHANGE
        }

        /// <summary>
        /// Buff归属的NPSupportId
        /// </summary>
        [ProtoMember(1)] public long NP_SupportId;

        /// <summary>
        /// BuffDataNode的Id
        /// </summary>
        [ProtoMember(2)] public long BuffNodeId;

        /// <summary>
        /// BuffData的Id
        /// </summary>
        [ProtoMember(9)] public long BuffId;

        /// <summary>
        /// Buff的层数
        /// </summary>
        [ProtoMember(3)] public int BuffLayer;

        /// <summary>
        /// Buff来源UnitId
        /// </summary>
        [ProtoMember(4)] public long FromUnitId;

        /// <summary>
        /// Buff归属UnitId
        /// </summary>
        [ProtoMember(5)] public long BelongtoUnitId;

        /// <summary>
        /// Buff归属NP_RuntimeTreeId
        /// </summary>
        [ProtoMember(6)] public long BelongtoNP_RuntimeTreeId;

        /// <summary>
        /// Buff会被移除的目标帧
        /// </summary>
        [ProtoMember(7)] public uint BuffMaxLimitFrame;

        [ProtoMember(8)] public BuffOperationType OperationType;

        public void Clear()
        {
        }

        public bool Equals(BuffSnapInfo other)
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
            return this.NP_SupportId == other.NP_SupportId && this.BuffNodeId == other.BuffNodeId &&
                   this.BuffId == other.BuffId &&
                   this.BuffLayer == other.BuffLayer && this.OperationType == other.OperationType &&
                   this.FromUnitId == other.FromUnitId && this.BelongtoUnitId == other.BelongtoUnitId &&
                   this.BelongtoNP_RuntimeTreeId == other.BelongtoNP_RuntimeTreeId &&
                   this.BuffMaxLimitFrame == other.BuffMaxLimitFrame;
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

            return Equals((BuffSnapInfo) obj);
        }

        public static bool operator ==(BuffSnapInfo lhs, BuffSnapInfo rhs)
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

        public static bool operator !=(BuffSnapInfo lhs, BuffSnapInfo rhs)
        {
            return !(lhs == rhs);
        }
    }

    [ProtoContract]
    public class BuffSnapInfoCollection : IReference
    {
        /// <summary>
        /// 单帧内变化的Buff信息
        /// </summary>
        [ProtoMember(1)] [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, BuffSnapInfo> FrameBuffChangeSnap = new Dictionary<long, BuffSnapInfo>();

        public bool Check(BuffSnapInfoCollection buffSnapInfoCollectionToCompare)
        {
            // 先对比数量
            if (buffSnapInfoCollectionToCompare.FrameBuffChangeSnap.Count != this.FrameBuffChangeSnap.Count)
            {
                return false;
            }

            foreach (var bbValue in buffSnapInfoCollectionToCompare.FrameBuffChangeSnap)
            {
                // 再对比是否存在
                if (!this.FrameBuffChangeSnap.ContainsKey(bbValue.Key))
                {
                    return false;
                }

                // 最后对比具体相不相等
                if (this.FrameBuffChangeSnap[bbValue.Key] != bbValue.Value)
                {
                    return false;
                }
            }


            return true;
        }

        public BuffSnapInfoCollection GetDifference(BuffSnapInfoCollection buffSnapInfoCollectionToCompare)
        {
            BuffSnapInfoCollection result = ReferencePool.Acquire<BuffSnapInfoCollection>();

            //先检测移除的
            foreach (var targetSnap in buffSnapInfoCollectionToCompare.FrameBuffChangeSnap)
            {
                if (!this.FrameBuffChangeSnap.ContainsKey(targetSnap.Key))
                {
                    BuffSnapInfo buffSnapInfo = targetSnap.Value.DeepCopy();
                    buffSnapInfo.OperationType = BuffSnapInfo.BuffOperationType.REMOVE;
                    result.FrameBuffChangeSnap[targetSnap.Key] = buffSnapInfo;
                }
            }

            //再检测新增和修改的
            foreach (var selfSnap in this.FrameBuffChangeSnap)
            {
                if (buffSnapInfoCollectionToCompare.FrameBuffChangeSnap.TryGetValue(selfSnap.Key, out var targetSnap))
                {
                    if (selfSnap.Value != targetSnap)
                    {
                        BuffSnapInfo buffSnapInfo = selfSnap.Value.DeepCopy();
                        buffSnapInfo.OperationType = BuffSnapInfo.BuffOperationType.CHANGE;
                        result.FrameBuffChangeSnap.Add(buffSnapInfo.BuffNodeId, buffSnapInfo);
                    }
                }
                else
                {
                    BuffSnapInfo buffSnapInfo = selfSnap.Value.DeepCopy();
                    buffSnapInfo.OperationType = BuffSnapInfo.BuffOperationType.ADD;
                    result.FrameBuffChangeSnap.Add(buffSnapInfo.BuffNodeId, buffSnapInfo);
                }
            }

            return result;
        }

        public void Clear()
        {
        }
    }
}