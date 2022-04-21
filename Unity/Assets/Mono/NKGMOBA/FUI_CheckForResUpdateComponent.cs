//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月26日 22:26:49
//------------------------------------------------------------

using libx;

namespace ET
{
    public class FUI_CheckForResUpdateComponent
    {
        private static Updater s_Updater;

        public static void Init(Updater updater)
        {
            s_Updater = updater;
            
            
            FUIEntry.LoadPackage_MonoOnly("CheckForResUpdate");

            FUI_CheckForResUpdateBinder.BindAll();
            FUI_CheckForResUpdate forResUpdate = FUI_CheckForResUpdate.CreateInstance();
            forResUpdate.MakeFullScreen();
            forResUpdate.m_processbar.max = 100;
            forResUpdate.m_processbar.value = 0;

            FUIManager_MonoOnly.AddUI(nameof(FUI_CheckForResUpdate), forResUpdate);

            s_Updater.OnStateUpdate += (msg) => { forResUpdate.m_updateInfo.text = msg; };
            s_Updater.OnProgressUpdate += (progress) => { forResUpdate.m_processbar.value = progress * 100; };

            s_Updater.ResPreparedCompleted += () =>
            {
                FUIManager_MonoOnly.RemoveUI(nameof(FUI_CheckForResUpdate));
                FUIEntry.RemovePackage_MonoOnly("CheckForResUpdate");
            };
        }
    }
}