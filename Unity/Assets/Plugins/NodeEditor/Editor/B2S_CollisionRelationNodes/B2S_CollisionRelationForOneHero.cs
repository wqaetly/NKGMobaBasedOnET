//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月25日 16:52:27
//------------------------------------------------------------

using B2S_CollisionRelation;
using ETMode;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.NodeEditor
{
    [Node(false, "Box2DSharp/碰撞关系图", typeof (B2S_CollisionRelationCanvas))]
    public class B2S_CollisionRelationForOneHero: Node
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "碰撞关系结点";

        /// <summary>
        /// 内部ID
        /// 内部ID
        /// </summary>
        public override string GetID => Id;

        public override Vector2 DefaultSize => new Vector2(100, 60);

        [LabelText("为这个节点设置一个标识吧！")]
        public string Flag;

        /// <summary>
        /// 碰撞关系数据
        /// </summary>
        public B2S_CollisionInstance MB2SCollisionInstance = new B2S_CollisionInstance();

        public override B2S_CollisionInstance B2SCollisionRelation_GetNodeData()
        {
            return this.MB2SCollisionInstance;
        }

        public override void NodeGUI()
        {
            RTEditorGUI.TextField("标识：" + Flag);
        }
    }
}