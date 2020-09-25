//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月13日 21:31:25
//------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using ETModel;
using Sirenix.OdinInspector;
using Vector2 = System.Numerics.Vector2;

namespace ETModel
{
    /// <summary>
    /// 多边形碰撞体的数据结构
    /// </summary>
    public class B2S_PolygonColliderDataStructure: B2S_ColliderDataStructureBase
    {
        [LabelText("碰撞体所包含的顶点信息(顺时针),可能由多个多边形组成")]
        [DisableInEditorMode]
        public List<List<Vector2>> finalPoints = new List<List<Vector2>>();

        [LabelText("总顶点数")]
        [DisableInEditorMode]
        public int pointCount;
    }
}