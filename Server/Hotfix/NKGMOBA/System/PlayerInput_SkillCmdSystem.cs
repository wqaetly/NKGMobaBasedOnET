//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月3日 19:10:37
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    public static class PlayerInput_SkillCmdSystem
    {
        public static void BroadcastPath(Unit unit, string skillCmd)
        {
            M2C_UserInput_SkillCmd m2CUserInputSkillCmd = new M2C_UserInput_SkillCmd() { Message = skillCmd, Id = unit.Id };

            MessageHelper.Broadcast(m2CUserInputSkillCmd);
        }
    }
}