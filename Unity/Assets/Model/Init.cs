using System;
using System.Threading;
using ETModel.NKGMOBA.Battle.State;
using libx;
using NETCoreTest.Framework;
using UnityEngine;

namespace ETModel
{
    public class Init: MonoBehaviour
    {
        public Camera MainCamera;

        private FixedUpdate fixedUpdate;

        private void Start()
        {
#if UNITY_EDITOR
            Define.ResModeIsEditor = this.GetComponent<Updater>().DevelopmentMode;
#else
            Define.ResModeIsEditor = false;
#endif
            this.StartAsync().Coroutine();
        }

        private async ETVoid StartAsync()
        {
            try
            {
                BsonHelper.Init();

                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
                DontDestroyOnLoad(gameObject);
                DontDestroyOnLoad(MainCamera);
                Game.EventSystem.Add(DLLType.Model, typeof (Init).Assembly);

                Game.Scene.AddComponent<NumericWatcherComponent>();

                fixedUpdate = new FixedUpdate() { UpdateCallback = () => Game.EventSystem.FixedUpdate() };

                Game.Scene.AddComponent<TimerComponent>();

                Game.Scene.AddComponent<GlobalConfigComponent>();
                Game.Scene.AddComponent<NetOuterComponent>();
                Game.Scene.AddComponent<ResourcesComponent>();
                Game.Scene.AddComponent<PlayerComponent>();
                Game.Scene.AddComponent<UnitComponent>();

                Game.Scene.AddComponent<FUIPackageComponent>();
                Game.Scene.AddComponent<FUIComponent>();
                Game.Scene.AddComponent<FUIInitComponent>();

                //用户输入管理组件
                Game.Scene.AddComponent<UserInputComponent>();
                Game.Scene.AddComponent<CampAllocManagerComponent>();
                Game.Scene.AddComponent<MouseTargetSelectorComponent>();

                Game.Scene.AddComponent<GameObjectPool>();

                // 下载ab包 
                await BundleHelper.DownloadBundle();

                Game.Hotfix.LoadHotfixAssembly();

                // 加载配置
                Game.Scene.AddComponent<ConfigComponent>();

                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatcherComponent>();

                Game.Scene.AddComponent<HeroBaseDataRepositoryComponent>();

                Game.Scene.AddComponent<B2S_DebuggerComponent>();

                Game.Hotfix.GotoHotfix();

                Game.Scene.AddComponent<NP_SyncComponent>();
                Game.Scene.AddComponent<NP_TreeDataRepository>();

                Game.Scene.AddComponent<SoundComponent>();
                
                //战斗系统的事件系统组件
                Game.Scene.AddComponent<BattleEventSystem>();
                //UnitFactory.NPBehaveTestCreate();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void Update()
        {
            OneThreadSynchronizationContext.Instance.Update();

            Game.Hotfix.Update?.Invoke();
            Game.EventSystem.Update();
        }

        private void FixedUpdate()
        {
            this.fixedUpdate.Tick();
        }

        private void LateUpdate()
        {
            Game.Hotfix.LateUpdate?.Invoke();
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.Hotfix.OnApplicationQuit?.Invoke();
            Game.Close();
        }
    }
}