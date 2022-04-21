namespace ET
{
    public class M2C_FrameCmdHandler: AMHandler<M2C_FrameCmd>
    {
        protected override async ETVoid Run(Session session, M2C_FrameCmd message)
        {
            LSF_Component lsfComponent = session.DomainScene()
                .GetComponent<RoomManagerComponent>().BattleRoom.GetComponent<LSF_Component>();
            
            lsfComponent.RefreshNetInfo(message.ServerTimeSnap, message.CmdContent.Frame);
            //将消息加入待处理列表
            lsfComponent.AddCmdToHandleQueue(message.CmdContent);
            // Log.Info(TimeHelper.ClientNow().ToString());
            await ETTask.CompletedTask;
        }
    }
}