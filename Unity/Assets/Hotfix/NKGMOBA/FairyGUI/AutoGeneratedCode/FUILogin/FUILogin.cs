/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUILoginAwakeSystem : AwakeSystem<FUILogin, GObject>
    {
        public override void Awake(FUILogin self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUILogin : FUI
    {	
        public const string UIPackageName = "FUILogin";
        public const string UIResName = "FUILogin";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    public GImage n0;
    public GImage n9;
    public GTextField n10;
    public Btn_Login Btn_Login;
    public Btn_Registe Btn_Registe;
    public GImage accountInput;
    public GImage passwordInput;
    public GTextInput accountText;
    public GTextField Tex_LoginInfo;
    public Btn_ToTestScene ToTestSceneBtn;
    public GTextInput passwordText;
    public GGroup n16;
    public Transition t0;
    public Transition t1;
    public const string URL = "ui://2jxt4hn8pdjl9";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static FUILogin CreateInstance()
    {			
        return ComponentFactory.Create<FUILogin, GObject>(CreateGObject());
    }
        
    public static ETTask<FUILogin> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<FUILogin> tcs = new ETTaskCompletionSource<FUILogin>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<FUILogin, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static FUILogin Create(GObject go)
    {
        return ComponentFactory.Create<FUILogin, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static FUILogin GetFormPool(GObject go)
    {
        var fui = go.Get<FUILogin>();
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
        
        self = (GComponent)go;
        
        self.Add(this);
        
        var com = go.asCom;
            
        if(com != null)
        {	
            
    		n0 = (GImage)com.GetChildAt(0);
    		n9 = (GImage)com.GetChildAt(1);
    		n10 = (GTextField)com.GetChildAt(2);
    		Btn_Login = Btn_Login.Create(com.GetChildAt(3));
    		Btn_Registe = Btn_Registe.Create(com.GetChildAt(4));
    		accountInput = (GImage)com.GetChildAt(5);
    		passwordInput = (GImage)com.GetChildAt(6);
    		accountText = (GTextInput)com.GetChildAt(7);
    		Tex_LoginInfo = (GTextField)com.GetChildAt(8);
    		ToTestSceneBtn = Btn_ToTestScene.Create(com.GetChildAt(9));
    		passwordText = (GTextInput)com.GetChildAt(10);
    		n16 = (GGroup)com.GetChildAt(11);
    		t0 = com.GetTransitionAt(0);
    		t1 = com.GetTransitionAt(1);
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
			n9 = null;
			n10 = null;
			Btn_Login = null;
			Btn_Registe = null;
			accountInput = null;
			passwordInput = null;
			accountText = null;
			Tex_LoginInfo = null;
			ToTestSceneBtn = null;
			passwordText = null;
			n16 = null;
			t0 = null;
			t1 = null;
		}
}
}