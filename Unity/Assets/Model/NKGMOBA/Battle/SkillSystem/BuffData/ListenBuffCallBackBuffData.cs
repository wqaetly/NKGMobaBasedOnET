//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月7日 15:32:03
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET
{
    /// <summary>
    /// 监听Buff事件数据基类，用以监听指定事件
    /// </summary>
    public class ListenBuffCallBackBuffData: BuffDataBase
    {
        [BoxGroup("自定义项")]
        [LabelText("要监听的事件ID标识")]
        public VTD_EventId EventId;

        /// <summary>
        /// 是否需要判断层数
        /// </summary>
        [BoxGroup("自定义项")]
        [LabelText("是否需要判断层数")]
        public bool HasOverlayerJudge;

        [BoxGroup("自定义项")]
        [LabelText("目标层数")]
        [ShowIf("HasOverlayerJudge")]
        public int TargetOverLayer;
        
        /// <summary>
        /// Buff回调条件达成时会添加的Buff的节点Id
        /// </summary>
        [BoxGroup("自定义项")]
        [InfoBox("注意，是在节点编辑器中的Buff节点Id，而不是Buff自身的Id，别搞错了！")]
        [LabelText("Buff回调条件达成时会添加的Buff的节点Id")]
        public List<VTD_BuffInfo> BuffInfoWillBeAdded = new List<VTD_BuffInfo>();
    }
}