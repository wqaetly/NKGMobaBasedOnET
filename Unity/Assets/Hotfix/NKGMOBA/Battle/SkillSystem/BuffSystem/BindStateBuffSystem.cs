//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月1日 21:22:19
//------------------------------------------------------------


namespace ET
{
    /// <summary>
    /// 绑定一个状态
    /// </summary>
    public class BindStateBuffSystem: ABuffSystemBase<BindStateBuffData>
    {
        public override void OnExecute(uint currentFrame)
        {
            ExcuteInternal();
        }

        public override void OnFinished(uint currentFrame)
        {
            if (this.GetBuffDataWithTType.OriState != null)
            {
                this.GetBuffTarget().GetComponent<StackFsmComponent>().RemoveState(this.GetBuffDataWithTType.OriState.StateName);
            }
        }

        public override void OnRefreshed(uint currentFrame)
        {
            ExcuteInternal();
        }

        private void ExcuteInternal()
        {
            foreach (var buffData in this.GetBuffDataWithTType.OriBuff)
            {
                buffData.AutoAddBuff(this.BuffData.BelongToBuffDataSupportorId, buffData.BuffNodeId.Value,
                    this.TheUnitFrom, this.TheUnitBelongto, this.BelongtoRuntimeTree);
            }

            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    this.GetBuffTarget().BelongToRoom.GetComponent<BattleEventSystemComponent>().Run($"{eventId.Value}{this.TheUnitFrom.Id}", this);
                }
            }

            if (this.GetBuffDataWithTType.OriState != null)
            {
                this.GetBuffTarget().GetComponent<StackFsmComponent>().ChangeState(this.GetBuffDataWithTType.OriState);
            }
        }
    }
}