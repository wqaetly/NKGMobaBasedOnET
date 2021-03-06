//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月3日 18:59:36
//------------------------------------------------------------

using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_UserInput_SkillCmdHandler: AMHandler<M2C_UserInput_SkillCmd>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_UserInput_SkillCmd message)
        {
            Unit unit = UnitComponent.Instance.Get(message.Id);
            if (unit == null) return;
            foreach (var runtimeTree in unit.GetComponent<NP_RuntimeTreeManager>().RuntimeTrees)
            {
                runtimeTree.Value.GetBlackboard().Set("PlayerInput", message.Message);
            }

            await ETTask.CompletedTask;
        }
    }
}