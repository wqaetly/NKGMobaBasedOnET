/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_PlayerDataAwakeSystem : AwakeSystem<FUI_PlayerData, GObject>
    {
        public override void Awake(FUI_PlayerData self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_PlayerData : FUI
    {	
        public const string UIPackageName = "Room";
        public const string UIResName = "PlayerData";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public Controller m_IsMaster;
    	public Controller m_HasAdminFunc;
    	public GTextField m_RoomPlayerLevel;
    	public GTextField m_RoomPlayerName;
    	public GLoader m_n3;
    	public FUI_KickPlayer m_KickButton;
    	public GTextField m_PlayerId;
    	public const string URL = "ui://hya28zzrbp616";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_PlayerData CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_PlayerData, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_PlayerData> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_PlayerData> tcs = ETTask<FUI_PlayerData>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_PlayerData, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_PlayerData Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_PlayerData, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_PlayerData GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_PlayerData>();
        
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
                
    			m_IsMaster = com.GetControllerAt(0);
    			m_HasAdminFunc = com.GetControllerAt(1);
    			m_RoomPlayerLevel = (GTextField)com.GetChildAt(0);
    			m_RoomPlayerName = (GTextField)com.GetChildAt(1);
    			m_n3 = (GLoader)com.GetChildAt(2);
    			m_KickButton = FUI_KickPlayer.Create(this, com.GetChildAt(3));
    			m_PlayerId = (GTextField)com.GetChildAt(4);
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
            
    		m_IsMaster = null;
    		m_HasAdminFunc = null;
    		m_RoomPlayerLevel = null;
    		m_RoomPlayerName = null;
    		m_n3 = null;
    		m_KickButton.Dispose();
    		m_KickButton = null;
    		m_PlayerId = null;
    	}
    }
}