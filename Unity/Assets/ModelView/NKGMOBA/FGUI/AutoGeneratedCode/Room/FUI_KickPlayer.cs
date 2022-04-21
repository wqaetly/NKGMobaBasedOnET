/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_KickPlayerAwakeSystem : AwakeSystem<FUI_KickPlayer, GObject>
    {
        public override void Awake(FUI_KickPlayer self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_KickPlayer : FUI
    {	
        public const string UIPackageName = "Room";
        public const string UIResName = "KickPlayer";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GButton self;
            
    	public Controller m_button;
    	public GImage m_n0;
    	public const string URL = "ui://hya28zzrbp61a";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_KickPlayer CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_KickPlayer, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_KickPlayer> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_KickPlayer> tcs = ETTask<FUI_KickPlayer>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_KickPlayer, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_KickPlayer Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_KickPlayer, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_KickPlayer GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_KickPlayer>();
        
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
    	}
    }
}