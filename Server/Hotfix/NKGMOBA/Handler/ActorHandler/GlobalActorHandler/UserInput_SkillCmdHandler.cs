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
            entity.GetComponent<B2S_HeroColliderDataManagerComponent>().CreateHeroColliderData(10001, 10001);
            Log.Info("创建碰撞体完成");
            PlayerInput_SkillCmdSystem.BroadcastPath(entity, message.Message);
            await ETTask.CompletedTask;
        }
    }
}