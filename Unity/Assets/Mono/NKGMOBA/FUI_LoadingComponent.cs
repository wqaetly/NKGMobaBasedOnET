using FairyGUI;

namespace ET
{
    public class FUI_LoadingComponent
    {
        private static Window s_LoadingModalWindow;

        public static void Init()
        {
            FUIEntry.LoadPackage_MonoOnly("Loading");

            FUI_LoadingBinder.BindAll();

            s_LoadingModalWindow = new Window();
            s_LoadingModalWindow.contentPane =  FUI_Loading.CreateInstance();
            s_LoadingModalWindow.MakeFullScreen();
            s_LoadingModalWindow.sortingOrder = 9999;
        }

        public static void ShowLoadingUI()
        {
            s_LoadingModalWindow.Show();
        }

        public static void HideLoadingUI()
        {
            s_LoadingModalWindow.Hide();
        }
    }
}