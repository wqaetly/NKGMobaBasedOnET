//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月29日 12:29:40
//------------------------------------------------------------

using System.Numerics;
using Box2DSharp.Collision.Shapes;
using ETHotfix;
using ETModel;

namespace EThotfix
{
    [Event(EventIdType.CreateCollider)]
    public class NP_CreateColliderAction: AEvent<long, long, long>
    {
        /// <summary>
        /// 创建一次一定要同步一次
        /// </summary>
        /// <param name="a">unitID</param>
        /// <param name="b">数据结点载体ID</param>
        /// <param name="c">数据ID</param>
        public override void Run(long a, long b, long c)
        {
            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(a);
            B2S_ColliderEntity colliderEntity = unit.GetComponent<B2S_HeroColliderDataManagerComponent>()
                    .CreateHeroColliderData(unit, b, c);

            //这里直接默认以英雄当前位置作为碰撞体生成的位置，如需提前指定位置，请在抛事件那里传参
            colliderEntity.SyncBody();
            Log.Info("生成技能碰撞体");

            //下面这一部分是Debug用的，稳定后请去掉
            {
                //广播碰撞体信息
                foreach (var VARIABLE in colliderEntity.m_Body.FixtureList)
                {
                    switch (VARIABLE.ShapeType)
                    {
                        case ShapeType.Polygon: //多边形
                            M2C_B2S_Debugger_Polygon test = new M2C_B2S_Debugger_Polygon() { Id = unit.Id, SustainTime = 2000, };
                            foreach (var VARIABLE1 in ((PolygonShape) VARIABLE.Shape).Vertices)
                            {
                                Vector2 worldPoint = colliderEntity.m_Body.GetWorldPoint(VARIABLE1);
                                test.Vects.Add(new M2C_B2S_VectorBase() { X = worldPoint.X, Y = worldPoint.Y });
                            }

                            MessageHelper.Broadcast(test);
                            break;
                        case ShapeType.Circle: //圆形
                            CircleShape myShape = (CircleShape) VARIABLE.Shape;
                            M2C_B2S_Debugger_Circle test1 = new M2C_B2S_Debugger_Circle()
                            {
                                Id = unit.Id,
                                SustainTime = 2000,
                                Radius = myShape.Radius,
                                Pos = new M2C_B2S_VectorBase()
                                {
                                    X = colliderEntity.m_Body.GetWorldPoint(myShape.Position).X,
                                    Y = colliderEntity.m_Body.GetWorldPoint(myShape.Position).Y
                                },
                            };
                            MessageHelper.Broadcast(test1);
                            //Log.Info($"是圆形，并且已经朝客户端发送绘制数据,半径为{myShape.Radius}");
                            break;
                    }
                }
            }
        }
    }
}