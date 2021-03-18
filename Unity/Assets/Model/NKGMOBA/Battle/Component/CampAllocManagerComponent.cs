//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年3月18日 18:22:06
//------------------------------------------------------------

using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 阵容分配管理者
    /// </summary>
    public class CampAllocManagerComponent: Component
    {
        private int HuiYueCount;

        private int TianZaiCount;

        /// <summary>
        /// 为一个Unit分配阵营
        /// </summary>
        public void AllocRoleCamp(Unit unit)
        {
            B2S_RoleCastComponent b2SRoleCastComponent = unit.GetComponent<B2S_RoleCastComponent>();
            if (b2SRoleCastComponent == null) return;
            if (TianZaiCount > this.HuiYueCount)
            {
                b2SRoleCastComponent.RoleCamp = RoleCamp.HuiYue;
                HuiYueCount++;
            }
            else
            {
                b2SRoleCastComponent.RoleCamp = RoleCamp.TianZai;
                HuiYueCount--;
            }
        }

        /// <summary>
        /// 玩家掉线就清空其信息
        /// </summary>
        /// <param name="player"></param>
        public void PlayerDisconnected(Player player)
        {
            RoleCamp roleCamp = UnitComponent.Instance.Get(player.UnitId).GetComponent<B2S_RoleCastComponent>().RoleCamp;
            switch (roleCamp)
            {
                case RoleCamp.HuiYue:
                    this.HuiYueCount--;
                    break;
                case RoleCamp.TianZai:
                    this.TianZaiCount--;
                    break;
                default: return;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            HuiYueCount = 0;
            TianZaiCount = 0;
        }
    }
}