/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_FlyFontAwakeSystem : AwakeSystem<FUI_FlyFont, GObject>
    {
        public override void Awake(FUI_FlyFont self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_FlyFont : FUI
    {	
        public const string UIPackageName = "FlyFont";
        public const string UIResName = "FlyFont";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public GTextField m_Tex_ValueToFall;
    	public GImage m_n3;
    	public Transition m_FallingBleed;
    	public const string URL = "ui://u9w4nusth2bc0";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_FlyFont CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_FlyFont, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_FlyFont> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_FlyFont> tcs = ETTask<FUI_FlyFont>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_FlyFont, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_FlyFont Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_FlyFont, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_FlyFont GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_FlyFont>();
        
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
            
            self = (GComponent)go;
            
            self.Add(this);
            
            var com = go.asCom;
                
            if(com != null)
            {	
                
    			m_Tex_ValueToFall = (GTextField)com.GetChildAt(0);
    			m_n3 = (GImage)com.GetChildAt(1);
    			m_FallingBleed = com.GetTransitionAt(0);
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
            
    		m_Tex_ValueToFall = null;
    		m_n3 = null;
    		m_FallingBleed = null;
    	}
    }
}