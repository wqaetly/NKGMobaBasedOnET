/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUILobbyAwakeSystem : AwakeSystem<FUILobby, GObject>
    {
        public override void Awake(FUILobby self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUILobby : FUI
    {	
        public const string UIPackageName = "FUILobby";
        public const string UIResName = "FUILobby";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    public GImage n1;
    public GImage n2;
    public BottomBtn shop;
    public BottomBtn team;
    public BottomBtn backpacket;
    public BottomBtn achievemen;
    public BottomBtn hero;
    public BottomBtn prebattle;
    public BottomBtn friend;
    public BottomBtn watch;
    public ActityHead activity;
    public GGroup Bottom;
    public GImage n23;
    public GImage n37;
    public GImage n44;
    public GTextField m_gemInfo;
    public GImage n40;
    public GGroup gem;
    public GImage n34;
    public GImage n42;
    public GTextField m_goldenInfo;
    public GImage n41;
    public GGroup golden;
    public GImage n43;
    public GImage n31;
    public GTextField m_pointInfo;
    public GImage n32;
    public GGroup point;
    public GImage mail;
    public GImage setting;
    public GGroup RightTop;
    public GImage n48;
    public GImage n51;
    public GTextField n61;
    public BattleBtn normalPVPBtn;
    public GGroup nomalpvp;
    public GImage n49;
    public GImage n53;
    public GTextField n62;
    public BattleBtn pveBtn;
    public GGroup pve;
    public GImage n50;
    public GTextField n63;
    public GImage n52;
    public BattleBtn SerPVPBtn;
    public GGroup seriviouspvp;
    public GGroup Right;
    public GImage UserAvatar;
    public GTextField userName;
    public GTextField UserLevel;
    public GImage n71;
    public GGroup LeftTop;
    public const string URL = "ui://9ta7gv7krfuv0";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static FUILobby CreateInstance()
    {			
        return ComponentFactory.Create<FUILobby, GObject>(CreateGObject());
    }
        
    public static ETTask<FUILobby> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<FUILobby> tcs = new ETTaskCompletionSource<FUILobby>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<FUILobby, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static FUILobby Create(GObject go)
    {
        return ComponentFactory.Create<FUILobby, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static FUILobby GetFormPool(GObject go)
    {
        var fui = go.Get<FUILobby>();
        if(fui == null)
        {
            fui = Create(go);
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
            
    		n1 = (GImage)com.GetChildAt(0);
    		n2 = (GImage)com.GetChildAt(1);
    		shop = BottomBtn.Create(com.GetChildAt(2));
    		team = BottomBtn.Create(com.GetChildAt(3));
    		backpacket = BottomBtn.Create(com.GetChildAt(4));
    		achievemen = BottomBtn.Create(com.GetChildAt(5));
    		hero = BottomBtn.Create(com.GetChildAt(6));
    		prebattle = BottomBtn.Create(com.GetChildAt(7));
    		friend = BottomBtn.Create(com.GetChildAt(8));
    		watch = BottomBtn.Create(com.GetChildAt(9));
    		activity = ActityHead.Create(com.GetChildAt(10));
    		Bottom = (GGroup)com.GetChildAt(11);
    		n23 = (GImage)com.GetChildAt(12);
    		n37 = (GImage)com.GetChildAt(13);
    		n44 = (GImage)com.GetChildAt(14);
    		m_gemInfo = (GTextField)com.GetChildAt(15);
    		n40 = (GImage)com.GetChildAt(16);
    		gem = (GGroup)com.GetChildAt(17);
    		n34 = (GImage)com.GetChildAt(18);
    		n42 = (GImage)com.GetChildAt(19);
    		m_goldenInfo = (GTextField)com.GetChildAt(20);
    		n41 = (GImage)com.GetChildAt(21);
    		golden = (GGroup)com.GetChildAt(22);
    		n43 = (GImage)com.GetChildAt(23);
    		n31 = (GImage)com.GetChildAt(24);
    		m_pointInfo = (GTextField)com.GetChildAt(25);
    		n32 = (GImage)com.GetChildAt(26);
    		point = (GGroup)com.GetChildAt(27);
    		mail = (GImage)com.GetChildAt(28);
    		setting = (GImage)com.GetChildAt(29);
    		RightTop = (GGroup)com.GetChildAt(30);
    		n48 = (GImage)com.GetChildAt(31);
    		n51 = (GImage)com.GetChildAt(32);
    		n61 = (GTextField)com.GetChildAt(33);
    		normalPVPBtn = BattleBtn.Create(com.GetChildAt(34));
    		nomalpvp = (GGroup)com.GetChildAt(35);
    		n49 = (GImage)com.GetChildAt(36);
    		n53 = (GImage)com.GetChildAt(37);
    		n62 = (GTextField)com.GetChildAt(38);
    		pveBtn = BattleBtn.Create(com.GetChildAt(39));
    		pve = (GGroup)com.GetChildAt(40);
    		n50 = (GImage)com.GetChildAt(41);
    		n63 = (GTextField)com.GetChildAt(42);
    		n52 = (GImage)com.GetChildAt(43);
    		SerPVPBtn = BattleBtn.Create(com.GetChildAt(44));
    		seriviouspvp = (GGroup)com.GetChildAt(45);
    		Right = (GGroup)com.GetChildAt(46);
    		UserAvatar = (GImage)com.GetChildAt(47);
    		userName = (GTextField)com.GetChildAt(48);
    		UserLevel = (GTextField)com.GetChildAt(49);
    		n71 = (GImage)com.GetChildAt(50);
    		LeftTop = (GGroup)com.GetChildAt(51);
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
            
			n1 = null;
			n2 = null;
			shop = null;
			team = null;
			backpacket = null;
			achievemen = null;
			hero = null;
			prebattle = null;
			friend = null;
			watch = null;
			activity = null;
			Bottom = null;
			n23 = null;
			n37 = null;
			n44 = null;
			m_gemInfo = null;
			n40 = null;
			gem = null;
			n34 = null;
			n42 = null;
			m_goldenInfo = null;
			n41 = null;
			golden = null;
			n43 = null;
			n31 = null;
			m_pointInfo = null;
			n32 = null;
			point = null;
			mail = null;
			setting = null;
			RightTop = null;
			n48 = null;
			n51 = null;
			n61 = null;
			normalPVPBtn = null;
			nomalpvp = null;
			n49 = null;
			n53 = null;
			n62 = null;
			pveBtn = null;
			pve = null;
			n50 = null;
			n63 = null;
			n52 = null;
			SerPVPBtn = null;
			seriviouspvp = null;
			Right = null;
			UserAvatar = null;
			userName = null;
			UserLevel = null;
			n71 = null;
			LeftTop = null;
		}
}
}