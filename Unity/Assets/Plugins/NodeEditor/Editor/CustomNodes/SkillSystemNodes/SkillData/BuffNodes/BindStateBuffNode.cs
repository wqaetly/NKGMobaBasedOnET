//------------------------------------------------------------
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using Plugins;
using Plugins.NodeEditor.Editor.Canvas;
using UnityEditor;

namespace SkillDemo
{
    [Node(false, "技能数据部分/绑定一个状态Buff", typeof (NPBehaveCanvas))]
    public class BindStateBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "绑定一个状态Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "绑定一个状态Buff", BuffData = new BindStateBuffData() { BelongBuffSystemType = BuffSystemType.BindStateBuffSystem }
                };

        public override BuffNodeDataBase Skill_GetNodeData()
        {
            return SkillBuffBases;
        }

        public override void AutoAddLinkedBuffs()
        {
            BindStateBuffData bindStateBuffData = SkillBuffBases.BuffData as BindStateBuffData;
            if (bindStateBuffData.OriBuff == null)
            {
                bindStateBuffData.OriBuff = new List<VTD_BuffInfo>();
            }

            //备份Buff Id和对应层数键值对，防止被覆写
            Dictionary<long, int> buffDataBack = new Dictionary<long, int>();

            foreach (var vtdBuffInfo in bindStateBuffData.OriBuff)
            {
                buffDataBack.Add(vtdBuffInfo.BuffNodeId.Value, vtdBuffInfo.Layers);
            }

            bindStateBuffData.OriBuff.Clear();

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
                            bindStateBuffData.OriBuff.Add(new VTD_BuffInfo()
                            {
                                BuffNodeId = targetNode.Skill_GetNodeData().NodeId
                            });
                        }
                    }

                    foreach (var vtdBuffInfo in bindStateBuffData.OriBuff)
                    {
                        if (buffDataBack.TryGetValue(vtdBuffInfo.BuffNodeId.Value, out var buffLayer))
                        {
                            vtdBuffInfo.Layers = buffLayer;
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