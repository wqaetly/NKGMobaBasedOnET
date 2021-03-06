//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月5日 20:09:52
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.ShowOfflineDialogUI)]
    public class ShowDialogUI: AEvent<int, string, string>
    {
        public override void Run(int mode, string tittle, string content)
        {
            //Log.Info("服务端主动断开连接要显示对话框");
            var hotfixui = FUIDialog.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = FUIPackage.FUIDialog;
            if (mode == 1)
            {
                hotfixui.towmode.visible = false;
            }
            else
            {
                hotfixui.onemode.visible = false;
            }

            hotfixui.Tittle.text = tittle;
            hotfixui.Conten.text = content;
            
            hotfixui.one_confirm.self.onClick.Add(() =>
            {
                //关闭所有UI，回到登录注册界面
                Game.Scene.GetComponent<FUIComponent>().Clear();
                Game.EventSystem.Run(EventIdType.ShowLoginUI);
            });
            
            hotfixui.GObject.sortingOrder = 40;
            hotfixui.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }

    [Event(ETModel.EventIdType.ShowOfflineDialogUI_Model)]
    public class ShowHotfixDialogUI: AEvent<int, string, string>
    {
        public override void Run(int mode, string tittle, string content)
        {
            //Log.Info("因与服务端断开连接要显示对话框");
            var hotfixui = FUIDialog.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = FUIPackage.FUIDialog;
            if (mode == 1)
            {
                hotfixui.towmode.visible = false;
            }
            else
            {
                hotfixui.onemode.visible = false;
            }

            hotfixui.Tittle.text = tittle;
            hotfixui.Conten.text = content;

            hotfixui.one_confirm.self.onClick.Add(() =>
            {
                //关闭所有UI，回到登录注册界面
                Game.Scene.GetComponent<FUIComponent>().Clear();

                UnitComponent.Instance.MyUnit = null;
                UnitComponent.Instance.RemoveAll();
                
                MapHelper.ExitMap();
                
                Game.EventSystem.Run(EventIdType.ShowLoginUI);
            });

            hotfixui.GObject.sortingOrder = 40;
            hotfixui.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }

    [Event(EventIdType.CloseDialogUI)]
    public class CloseDialogUI: AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<FUIComponent>().Remove(FUIPackage.FUIDialog);
        }
    }
}