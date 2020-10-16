using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public static class BundleHelper
    {
        public static async ETTask DownloadBundle()
        {
            using (BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.AddComponent<BundleDownloaderComponent>())
            {
                Game.EventSystem.Run(EventIdType.CheckForUpdateBegin);
                Game.EventSystem.Run(EventIdType.CreateLoadingUI);

                await bundleDownloaderComponent.StartUpdate();
                
                Log.Info("资源热更完成，进入大厅");
            }
        }
    }
}