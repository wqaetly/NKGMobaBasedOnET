//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:14:44
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    public class NP_NodeDataBase
    {
        /// <summary>
        /// 此结点ID
        /// </summary>
        [LabelText("此结点ID")]
        public long id = IdGenerater.GenerateId();

        [LabelText("此结点类型")]
        public NodeType NodeType;
        
        /// <summary>
        /// 优先级，值越小，优先级越高
        /// </summary>
        [LabelText("优先级")]
        public long priority;

        /// <summary>
        /// 与此结点相连的ID
        /// </summary>
        [LabelText("与此结点相连的ID")]
        public List<long> linkedID = new List<long>();
        
        [LabelText("结点信息描述")]
        public string NodeDes;

        public virtual Node NP_GetNode()
        {
            return null;
        }
        
        public virtual Action CreateAction()
        {
            return null;
        }
        
        public virtual Root CreateRoot(Action action)
        {
            return null;
        }
    }
}