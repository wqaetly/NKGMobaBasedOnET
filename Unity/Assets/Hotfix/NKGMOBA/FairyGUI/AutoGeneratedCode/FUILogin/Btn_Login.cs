/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class Btn_LoginAwakeSystem : AwakeSystem<Btn_Login, GObject>
    {
        public override void Awake(Btn_Login self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class Btn_Login : FUI
    {	
        public const string UIPackageName = "FUILogin";
        public const string UIResName = "Btn_Login";
        
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
        
    public static Btn_Login CreateInstance()
    {			
        return ComponentFactory.Create<Btn_Login, GObject>(CreateGObject());
    }
        
    public static ETTask<Btn_Login> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<Btn_Login> tcs = new ETTaskCompletionSource<Btn_Login>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<Btn_Login, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static Btn_Login Create(GObject go)
    {
        return ComponentFactory.Create<Btn_Login, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static Btn_Login GetFormPool(GObject go)
    {
        var fui = go.Get<Btn_Login>();
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