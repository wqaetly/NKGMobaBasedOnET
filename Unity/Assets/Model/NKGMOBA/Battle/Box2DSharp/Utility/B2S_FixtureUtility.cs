//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月20日 18:45:40
//------------------------------------------------------------

using System.Collections.Generic;
using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;

namespace ET
{
    /// <summary>
    /// Box2D形状使用函数集
    /// </summary>
    public static class B2S_FixtureUtility
    {
        /// <summary>
        /// 为刚体挂载一个矩形碰撞体
        /// </summary>
        /// <param name="self"></param>
        /// <param name="hx">半宽</param>
        /// <param name="hy">半高</param>
        /// <param name="offset">偏移量</param>
        /// <param name="angle">角度</param>
        /// <param name="isSensor">是否为触发器</param>
        /// <param name="userData">用户自定义信息</param>
        public static void CreateBoxFixture(this Body self, float hx, float hy, Vector2 offset, float angle, bool isSensor, object userData)
        {
            PolygonShape m_BoxShape = new PolygonShape();
            m_BoxShape.SetAsBox(hx, hy, offset, angle);
            FixtureDef fixtureDef = new FixtureDef();
            fixtureDef.IsSensor = isSensor;
            fixtureDef.Shape = m_BoxShape;
            fixtureDef.UserData = userData;
            self.CreateFixture(fixtureDef);
        }

        /// <summary>
        /// 为刚体挂载一个圆形碰撞体
        /// </summary>
        /// <param name="self"></param>
        /// <param name="radius">半径</param>
        /// <param name="offset">偏移量</param>
        /// <param name="isSensor">是否为触发器</param>
        /// <param name="userData">用户自定义信息</param>
        public static void CreateCircleFixture(this Body self, float radius, Vector2 offset, bool isSensor, object userData)
        {
            CircleShape m_CircleShape = new CircleShape();
            m_CircleShape.Radius = radius;
            m_CircleShape.Position = offset;
            FixtureDef fixtureDef = new FixtureDef();
            fixtureDef.IsSensor = isSensor;
            fixtureDef.Shape = m_CircleShape;
            fixtureDef.UserData = userData;
            self.CreateFixture(fixtureDef);
        }

        /// <summary>
        /// 为刚体挂载一个多边形碰撞体
        /// </summary>
        /// <param name="self"></param>
        /// <param name="points">顶点数据</param>
        /// <param name="isSensor">是否为触发器</param>
        /// <param name="userData">用户自定义信息</param>
        public static void CreatePolygonFixture(this Body self, List<Vector2> points, bool isSensor, object userData)
        {
            PolygonShape m_PolygonShape = new PolygonShape();
            m_PolygonShape.Set(points.ToArray());
            FixtureDef fixtureDef3 = new FixtureDef();
            fixtureDef3.IsSensor = isSensor;
            fixtureDef3.Shape = m_PolygonShape;
            fixtureDef3.UserData = userData;
            self.CreateFixture(fixtureDef3);
        }
    }
}