/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class loginBtnAwakeSystem : AwakeSystem<loginBtn, GObject>
    {
        public override void Awake(loginBtn self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class loginBtn : FUI
    {	
        public const string UIPackageName = "FUILogin";
        public const string UIResName = "loginBtn";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GButton self;
            
    public Controller button;
    public GImage n0;
    public GImage n1;
    public GTextField title;
    public const string URL = "ui://2jxt4hn8pdjlb";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static loginBtn CreateInstance()
    {			
        return ComponentFactory.Create<loginBtn, GObject>(CreateGObject());
    }
        
    public static ETTask<loginBtn> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<loginBtn> tcs = new ETTaskCompletionSource<loginBtn>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<loginBtn, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static loginBtn Create(GObject go)
    {
        return ComponentFactory.Create<loginBtn, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static loginBtn GetFormPool(GObject go)
    {
        var fui = go.Get<loginBtn>();
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
            
    		button = com.GetControllerAt(0);
    		n0 = (GImage)com.GetChildAt(0);
    		n1 = (GImage)com.GetChildAt(1);
    		title = (GTextField)com.GetChildAt(2);
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
            
			button = null;
			n0 = null;
			n1 = null;
			title = null;
		}
}
}