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

namespace SkillDemo
{
    [Node(false, "技能数据部分/监听Buff", typeof (NPBehaveCanvas))]
    public class ListenBuffCallBackBuffNode: BuffNodeBase
    {
        public override string GetID => Id;

        public const string Id = "监听Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "监听Buff",
                    BuffData = new ListenBuffCallBackBuffData() { BelongBuffSystemType = BuffSystemType.ListenBuffCallBackBuffSystem }
                };

        public override BuffNodeDataBase Skill_GetNodeData()
        {
            return SkillBuffBases;
        }

        public override void AutoAddLinkedBuffs()
        {
            ListenBuffCallBackBuffData listenBuffCallBackBuffData = SkillBuffBases.BuffData as ListenBuffCallBackBuffData;
            if (listenBuffCallBackBuffData.ListenBuffEventNormal == null)
            {
                listenBuffCallBackBuffData.ListenBuffEventNormal = new ListenBuffEvent_Normal();
            }

            //备份Buff Id和对应层数键值对，防止被覆写
            Dictionary<long, int> buffDataBack = new Dictionary<long, int>();

            foreach (var vtdBuffInfo in listenBuffCallBackBuffData.ListenBuffEventNormal.BuffInfoWillBeAdded)
            {
                buffDataBack.Add(vtdBuffInfo.BuffId.Value, vtdBuffInfo.Layers);
            }

            listenBuffCallBackBuffData.ListenBuffEventNormal.BuffInfoWillBeAdded.Clear();

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
                            listenBuffCallBackBuffData.ListenBuffEventNormal.BuffInfoWillBeAdded.Add(new VTD_BuffInfo()
                            {
                                BuffId = targetNode.Skill_GetNodeData().NodeId
                            });
                        }
                    }

                    foreach (var vtdBuffInfo in listenBuffCallBackBuffData.ListenBuffEventNormal.BuffInfoWillBeAdded)
                    {
                        if (buffDataBack.TryGetValue(vtdBuffInfo.BuffId.Value, out var buffLayer))
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
            RTEditorGUI.TextField(SkillBuffBases?.BuffDes);
        }
    }
}