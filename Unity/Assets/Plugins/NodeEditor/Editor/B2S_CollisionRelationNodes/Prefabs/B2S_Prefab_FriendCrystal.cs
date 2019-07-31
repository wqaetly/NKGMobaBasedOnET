//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月31日 14:53:22
//------------------------------------------------------------

using B2S_CollisionRelation;
using ETMode;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.NodeEditor
{
    [Node(false, "Box2DSharp/预制件/友方水晶", typeof (B2S_CollisionRelationCanvas))]
    public class B2S_Prefab_FriendCrystal:Node
    {
        /// <summary>
        /// 内部ID
        /// </summary>
        private const string Id = "友方水晶";

        public override string GetID => Id;

        public override Vector2 DefaultSize => new Vector2(150, 60);


        [ValueConnectionKnob("PrevB2S", Direction.In, "PrevB2SDatas", ConnectionCount.Multi, NodeSide.Left, 30f)]
        [LabelText("左边的输入端")]
        public ValueConnectionKnob PrevSkill;

        [LabelText("上边的输入端")]
        [ValueConnectionKnob("PrevB2Sl", Direction.In, "PrevB2SDatas", ConnectionCount.Multi, NodeSide.Top, 75f)]
        public ValueConnectionKnob PrevSkill1;
        
        /// <summary>
        /// 碰撞关系数据
        /// </summary>
        public B2S_CollisionInstance MB2SCollisionInstance = new B2S_CollisionInstance();
        
        public override B2S_CollisionInstance B2SCollisionRelation_GetNodeData()
        {
            return this.MB2SCollisionInstance;
        }

        private void OnEnable()
        {
            this.MB2SCollisionInstance.Flag = "友方水晶";
            this.MB2SCollisionInstance.nodeDataId = B2S_PrefabIDDefine.FriendCrystal;
            this.MB2SCollisionInstance.MB2SAllCollideableObject = B2S_AllCollideableObject.FriendCrystal;
        }
        public override void NodeGUI()
        {
            RTEditorGUI.TextField("标识：" + MB2SCollisionInstance.Flag);
        }
    }
}