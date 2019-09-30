//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月3日 19:06:48
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class UserInput_SkillCmdHandler: AMActorLocationHandler<Unit, UserInput_SkillCmd>
    {
        protected override async ETTask Run(Unit entity, UserInput_SkillCmd message)
        {
            M2C_UserInput_SkillCmd m2CUserInputSkillCmd = new M2C_UserInput_SkillCmd() { Message = message.Message, Id = entity.Id };
            foreach (var VARIABLE in entity.GetComponent<NP_RuntimeTreeManager>().RuntimeTrees)
            {
                VARIABLE.Value.GetBlackboard()["PlayerInput"] = message.Message;
            }
            //广播技能指令(BroadcastSkillCmd),让客户端行为树做出反应
            MessageHelper.Broadcast(m2CUserInputSkillCmd);
            await ETTask.CompletedTask;
        }
    }
}