//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:12:21
//------------------------------------------------------------

namespace ETModel
{
    public class PhysicalDamageBuff: BuffBase
    {
        public override void OnInit()
        {
            this.MBuffTypes = BuffTypes.BuffValue_Physical;
        }

        public override void OnExecute()
        {
        }

        public override void OnFinished()
        {
            
        }
    }
}