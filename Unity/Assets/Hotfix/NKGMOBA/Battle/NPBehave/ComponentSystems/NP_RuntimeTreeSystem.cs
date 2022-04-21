using System.Collections.Generic;

namespace ET
{
    public class NP_RuntimeTreeAwakeSystem : AwakeSystem<NP_RuntimeTree, NP_DataSupportor, NP_SyncComponent, Unit>
    {
        public override void Awake(NP_RuntimeTree self, NP_DataSupportor belongNP_DataSupportor,
            NP_SyncComponent npSyncComponent, Unit belongToUnit)
        {
            self.BelongToUnit = belongToUnit;
            self.BelongNP_DataSupportor = belongNP_DataSupportor;
            self.NpSyncComponent = npSyncComponent;
        }
    }

    public class NP_RuntimeTreeDestroySystem : DestroySystem<NP_RuntimeTree>
    {
        public override void Destroy(NP_RuntimeTree self)
        {
            self.Finish().Coroutine();
        }
    }


    public static class NP_RuntimeTreeUtilities
    {
        /// <summary>
        /// 获取当前帧黑板键值快照
        /// </summary>
        /// <returns></returns>
        public static NP_RuntimeTreeBBSnap AcquireCurrentFrameBBValueSnap(this NP_RuntimeTree self)
        {
            NP_RuntimeTreeBBSnap snap = ReferencePool.Acquire<NP_RuntimeTreeBBSnap>();
            snap.NP_FrameBBValues = self.GetBlackboard().GetDatas().DeepCopy();
            return snap;
        }
    }
}