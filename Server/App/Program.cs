using System;
using System.Threading;
using System.Threading.Tasks;
using ETModel;
using NETCoreTest.Framework;
using NLog;

namespace App
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            try
            {
                BsonHelper.Init();
                //添加Model.dll到字典维护
                Game.EventSystem.Add(DLLType.Model, typeof (Game).Assembly);
                //添加Hotfix.dll到字典维护
                Game.EventSystem.Add(DLLType.Hotfix, DllHelper.GetHotfixAssembly());
                //添加并获取设置组件的引用
                Options options = Game.Scene.AddComponent<OptionComponent, string[]>(args).Options;
                //添加并获取初始配置的组件的引用
                StartConfig startConfig = Game.Scene.AddComponent<StartConfigComponent, string, int>(options.Config, options.AppId).StartConfig;

                //判断配置文件是否正确
                if (!options.AppType.Is(startConfig.AppType))
                {
                    Log.Error("命令行参数apptype与配置不一致");
                    return;
                }

                //配置文件相关
                IdGenerater.AppId = options.AppId;

                LogManager.Configuration.Variables["appType"] = $"{startConfig.AppType}";
                LogManager.Configuration.Variables["appId"] = $"{startConfig.AppId}";
                LogManager.Configuration.Variables["appTypeFormat"] = $"{startConfig.AppType,-8}";
                LogManager.Configuration.Variables["appIdFormat"] = $"{startConfig.AppId:0000}";

                Console.WriteLine($"server start........................ {startConfig.AppId} {startConfig.AppType}");

                //增加计时器组件
                Game.Scene.AddComponent<TimerComponent>();
                //增加OpcodeType组件（是双端通讯协议的重要组成部分）
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                //增加消息分发组件（确保从服务端收到的消息能正确的送到到接受者手中）
                Game.Scene.AddComponent<MessageDispatcherComponent>();

                // 根据不同的AppType添加不同的组件
                OuterConfig outerConfig = startConfig.GetComponent<OuterConfig>();
                InnerConfig innerConfig = startConfig.GetComponent<InnerConfig>();
                ClientConfig clientConfig = startConfig.GetComponent<ClientConfig>();

                switch (startConfig.AppType)
                {
                    case AppType.Manager:
                        Game.Scene.AddComponent<AppManagerComponent>();
                        Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
                        Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
                        break;
                    case AppType.Realm:
                        Game.Scene.AddComponent<MailboxDispatcherComponent>();
                        Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
                        Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
                        Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
                        Game.Scene.AddComponent<LocationProxyComponent>();
                        Game.Scene.AddComponent<RealmGateAddressComponent>();
                        break;
                    case AppType.Gate:
                        Game.Scene.AddComponent<PlayerComponent>();
                        Game.Scene.AddComponent<MailboxDispatcherComponent>();
                        Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
                        Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
                        Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
                        Game.Scene.AddComponent<LocationProxyComponent>();
                        Game.Scene.AddComponent<ActorMessageSenderComponent>();
                        Game.Scene.AddComponent<ActorLocationSenderComponent>();
                        Game.Scene.AddComponent<GateSessionKeyComponent>();
                        Game.Scene.AddComponent<CoroutineLockComponent>();
                        break;
                    case AppType.Location:
                        Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
                        Game.Scene.AddComponent<LocationComponent>();
                        Game.Scene.AddComponent<CoroutineLockComponent>();
                        break;
                    case AppType.Map:
                        Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
                        Game.Scene.AddComponent<UnitComponent>();
                        Game.Scene.AddComponent<LocationProxyComponent>();
                        Game.Scene.AddComponent<ActorMessageSenderComponent>();
                        Game.Scene.AddComponent<ActorLocationSenderComponent>();
                        Game.Scene.AddComponent<MailboxDispatcherComponent>();
                        Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
                        Game.Scene.AddComponent<RecastPathComponent>();
                        Game.Scene.AddComponent<CoroutineLockComponent>();
                        break;
                    case AppType.AllServer:
                        // 发送普通actor消息
                        Game.Scene.AddComponent<ActorMessageSenderComponent>();

                        // 发送location actor消息
                        Game.Scene.AddComponent<ActorLocationSenderComponent>();

                        //添加MongoDB组件，处理与服务器的交互
                        Game.Scene.AddComponent<DBComponent>();
                        //添加MongoDB代理组件，代理服务端对数据库的操作
                        Game.Scene.AddComponent<DBProxyComponent>();

                        // location server需要的组件
                        Game.Scene.AddComponent<LocationComponent>();

                        // 访问location server的组件
                        Game.Scene.AddComponent<LocationProxyComponent>();

                        // 这两个组件是处理actor消息使用的
                        Game.Scene.AddComponent<MailboxDispatcherComponent>();
                        Game.Scene.AddComponent<ActorMessageDispatcherComponent>();

                        // 内网消息组件
                        Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);

                        // 外网消息组件
                        Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);

                        // manager server组件，用来管理其它进程使用
                        Game.Scene.AddComponent<AppManagerComponent>();
                        Game.Scene.AddComponent<RealmGateAddressComponent>();
                        Game.Scene.AddComponent<GateSessionKeyComponent>();

                        Game.Scene.AddComponent<NumericWatcherComponent>();
                        // 配置管理
                        Game.Scene.AddComponent<ConfigComponent>();

                        //CD组件
                        Game.Scene.AddComponent<CDComponent>();

                        // recast寻路组件
                        Game.Scene.AddComponent<RecastPathComponent>();

                        //添加玩家组件（使用字典维护，可当做抽象化的玩家，处于不同的游戏流程会有不同的身份）
                        Game.Scene.AddComponent<PlayerComponent>();

                        //添加单位组件（这是游戏中物体的最小单元，继承自Entity）
                        Game.Scene.AddComponent<UnitComponent>();

                        Game.Scene.AddComponent<ConsoleComponent>();

                        //RealmGlobalComponent,增加在线组件，记录在线玩家
                        Game.Scene.AddComponent<OnlineComponent>();

                        //添加碰撞实例管理者 TODO 待优化，一场游戏一个碰撞实例管理者
                        Game.Scene.AddComponent<B2S_WorldColliderManagerComponent>();

                        //添加物理世界 TODO 待优化，一场游戏一个物理世界
                        Game.Scene.AddComponent<B2S_WorldComponent>();

                        //添加碰撞检测监听者 TODO 待优化，一场游戏一个碰撞检测监听者
                        Game.Scene.AddComponent<B2S_CollisionListenerComponent>();

                        //增加碰撞体数据仓库
                        Game.Scene.AddComponent<B2S_ColliderDataRepositoryComponent>();

                        Game.Scene.AddComponent<B2S_CollisionRelationRepositoryComponent>();

                        //增加英雄基础数据仓库组件
                        Game.Scene.AddComponent<HeroBaseDataRepositoryComponent>();
                        Game.Scene.AddComponent<CoroutineLockComponent>();

                        Game.Scene.AddComponent<NP_SyncComponent>();
                        Game.Scene.AddComponent<NP_TreeDataRepository>();
                        //战斗系统中的事件系统组件 TODO 待优化，一场游戏挂载一个战斗系统的事件系统
                        Game.Scene.AddComponent<BattleEventSystem>();
                        break;
                    case AppType.Benchmark:
                        Game.Scene.AddComponent<NetOuterComponent>();
                        Game.Scene.AddComponent<BenchmarkComponent, string>(clientConfig.Address);
                        break;
                    case AppType.BenchmarkWebsocketServer:
                        Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
                        break;
                    case AppType.BenchmarkWebsocketClient:
                        Game.Scene.AddComponent<NetOuterComponent>();
                        Game.Scene.AddComponent<WebSocketBenchmarkComponent, string>(clientConfig.Address);
                        break;
                    default:
                        throw new Exception($"命令行参数没有设置正确的AppType: {startConfig.AppType}");
                }

                //用于服务端逻辑帧更新，默认更新频率为30hz
                FixedUpdate fixedUpdate = new FixedUpdate()
                {
                    UpdateCallback = () =>
                    {
                        OneThreadSynchronizationContext.Instance.Update();
                        Game.EventSystem.FixedUpdate();
                        Game.EventSystem.Update();
                    }
                };

                while (true)
                {
                    try
                    {
                        Thread.Sleep(1);
                        fixedUpdate.Tick();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}