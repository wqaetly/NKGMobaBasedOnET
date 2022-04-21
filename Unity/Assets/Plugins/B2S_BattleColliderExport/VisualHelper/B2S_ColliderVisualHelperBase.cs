//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 20:58:09
//------------------------------------------------------------

#if UNITY_EDITOR


using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public abstract class B2S_ColliderVisualHelperBase
    {
        [InfoBox("将想要编辑的游戏对象拖放到此处")]
        [HideLabel]
        [BsonIgnore]
        public GameObject theObjectWillBeEdited;

        /// <summary>
        /// 缓存的游戏对象，用于对比更新
        /// </summary>
        [HideInEditorMode]
        public GameObject CachedGameObject;

        /// <summary>
        /// 拖放进来的游戏对象平移矩阵，因为我们还有要将碰撞体显示在Scene视图，所以要加一个移动矩阵
        /// </summary>
        [BsonIgnore]
        [HideInEditorMode]
        public Matrix4x4 GoTranslateMatrix4X4;

        /// <summary>
        /// 拖放进来的游戏对象矩阵（仅包含缩放和旋转信息）
        /// 之所以没有平移信息，因为我们在初始化和保存Box2d碰撞信息时就已经将缩放和旋转应用到其数据中了（可以想象成在世界坐标原点处实例化了一个Box2d碰撞体），到此为止，我们Box2d碰撞体信息已经准本完毕了
        /// </summary>
        public Matrix4x4 GoScaleAndRotMatrix4X4;
        
        [ColorPalette]
        [Title("绘制线条颜色")]
        [HideLabel]
        [BsonIgnore]
        public Color mDrawColor = Color.red;

        [BsonIgnore]
        [HideInEditorMode]
        public bool canDraw;

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
        public virtual void InitPointInfo()
        {
            GoScaleAndRotMatrix4X4 = Matrix4x4.TRS(Vector3.zero, theObjectWillBeEdited.transform.rotation, theObjectWillBeEdited.transform.localScale);
            GoTranslateMatrix4X4 = Matrix4x4.Translate(theObjectWillBeEdited.transform.position);
        }

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
#endif
