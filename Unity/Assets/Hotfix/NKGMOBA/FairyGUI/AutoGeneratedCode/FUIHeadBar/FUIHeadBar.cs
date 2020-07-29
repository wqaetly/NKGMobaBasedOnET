/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUIHeadBarAwakeSystem : AwakeSystem<FUIHeadBar, GObject>
    {
        public override void Awake(FUIHeadBar self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUIHeadBar : FUI
    {	
        public const string UIPackageName = "FUIHeadBar";
        public const string UIResName = "FUIHeadBar";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    public GImage n16;
    public GTextField Tex_Level;
    public GTextField Tex_PlayerName;
    public GImage n15;
    public Bar_HP Bar_HP;
    public GList HPGapList;
    public Bar_MP Bar_MP;
    public const string URL = "ui://ny5r4rwcqrur4";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static FUIHeadBar CreateInstance()
    {			
        return ComponentFactory.Create<FUIHeadBar, GObject>(CreateGObject());
    }
        
    public static ETTask<FUIHeadBar> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<FUIHeadBar> tcs = new ETTaskCompletionSource<FUIHeadBar>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<FUIHeadBar, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static FUIHeadBar Create(GObject go)
    {
        return ComponentFactory.Create<FUIHeadBar, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static FUIHeadBar GetFormPool(GObject go)
    {
        var fui = go.Get<FUIHeadBar>();
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
            
    		n16 = (GImage)com.GetChildAt(0);
    		Tex_Level = (GTextField)com.GetChildAt(1);
    		Tex_PlayerName = (GTextField)com.GetChildAt(2);
    		n15 = (GImage)com.GetChildAt(3);
    		Bar_HP = Bar_HP.Create(com.GetChildAt(4));
    		HPGapList = (GList)com.GetChildAt(5);
    		Bar_MP = Bar_MP.Create(com.GetChildAt(6));
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
            
			n16 = null;
			Tex_Level = null;
			Tex_PlayerName = null;
			n15 = null;
			Bar_HP = null;
			HPGapList = null;
			Bar_MP = null;
		}
}
}