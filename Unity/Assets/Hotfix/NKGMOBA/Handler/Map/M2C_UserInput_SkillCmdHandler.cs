//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月3日 18:59:36
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_UserInput_SkillCmdHandler: AMHandler<M2C_UserInput_SkillCmd>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_UserInput_SkillCmd message)
        {
            Unit unit = ETModel.Game.Scene.GetComponent<UnitComponent>().Get(message.Id);
            if (message.Message == "Q")
                unit.GetComponent<HeroSkillBehaveComponent>().OnQSkillPressed();
            foreach (var VARIABLE in unit.GetComponent<NP_RuntimeTreeManager>().RuntimeTrees)
            {
                VARIABLE.Value.GetBlackboard()["PlayerInput"] = message.Message;
            }
            await ETTask.CompletedTask;
        }
    }
}