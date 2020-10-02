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
            this.BuffState = BuffState.Finished;
        }

        public override void OnFinished()
        {

        }
    }
}