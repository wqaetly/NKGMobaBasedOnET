/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_LobbyAwakeSystem : AwakeSystem<FUI_Lobby, GObject>
    {
        public override void Awake(FUI_Lobby self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_Lobby : FUI
    {	
        public const string UIPackageName = "Lobby";
        public const string UIResName = "Lobby";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public GImage m_n1;
    	public GImage m_n2;
    	public FUI_BottomBtn m_shop;
    	public FUI_BottomBtn m_team;
    	public FUI_BottomBtn m_backpacket;
    	public FUI_BottomBtn m_achievemen;
    	public FUI_BottomBtn m_hero;
    	public FUI_BottomBtn m_prebattle;
    	public FUI_BottomBtn m_friend;
    	public FUI_BottomBtn m_watch;
    	public FUI_ActityHead m_activity;
    	public GGroup m_Bottom;
    	public GImage m_n23;
    	public GImage m_n37;
    	public GImage m_n44;
    	public GTextField m_m_gemInfo;
    	public GImage m_n40;
    	public GGroup m_gem;
    	public GImage m_n34;
    	public GImage m_n42;
    	public GTextField m_m_goldenInfo;
    	public GImage m_n41;
    	public GGroup m_golden;
    	public GImage m_n43;
    	public GImage m_n31;
    	public GTextField m_m_pointInfo;
    	public GImage m_n32;
    	public GGroup m_point;
    	public GImage m_mail;
    	public GImage m_setting;
    	public GGroup m_RightTop;
    	public GImage m_n48;
    	public GImage m_n51;
    	public GTextField m_n61;
    	public FUI_BattleBtn m_Btn_PVP;
    	public GGroup m_nomalpvp;
    	public GImage m_n49;
    	public GImage m_n53;
    	public GTextField m_n62;
    	public FUI_BattleBtn m_Btn_PVE;
    	public GGroup m_pve;
    	public GImage m_n50;
    	public GTextField m_n63;
    	public GImage m_n52;
    	public FUI_BattleBtn m_Btn_RoomMode;
    	public GGroup m_seriviouspvp;
    	public GGroup m_Right;
    	public GImage m_UserAvatar;
    	public GTextField m_userName;
    	public GTextField m_UserLevel;
    	public GImage m_n71;
    	public GGroup m_LeftTop;
    	public const string URL = "ui://9ta7gv7krfuv0";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Lobby CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Lobby, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_Lobby> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_Lobby> tcs = ETTask<FUI_Lobby>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_Lobby, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Lobby Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Lobby, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Lobby GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Lobby>();
        
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
                
    			m_n1 = (GImage)com.GetChildAt(0);
    			m_n2 = (GImage)com.GetChildAt(1);
    			m_shop = FUI_BottomBtn.Create(this, com.GetChildAt(2));
    			m_team = FUI_BottomBtn.Create(this, com.GetChildAt(3));
    			m_backpacket = FUI_BottomBtn.Create(this, com.GetChildAt(4));
    			m_achievemen = FUI_BottomBtn.Create(this, com.GetChildAt(5));
    			m_hero = FUI_BottomBtn.Create(this, com.GetChildAt(6));
    			m_prebattle = FUI_BottomBtn.Create(this, com.GetChildAt(7));
    			m_friend = FUI_BottomBtn.Create(this, com.GetChildAt(8));
    			m_watch = FUI_BottomBtn.Create(this, com.GetChildAt(9));
    			m_activity = FUI_ActityHead.Create(this, com.GetChildAt(10));
    			m_Bottom = (GGroup)com.GetChildAt(11);
    			m_n23 = (GImage)com.GetChildAt(12);
    			m_n37 = (GImage)com.GetChildAt(13);
    			m_n44 = (GImage)com.GetChildAt(14);
    			m_m_gemInfo = (GTextField)com.GetChildAt(15);
    			m_n40 = (GImage)com.GetChildAt(16);
    			m_gem = (GGroup)com.GetChildAt(17);
    			m_n34 = (GImage)com.GetChildAt(18);
    			m_n42 = (GImage)com.GetChildAt(19);
    			m_m_goldenInfo = (GTextField)com.GetChildAt(20);
    			m_n41 = (GImage)com.GetChildAt(21);
    			m_golden = (GGroup)com.GetChildAt(22);
    			m_n43 = (GImage)com.GetChildAt(23);
    			m_n31 = (GImage)com.GetChildAt(24);
    			m_m_pointInfo = (GTextField)com.GetChildAt(25);
    			m_n32 = (GImage)com.GetChildAt(26);
    			m_point = (GGroup)com.GetChildAt(27);
    			m_mail = (GImage)com.GetChildAt(28);
    			m_setting = (GImage)com.GetChildAt(29);
    			m_RightTop = (GGroup)com.GetChildAt(30);
    			m_n48 = (GImage)com.GetChildAt(31);
    			m_n51 = (GImage)com.GetChildAt(32);
    			m_n61 = (GTextField)com.GetChildAt(33);
    			m_Btn_PVP = FUI_BattleBtn.Create(this, com.GetChildAt(34));
    			m_nomalpvp = (GGroup)com.GetChildAt(35);
    			m_n49 = (GImage)com.GetChildAt(36);
    			m_n53 = (GImage)com.GetChildAt(37);
    			m_n62 = (GTextField)com.GetChildAt(38);
    			m_Btn_PVE = FUI_BattleBtn.Create(this, com.GetChildAt(39));
    			m_pve = (GGroup)com.GetChildAt(40);
    			m_n50 = (GImage)com.GetChildAt(41);
    			m_n63 = (GTextField)com.GetChildAt(42);
    			m_n52 = (GImage)com.GetChildAt(43);
    			m_Btn_RoomMode = FUI_BattleBtn.Create(this, com.GetChildAt(44));
    			m_seriviouspvp = (GGroup)com.GetChildAt(45);
    			m_Right = (GGroup)com.GetChildAt(46);
    			m_UserAvatar = (GImage)com.GetChildAt(47);
    			m_userName = (GTextField)com.GetChildAt(48);
    			m_UserLevel = (GTextField)com.GetChildAt(49);
    			m_n71 = (GImage)com.GetChildAt(50);
    			m_LeftTop = (GGroup)com.GetChildAt(51);
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
            
    		m_n1 = null;
    		m_n2 = null;
    		m_shop.Dispose();
    		m_shop = null;
    		m_team.Dispose();
    		m_team = null;
    		m_backpacket.Dispose();
    		m_backpacket = null;
    		m_achievemen.Dispose();
    		m_achievemen = null;
    		m_hero.Dispose();
    		m_hero = null;
    		m_prebattle.Dispose();
    		m_prebattle = null;
    		m_friend.Dispose();
    		m_friend = null;
    		m_watch.Dispose();
    		m_watch = null;
    		m_activity.Dispose();
    		m_activity = null;
    		m_Bottom = null;
    		m_n23 = null;
    		m_n37 = null;
    		m_n44 = null;
    		m_m_gemInfo = null;
    		m_n40 = null;
    		m_gem = null;
    		m_n34 = null;
    		m_n42 = null;
    		m_m_goldenInfo = null;
    		m_n41 = null;
    		m_golden = null;
    		m_n43 = null;
    		m_n31 = null;
    		m_m_pointInfo = null;
    		m_n32 = null;
    		m_point = null;
    		m_mail = null;
    		m_setting = null;
    		m_RightTop = null;
    		m_n48 = null;
    		m_n51 = null;
    		m_n61 = null;
    		m_Btn_PVP.Dispose();
    		m_Btn_PVP = null;
    		m_nomalpvp = null;
    		m_n49 = null;
    		m_n53 = null;
    		m_n62 = null;
    		m_Btn_PVE.Dispose();
    		m_Btn_PVE = null;
    		m_pve = null;
    		m_n50 = null;
    		m_n63 = null;
    		m_n52 = null;
    		m_Btn_RoomMode.Dispose();
    		m_Btn_RoomMode = null;
    		m_seriviouspvp = null;
    		m_Right = null;
    		m_UserAvatar = null;
    		m_userName = null;
    		m_UserLevel = null;
    		m_n71 = null;
    		m_LeftTop = null;
    	}
    }
}