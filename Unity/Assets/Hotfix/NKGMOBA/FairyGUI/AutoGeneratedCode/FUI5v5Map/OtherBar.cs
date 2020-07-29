/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class OtherBarAwakeSystem : AwakeSystem<OtherBar, GObject>
    {
        public override void Awake(OtherBar self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class OtherBar : FUI
    {	
        public const string UIPackageName = "FUI5v5Map";
        public const string UIResName = "OtherBar";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GProgressBar self;
            
    public GGraph n0;
    public GImage bar;
    public const string URL = "ui://9sdz56q4clst5k";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static OtherBar CreateInstance()
    {			
        return ComponentFactory.Create<OtherBar, GObject>(CreateGObject());
    }
        
    public static ETTask<OtherBar> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<OtherBar> tcs = new ETTaskCompletionSource<OtherBar>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<OtherBar, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static OtherBar Create(GObject go)
    {
        return ComponentFactory.Create<OtherBar, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static OtherBar GetFormPool(GObject go)
    {
        var fui = go.Get<OtherBar>();
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
        
        self = (GProgressBar)go;
        
        self.Add(this);
        
        var com = go.asCom;
            
        if(com != null)
        {	
            
    		n0 = (GGraph)com.GetChildAt(0);
    		bar = (GImage)com.GetChildAt(1);
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
			bar = null;
		}
}
}