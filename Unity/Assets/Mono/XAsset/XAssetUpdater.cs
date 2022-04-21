//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月21日 15:52:53
//------------------------------------------------------------

using libx;

namespace ET
{
    public class XAssetUpdater
    {
        public Updater Updater;

        public XAssetUpdater(Updater updater)
        {
            Updater = updater;
            Updater.Init();
        }
        
        public ETTask StartUpdate()
        {
            ETTask etTask = ETTask.Create(true);
            Updater.ResPreparedCompleted += () =>
            {
                etTask.SetResult();
            };
            Updater.StartUpdate();
            return etTask;
        }
    }
}