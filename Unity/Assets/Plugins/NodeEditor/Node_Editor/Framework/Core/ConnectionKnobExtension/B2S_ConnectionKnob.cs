//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月28日 23:36:27
//------------------------------------------------------------

using System.Collections.Generic;
using ETMode;
using ETModel;
using Sirenix.OdinInspector;

namespace NodeEditorFramework
{
    public class InfoWithDeleteBtn
    {
        public string Flag;

        public ValueConnectionKnob connectionInfo;

        [Button("移除这条连接"), GUIColor(0.4f, 0.8f, 1)]
        public void RemoveTheConnection()
        {
            int theIndexWillBeRemove_theNodeValue = -1;
            int theIndexWillBeRemove_theConnectedValue = -1;
            for (int i = 0; i < connectionInfo.connections.Count; i++)
            {
                for (int j = 0; j < connectionInfo.connections[i].connections.Count; j++)
                {
                    if (connectionInfo.connections[i].connections[j].body.B2SCollisionRelation_GetNodeData().Flag == this.Flag)
                    {
                        theIndexWillBeRemove_theNodeValue = i;
                        theIndexWillBeRemove_theConnectedValue = j;
                        Log.Info("成功移除");
                        break;
                    }
                }

                if (theIndexWillBeRemove_theConnectedValue != -1) break;
            }

            this.connectionInfo.connections[theIndexWillBeRemove_theNodeValue].RemoveConnection(this.connectionInfo
                    .connections[theIndexWillBeRemove_theNodeValue].connections[theIndexWillBeRemove_theConnectedValue]);
        }

        public void SetconnectionInfo(ValueConnectionKnob valueConnectionKnob)
        {
            this.connectionInfo = valueConnectionKnob;
        }

        public InfoWithDeleteBtn(string flag)
        {
            this.Flag = flag;
        }
    }

    public partial class ValueConnectionKnob
    {
        [LabelText("同Connections顺序，其连接的结点标识为")]
        public List<InfoWithDeleteBtn> b2sInfo = new List<InfoWithDeleteBtn>();

        [Button("尝试自动读取所连接结点信息", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AddAllNodeData()
        {
            b2sInfo.Clear();
            foreach (var VARIABLE in this.connections)
            {
                this.b2sInfo.Add(new InfoWithDeleteBtn(VARIABLE.body.B2SCollisionRelation_GetNodeData().Flag));
                this.b2sInfo[this.b2sInfo.Count - 1].SetconnectionInfo(VARIABLE);
            }
        }
    }
}