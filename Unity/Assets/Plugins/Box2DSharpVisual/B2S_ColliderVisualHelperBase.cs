//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 20:58:09
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ETModel
{
    public enum B2S_ColliderType
    {
        [LabelText("矩形碰撞体")]
        BoxColllider,

        [LabelText("圆形碰撞体")]
        CircleCollider,

        [LabelText("多边形碰撞体")]
        PolygonCollider,
    }

    public abstract class B2S_ColliderVisualHelperBase
    {
        [InfoBox("将想要编辑的游戏对象的含有碰撞体的一阶子物体拖放到此处")]
        [HideLabel]
        [BsonIgnore]
        public GameObject theObjectWillBeEdited;

        [LabelText("碰撞体ID")]
        public long id;

        [Title("碰撞体种类")]
        [HideLabel]
        [HideInEditorMode]
        [EnumToggleButtons]
        [GUIColor(0.9f, 0.4f, 9.5f)]
        public B2S_ColliderType mColliderShape;

        [BsonIgnore]
        [HideInEditorMode]
        public Matrix4x4 matrix4X4;

        [LabelText("碰撞体偏移信息")]
        [DisableInEditorMode]
        public Vector2 offsetInfo;
        
        [LabelText("碰撞体所包含的顶点信息(顺时针)")]
        [DisableInEditorMode]
        public List<Vector2> points = new List<Vector2>();

        [ColorPalette]
        [Title("绘制线条颜色")]
        [HideLabel]
        [BsonIgnore]
        public Color mDrawColor = Color.red;

        [HideInEditorMode]
        public int pointCount;

        [BsonIgnore]
        [HideInEditorMode]
        public bool canDraw;

        [DisableInEditorMode]
        [LabelText("映射文件保存路径")]
        public string SavePath = "Assets/Res/EditorExtensionInfoSave/";

        [HideInEditorMode]
        [BsonIgnore]
        public ColliderNameAndIdInflectSupporter McolliderNameAndIdInflectSupporter;

        public B2S_ColliderVisualHelperBase(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter)
        {
            this.McolliderNameAndIdInflectSupporter = colliderNameAndIdInflectSupporter;
        }

        /// <summary>
        /// 设置碰撞体基础信息
        /// </summary>
        public abstract void InitColliderBaseInfo();

        /// <summary>
        /// 重新绘制Box2DSharp
        /// </summary>
        public abstract void InitPointInfo();

        /// <summary>
        /// 重新绘制Box2DSharp
        /// </summary>
        public abstract void DrawCollider();

        /// <summary>
        /// 保存名称Id映射信息
        /// </summary>
        public abstract void SavecolliderNameAndIdInflect();

        public abstract void OnUpdate();

        public void OnDrawGizmos()
        {
            Gizmos.color = this.mDrawColor;
            DrawCollider();
        }
    }
}