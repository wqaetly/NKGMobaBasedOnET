//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 16:52:27
//------------------------------------------------------------

using System.Collections.Generic;
using B2S_CollisionRelation;
using ETModel;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.NodeEditor
{
    [Node(false, "Box2DSharp/英雄碰撞数据结点", typeof (B2S_CollisionRelationCanvas))]
    public class B2S_CollisionRelationForOneHero: Node
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "英雄碰撞数据结点";

        public override string GetID => Id;

        public override Vector2 DefaultSize => new Vector2(150, 60);

        [ValueConnectionKnob("PrevB2S", Direction.In, "PrevB2SDatas", ConnectionCount.Multi, NodeSide.Left, 30f)]
        [LabelText("左边的输入端")]
        public ValueConnectionKnob PrevSkill;

        [LabelText("右边的输出端")]
        [ValueConnectionKnob("NextB2S", Direction.Out, "NextB2SDatas", ConnectionCount.Multi, NodeSide.Right, 30f)]
        public ValueConnectionKnob NextSkill;

        [LabelText("上边的输入端")]
        [ValueConnectionKnob("PrevB2Sl", Direction.In, "PrevB2SDatas", ConnectionCount.Multi, NodeSide.Top, 75f)]
        public ValueConnectionKnob PrevSkill1;

        [LabelText("下边的输出端")]
        [ValueConnectionKnob("NextB2S1", Direction.Out, "NextB2SDatas", ConnectionCount.Multi, NodeSide.Bottom, 75f)]
        public ValueConnectionKnob NextSkill1;

        /// <summary>
        /// 碰撞关系数据
        /// </summary>
        public B2S_CollisionInstance MB2SCollisionInstance = new B2S_CollisionInstance();

        public override B2S_CollisionInstance B2SCollisionRelation_GetNodeData()
        {
            return this.MB2SCollisionInstance;
        }

        [Button("自动配置此Node碰撞关系", 25), GUIColor(0.4f, 0.8f, 1)]
        public void AutoSetCollisionRelations()
        {
            this.MB2SCollisionInstance.CollisionRelations.Clear();
            if (this.NextSkill.connections.Count > 0)
                foreach (var VARIABLE in this.NextSkill.connections)
                {
                    if (this.MB2SCollisionInstance.CollisionRelations == null)
                    {
                        this.MB2SCollisionInstance.CollisionRelations = new List<long>();
                    }

                    this.MB2SCollisionInstance.CollisionRelations.Add(VARIABLE.body.B2SCollisionRelation_GetNodeData().nodeDataId);
                }

            if (this.NextSkill1.connections.Count > 0)
                foreach (var VARIABLE in this.NextSkill1.connections)
                {
                    if (this.MB2SCollisionInstance.CollisionRelations == null)
                    {
                        this.MB2SCollisionInstance.CollisionRelations = new List<long>();
                    }

                    this.MB2SCollisionInstance.CollisionRelations.Add(VARIABLE.body.B2SCollisionRelation_GetNodeData().nodeDataId);
                }
        }

        public override void NodeGUI()
        {
            RTEditorGUI.TextField("标识：" + MB2SCollisionInstance.Flag);
        }
    }
}