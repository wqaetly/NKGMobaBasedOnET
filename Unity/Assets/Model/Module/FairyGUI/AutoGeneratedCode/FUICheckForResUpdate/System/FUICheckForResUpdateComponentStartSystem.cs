//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年4月27日 11:25:25
//------------------------------------------------------------

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
                await timerComponent.WaitAsync(100);

                if (self.InstanceId != instanceId)
                {
                    return;
                }

                BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.GetComponent<BundleDownloaderComponent>();
                if (bundleDownloaderComponent == null)
                {
                    continue;
                }

                if (bundleDownloaderComponent.CheckResBegin)
                {
                    if (!bundleDownloaderComponent.CheckResCompleted)
                    {
                        self.FUICheackForResUpdate.m_text.text = "正在为您检查资源更新：" + $"{bundleDownloaderComponent.CheckUpdateResProgress}%";
                        self.FUICheackForResUpdate.m_bar.value = bundleDownloaderComponent.CheckUpdateResProgress;
                    }
                    else
                    {
                        self.FUICheackForResUpdate.m_text.text = "正在为您更新资源：" + $"{bundleDownloaderComponent.UpdateResProgress}%";
                        self.FUICheackForResUpdate.m_bar.value = bundleDownloaderComponent.UpdateResProgress;
                    }
                }
            }
        }
    }

}