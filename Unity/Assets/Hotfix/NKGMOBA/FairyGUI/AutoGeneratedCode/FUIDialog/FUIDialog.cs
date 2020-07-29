/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUIDialogAwakeSystem : AwakeSystem<FUIDialog, GObject>
    {
        public override void Awake(FUIDialog self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUIDialog : FUI
    {	
        public const string UIPackageName = "FUIDialog";
        public const string UIResName = "FUIDialog";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    public GImage n0;
    public GTextField Tittle;
    public GTextField Conten;
    public tow_cancel tow_cancel;
    public one_confirm tow_confirm;
    public GGroup towmode;
    public one_confirm one_confirm;
    public GGroup onemode;
    public const string URL = "ui://3gqem46sifyf1";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static FUIDialog CreateInstance()
    {			
        return ComponentFactory.Create<FUIDialog, GObject>(CreateGObject());
    }
        
    public static ETTask<FUIDialog> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<FUIDialog> tcs = new ETTaskCompletionSource<FUIDialog>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<FUIDialog, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static FUIDialog Create(GObject go)
    {
        return ComponentFactory.Create<FUIDialog, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static FUIDialog GetFormPool(GObject go)
    {
        var fui = go.Get<FUIDialog>();
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
    		Tittle = (GTextField)com.GetChildAt(1);
    		Conten = (GTextField)com.GetChildAt(2);
    		tow_cancel = tow_cancel.Create(com.GetChildAt(3));
    		tow_confirm = one_confirm.Create(com.GetChildAt(4));
    		towmode = (GGroup)com.GetChildAt(5);
    		one_confirm = one_confirm.Create(com.GetChildAt(6));
    		onemode = (GGroup)com.GetChildAt(7);
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
			Tittle = null;
			Conten = null;
			tow_cancel = null;
			tow_confirm = null;
			towmode = null;
			one_confirm = null;
			onemode = null;
		}
}
}