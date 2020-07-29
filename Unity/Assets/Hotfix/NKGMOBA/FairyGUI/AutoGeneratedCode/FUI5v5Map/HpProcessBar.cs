/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class HpProcessBarAwakeSystem : AwakeSystem<HpProcessBar, GObject>
    {
        public override void Awake(HpProcessBar self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class HpProcessBar : FUI
    {	
        public const string UIPackageName = "FUI5v5Map";
        public const string UIResName = "HpProcessBar";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GProgressBar self;
            
    public GGraph n0;
    public GImage bar;
    public const string URL = "ui://9sdz56q4clst5j";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static HpProcessBar CreateInstance()
    {			
        return ComponentFactory.Create<HpProcessBar, GObject>(CreateGObject());
    }
        
    public static ETTask<HpProcessBar> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<HpProcessBar> tcs = new ETTaskCompletionSource<HpProcessBar>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<HpProcessBar, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static HpProcessBar Create(GObject go)
    {
        return ComponentFactory.Create<HpProcessBar, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static HpProcessBar GetFormPool(GObject go)
    {
        var fui = go.Get<HpProcessBar>();
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