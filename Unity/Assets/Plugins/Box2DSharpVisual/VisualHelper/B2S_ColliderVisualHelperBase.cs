//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 20:58:09
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ETModel
{
    public abstract class B2S_ColliderVisualHelperBase
    {
        [InfoBox("将想要编辑的游戏对象的含有碰撞体的一阶子物体拖放到此处")]
        [HideLabel]
        [BsonIgnore]
        public GameObject theObjectWillBeEdited;

        /// <summary>
        /// 缓存的游戏对象，用于对比更新
        /// </summary>
        [HideInEditorMode]
        public GameObject CachedGameObject;

        [BsonIgnore]
        [HideInEditorMode]
        public Matrix4x4 matrix4X4;

        [ColorPalette]
        [Title("绘制线条颜色")]
        [HideLabel]
        [BsonIgnore]
        public Color mDrawColor = Color.red;

        [BsonIgnore]
        [HideInEditorMode]
        public bool canDraw;

        [DisableInEditorMode]
        [LabelText("映射文件保存路径")]
        public string NameAndIdInflectSavePath = "Assets/Res/EditorExtensionInfoSave/";

        [DisableInEditorMode]
        [LabelText("碰撞数据文件保存路径")]
        public string ColliderDataSavePath = "../Config/ColliderDatas/";

        [HideInEditorMode]
        public ColliderNameAndIdInflectSupporter MColliderNameAndIdInflectSupporter;

        [HideInEditorMode]
        public ColliderDataSupporter MColliderDataSupporter;

        public B2S_ColliderVisualHelperBase(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter,
        ColliderDataSupporter colliderDataSupporter)
        {
            this.MColliderNameAndIdInflectSupporter = colliderNameAndIdInflectSupporter;
            this.MColliderDataSupporter = colliderDataSupporter;
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

        /// <summary>
        /// 保存碰撞体信息
        /// </summary>
        public abstract void SavecolliderData();

        public abstract void OnUpdate();

        public void OnDrawGizmos()
        {
            Gizmos.color = this.mDrawColor;
            DrawCollider();
        }

        /// <summary>
        /// 删除此碰撞体相关所有信息
        /// </summary>
        public abstract void DeletecolliderData();

        /// <summary>
        /// 删除此类型碰撞体相关所有信息
        /// </summary>
        public abstract void DeleteAllcolliderData();
    }
}