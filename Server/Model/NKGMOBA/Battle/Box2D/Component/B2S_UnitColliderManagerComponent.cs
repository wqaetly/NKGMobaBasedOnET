//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:05:00
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;

namespace ETModel
{
    /// <summary>
    /// 管理一个Unit身上所有碰撞体（包括技能创建）
    /// </summary>
    public class B2S_UnitColliderManagerComponent: Component
    {
        /// <summary>
        /// 所有碰撞数据，long为碰撞数据id，int为此碰撞数据细分的id(从零开始)，比如卡特q会有多个匕首，第一个是1，第二个是2.。。,bool为此碰撞数据是否正在使用
        /// </summary>
        public Dictionary<(long, int, Unit), bool> AllColliderData = new Dictionary<(long, int, Unit), bool>();

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            this.AllColliderData.Clear();
        }
    }
}