//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月4日 16:05:00
//------------------------------------------------------------

using System.Collections.Generic;
using ETMode;

namespace ETModel
{
    /// <summary>
    /// 管理一个Unit身上所有碰撞体（包括技能创建）
    /// </summary>
    public class B2S_HeroColliderDataManagerComponent:Component
    {
        public Dictionary<long,B2S_HeroColliderDataComponent> AllColliderData = new Dictionary<long, B2S_HeroColliderDataComponent>();
    }
}