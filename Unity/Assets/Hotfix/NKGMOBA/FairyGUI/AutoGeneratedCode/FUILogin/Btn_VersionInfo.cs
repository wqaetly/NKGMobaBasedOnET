/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class Btn_VersionInfoAwakeSystem : AwakeSystem<Btn_VersionInfo, GObject>
    {
        public override void Awake(Btn_VersionInfo self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class Btn_VersionInfo : FUI
    {	
        public const string UIPackageName = "FUILogin";
        public const string UIResName = "Btn_VersionInfo";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GButton self;
            
    public GImage n0;
    public const string URL = "ui://2jxt4hn8r0zzsf";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static Btn_VersionInfo CreateInstance()
    {			
        return ComponentFactory.Create<Btn_VersionInfo, GObject>(CreateGObject());
    }
        
    public static ETTask<Btn_VersionInfo> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<Btn_VersionInfo> tcs = new ETTaskCompletionSource<Btn_VersionInfo>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<Btn_VersionInfo, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static Btn_VersionInfo Create(GObject go)
    {
        return ComponentFactory.Create<Btn_VersionInfo, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static Btn_VersionInfo GetFormPool(GObject go)
    {
        var fui = go.Get<Btn_VersionInfo>();
        if(fui == null)
        {
            fui = Create(go);
        }
        fui.isFromFGUIPool = true;
        return fui;
    }
        
    public void Awake(GObject go)
    {
        if(go == null)
        {
            return;
        }
        
        GObject = go;	
        
        if (string.IsNullOrWhiteSpace(Name))
        {
            Name = Id.ToString();
        }
        
        self = (GButton)go;
        
        self.Add(this);
        
        var com = go.asCom;
            
        if(com != null)
        {	
            
    		n0 = (GImage)com.GetChildAt(0);
    	}
}
       public override void Dispose()
       {
            if(IsDisposed)
            {
                return;
            }
            
            base.Dispose();
            
            self.Remove();
            self = null;
            
			n0 = null;
		}
}
}