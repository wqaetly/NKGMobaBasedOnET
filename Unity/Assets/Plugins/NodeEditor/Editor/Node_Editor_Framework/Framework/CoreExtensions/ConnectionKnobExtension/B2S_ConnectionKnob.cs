//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月28日 23:36:27
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;
using Sirenix.OdinInspector;

namespace NodeEditorFramework
{
    public class InfoWithDeleteBtn
    {
        public string Flag;

        private string ConnectedFlag;

        /// <summary>
        /// 这个数据块的ValueConnectionKnob信息（对方）
        /// </summary>
        public ValueConnectionKnob connectionInfo;

        [Button("移除这条连接(注意自己清理依赖数据)"), GUIColor(0.4f, 0.8f, 1)]
        public void RemoveTheConnection()
        {
            int theIndexWillBeRemove_theConnectedValue = -1;

            for (int j = 0; j < connectionInfo.connections.Count; j++)
            {
                if (connectionInfo.connections[j].body.B2SCollisionRelation_GetNodeData().Flag == this.ConnectedFlag)
                {
                    theIndexWillBeRemove_theConnectedValue = j;
                    break;
                }

                if (theIndexWillBeRemove_theConnectedValue != -1) break;
            }

            if (theIndexWillBeRemove_theConnectedValue == -1)
            {
                Log.Error("删除失败，请检查问题");
            }
            else
            {
                //双方最后重新读取一下信息，防止出错
                this.connectionInfo.TryToAutoReadAllConnectedNode();
                this.connectionInfo
                        .connections[theIndexWillBeRemove_theConnectedValue].TryToAutoReadAllConnectedNode();
                
                //自身移除联系结点关系id
                this.connectionInfo
                        .body.B2SCollisionRelation_GetNodeData().CollisionRelations
                        .Remove(this.connectionInfo
                                .connections[theIndexWillBeRemove_theConnectedValue].body.B2SCollisionRelation_GetNodeData().nodeDataId);

                //联系结点移除自身关系id
                this.connectionInfo
                        .connections[theIndexWillBeRemove_theConnectedValue].body.B2SCollisionRelation_GetNodeData().CollisionRelations.Remove(this
                                .connectionInfo
                                .body.B2SCollisionRelation_GetNodeData().nodeDataId);

                //自身移除联系信息btn
                this.connectionInfo.RemoveInfoWithDeleteBtn(this.connectionInfo.b2sInfo[theIndexWillBeRemove_theConnectedValue]);

                //从联系结点移除自身btn
                this.connectionInfo
                        .connections[theIndexWillBeRemove_theConnectedValue].RemoveInfoWithDeleteBtn(this);

                //双方移除联系信息
                this.connectionInfo
                        .connections[theIndexWillBeRemove_theConnectedValue].RemoveConnection(this.connectionInfo);
            }
        }

        public void SetconnectionInfo(ValueConnectionKnob valueConnectionKnob)
        {
            this.connectionInfo = valueConnectionKnob;
        }

        public InfoWithDeleteBtn(string flag, string ConnectedFlag)
        {
            this.Flag = flag;
            this.ConnectedFlag = ConnectedFlag;
        }
    }

    public partial class ValueConnectionKnob
    {
        [LabelText("同Connections顺序，其连接的结点标识为(仅碰撞关系可用)")]
        public List<InfoWithDeleteBtn> b2sInfo = new List<InfoWithDeleteBtn>();

        [Button("尝试自动读取所连接结点信息(仅碰撞关系可用)", 25), GUIColor(0.4f, 0.8f, 1)]
        public void TryToAutoReadAllConnectedNode()
        {
            b2sInfo.Clear();
            for (int i = 0; i < connections.Count; i++)
            {
                this.b2sInfo.Add(new InfoWithDeleteBtn(this.connections[i].body.B2SCollisionRelation_GetNodeData().Flag,
                    this.body.B2SCollisionRelation_GetNodeData().Flag));
                this.b2sInfo[i].SetconnectionInfo(connections[i]);
            }
        }

        public void RemoveInfoWithDeleteBtn(InfoWithDeleteBtn infoWithDeleteBtn)
        {
            if (this.b2sInfo.Count > 0)
                b2sInfo.Remove(infoWithDeleteBtn);
        }
    }
}