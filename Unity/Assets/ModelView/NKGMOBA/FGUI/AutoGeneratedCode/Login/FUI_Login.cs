/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_LoginAwakeSystem : AwakeSystem<FUI_Login, GObject>
    {
        public override void Awake(FUI_Login self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_Login : FUI
    {	
        public const string UIPackageName = "Login";
        public const string UIResName = "Login";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public Controller m_Gro_ShowVersionInfo;
    	public GImage m_n0;
    	public GImage m_n9;
    	public GTextField m_n10;
    	public FUI_Btn_Login m_Btn_Login;
    	public FUI_Btn_Registe m_Btn_Registe;
    	public GImage m_accountInput;
    	public GImage m_passwordInput;
    	public GTextInput m_accountText;
    	public GTextField m_Tex_LoginInfo;
    	public FUI_Btn_ToTestScene m_ToTestSceneBtn;
    	public GTextInput m_passwordText;
    	public GGroup m_Gro_LoginInfo;
    	public Transition m_Tween_LoginPanelFlyIn;
    	public Transition m_Tween_LoginInfoFadeIn;
    	public const string URL = "ui://2jxt4hn8pdjl9";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Login CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Login, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_Login> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_Login> tcs = ETTask<FUI_Login>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_Login, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Login Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Login, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Login GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Login>();
        
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
                
    			m_Gro_ShowVersionInfo = com.GetControllerAt(0);
    			m_n0 = (GImage)com.GetChildAt(0);
    			m_n9 = (GImage)com.GetChildAt(1);
    			m_n10 = (GTextField)com.GetChildAt(2);
    			m_Btn_Login = FUI_Btn_Login.Create(this, com.GetChildAt(3));
    			m_Btn_Registe = FUI_Btn_Registe.Create(this, com.GetChildAt(4));
    			m_accountInput = (GImage)com.GetChildAt(5);
    			m_passwordInput = (GImage)com.GetChildAt(6);
    			m_accountText = (GTextInput)com.GetChildAt(7);
    			m_Tex_LoginInfo = (GTextField)com.GetChildAt(8);
    			m_ToTestSceneBtn = FUI_Btn_ToTestScene.Create(this, com.GetChildAt(9));
    			m_passwordText = (GTextInput)com.GetChildAt(10);
    			m_Gro_LoginInfo = (GGroup)com.GetChildAt(11);
    			m_Tween_LoginPanelFlyIn = com.GetTransitionAt(0);
    			m_Tween_LoginInfoFadeIn = com.GetTransitionAt(1);
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
            
    		m_Gro_ShowVersionInfo = null;
    		m_n0 = null;
    		m_n9 = null;
    		m_n10 = null;
    		m_Btn_Login.Dispose();
    		m_Btn_Login = null;
    		m_Btn_Registe.Dispose();
    		m_Btn_Registe = null;
    		m_accountInput = null;
    		m_passwordInput = null;
    		m_accountText = null;
    		m_Tex_LoginInfo = null;
    		m_ToTestSceneBtn.Dispose();
    		m_ToTestSceneBtn = null;
    		m_passwordText = null;
    		m_Gro_LoginInfo = null;
    		m_Tween_LoginPanelFlyIn = null;
    		m_Tween_LoginInfoFadeIn = null;
    	}
    }
}