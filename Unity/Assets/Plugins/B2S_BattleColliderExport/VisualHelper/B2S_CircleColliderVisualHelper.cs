//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月10日 21:01:36
//------------------------------------------------------------

#if UNITY_EDITOR


using System.IO;
using System.Numerics;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace ET
{
    /// <summary>
    /// 圆形可视化辅助
    /// </summary>
    public class B2S_CircleColliderVisualHelper : B2S_ColliderVisualHelperBase
    {
        [DisableInEditorMode] [LabelText("映射文件名称")]
        public string NameAndIdInflectFileName = "CircleColliderNameAndIdInflect";

        [DisableInEditorMode] [LabelText("碰撞数据文件名称")]
        public string ColliderDataFileName = "CircleColliderData";

        [InlineEditor] [DisableInEditorMode] [Required("需要至少一个Unity2D圆形碰撞器")] [HideLabel] [BsonIgnore]
        public CircleCollider2D mCollider2D;

        [LabelText("圆形碰撞体数据")]
        public B2S_CircleColliderDataStructure MB2S_CircleColliderDataStructure = new B2S_CircleColliderDataStructure();

        [BsonIgnore] [LabelText("圆线段数")] public int Segments = 66;

        public override void InitColliderBaseInfo()
        {
            this.MB2S_CircleColliderDataStructure.b2SColliderType = B2S_ColliderType.CircleCollider;
        }

        [Button("重新绘制圆形碰撞体", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void InitPointInfo()
        {
            base.InitPointInfo();
            this.MB2S_CircleColliderDataStructure.radius =
                this.mCollider2D.radius * this.theObjectWillBeEdited.transform.localScale.x;

            var finalOffset = this.GoScaleAndRotMatrix4X4.MultiplyPoint(
                new Vector3(MB2S_CircleColliderDataStructure.finalOffset.X, 0,
                    MB2S_CircleColliderDataStructure.finalOffset.Y));
            MB2S_CircleColliderDataStructure.finalOffset = new System.Numerics.Vector2(finalOffset.x, finalOffset.z);
            this.canDraw = true;
        }

        public override void DrawCollider()
        {
            var step = Mathf.RoundToInt(360f / Segments);

            Vector3 startPoint = GoTranslateMatrix4X4.MultiplyPoint(new Vector3(
                MB2S_CircleColliderDataStructure.radius *
                Mathf.Cos(0 * Mathf.Deg2Rad) + MB2S_CircleColliderDataStructure.finalOffset.X, 1,
                MB2S_CircleColliderDataStructure.radius *
                Mathf.Sin(0 * Mathf.Deg2Rad) + MB2S_CircleColliderDataStructure.finalOffset.Y));
            for (int i = step; i <= 360; i += step)
            {
                var nextPoint = GoTranslateMatrix4X4.MultiplyPoint(new Vector3(
                    MB2S_CircleColliderDataStructure.radius *
                    Mathf.Cos(i * 1.0f * Mathf.Deg2Rad) + MB2S_CircleColliderDataStructure.finalOffset.X, 1,
                    MB2S_CircleColliderDataStructure.radius *
                    Mathf.Sin(i * 1.0f * Mathf.Deg2Rad) + MB2S_CircleColliderDataStructure.finalOffset.Y));
                Gizmos.DrawLine(startPoint, nextPoint);
                startPoint = nextPoint;
            }
        }

        [Button("保存所有圆形碰撞体名称与ID映射信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderNameAndIdInflect()
        {
            if (!this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.ContainsKey(
                this.theObjectWillBeEdited.name))
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.Add(this.theObjectWillBeEdited.name,
                    this.MB2S_CircleColliderDataStructure.id);
            }
            else
            {
                MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic[this.theObjectWillBeEdited.name] =
                    this.MB2S_CircleColliderDataStructure.id;
            }

            using (FileStream file =
                File.Create($"{B2S_BattleColliderExportPathDefine.ColliderNameAndIdInflectSavePath}/{this.NameAndIdInflectFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderNameAndIdInflectSupporter);
            }
        }

        [Button("保存所有圆形碰撞体信息", 25), GUIColor(0.2f, 0.9f, 1.0f)]
        public override void SavecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                if (!this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_CircleColliderDataStructure.id))
                {
                    B2S_CircleColliderDataStructure b2SCircleColliderDataStructure =
                        new B2S_CircleColliderDataStructure();
                    b2SCircleColliderDataStructure.id = MB2S_CircleColliderDataStructure.id;
                    b2SCircleColliderDataStructure.finalOffset.X = MB2S_CircleColliderDataStructure.finalOffset.X;
                    b2SCircleColliderDataStructure.finalOffset.Y = MB2S_CircleColliderDataStructure.finalOffset.Y;
                    b2SCircleColliderDataStructure.isSensor = MB2S_CircleColliderDataStructure.isSensor;
                    b2SCircleColliderDataStructure.b2SColliderType = MB2S_CircleColliderDataStructure.b2SColliderType;
                    b2SCircleColliderDataStructure.radius = MB2S_CircleColliderDataStructure.radius;
                    this.MColliderDataSupporter.colliderDataDic.Add(this.MB2S_CircleColliderDataStructure.id,
                        b2SCircleColliderDataStructure);
                }
                else
                {
                    this.MColliderDataSupporter.colliderDataDic[this.MB2S_CircleColliderDataStructure.id] =
                        this.MB2S_CircleColliderDataStructure;
                }
            }

            using (FileStream file = File.Create($"{B2S_BattleColliderExportPathDefine.ServerColliderDataSavePath}/{this.ColliderDataFileName}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), this.MColliderDataSupporter);
            }
        }

        [Button("清除所有圆形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
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

        [Button("清除此圆形碰撞体信息", 25), GUIColor(1.0f, 20 / 255f, 147 / 255f)]
        public override void DeletecolliderData()
        {
            if (this.theObjectWillBeEdited != null && this.mCollider2D != null)
            {
                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_CircleColliderDataStructure.id))
                {
                    this.MColliderDataSupporter.colliderDataDic.Remove(this.MB2S_CircleColliderDataStructure.id);
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
                this.ResetData();
                return;
            }

            if (theObjectWillBeEdited == null)
            {
                this.ResetData();
                return;
            }

            if (this.MB2S_CircleColliderDataStructure.id == 0)
            {
                if (this.MColliderNameAndIdInflectSupporter.colliderNameAndIdInflectDic.TryGetValue(
                    this.theObjectWillBeEdited.name,
                    out this.MB2S_CircleColliderDataStructure.id))
                {
                    Debug.Log($"自动设置圆形碰撞体ID成功，ID为{MB2S_CircleColliderDataStructure.id}");
                }

                if (this.MColliderDataSupporter.colliderDataDic.ContainsKey(this.MB2S_CircleColliderDataStructure.id))
                {
                    this.MB2S_CircleColliderDataStructure =
                        (B2S_CircleColliderDataStructure) this.MColliderDataSupporter.colliderDataDic[
                            this.MB2S_CircleColliderDataStructure.id];
                }
            }

            if (mCollider2D == null)
            {
                mCollider2D = theObjectWillBeEdited.GetComponent<CircleCollider2D>();
                if (mCollider2D == null)
                {
                    this.canDraw = false;
                }
            }
        }

        private void ResetData()
        {
            mCollider2D = null;
            this.canDraw = false;
            this.MB2S_CircleColliderDataStructure.id = 0;
            this.MB2S_CircleColliderDataStructure.radius = 0;
            MB2S_CircleColliderDataStructure.finalOffset = System.Numerics.Vector2.Zero;
            this.MB2S_CircleColliderDataStructure.isSensor = false;
        }

        public B2S_CircleColliderVisualHelper(ColliderNameAndIdInflectSupporter colliderNameAndIdInflectSupporter,
            ColliderDataSupporter colliderDataSupporter) : base(colliderNameAndIdInflectSupporter,
            colliderDataSupporter)
        {
        }
    }
}

#endif