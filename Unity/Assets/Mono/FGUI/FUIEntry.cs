
using FairyGUI;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 初始化Model层的检查资源更新UI，以及加载圆圈
    /// </summary>
    public class FUIEntry
    {
        //暂时不需要设置字体
        //public const string DefaultFont = "FZXuanZhenZhuanBianS-R-GB";

        public static void Init()
        {
            //对于FGUI来说，其内部在执行 `UIPackage.RemovePackage` 时会进行`ab.Unload(true)`操作，应该是个很贴心的设计，但我们xasset需要管理资源的引用计数，所以不需要这个贴心的功能，故：
            UIPackage.unloadBundleByFGUI = false;
            
            //设置分辨率
			GRoot.inst.SetContentScaleFactor(1280, 720, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
            
            //设置CustomLoader
            UIObjectFactory.SetLoaderExtension(typeof(NKGGLoader));
        }

        public static void Clean()
        {

        }

        public static void LoadPackage_MonoOnly(string name)
        {
            UIPackage.AddPackage($"FGUI/{name}");
        }
        
        public static void RemovePackage_MonoOnly(string name)
        {
            UIPackage.RemovePackage($"FGUI/{name}");
        }
    }
}