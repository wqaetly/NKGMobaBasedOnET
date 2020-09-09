//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:13:45
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    [BoxGroup("等待结点数据")]
    [HideLabel]
    public class NP_WaitNodeData : NP_NodeDataBase
    {
        [HideInEditorMode] private Wait m_WaitNode;
        
        public NP_BlackBoardRelationData NPBalckBoardRelationData = new NP_BlackBoardRelationData();
        
        public override Task CreateTask(long unitId, NP_RuntimeTree runtimeTree)
        {
            this.m_WaitNode = new Wait(this.NPBalckBoardRelationData.BBKey);
            return this.m_WaitNode;
        }

        public override Node NP_GetNode()
        {
            return this.m_WaitNode;
        }
    }
}