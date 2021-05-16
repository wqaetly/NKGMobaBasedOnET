//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月2日 16:53:17
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using Plugins;
using Plugins.NodeEditor.Editor.Canvas;
using UnityEditor;
using UnityEngine;

namespace SkillDemo
{
    [Node(false, "技能数据部分/往客户端发送Buff信息Buff", typeof (NPBehaveCanvas))]
    public class SendBuffInfoToClientBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "往客户端发送Buff信息Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "往客户端发送Buff信息Buff",
                    BuffData = new SendBuffInfoToClientBuffData()
                    {
                        BBKey = new NP_BlackBoardRelationData(), BelongBuffSystemType = BuffSystemType.SendBuffInfoToClientBuffSystem
                    }
                };

        public override Vector2 DefaultSize
        {
            get
            {
                return new Vector2(180, 60);
            }
        }

        public override BuffNodeDataBase Skill_GetNodeData()
        {
            return SkillBuffBases;
        }

        public override void AutoAddLinkedBuffs()
        {
            SendBuffInfoToClientBuffData sendBuffInfoToClientBuffData = SkillBuffBases.BuffData as SendBuffInfoToClientBuffData;

            foreach (var connection in this.connectionPorts)
            {
                //只有出方向的端口才是添加LinkedBuffId的地方
                if (connection.direction == Direction.Out)
                {
                    foreach (var connectTagrets in connection.connections)
                    {
                        BuffNodeBase targetNode = (connectTagrets.body as BuffNodeBase);
                        if (targetNode != null)
                        {
                            VTD_Id temp = targetNode.Skill_GetNodeData().NodeId;
                            sendBuffInfoToClientBuffData.TargetBuffNodeId = new VTD_Id() { IdKey = temp.IdKey, Value = temp.Value };
                        }
                    }

                    return;
                }
            }
        }

        public override void NodeGUI()
        {
            EditorGUILayout.TextField(SkillBuffBases?.BuffDes);
        }
    }
}