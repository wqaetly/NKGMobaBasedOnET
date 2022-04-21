using System.Collections.Generic;
using NPBehave;
using ProtoBuf;

namespace ET
{
    public enum NP_RuntimeTreeBBOperationType
    {
        NONE,
        ADD,
        REMOVE,
        CHANGE
    }

    /// <summary>
    /// NP行为树黑板快照封装，用于快捷的合并，对比操作
    /// </summary>
    [ProtoContract]
    public class NP_RuntimeTreeBBSnap : IReference
    {
        /// <summary>
        /// 目标帧所有黑板数据快照
        /// </summary>
        [ProtoMember(1)]
        public Dictionary<string, ANP_BBValue> NP_FrameBBValues = new Dictionary<string, ANP_BBValue>();

        [ProtoMember(2)] public Dictionary<string, NP_RuntimeTreeBBOperationType> NP_FrameBBValueOperations =
            new Dictionary<string, NP_RuntimeTreeBBOperationType>();

        public bool Check(NP_RuntimeTreeBBSnap npRuntimeTreeBbSnapToBeCompared)
        {
            // 先对比数量
            if (npRuntimeTreeBbSnapToBeCompared.NP_FrameBBValues.Count != NP_FrameBBValues.Count)
            {
                return false;
            }

            foreach (var bbValue in npRuntimeTreeBbSnapToBeCompared.NP_FrameBBValues)
            {
                // 再对比是否存在
                if (!NP_FrameBBValues.ContainsKey(bbValue.Key))
                {
                    return false;
                }

                // 最后对比具体相不相等
                if (NP_BBValueHelper.Compare(NP_FrameBBValues[bbValue.Key],
                    npRuntimeTreeBbSnapToBeCompared.NP_FrameBBValues[bbValue.Key], Operator.IS_NOT_EQUAL))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 合并两个黑板快照，合并的规则以targetBBSnapToMerge中的操作类型为准
        /// targetBBSnapToMerge一般为脏数据
        /// </summary>
        /// <returns></returns>
        public NP_RuntimeTreeBBSnap Merge(NP_RuntimeTreeBBSnap targetBBSnapToMerge)
        {
            foreach (var snap in targetBBSnapToMerge.NP_FrameBBValueOperations)
            {
                switch (snap.Value)
                {
                    case NP_RuntimeTreeBBOperationType.ADD:
                        ANP_BBValue anpBbValueAdded = targetBBSnapToMerge.NP_FrameBBValues[snap.Key];
                        this.NP_FrameBBValues.Add(snap.Key, anpBbValueAdded.DeepCopy());
                        break;
                    case NP_RuntimeTreeBBOperationType.REMOVE:
                        this.NP_FrameBBValues.Remove(snap.Key);
                        break;
                    case NP_RuntimeTreeBBOperationType.CHANGE:
                        ANP_BBValue anpBbValueChanged = targetBBSnapToMerge.NP_FrameBBValues[snap.Key];
                        if (this.NP_FrameBBValues.TryGetValue(snap.Key, out var target))
                        {
                            target.SetValueFrom(anpBbValueChanged);
                        }

                        break;
                }
            }

            return this;
        }

        /// <summary>
        /// 对比this和targetBBSnapToCompare获取脏数据，targetBBSnapToCompare一般为正常的黑板快照
        /// </summary>
        /// <param name="targetBBSnapToCompare"></param>
        /// <returns></returns>
        public NP_RuntimeTreeBBSnap GetDifference(NP_RuntimeTreeBBSnap targetBBSnapToCompare)
        {
            NP_RuntimeTreeBBSnap npRuntimeTreeBbSnap = ReferencePool.Acquire<NP_RuntimeTreeBBSnap>();
            //先检测移除的
            foreach (var targetSnap in targetBBSnapToCompare.NP_FrameBBValues)
            {
                if (!this.NP_FrameBBValues.ContainsKey(targetSnap.Key))
                {
                    npRuntimeTreeBbSnap.NP_FrameBBValueOperations.Add(targetSnap.Key,
                        NP_RuntimeTreeBBOperationType.REMOVE);
                }
            }

            //再检测新增和修改的
            foreach (var selfSnap in this.NP_FrameBBValues)
            {
                if (targetBBSnapToCompare.NP_FrameBBValues.TryGetValue(selfSnap.Key, out var targetSnap))
                {
                    if (!NP_BBValueHelper.Compare(selfSnap.Value, targetSnap, Operator.IS_EQUAL))
                    {
                        npRuntimeTreeBbSnap.NP_FrameBBValues.Add(selfSnap.Key, selfSnap.Value.DeepCopy());
                        npRuntimeTreeBbSnap.NP_FrameBBValueOperations.Add(selfSnap.Key,
                            NP_RuntimeTreeBBOperationType.CHANGE);
                    }
                }
                else
                {
                    npRuntimeTreeBbSnap.NP_FrameBBValues.Add(selfSnap.Key, selfSnap.Value.DeepCopy());
                    npRuntimeTreeBbSnap.NP_FrameBBValueOperations.Add(selfSnap.Key, NP_RuntimeTreeBBOperationType.ADD);
                }
            }

            return npRuntimeTreeBbSnap;
        }

        public void Clear()
        {
            NP_FrameBBValues.Clear();
            NP_FrameBBValueOperations.Clear();
        }
    }
}