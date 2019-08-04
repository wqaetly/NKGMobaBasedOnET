//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月13日 21:34:42
//------------------------------------------------------------

using ETMode;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
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

    public class CostumVector2
    {
        public float x;
        public float y;

        public CostumVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Clean()
        {
            this.x = 0;
            this.y = 0;
        }

        public void Fill(UnityEngine.Vector2 vector2)
        {
            this.x = vector2.x;
            this.y = vector2.y;
        }

        public Vector2 ToUnityVector2()
        {
            return new Vector2(this.x, this.y);
        }

        public System.Numerics.Vector2 ToSystemVector2()
        {
            return new System.Numerics.Vector2(this.x, this.y);
        }
    }

    public static class CostumVector2Helper
    {
        public static CostumVector2 UnityVector2ToCoustumVector2(this Vector2 theVector2WillbeConvert)
        {
            return new CostumVector2(theVector2WillbeConvert.x, theVector2WillbeConvert.y);
        }
    }

    public class B2S_ColliderDataStructureBase
    {
        [LabelText("碰撞体ID")]
        public long id;

        [LabelText("是否为触发器")]
        public bool isSensor;

        [LabelText("Box2D碰撞体类型")]
        public B2S_ColliderType b2SColliderType;

        [LabelText("碰撞体偏移信息")]
        public CostumVector2 offset = new CostumVector2(0, 0);
    }
}