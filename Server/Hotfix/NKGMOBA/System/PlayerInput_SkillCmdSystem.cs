//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月3日 19:10:37
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    public static class PlayerInput_SkillCmdSystem
    {
        public static void BroadcastSkillCmd(Unit unit, string skillCmd)
        {
            M2C_UserInput_SkillCmd m2CUserInputSkillCmd = new M2C_UserInput_SkillCmd() { Message = skillCmd, Id = unit.Id };

            MessageHelper.Broadcast(m2CUserInputSkillCmd);
        }

        /// <summary>
        /// 广播碰撞体数据（Debug用，正式上线后请使用上面那个）
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="heroColliderData"></param>
        /// <param name="skillCmd"></param>
        public static void BroadcastB2S_ColliderData(Unit unit, B2S_HeroColliderData heroColliderData, string skillCmd)
        {
            M2C_UserInput_SkillCmd m2CUserInputSkillCmd = new M2C_UserInput_SkillCmd() { Message = skillCmd, Id = unit.Id };

            MessageHelper.Broadcast(m2CUserInputSkillCmd);

            foreach (var VARIABLE in heroColliderData.m_B2S_ColliderDataStructureBase)
            {
                switch (VARIABLE.b2SColliderType)
                {
                    case B2S_ColliderType.BoxColllider:
                        MessageHelper.Broadcast(new M2C_B2S_Debugger_Box()
                        {
                            Hx = ((B2S_BoxColliderDataStructure) VARIABLE).hx,
                            Hy = ((B2S_BoxColliderDataStructure) VARIABLE).hy,
                            Pos = new M2C_B2S_VectorBase()
                            {
                                X = heroColliderData.m_Body.GetWorldCenter().X, Y = heroColliderData.m_Body.GetWorldCenter().Y
                            },
                            OffsetInfo = new M2C_B2S_VectorBase() { X = VARIABLE.finalOffset.X, Y = VARIABLE.finalOffset.Y },
                            Id = unit.Id,
                            SustainTime = 1,
                        });
                        break;
                    case B2S_ColliderType.CircleCollider:
                        MessageHelper.Broadcast(new M2C_B2S_Debugger_Circle()
                        {
                            Radius = ((B2S_CircleColliderDataStructure) VARIABLE).radius,
                            Id = unit.Id,
                            Pos = new M2C_B2S_VectorBase()
                            {
                                X = heroColliderData.m_Body.GetWorldCenter().X, Y = heroColliderData.m_Body.GetWorldCenter().Y
                            },
                            OffsetInfo = new M2C_B2S_VectorBase() { X = VARIABLE.finalOffset.X, Y = VARIABLE.finalOffset.Y },
                            SustainTime = 1,
                        });
                        break;
                    case B2S_ColliderType.PolygonCollider:
                        foreach (var VARIABLE1 in ((B2S_PolygonColliderDataStructure) VARIABLE).points)
                        {
                            M2C_B2S_Debugger_Polygon test = new M2C_B2S_Debugger_Polygon()
                            {
                                Id = unit.Id,
                                OffsetInfo = new M2C_B2S_VectorBase() { X = VARIABLE.finalOffset.X, Y = VARIABLE.finalOffset.Y },
                                Pos = new M2C_B2S_VectorBase()
                                {
                                    X = heroColliderData.m_Body.GetWorldCenter().X, Y = heroColliderData.m_Body.GetWorldCenter().Y
                                },
                                SustainTime = 1,
                            };
                            foreach (var VARIABLE2 in VARIABLE1)
                            {
                                test.Vects.Add(new M2C_B2S_VectorBase() { X = VARIABLE2.x, Y = VARIABLE2.y });
                            }

                            MessageHelper.Broadcast(test);
                        }

                        break;
                }
            }
        }
    }
}