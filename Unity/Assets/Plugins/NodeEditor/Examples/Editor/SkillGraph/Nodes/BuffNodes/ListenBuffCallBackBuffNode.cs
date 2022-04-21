//------------------------------------------------------------
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
// 此代码由工具自动生成，请勿更改
//------------------------------------------------------------

using System.Collections.Generic;
using ET;
using GraphProcessor;
using UnityEditor;

namespace Plugins.NodeEditor
{
    [NodeMenuItem("技能数据部分/监听Buff", typeof (SkillGraph))]
    public class ListenBuffCallBackBuffNode: BuffNodeBase
    {
        public override string name => "监听Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "监听Buff",
                    BuffData = new ListenBuffCallBackBuffData() { }
                };

        public override BuffNodeDataBase GetBuffNodeData()
        {
            return SkillBuffBases;
        }

        public override void AutoAddLinkedBuffs()
        {
            ListenBuffCallBackBuffData listenBuffCallBackBuffData = SkillBuffBases.BuffData as ListenBuffCallBackBuffData;
            if (listenBuffCallBackBuffData.BuffInfoWillBeAdded == null)
            {
                listenBuffCallBackBuffData.BuffInfoWillBeAdded = new List<VTD_BuffInfo>();
            }

            //备份Buff Id和对应层数键值对，防止被覆写
            Dictionary<long, int> buffDataBack = new Dictionary<long, int>();

            foreach (var vtdBuffInfo in listenBuffCallBackBuffData.BuffInfoWillBeAdded)
            {
                buffDataBack[vtdBuffInfo.BuffNodeId.Value] = vtdBuffInfo.Layers;
            }

            listenBuffCallBackBuffData.BuffInfoWillBeAdded.Clear();

            foreach (var outputNode in this.GetOutputNodes())
            {
                BuffNodeBase targetNode = (outputNode as BuffNodeBase);
                if (targetNode != null)
                {
                    listenBuffCallBackBuffData.BuffInfoWillBeAdded.Add(new VTD_BuffInfo()
                    {
                        BuffNodeId = targetNode.GetBuffNodeData().NodeId
                    });
                }
            }

            foreach (var vtdBuffInfo in listenBuffCallBackBuffData.BuffInfoWillBeAdded)
            {
                if (buffDataBack.TryGetValue(vtdBuffInfo.BuffNodeId.Value, out var buffLayer))
                {
                    vtdBuffInfo.Layers = buffLayer;
                }
            }
        }
    }
}