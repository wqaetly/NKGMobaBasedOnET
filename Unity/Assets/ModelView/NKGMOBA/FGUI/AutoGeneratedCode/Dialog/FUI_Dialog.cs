/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_DialogAwakeSystem : AwakeSystem<FUI_Dialog, GObject>
    {
        public override void Awake(FUI_Dialog self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_Dialog : FUI
    {	
        public const string UIPackageName = "Dialog";
        public const string UIResName = "Dialog";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public GImage m_n0;
    	public GTextField m_Tittle;
    	public GTextField m_Conten;
    	public FUI_tow_cancel m_tow_cancel;
    	public FUI_one_confirm m_tow_confirm;
    	public GGroup m_towmode;
    	public FUI_one_confirm m_one_confirm;
    	public GGroup m_onemode;
    	public const string URL = "ui://3gqem46sifyf1";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Dialog CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Dialog, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_Dialog> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_Dialog> tcs = ETTask<FUI_Dialog>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_Dialog, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Dialog Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Dialog, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Dialog GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Dialog>();
        
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
                
    			m_n0 = (GImage)com.GetChildAt(0);
    			m_Tittle = (GTextField)com.GetChildAt(1);
    			m_Conten = (GTextField)com.GetChildAt(2);
    			m_tow_cancel = FUI_tow_cancel.Create(this, com.GetChildAt(3));
    			m_tow_confirm = FUI_one_confirm.Create(this, com.GetChildAt(4));
    			m_towmode = (GGroup)com.GetChildAt(5);
    			m_one_confirm = FUI_one_confirm.Create(this, com.GetChildAt(6));
    			m_onemode = (GGroup)com.GetChildAt(7);
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
            
    		m_n0 = null;
    		m_Tittle = null;
    		m_Conten = null;
    		m_tow_cancel.Dispose();
    		m_tow_cancel = null;
    		m_tow_confirm.Dispose();
    		m_tow_confirm = null;
    		m_towmode = null;
    		m_one_confirm.Dispose();
    		m_one_confirm = null;
    		m_onemode = null;
    	}
    }
}