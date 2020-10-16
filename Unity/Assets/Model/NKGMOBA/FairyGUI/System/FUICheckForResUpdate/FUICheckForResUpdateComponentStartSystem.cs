//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年4月27日 11:25:25
//------------------------------------------------------------

using libx;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class FUICheckForResUpdateComponentStartSystem: StartSystem<FUICheckForResUpdateComponent>
    {
        public override void Start(FUICheckForResUpdateComponent self)
        {
            StartAsync(self).Coroutine();
        }

        private async ETVoid StartAsync(FUICheckForResUpdateComponent self)
        {
            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();
            long instanceId = self.InstanceId;
            while (true)
            {
                await timerComponent.WaitAsync(1);

                if (self.InstanceId != instanceId)
                {
                    return;
                }

                BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.GetComponent<BundleDownloaderComponent>();
                if (bundleDownloaderComponent == null)
                {
                    continue;
                }

                if (bundleDownloaderComponent.Updater.Step == Step.Versions)
                {
                    self.FUICheackForResUpdate.processbar.text = "正在为您检查资源更新...";
                    self.FUICheackForResUpdate.processbar.value = 0;
                }

                if (bundleDownloaderComponent.Updater.Step == Step.Prepared)
                {
                    self.FUICheackForResUpdate.processbar.text = "检查更新完毕";
                }
                
                if(bundleDownloaderComponent.Updater.Step == Step.Download)
                {
                    self.FUICheackForResUpdate.processbar.text = "正在为您更新资源：" + $"{bundleDownloaderComponent.Updater.UpdateProgress}%";
                    self.FUICheackForResUpdate.processbar.value = bundleDownloaderComponent.Updater.UpdateProgress;
                    if (bundleDownloaderComponent.Updater.UpdateProgress >= 100)
                    {
                        self.FUICheackForResUpdate.processbar.text = "资源更新完成，祝您游戏愉快。";
                    }
                }
            }
        }
    }
}