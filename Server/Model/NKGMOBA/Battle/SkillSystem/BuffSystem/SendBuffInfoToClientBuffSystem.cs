//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月2日 17:39:06
//------------------------------------------------------------

namespace ETModel
{
    public class SendBuffInfoToClientBuffSystem: ABuffSystemBase
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
            SendBuffInfoToClientBuffData sendBuffInfoToClientBuffData = this.BuffData as SendBuffInfoToClientBuffData;

            ABuffSystemBase targetBuffSystem = this.TheUnitBelongto.GetComponent<BuffManagerComponent>()
                    .GetBuffById(
                        (this.BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[sendBuffInfoToClientBuffData.TargetBuffNodeId.Value] as
                                NormalBuffNodeData).BuffData.BuffId);

            Game.EventSystem.Run(EventIdType.SendBuffInfoToClient,
                new M2C_BuffInfo()
                {
                    UnitId = this.BelongtoRuntimeTree.BelongToUnitId,
                    SkillId = sendBuffInfoToClientBuffData.BelongToSkillId.Value,
                    BBKey = sendBuffInfoToClientBuffData.BBKey.BBKey,
                    TheUnitFromId = this.TheUnitFrom.Id,
                    TheUnitBelongToId = this.TheUnitBelongto.Id,
                    BuffLayers = targetBuffSystem.CurrentOverlay,
                    BuffMaxLimitTime = targetBuffSystem.MaxLimitTime,
                });
            this.BuffState = BuffState.Finished;
        }

        public override void OnFinished()
        {
        }
    }
}