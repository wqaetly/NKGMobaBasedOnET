using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using libx;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class UiBundleDownloaderComponentAwakeSystem: AwakeSystem<BundleDownloaderComponent>
    {
        public override void Awake(BundleDownloaderComponent self)
        {
            self.Updater = GameObject.FindObjectOfType<Updater>();
        }
    }
    
    [ObjectSystem]
    public class UiBundleDownloaderComponentSystem: UpdateSystem<BundleDownloaderComponent>
    {
        public override void Update(BundleDownloaderComponent self)
        {
            if (self.Updater.Step == Step.Completed)
            {
                self.Tcs.SetResult();
            }
        }
    }

    /// <summary>
    /// 封装XAsset Updater
    /// </summary>
    public class BundleDownloaderComponent: Component
    {
        public Updater Updater;

        public ETTaskCompletionSource Tcs;

        public ETTask StartUpdate()
        {
            Tcs = new ETTaskCompletionSource();
            Updater.ResPreparedCompleted = () =>
            {
                Tcs.SetResult();
            };
            Updater.StartUpdate();
            
            return Tcs.Task;
        }
    }
}