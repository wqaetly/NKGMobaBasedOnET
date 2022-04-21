/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_RoomDataAwakeSystem : AwakeSystem<FUI_RoomData, GObject>
    {
        public override void Awake(FUI_RoomData self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_RoomData : FUI
    {	
        public const string UIPackageName = "Room";
        public const string UIResName = "RoomData";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public GTextField m_RoomName;
    	public GTextField m_n8;
    	public GTextField m_PlayerNum;
    	public FUI_Join m_JoinButton;
    	public GTextField m_RoomId;
    	public const string URL = "ui://hya28zzrbp61d";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_RoomData CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_RoomData, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_RoomData> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_RoomData> tcs = ETTask<FUI_RoomData>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_RoomData, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_RoomData Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_RoomData, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_RoomData GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_RoomData>();
        
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
                
    			m_RoomName = (GTextField)com.GetChildAt(0);
    			m_n8 = (GTextField)com.GetChildAt(1);
    			m_PlayerNum = (GTextField)com.GetChildAt(2);
    			m_JoinButton = FUI_Join.Create(this, com.GetChildAt(3));
    			m_RoomId = (GTextField)com.GetChildAt(4);
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
            
    		m_RoomName = null;
    		m_n8 = null;
    		m_PlayerNum = null;
    		m_JoinButton.Dispose();
    		m_JoinButton = null;
    		m_RoomId = null;
    	}
    }
}