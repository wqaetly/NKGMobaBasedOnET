//------------------------------------------------------------
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
//------------------------------------------------------------

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
            AutoAddLinkedBuffs();
            return SkillBuffBases;
        }
        
        public override void AutoAddLinkedBuffs()
        {
            ListenBuffCallBackBuffData listenBuffCallBackBuffData = SkillBuffBases.BuffData as ListenBuffCallBackBuffData;
            if (listenBuffCallBackBuffData.ListenBuffEventNormal == null)
            {
                listenBuffCallBackBuffData.ListenBuffEventNormal = new ListenBuffEvent_Normal();
            }
            else
            {
                listenBuffCallBackBuffData.ListenBuffEventNormal.BuffsIdWillBeAdded.Clear();
            }

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
                            listenBuffCallBackBuffData.ListenBuffEventNormal.BuffsIdWillBeAdded.Add(targetNode.Skill_GetNodeData().NodeId);
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
