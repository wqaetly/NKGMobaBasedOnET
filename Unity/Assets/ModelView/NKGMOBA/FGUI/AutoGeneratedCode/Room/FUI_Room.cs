/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_RoomAwakeSystem : AwakeSystem<FUI_Room, GObject>
    {
        public override void Awake(FUI_Room self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_Room : FUI
    {	
        public const string UIPackageName = "Room";
        public const string UIResName = "Room";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public Controller m_IsMaster;
    	public GImage m_n2;
    	public GGraph m_n4;
    	public GTextField m_RoomName;
    	public FUI_Btn_QuitRoom m_Btn_QuitRoom;
    	public GList m_Team1;
    	public GList m_Team2;
    	public FUI_Btn_StartGame m_Btn_StartGame;
    	public const string URL = "ui://hya28zzrbp610";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Room CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Room, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_Room> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_Room> tcs = ETTask<FUI_Room>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_Room, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Room Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Room, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Room GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Room>();
        
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
    			m_n2 = (GImage)com.GetChildAt(0);
    			m_n4 = (GGraph)com.GetChildAt(1);
    			m_RoomName = (GTextField)com.GetChildAt(2);
    			m_Btn_QuitRoom = FUI_Btn_QuitRoom.Create(this, com.GetChildAt(3));
    			m_Team1 = (GList)com.GetChildAt(4);
    			m_Team2 = (GList)com.GetChildAt(5);
    			m_Btn_StartGame = FUI_Btn_StartGame.Create(this, com.GetChildAt(6));
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
    		m_n2 = null;
    		m_n4 = null;
    		m_RoomName = null;
    		m_Btn_QuitRoom.Dispose();
    		m_Btn_QuitRoom = null;
    		m_Team1 = null;
    		m_Team2 = null;
    		m_Btn_StartGame.Dispose();
    		m_Btn_StartGame = null;
    	}
    }
}