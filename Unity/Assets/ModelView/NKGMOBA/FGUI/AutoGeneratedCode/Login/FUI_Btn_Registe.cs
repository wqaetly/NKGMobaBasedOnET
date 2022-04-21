/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_Btn_RegisteAwakeSystem : AwakeSystem<FUI_Btn_Registe, GObject>
    {
        public override void Awake(FUI_Btn_Registe self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_Btn_Registe : FUI
    {	
        public const string UIPackageName = "Login";
        public const string UIResName = "Btn_Registe";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GButton self;
            
    	public Controller m_button;
    	public GImage m_n0;
    	public GImage m_n1;
    	public GTextField m_title;
    	public const string URL = "ui://2jxt4hn8pdjla";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Btn_Registe CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Btn_Registe, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_Btn_Registe> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_Btn_Registe> tcs = ETTask<FUI_Btn_Registe>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_Btn_Registe, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Btn_Registe Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Btn_Registe, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Btn_Registe GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Btn_Registe>();
        
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
            
            self = (GButton)go;
            
            self.Add(this);
            
            var com = go.asCom;
                
            if(com != null)
            {	
                
    			m_button = com.GetControllerAt(0);
    			m_n0 = (GImage)com.GetChildAt(0);
    			m_n1 = (GImage)com.GetChildAt(1);
    			m_title = (GTextField)com.GetChildAt(2);
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
            
    		m_button = null;
    		m_n0 = null;
    		m_n1 = null;
    		m_title = null;
    	}
    }
}