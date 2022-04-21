using System;
using System.Reflection;
using System.Threading;
using FairyGUI;
using libx;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace ET
{
    public class Init : MonoSingleton<Init>
    {
        [HideInInspector] public bool ILRuntimeMode = false;

        [Tooltip("验证服地址")]
        [LabelText("验证服地址")]
        [HideIf("DevelopMode")]
        public string LoginAddress = "127.0.0.1:10002";
        
        [Tooltip("如果开启，将直连本地的服务端并且以编辑器模式加载资源")]
        [LabelText("本地调试模式")]
        public bool DevelopMode;

        private XAssetUpdater m_XAssetUpdater;

        /// <summary>
        /// 来自Xenko的固定时间间隔刷新器
        /// </summary>
        private FixedUpdate m_FixedUpdate;
        
        public Camera MainCamera;
        
        private void Awake()
        {
            InternalAwake().Coroutine();
        }

        private async ETVoid InternalAwake()
        {
            try
            {
                // 设置全局模式
                GlobalDefine.ILRuntimeMode = this.ILRuntimeMode;
                GlobalDefine.DevelopMode = this.DevelopMode;
                GlobalDefine.SetLoginAddress(LoginAddress);
                
                // 吸住
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;

                SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

                DontDestroyOnLoad(gameObject);
                DontDestroyOnLoad(MainCamera);

                // 初始化FGUI系统
                FUIEntry.Init();

                Updater updater = this.GetComponent<Updater>();
                //updater.DevelopmentMode = DevelopMode;

                m_XAssetUpdater = new XAssetUpdater(updater);

                FUI_LoadingComponent.Init();
                FUI_CheckForResUpdateComponent.Init(m_XAssetUpdater.Updater);

                await m_XAssetUpdater.StartUpdate();

                HotfixHelper.GoToHotfix();

                GlobalLifeCycle.StartAction?.Invoke();
                
                // 注册FixedUpdate，注意必须是在这实例化，否则会因为初始化时间与下一个Tick时间间隔过长而产生追帧操作，造成大量的重复执行后果
                m_FixedUpdate = new FixedUpdate(){UpdateCallback = GlobalLifeCycle.FixedUpdateAction};
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        private void Update()
        {
            GlobalLifeCycle.UpdateAction?.Invoke();
            //让Update去驱动固定间隔定时器(FxiedUpdate)，更加准确
            m_FixedUpdate?.Tick();
        }

        private void LateUpdate()
        {
            GlobalLifeCycle.LateUpdateAction?.Invoke();
            GlobalLifeCycle.FrameFinishAction?.Invoke();
        }

        private void OnApplicationQuit()
        {
            GlobalLifeCycle.OnApplicationQuitAction?.Invoke();
        }
    }
}