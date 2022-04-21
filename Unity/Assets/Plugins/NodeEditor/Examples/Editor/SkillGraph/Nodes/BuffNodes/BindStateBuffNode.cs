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
    [NodeMenuItem("技能数据部分/绑定一个状态Buff", typeof (SkillGraph))]
    public class BindStateBuffNode: BuffNodeBase
    {
        public override string name => "绑定一个状态Buff";

        public NormalBuffNodeData SkillBuffBases =
                new NormalBuffNodeData()
                {
                    BuffDes = "绑定一个状态Buff", BuffData = new BindStateBuffData() { }
                };

        public override BuffNodeDataBase GetBuffNodeData()
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
                buffDataBack[vtdBuffInfo.BuffNodeId.Value] = vtdBuffInfo.Layers;
            }

            bindStateBuffData.OriBuff.Clear();

            //配置Buff节点
            foreach (var outputNode in this.GetOutputNodes())
            {
                BuffNodeBase targetNode = (outputNode as BuffNodeBase);
                if (targetNode != null)
                {
                    bindStateBuffData.OriBuff.Add(new VTD_BuffInfo() { BuffNodeId = targetNode.GetBuffNodeData().NodeId });
                }
            }
            //配置Buff层数
            foreach (var vtdBuffInfo in bindStateBuffData.OriBuff)
            {
                if (buffDataBack.TryGetValue(vtdBuffInfo.BuffNodeId.Value, out var buffLayer))
                {
                    vtdBuffInfo.Layers = buffLayer;
                }
            }
        }
    }
}