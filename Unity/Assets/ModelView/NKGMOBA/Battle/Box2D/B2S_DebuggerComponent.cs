//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月8日 21:11:46
//------------------------------------------------------------

using System.Collections.Generic;
using ET;
using UnityEngine;

namespace ET
{
#if !SERVER
    /// <summary>
    /// Box2D的碰撞体可视化Debugger组件
    /// </summary>
    public class B2S_DebuggerComponent : Entity
    {
        public Dictionary<Unit, B2S_DebuggerProcessor> AllLinerRendersDic =
            new Dictionary<Unit, B2S_DebuggerProcessor>();

        public Dictionary<Unit, Vector3[]> AllVexs = new Dictionary<Unit, Vector3[]>();

        public GameObject GoSupportor;

        public List<Unit> TobeRemovedProcessors = new List<Unit>();

        /// <summary>
        /// 用于绘制圆形的点数量
        /// </summary>
        public static int CircleDrawPointCount = 30;
    }
#endif
}