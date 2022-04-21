//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月2日 15:51:13
//------------------------------------------------------------

using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 向客户端发送Buff信息，主要用于往客户端发送Buff信息让其播放指定特效/动画
    /// Warning ：在配置此节点时要十分注意与要同步的Buff顺序问题
    /// 一般是要在目标Buff刷新过状态之后才执行这个Buff的逻辑，这样保证同步的Buff数据是最新的
    /// 信息包括：对应黑板键，将要同步的Buff节点Id，Buff层数，Buff剩余时间
    /// </summary>
    public class SendBuffInfoToClientBuffData: BuffDataBase
    {
        [Tooltip("默认BBValue为true（bool）")]
        [BoxGroup("自定义项")]
        [LabelText("对应黑板键")]
        public NP_BlackBoardRelationData BBKey;

        [BoxGroup("自定义项")]
        [LabelText("将要同步的Buff节点Id")]
        public VTD_Id TargetBuffNodeId;
    }
}