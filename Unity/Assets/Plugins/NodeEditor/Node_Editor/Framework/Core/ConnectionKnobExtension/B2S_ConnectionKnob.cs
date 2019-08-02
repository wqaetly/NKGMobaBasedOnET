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

        public ValueConnectionKnob connectionInfo;

        [Button("移除这条连接"), GUIColor(0.4f, 0.8f, 1)]
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
                this.connectionInfo
                        .connections[theIndexWillBeRemove_theConnectedValue].Remove(this);
                this.connectionInfo.RemoveConnection(this.connectionInfo
                        .connections[theIndexWillBeRemove_theConnectedValue]);
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
        [LabelText("同Connections顺序，其连接的结点标识为")]
        public List<InfoWithDeleteBtn> b2sInfo = new List<InfoWithDeleteBtn>();

        [Button("尝试自动读取所连接结点信息", 25), GUIColor(0.4f, 0.8f, 1)]
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

        public void Remove(InfoWithDeleteBtn infoWithDeleteBtn)
        {
            b2sInfo.Remove(infoWithDeleteBtn);
        }
    }
}