using System.Collections.Generic;
using System.Net;

namespace ET
{
    public class AppStart_Init : AEvent<EventType.AppStart>
    {
        protected override async ETTask Run(EventType.AppStart args)
        {
            Game.Scene.AddComponent<ConfigComponent>();

            await ConfigComponent.Instance.LoadAsync();

            //读取StartProcessConfig中id为Game.Options.Process（这里值为1）的整行配置
            //ET6.0由于使用了protobuffer作为导表工具，所以请去Excel文件夹查看原数据
            StartProcessConfig processConfig = StartProcessConfigCategory.Instance.Get(GlobalDefine.Options.Process);

            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();
            // 发送普通actor消息
            Game.Scene.AddComponent<ActorMessageSenderComponent>();
            // 发送location actor消息
            Game.Scene.AddComponent<ActorLocationSenderComponent>();
            // 访问location server的组件
            Game.Scene.AddComponent<LocationProxyComponent>();
            Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
            // 数值订阅组件
            Game.Scene.AddComponent<NumericWatcherComponent>();

            Game.Scene.AddComponent<NetThreadComponent>();

            // 管理所有玩家信息
            Game.Scene.AddComponent<PlayerComponent>();

            // 两个全局的函数处理封装组件
            Game.Scene.AddComponent<B2S_CollisionDispatcherComponent>();

            Game.Scene.AddComponent<LSF_CmdDispatcherComponent>();
            Game.Scene.AddComponent<LSF_TickDispatcherComponent>();

            // 添加数据库组件，可以查询数据
            StartZoneConfig startZoneConfig = StartZoneConfigCategory.Instance.Get(GlobalDefine.Options.Process);
            Game.Scene.AddComponent<DBComponent, string, string>(startZoneConfig.DBConnection, startZoneConfig.DBName);

            //根据自身的类型来决定要添加的组件
            switch (GlobalDefine.Options.AppType)
            {
                case AppType.Server:
                {
                    //是一个Sever就添加一个内网组件，用于内网通信
                    Game.Scene.AddComponent<NetInnerComponent, IPEndPoint>(processConfig.InnerIPPort);

                    //以GloabDefine.Options.Process为凭证获取自己要添加的Scene
                    //注意在StartSceneConfig.xlsx中有两份配置，第一份是给Server用的，第二份是给Robot用的
                    List<StartSceneConfig> processScenes =
                        StartSceneConfigCategory.Instance.GetByProcess(GlobalDefine.Options.Process);

                    foreach (StartSceneConfig startConfig in processScenes)
                    {
                        await SceneFactory.Create(Game.Scene, startConfig.Id, startConfig.InstanceId, startConfig.Zone,
                            startConfig.Name,
                            startConfig.Type, startConfig);
                    }

                    break;
                }
                case AppType.Watcher:
                {
                    //是一个守护进程就额外添加一个WatcherComponent，守护进程可以用来负责拉起和管理全部的进程
                    StartMachineConfig startMachineConfig = WatcherHelper.GetThisMachineConfig();
                    WatcherComponent watcherComponent = Game.Scene.AddComponent<WatcherComponent>();
                    watcherComponent.Start(GlobalDefine.Options.CreateScenes);
                    Game.Scene.AddComponent<NetInnerComponent, IPEndPoint>(
                        NetworkHelper.ToIPEndPoint($"{startMachineConfig.InnerIP}:{startMachineConfig.WatcherPort}"));
                    break;
                }
                case AppType.GameTool:
                    break;
            }

            if (GlobalDefine.Options.Console == 1)
            {
                Game.Scene.AddComponent<ConsoleComponent>();
            }
        }
    }
}