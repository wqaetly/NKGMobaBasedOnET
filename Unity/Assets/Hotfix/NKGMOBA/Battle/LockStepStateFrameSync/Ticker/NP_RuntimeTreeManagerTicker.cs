using System.Collections.Generic;

namespace ET
{
    [LSF_Tickable(EntityType = typeof(NP_RuntimeTreeManager))]
    public class NP_RuntimeTreeManagerTicker : ALSF_TickHandler<NP_RuntimeTreeManager>
    {
#if !SERVER
        /// <summary>
        /// 这种可变内容的状态数据一致性检查比较特殊，需要使用发过来的脏数据和本地已经经过验证的数据做merge之后再检查才行（当然只需要客户端去做，因为服务端只需要进行脏数据构建就行了）
        /// 只对比每帧脏数据即可
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="frame"></param>
        /// <param name="stateToCompare"></param>
        /// <returns></returns>
        public override bool OnLSF_CheckConsistency(NP_RuntimeTreeManager entity, uint frame, ALSF_Cmd stateToCompare)
        {
            if (stateToCompare is LSF_PlaySkillInputCmd skillInputCmd)
            {
                skillInputCmd.PassingConsistencyCheck = true;
                return true;
            }
            
            LSF_ChangeBBValueCmd changeBbValueCmd = stateToCompare as LSF_ChangeBBValueCmd;

            if (changeBbValueCmd == null)
            {
                return true;
            }
            
            if (entity.FrameSnaps_DeltaOnly.TryGetValue(frame, out var localDeltaSnaps))
            {
                if (localDeltaSnaps.TryGetValue(changeBbValueCmd.TargetNPBehaveTreeId, out var localDeltaSnap))
                {
                    stateToCompare.PassingConsistencyCheck =
                        localDeltaSnap.NP_RuntimeTreeBBSnap.Check(changeBbValueCmd.NP_RuntimeTreeBBSnap);
                    return stateToCompare.PassingConsistencyCheck;
                }
            }

            return false;
        }
#endif

        public override void OnLSF_Tick(NP_RuntimeTreeManager entity, uint currentFrame, long deltaTime)
        {
        }

        /// <summary>
        /// 我们在每一帧的Tick结尾都自动检测脏数据（对于服务端来说），这样就不需要我们手动去维护黑板值改变时需要做的数据收集工作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="frame"></param>
        /// <param name="deltaTime"></param>
        public override void OnLSF_TickEnd(NP_RuntimeTreeManager entity, uint frame, long deltaTime)
        {
            Unit unit = entity.GetParent<Unit>();
            entity.FrameSnaps_DeltaOnly[frame] = new Dictionary<long, LSF_ChangeBBValueCmd>();
            entity.FrameSnaps_Whole[frame] = new Dictionary<long, NP_RuntimeTreeBBSnap>();

            foreach (var runtimeTree in entity.RuntimeTrees)
            {
                NP_RuntimeTreeBBSnap currentFrameNPRuntimeTreeBbSnap =
                    runtimeTree.Value.AcquireCurrentFrameBBValueSnap();
                entity.FrameSnaps_Whole[frame].Add(runtimeTree.Key, currentFrameNPRuntimeTreeBbSnap);

                LSF_ChangeBBValueCmd changeBbValueCmd =
                    ReferencePool.Acquire<LSF_ChangeBBValueCmd>().Init(unit.Id) as LSF_ChangeBBValueCmd;
                changeBbValueCmd.TargetNPBehaveTreeId = runtimeTree.Key;
                changeBbValueCmd.Frame = frame;

                bool hasPreviousSnapValue = entity.FrameSnaps_Whole.ContainsKey(frame - 1) &&
                                            entity.FrameSnaps_Whole[frame - 1].ContainsKey(runtimeTree.Key);

                // 如果前一帧有他的数据，就对比脏数据，如果没有，就直接把当前帧所有数据作为脏数据
                if (hasPreviousSnapValue)
                {
                    // 与前一帧快照对比得出脏数据
                    changeBbValueCmd.NP_RuntimeTreeBBSnap =
                        currentFrameNPRuntimeTreeBbSnap.GetDifference(
                            entity.FrameSnaps_Whole[frame - 1][runtimeTree.Key]);
                }
                else
                {
                    changeBbValueCmd.NP_RuntimeTreeBBSnap = currentFrameNPRuntimeTreeBbSnap;
                    foreach (var snap in changeBbValueCmd.NP_RuntimeTreeBBSnap.NP_FrameBBValues)
                    {
                        changeBbValueCmd.NP_RuntimeTreeBBSnap.NP_FrameBBValueOperations.Add(snap.Key,
                            NP_RuntimeTreeBBOperationType.ADD);
                    }
                }

                // 如果没有脏数据，就直接返回
                if (changeBbValueCmd.NP_RuntimeTreeBBSnap.NP_FrameBBValues.Count == 0 &&
                    changeBbValueCmd.NP_RuntimeTreeBBSnap.NP_FrameBBValueOperations.Count == 0)
                {
                    continue;
                }

#if SERVER
                unit.BelongToRoom.GetComponent<LSF_Component>().AddCmdToSendQueue(changeBbValueCmd);
#else
                entity.FrameSnaps_DeltaOnly[frame][runtimeTree.Key] = changeBbValueCmd;
#endif
            }
        }
    }
}