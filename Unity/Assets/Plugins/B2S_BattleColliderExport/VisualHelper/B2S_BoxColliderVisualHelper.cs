//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:01:06
//------------------------------------------------------------

#if UNITY_EDITOR


using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 矩形可视化辅助
    /// </summary>
    public class B2S_BoxColliderVisualHelper : B2S_ColliderVisualHelperBase
    {
        [DisableInEditorMode] [LabelText("映射文件名称")]
        public string NameAndIdInflectFileName = "BoxColliderNameAndIdInflect";

        [DisableInEditorMode] [LabelText("碰撞数据文件名称")]
        public string ColliderDataFileName = "BoxColliderData";

        [InlineEditor] [DisableInEditorMode] [Required("需要至少一个Unity2D矩形碰撞器")] [BsonIgnore]
        public BoxCollider2D mCollider2D;

        [LabelText("矩形碰撞体数据")]
        public B2S_BoxColliderDataStructure MB2S_BoxColliderDataStructure = new B2S_BoxColliderDataStructure();

        private Vector3[] Points = new Vector3[4];

        public override void InitColliderBaseInfo()
        {
            this.MB2S_BoxColliderDataStructure.b2SColliderType = B2S_ColliderType.BoxColllider;
        }

        [Button("重新绘制矩形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            base.InitPointInfo();

            BoxCollider2D tempBox2D = this.mCollider2D;
            // 这里不需要再去根据Go的缩放计算了，因为Unity已经帮我们计算好了
            this.MB2S_BoxColliderDataStructure.hx = tempBox2D.bounds.size.x / 2;
            this.MB2S_BoxColliderDataStructure.hy = tempBox2D.bounds.size.y / 2;

            var finalOffset = this.GoScaleAndRotMatrix4X4.MultiplyPoint(
                new Vector3(this.mCollider2D.offset.x, 0, this.mCollider2D.offset.y));

            MB2S_BoxColliderDataStructure.finalOffset = new System.Numerics.Vector2(finalOffset.x, finalOffset.z);

            // 从左上角开始顺时针计算顶点
            Points[0] = GoTranslateMatrix4X4.MultiplyPoint(new Vector3(
                -MB2S_BoxColliderDataStructure.hx + MB2S_BoxColliderDataStructure.finalOffset.X, 1,
                MB2S_BoxColliderDataStructure.hy + MB2S_BoxColliderDataStructure.finalOffset.Y));
            Points[1] = GoTranslateMatrix4X4.MultiplyPoint(new Vector3(
                MB2S_BoxColliderDataStructure.hx + MB2S_BoxColliderDataStructure.finalOffset.X, 1,
                MB2S_BoxColliderDataStructure.hy + MB2S_BoxColliderDataStructure.finalOffset.Y));
            Points[2] = GoTranslateMatrix4X4.MultiplyPoint(new Vector3(
                MB2S_BoxColliderDataStructure.hx + MB2S_BoxColliderDataStructure.finalOffset.X, 1,
                -MB2S_BoxColliderDataStructure.hy + MB2S_BoxColliderDataStructure.finalOffset.Y));
            Points[3] = GoTranslateMatrix4X4.MultiplyPoint(new Vector3(
                -MB2S_BoxColliderDataStructure.hx + MB2S_BoxColliderDataStructure.finalOffset.X, 1,
                -MB2S_BoxColliderDataStructure.hy + MB2S_BoxColliderDataStructure.finalOffset.Y));

            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(Points[i], Points[(i + 1) % 4]);
            }
        }

        [Button("保存所有矩形碰撞体名称与ID映射信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderNameAndIdInflect()
        {
            if (!this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(
                this.theObjectWillBeEdited.name))
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Add(this.theObjectWillBeEdited.name,
                    this.MB2S_BoxColliderDataStructure.id);
            }
            else
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic[this.theObjectWillBeEdited.name] =
                    this.MB2S_BoxColliderDataStructure.id;
            }

            using (FileStream file =
                File.Create($"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/{this.NameAndIdInflectFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
            }
        }

        [Button("保存所有矩形碰撞体信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                if (!this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_BoxColliderDataStructure.id))
                {
                    B2S_BoxColliderDataStructure b2SBoxColliderDataStructure = new B2S_BoxColliderDataStructure();
                    b2SBoxColliderDataStructure.id = MB2S_BoxColliderDataStructure.id;
                    b2SBoxColliderDataStructure.finalOffset.X = MB2S_BoxColliderDataStructure.finalOffset.X;
                    b2SBoxColliderDataStructure.finalOffset.Y = MB2S_BoxColliderDataStructure.finalOffset.Y;
                    b2SBoxColliderDataStructure.isSensor = MB2S_BoxColliderDataStructure.isSensor;
                    b2SBoxColliderDataStructure.b2SColliderType = MB2S_BoxColliderDataStructure.b2SColliderType;
                    b2SBoxColliderDataStructure.hx = MB2S_BoxColliderDataStructure.hx;
                    b2SBoxColliderDataStructure.hy = this.MB2S_BoxColliderDataStructure.hy;
                    this.MColliderDataSupporter.colliderDataDic.Add(this.MB2S_BoxColliderDataStructure.id,
                        b2SBoxColliderDataStructure);
                }
                else
                {
                    this.MColliderDataSupporter.colliderDataDic[this.MB2S_BoxColliderDataStructure.id] =
                        this.MB2S_BoxColliderDataStructure;
                }
            }

            using (FileStream file = File.Create($"{B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath}/{this.ColliderDataFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
            }
        }

        [Button("清除所有矩形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
        public override void DeleteAllcolliderData()
        {
            this.MColliderDataSupporter.colliderDataDic.Clear();
            this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Clear();

            using (FileStream file =
                File.Create($"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/{this.NameAndIdInflectFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
            }

            using (FileStream file = File.Create($"{B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath}/{this.ColliderDataFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
            }
        }

        [Button("清除此矩形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
        public override void DeletecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_BoxColliderDataStructure.id))
                {
                    this.MColliderDataSupporter.colliderDataDic.Remove(this.MB2S_BoxColliderDataStructure.id);
                }

                if (this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(
                    this.theObjectWillBeEdited.name))
                {
                    this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Remove(
                        this.theObjectWillBeEdited.name);
                }

                using (FileStream file =
                    File.Create($"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/{this.NameAndIdInflectFileName}.bytes"))
                {
                    BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
                }

                using (FileStream file = File.Create($"{B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath}/{this.ColliderDataFileName}.bytes"))
                {
                    BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
                }
            }
        }

        public override void OnUpdate()
        {
            if (CachedGameObject != theObjectWillBeEdited)
            {
                if (theObjectWillBeEdited != null)
                    CachedGameObject = theObjectWillBeEdited;
                ResetData();
                return;
            }

            if (theObjectWillBeEdited == null)
            {
                ResetData();
                return;
            }

            if (mCollider2D == null)
            {
                mCollider2D = theObjectWillBeEdited.GetComponent<BoxCollider2D>();
                if (mCollider2D == null)
                {
                    this.canDraw = false;
                }
            }

            if (this.MB2S_BoxColliderDataStructure.id == 0)
            {
                this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(
                    this.theObjectWillBeEdited.name,
                    out this.MB2S_BoxColliderDataStructure.id);
                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_BoxColliderDataStructure.id))
                {
                    this.MB2S_BoxColliderDataStructure =
                        (B2S_BoxColliderDataStructure) this.MColliderDataSupporter.colliderDataDic[
                            this.MB2S_BoxColliderDataStructure.id];
                }
            }
        }

        private void ResetData()
        {
            mCollider2D = null;
            this.canDraw = false;
            this.MB2S_BoxColliderDataStructure.id = 0;
            this.MB2S_BoxColliderDataStructure.isSensor = false;
            MB2S_BoxColliderDataStructure.finalOffset = System.Numerics.Vector2.Zero;
        }


        public B2S_BoxColliderVisualHelper(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter,
            ColliderDataSupporter colliderDataSupporter) : base(colliderNameAndIdInflectSupporter,
            colliderDataSupporter)
        {
        }
    }
}

#endif