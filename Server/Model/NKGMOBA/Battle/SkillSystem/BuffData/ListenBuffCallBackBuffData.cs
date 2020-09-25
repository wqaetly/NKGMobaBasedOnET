//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月7日 15:32:03
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ETModel
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
        /// Buff事件
        /// </summary>
        [BoxGroup("自定义项")]
        [HideLabel]
        public ListenBuffEvent_Normal ListenBuffEventNormal;
    }
}