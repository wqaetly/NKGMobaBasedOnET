using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class C2G_LoginGateHandler: AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
    {
        protected override async void Run(Session session, C2G_LoginGate message, Action<G2C_LoginGate> reply)
        {
            G2C_LoginGate response = new G2C_LoginGate();
            try
            {
                GateSessionKeyComponent gateSessionKeyComponent = Game.Scene.GetComponent<GateSessionKeyComponent>();
                //从已经分发的KEY里面寻找，如果没找到，说明非法用户，不给他连接gate服务器
                string account = gateSessionKeyComponent.Get(message.Key);
                if (account == null)
                {
                    response.Error = ErrorCode.ERR_ConnectGateKeyError;
                    response.Message = "Gate key验证失败!";
                    reply(response);
                    return;
                }

                //Key失效
                gateSessionKeyComponent.Remove(message.Key);

                //专门给这个玩家创建一个Player对象
                Player player = ComponentFactory.Create<Player, string>(account);
                player.AddComponent<UnitGateComponent, long>(session.InstanceId);

                //注册到PlayerComponent，方便管理
                Game.Scene.GetComponent<PlayerComponent>().Add(player);

                //给这个session安排上Player
                session.AddComponent<SessionPlayerComponent>().Player = player;

                // 增加掉线组件
                session.AddComponent<SessionOfflineComponent>();
                
                // 增加心跳包
                session.AddComponent<HeartBeatComponent>().CurrentTime = TimeHelper.ClientNowSeconds();

                //添加邮箱组件表示该session是一个Actor,接收的消息将会队列处理
                await session.AddComponent<MailBoxComponent, string>(MailboxType.GateSession).AddLocation();
                

                //向登录服务器发送玩家上线消息
                StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
                IPEndPoint realmIPEndPoint = config.RealmConfig.GetComponent<InnerConfig>().IPEndPoint;
                Session realmSession = Game.Scene.GetComponent<NetInnerComponent>().Get(realmIPEndPoint);

                await realmSession.Call(
                    new G2R_PlayerOnline() { playerAccount = account, PlayerId = player.Id, GateAppID = config.StartConfig.AppId });

                //回复客户端的连接gate服务器请求
                reply(response);

                //向客户端发送热更层信息
                session.Send(new G2C_TestHotfixMessage() { Info = "recv hotfix message success" });
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}