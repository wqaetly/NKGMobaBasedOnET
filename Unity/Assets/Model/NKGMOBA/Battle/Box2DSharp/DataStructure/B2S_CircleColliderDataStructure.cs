//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月13日 21:31:05
//------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using ETModel;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 圆形碰撞体的数据结构
    /// </summary>
    public class B2S_CircleColliderDataStructure: B2S_ColliderDataStructureBase
    {
        [LabelText("半径")]
        public float radius;
    }
}