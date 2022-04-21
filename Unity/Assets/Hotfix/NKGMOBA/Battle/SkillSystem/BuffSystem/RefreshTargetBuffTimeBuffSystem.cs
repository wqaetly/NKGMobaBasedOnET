//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年3月28日 21:58:45
//------------------------------------------------------------

namespace ET
{
    public class RefreshTargetBuffTimeBuffSystem: ABuffSystemBase<RefreshTargetBuffTimeBuffData>
    {
        public override void OnExecute(uint currentFrame)
        {
            BuffManagerComponent buffManagerComponent = this.GetBuffTarget().GetComponent<BuffManagerComponent>();

            foreach (var buffNodeId in this.GetBuffDataWithTType.TheBuffNodeIdToBeRefreshed)
            {
                //Log.Info($"准备刷新指定Buff——{buffId}持续时间");
                IBuffSystem aBuffSystemBase =
                        buffManagerComponent.GetBuffById(
                            (this.BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[buffNodeId.Value] as NormalBuffNodeData).BuffData
                            .BuffId);
                if (aBuffSystemBase != null && aBuffSystemBase.BuffData.SustainTime + 1 > 0)
                {
                    // Log.Info(
                    //     $"刷新了指定Buff——{buffId}持续时间{TimeHelper.Now() + buffSystemBase.MSkillBuffDataBase.SustainTime},原本为{buffSystemBase.MaxLimitTime}");
                    aBuffSystemBase.MaxLimitFrame = currentFrame + TimeAndFrameConverter.Frame_Long2Frame(aBuffSystemBase.BuffData.SustainTime);
                }
            }
        }
    }
}