namespace ET
{
    [LSF_MessageHandler(LSF_CmdHandlerType = LSF_PlaySkillInputCmd.CmdType)]
    public class LSF_PlayerSkillInputHandler : ALockStepStateFrameSyncMessageHandler<LSF_PlaySkillInputCmd>
    {
        protected override async ETVoid Run(Unit unit, LSF_PlaySkillInputCmd cmd)
        {
            foreach (var skillTree in unit.GetComponent<NP_RuntimeTreeManager>().RuntimeTrees)
            {
                skillTree.Value.GetBlackboard().Set(cmd.InputTag, cmd.InputKey);
                skillTree.Value.GetBlackboard().Set("SkillTargetAngle", cmd.Angle);
            }
            
#if SERVER
            // 对于客户端发来的每一条指令，都要进行一次广播，因为多人模式需要进行同步，
            LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();
            lsfComponent.AddCmdToSendQueue(cmd);
#endif

            await ETTask.CompletedTask;
        }
    }
}