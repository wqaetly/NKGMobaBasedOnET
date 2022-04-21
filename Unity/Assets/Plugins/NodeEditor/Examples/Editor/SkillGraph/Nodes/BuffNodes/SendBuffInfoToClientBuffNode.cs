//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月2日 16:53:17
//------------------------------------------------------------

using ET;
using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("技能数据部分/往客户端发送Buff信息Buff", typeof (SkillGraph))]
    public class SendBuffInfoToClientBuffNode: BuffNodeBase
    {
        public override string name => "往客户端发送Buff信息Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "往客户端发送Buff信息Buff",
                    BuffData = new SendBuffInfoToClientBuffData()
                    {
                        BBKey = new NP_BlackBoardRelationData(), 
                    }
                };

        public override BuffNodeDataBase GetBuffNodeData()
        {
            return SkillBuffBases;
        }

        public override void AutoAddLinkedBuffs()
        {
            SendBuffInfoToClientBuffData sendBuffInfoToClientBuffData = SkillBuffBases.BuffData as SendBuffInfoToClientBuffData;

            foreach (var outputNode in this.GetOutputNodes())
            {
                BuffNodeBase targetNode = (outputNode as BuffNodeBase);
                if (targetNode != null)
                {
                    VTD_Id temp = targetNode.GetBuffNodeData().NodeId;
                    sendBuffInfoToClientBuffData.TargetBuffNodeId = new VTD_Id() { IdKey = temp.IdKey, Value = temp.Value };
                }
            }
        }
    }
}