//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年3月28日 21:58:45
//------------------------------------------------------------

namespace ETModel
{
    public class RefreshTargetBuffTimeBuffSystem: ABuffSystemBase
    {
        public override void OnInit(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.TheUnitFrom = theUnitFrom;
            this.TheUnitBelongto = theUnitBelongto;
            this.BuffData = buffData;

            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, this.BuffData);
        }

        public override void OnExecute()
        {
            RefreshTargetBuffTimeBuffData refreshTargetBuffTimeBuffData = this.BuffData as RefreshTargetBuffTimeBuffData;
            BuffManagerComponent buffManagerComponent = this.GetBuffTarget().GetComponent<BuffManagerComponent>();

            foreach (var buffNodeId in refreshTargetBuffTimeBuffData.TheBuffNodeIdToBeRefreshed)
            {
                //Log.Info($"准备刷新指定Buff——{buffId}持续时间");
                ABuffSystemBase aBuffSystemBase =
                        buffManagerComponent.GetBuffById(
                            (this.BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[buffNodeId.Value] as NormalBuffNodeData).BuffData
                            .BuffId);
                if (aBuffSystemBase != null && aBuffSystemBase.BuffData.SustainTime + 1 > 0)
                {
                    // Log.Info(
                    //     $"刷新了指定Buff——{buffId}持续时间{TimeHelper.Now() + buffSystemBase.MSkillBuffDataBase.SustainTime},原本为{buffSystemBase.MaxLimitTime}");
                    aBuffSystemBase.MaxLimitTime = TimeHelper.Now() + aBuffSystemBase.BuffData.SustainTime;
                }
            }

            this.BuffState = BuffState.Finished;
        }

        public override void OnFinished()
        {
        }
    }
}