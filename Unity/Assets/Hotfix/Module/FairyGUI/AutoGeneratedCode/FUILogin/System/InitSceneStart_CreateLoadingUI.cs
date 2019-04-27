using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.InitSceneStart)]
	public class InitSceneStart_CreateLoadingUI : AEvent
	{
        public override void Run()
        {
            var hotfixui = FUILogin.FUILogin.CreateInstance();
            //默认将会以Id为Name，也可以自定义Name，方便查询和管理
            hotfixui.Name = FUILogin.FUILogin.UIResName;
            hotfixui.MakeFullScreen();
            Game.Scene.GetComponent<FUIComponent>().Add(hotfixui, true);
        }
    }
}