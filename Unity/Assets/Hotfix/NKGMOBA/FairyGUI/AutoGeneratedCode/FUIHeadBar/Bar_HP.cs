/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class Bar_HPAwakeSystem : AwakeSystem<Bar_HP, GObject>
    {
        public override void Awake(Bar_HP self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class Bar_HP : FUI
    {	
        public const string UIPackageName = "FUIHeadBar";
        public const string UIResName = "Bar_HP";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GProgressBar self;
            
    public GImage bar;
    public const string URL = "ui://ny5r4rwcu6tfi";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static Bar_HP CreateInstance()
    {			
        return ComponentFactory.Create<Bar_HP, GObject>(CreateGObject());
    }
        
    public static ETTask<Bar_HP> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<Bar_HP> tcs = new ETTaskCompletionSource<Bar_HP>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<Bar_HP, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static Bar_HP Create(GObject go)
    {
        return ComponentFactory.Create<Bar_HP, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static Bar_HP GetFormPool(GObject go)
    {
        var fui = go.Get<Bar_HP>();
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
            
    		bar = (GImage)com.GetChildAt(0);
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
            
			bar = null;
		}
}
}