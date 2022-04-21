/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_BuffProBarAwakeSystem : AwakeSystem<FUI_BuffProBar, GObject>
    {
        public override void Awake(FUI_BuffProBar self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_BuffProBar : FUI
    {	
        public const string UIPackageName = "BattleMain";
        public const string UIResName = "BuffProBar";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GProgressBar self;
            
    	public GImage m_n2;
    	public GImage m_bar;
    	public const string URL = "ui://9sdz56q4qraf6c";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_BuffProBar CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_BuffProBar, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_BuffProBar> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_BuffProBar> tcs = ETTask<FUI_BuffProBar>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_BuffProBar, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_BuffProBar Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_BuffProBar, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_BuffProBar GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_BuffProBar>();
        
            if(fui == null)
            {
                fui = Create(domain, go);
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
                
    			m_n2 = (GImage)com.GetChildAt(0);
    			m_bar = (GImage)com.GetChildAt(1);
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
            
    		m_n2 = null;
    		m_bar = null;
    	}
    }
}