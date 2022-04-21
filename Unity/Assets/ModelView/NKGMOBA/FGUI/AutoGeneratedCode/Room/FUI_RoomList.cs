/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_RoomListAwakeSystem : AwakeSystem<FUI_RoomList, GObject>
    {
        public override void Awake(FUI_RoomList self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_RoomList : FUI
    {	
        public const string UIPackageName = "Room";
        public const string UIResName = "RoomList";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public GImage m_n2;
    	public GGraph m_n4;
    	public FUI_Btn_QuitRoom m_CreateButton;
    	public GList m_RoomList;
    	public FUI_Btn_QuitRoom m_RefreshButton;
    	public FUI_Btn_QuitRoom m_QutiButton;
    	public const string URL = "ui://hya28zzrbp61c";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_RoomList CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_RoomList, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_RoomList> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_RoomList> tcs = ETTask<FUI_RoomList>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_RoomList, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_RoomList Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_RoomList, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_RoomList GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_RoomList>();
        
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
                
    			m_n2 = (GImage)com.GetChildAt(0);
    			m_n4 = (GGraph)com.GetChildAt(1);
    			m_CreateButton = FUI_Btn_QuitRoom.Create(this, com.GetChildAt(2));
    			m_RoomList = (GList)com.GetChildAt(3);
    			m_RefreshButton = FUI_Btn_QuitRoom.Create(this, com.GetChildAt(4));
    			m_QutiButton = FUI_Btn_QuitRoom.Create(this, com.GetChildAt(5));
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
    		m_n4 = null;
    		m_CreateButton.Dispose();
    		m_CreateButton = null;
    		m_RoomList = null;
    		m_RefreshButton.Dispose();
    		m_RefreshButton = null;
    		m_QutiButton.Dispose();
    		m_QutiButton = null;
    	}
    }
}