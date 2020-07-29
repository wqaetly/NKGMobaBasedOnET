/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class FUI5V5MapAwakeSystem : AwakeSystem<FUI5V5Map, GObject>
    {
        public override void Awake(FUI5V5Map self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI5V5Map : FUI
    {	
        public const string UIPackageName = "FUI5v5Map";
        public const string UIResName = "FUI5V5Map";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    public GImage SkillAndStateBackGround;
    public HpProcessBar RedProBar;
    public GTextField RedText;
    public OtherBar BlueProBar;
    public GTextField BlueText;
    public GLoader SkillQ_Loader;
    public SkillProBar SkillQ_Bar;
    public GTextField SkillQ_CDInfo;
    public GTextField SkillQ_Text;
    public GGroup SkillQ;
    public GLoader SkillE_Loader;
    public SkillProBar SkillE_Bar;
    public GTextField SkillE_Text;
    public GTextField SkillE_CDInfo;
    public GGroup SkillE;
    public GLoader SkillW_Loader;
    public SkillProBar SkillW_Bar;
    public GTextField SkillW_Text;
    public GTextField SkillW_CDInfo;
    public GGroup SkillW;
    public GLoader SkillR_Loader;
    public SkillProBar SkillR_Bar;
    public GTextField SkillR_Text;
    public GTextField SkillR_CDInfo;
    public GGroup SkillR;
    public GLoader SkillTalent_Loader;
    public SkillSmallProBar SkillTalent_Bar;
    public GTextField SkillTalent_CDInfo;
    public GGroup SkillTalent;
    public GLoader SkillD_Loader;
    public SkillSmallProBar SkillD_Bar;
    public GTextField SkillD_CDInfo;
    public GTextField SkillD_Text;
    public GGroup SkillD;
    public GLoader SkillF_Loader;
    public SkillSmallProBar SkillF_Bar;
    public GTextField SkillF_CDInfo;
    public GTextField SkillF_Text;
    public GGroup SkillF;
    public GGroup SkillAndState;
    public GImage n96;
    public GImage Attack;
    public GImage Magic;
    public GImage Armorpenetration;
    public GImage Magicpenetration;
    public GImage Armor;
    public GImage SpellResistance;
    public GImage AttackSpeed;
    public GImage ExtraMagic;
    public GImage ExtraAttack;
    public GImage SkillCD;
    public GImage Criticalstrike;
    public GTextField AttackInfo;
    public GImage MoveSpeed;
    public GTextField SkillCDInfo;
    public GTextField MagicpenetrationInfo;
    public GTextField ArmorpenetrationInfo;
    public GTextField ExtraMagicInfo;
    public GTextField ExtraAttackInfo;
    public GTextField CriticalstrikeInfo;
    public GTextField AttackSpeedInfo;
    public GTextField SpellResistanceInfo;
    public GTextField ArmorInfo;
    public GTextField MagicInfo;
    public GTextField MoveSpeedInfo;
    public GGroup DetailData;
    public GLoader HeroAvatarLoader;
    public GImage AvatarBack;
    public GTextField HeroLevel;
    public ProgressBar1 ExpBar;
    public GGroup Avatar;
    public GImage UIBack;
    public GTextField t1;
    public GTextField t6;
    public GTextField t5;
    public GTextField t3;
    public GTextField t2;
    public GTextField t7;
    public GGroup Zhuangbei;
    public GTextField t4;
    public GTextField tB;
    public GGroup BasicSkills;
    public GoldenToShopBtn n58;
    public GTextField GoldenCount;
    public GGroup Golden;
    public GGroup ShopAndBack;
    public GGroup Bottom;
    public GList BuffList;
    public GList DeBuffList;
    public GLoader SmallMapSprite;
    public GImage SmallMapFrame;
    public GGroup SmallMap;
    public GImage GM_BackGround;
    public Btn_NoMPCost Btn_NoMPCost;
    public Btn_NoHPCost Btn_NoHPCost;
    public Btn_CreateSpiling Btn_CreateSpiling;
    public Btn_GMController_Disable Btn_GMController_Disable;
    public Btn_NoCDCost Btn_NoCDCost;
    public Btn_GMController_Disable Btn_GMController_Enable;
    public GGroup GM;
    public Transition Par_GMControllerDis;
    public Transition Part_GMControllerEnable;
    public const string URL = "ui://9sdz56q4rdf51";

    private static GObject CreateGObject()
    {
        return UIPackage.CreateObject(UIPackageName, UIResName);
    }
    
    private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
    {
        UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
    }
        
    public static FUI5V5Map CreateInstance()
    {			
        return ComponentFactory.Create<FUI5V5Map, GObject>(CreateGObject());
    }
        
    public static ETTask<FUI5V5Map> CreateInstanceAsync(Entity domain)
    {
        ETTaskCompletionSource<FUI5V5Map> tcs = new ETTaskCompletionSource<FUI5V5Map>();
        CreateGObjectAsync((go) =>
        {
            tcs.SetResult(ComponentFactory.Create<FUI5V5Map, GObject>(go));
        });
        return tcs.Task;
    }
        
    public static FUI5V5Map Create(GObject go)
    {
        return ComponentFactory.Create<FUI5V5Map, GObject>(go);
    }
        
    /// <summary>
    /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
    /// </summary>
    public static FUI5V5Map GetFormPool(GObject go)
    {
        var fui = go.Get<FUI5V5Map>();
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
            
    		SkillAndStateBackGround = (GImage)com.GetChildAt(0);
    		RedProBar = HpProcessBar.Create(com.GetChildAt(1));
    		RedText = (GTextField)com.GetChildAt(2);
    		BlueProBar = OtherBar.Create(com.GetChildAt(3));
    		BlueText = (GTextField)com.GetChildAt(4);
    		SkillQ_Loader = (GLoader)com.GetChildAt(5);
    		SkillQ_Bar = SkillProBar.Create(com.GetChildAt(6));
    		SkillQ_CDInfo = (GTextField)com.GetChildAt(7);
    		SkillQ_Text = (GTextField)com.GetChildAt(8);
    		SkillQ = (GGroup)com.GetChildAt(9);
    		SkillE_Loader = (GLoader)com.GetChildAt(10);
    		SkillE_Bar = SkillProBar.Create(com.GetChildAt(11));
    		SkillE_Text = (GTextField)com.GetChildAt(12);
    		SkillE_CDInfo = (GTextField)com.GetChildAt(13);
    		SkillE = (GGroup)com.GetChildAt(14);
    		SkillW_Loader = (GLoader)com.GetChildAt(15);
    		SkillW_Bar = SkillProBar.Create(com.GetChildAt(16));
    		SkillW_Text = (GTextField)com.GetChildAt(17);
    		SkillW_CDInfo = (GTextField)com.GetChildAt(18);
    		SkillW = (GGroup)com.GetChildAt(19);
    		SkillR_Loader = (GLoader)com.GetChildAt(20);
    		SkillR_Bar = SkillProBar.Create(com.GetChildAt(21));
    		SkillR_Text = (GTextField)com.GetChildAt(22);
    		SkillR_CDInfo = (GTextField)com.GetChildAt(23);
    		SkillR = (GGroup)com.GetChildAt(24);
    		SkillTalent_Loader = (GLoader)com.GetChildAt(25);
    		SkillTalent_Bar = SkillSmallProBar.Create(com.GetChildAt(26));
    		SkillTalent_CDInfo = (GTextField)com.GetChildAt(27);
    		SkillTalent = (GGroup)com.GetChildAt(28);
    		SkillD_Loader = (GLoader)com.GetChildAt(29);
    		SkillD_Bar = SkillSmallProBar.Create(com.GetChildAt(30));
    		SkillD_CDInfo = (GTextField)com.GetChildAt(31);
    		SkillD_Text = (GTextField)com.GetChildAt(32);
    		SkillD = (GGroup)com.GetChildAt(33);
    		SkillF_Loader = (GLoader)com.GetChildAt(34);
    		SkillF_Bar = SkillSmallProBar.Create(com.GetChildAt(35));
    		SkillF_CDInfo = (GTextField)com.GetChildAt(36);
    		SkillF_Text = (GTextField)com.GetChildAt(37);
    		SkillF = (GGroup)com.GetChildAt(38);
    		SkillAndState = (GGroup)com.GetChildAt(39);
    		n96 = (GImage)com.GetChildAt(40);
    		Attack = (GImage)com.GetChildAt(41);
    		Magic = (GImage)com.GetChildAt(42);
    		Armorpenetration = (GImage)com.GetChildAt(43);
    		Magicpenetration = (GImage)com.GetChildAt(44);
    		Armor = (GImage)com.GetChildAt(45);
    		SpellResistance = (GImage)com.GetChildAt(46);
    		AttackSpeed = (GImage)com.GetChildAt(47);
    		ExtraMagic = (GImage)com.GetChildAt(48);
    		ExtraAttack = (GImage)com.GetChildAt(49);
    		SkillCD = (GImage)com.GetChildAt(50);
    		Criticalstrike = (GImage)com.GetChildAt(51);
    		AttackInfo = (GTextField)com.GetChildAt(52);
    		MoveSpeed = (GImage)com.GetChildAt(53);
    		SkillCDInfo = (GTextField)com.GetChildAt(54);
    		MagicpenetrationInfo = (GTextField)com.GetChildAt(55);
    		ArmorpenetrationInfo = (GTextField)com.GetChildAt(56);
    		ExtraMagicInfo = (GTextField)com.GetChildAt(57);
    		ExtraAttackInfo = (GTextField)com.GetChildAt(58);
    		CriticalstrikeInfo = (GTextField)com.GetChildAt(59);
    		AttackSpeedInfo = (GTextField)com.GetChildAt(60);
    		SpellResistanceInfo = (GTextField)com.GetChildAt(61);
    		ArmorInfo = (GTextField)com.GetChildAt(62);
    		MagicInfo = (GTextField)com.GetChildAt(63);
    		MoveSpeedInfo = (GTextField)com.GetChildAt(64);
    		DetailData = (GGroup)com.GetChildAt(65);
    		HeroAvatarLoader = (GLoader)com.GetChildAt(66);
    		AvatarBack = (GImage)com.GetChildAt(67);
    		HeroLevel = (GTextField)com.GetChildAt(68);
    		ExpBar = ProgressBar1.Create(com.GetChildAt(69));
    		Avatar = (GGroup)com.GetChildAt(70);
    		UIBack = (GImage)com.GetChildAt(71);
    		t1 = (GTextField)com.GetChildAt(72);
    		t6 = (GTextField)com.GetChildAt(73);
    		t5 = (GTextField)com.GetChildAt(74);
    		t3 = (GTextField)com.GetChildAt(75);
    		t2 = (GTextField)com.GetChildAt(76);
    		t7 = (GTextField)com.GetChildAt(77);
    		Zhuangbei = (GGroup)com.GetChildAt(78);
    		t4 = (GTextField)com.GetChildAt(79);
    		tB = (GTextField)com.GetChildAt(80);
    		BasicSkills = (GGroup)com.GetChildAt(81);
    		n58 = GoldenToShopBtn.Create(com.GetChildAt(82));
    		GoldenCount = (GTextField)com.GetChildAt(83);
    		Golden = (GGroup)com.GetChildAt(84);
    		ShopAndBack = (GGroup)com.GetChildAt(85);
    		Bottom = (GGroup)com.GetChildAt(86);
    		BuffList = (GList)com.GetChildAt(87);
    		DeBuffList = (GList)com.GetChildAt(88);
    		SmallMapSprite = (GLoader)com.GetChildAt(89);
    		SmallMapFrame = (GImage)com.GetChildAt(90);
    		SmallMap = (GGroup)com.GetChildAt(91);
    		GM_BackGround = (GImage)com.GetChildAt(92);
    		Btn_NoMPCost = Btn_NoMPCost.Create(com.GetChildAt(93));
    		Btn_NoHPCost = Btn_NoHPCost.Create(com.GetChildAt(94));
    		Btn_CreateSpiling = Btn_CreateSpiling.Create(com.GetChildAt(95));
    		Btn_GMController_Disable = Btn_GMController_Disable.Create(com.GetChildAt(96));
    		Btn_NoCDCost = Btn_NoCDCost.Create(com.GetChildAt(97));
    		Btn_GMController_Enable = Btn_GMController_Disable.Create(com.GetChildAt(98));
    		GM = (GGroup)com.GetChildAt(99);
    		Par_GMControllerDis = com.GetTransitionAt(0);
    		Part_GMControllerEnable = com.GetTransitionAt(1);
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
            
			SkillAndStateBackGround = null;
			RedProBar = null;
			RedText = null;
			BlueProBar = null;
			BlueText = null;
			SkillQ_Loader = null;
			SkillQ_Bar = null;
			SkillQ_CDInfo = null;
			SkillQ_Text = null;
			SkillQ = null;
			SkillE_Loader = null;
			SkillE_Bar = null;
			SkillE_Text = null;
			SkillE_CDInfo = null;
			SkillE = null;
			SkillW_Loader = null;
			SkillW_Bar = null;
			SkillW_Text = null;
			SkillW_CDInfo = null;
			SkillW = null;
			SkillR_Loader = null;
			SkillR_Bar = null;
			SkillR_Text = null;
			SkillR_CDInfo = null;
			SkillR = null;
			SkillTalent_Loader = null;
			SkillTalent_Bar = null;
			SkillTalent_CDInfo = null;
			SkillTalent = null;
			SkillD_Loader = null;
			SkillD_Bar = null;
			SkillD_CDInfo = null;
			SkillD_Text = null;
			SkillD = null;
			SkillF_Loader = null;
			SkillF_Bar = null;
			SkillF_CDInfo = null;
			SkillF_Text = null;
			SkillF = null;
			SkillAndState = null;
			n96 = null;
			Attack = null;
			Magic = null;
			Armorpenetration = null;
			Magicpenetration = null;
			Armor = null;
			SpellResistance = null;
			AttackSpeed = null;
			ExtraMagic = null;
			ExtraAttack = null;
			SkillCD = null;
			Criticalstrike = null;
			AttackInfo = null;
			MoveSpeed = null;
			SkillCDInfo = null;
			MagicpenetrationInfo = null;
			ArmorpenetrationInfo = null;
			ExtraMagicInfo = null;
			ExtraAttackInfo = null;
			CriticalstrikeInfo = null;
			AttackSpeedInfo = null;
			SpellResistanceInfo = null;
			ArmorInfo = null;
			MagicInfo = null;
			MoveSpeedInfo = null;
			DetailData = null;
			HeroAvatarLoader = null;
			AvatarBack = null;
			HeroLevel = null;
			ExpBar = null;
			Avatar = null;
			UIBack = null;
			t1 = null;
			t6 = null;
			t5 = null;
			t3 = null;
			t2 = null;
			t7 = null;
			Zhuangbei = null;
			t4 = null;
			tB = null;
			BasicSkills = null;
			n58 = null;
			GoldenCount = null;
			Golden = null;
			ShopAndBack = null;
			Bottom = null;
			BuffList = null;
			DeBuffList = null;
			SmallMapSprite = null;
			SmallMapFrame = null;
			SmallMap = null;
			GM_BackGround = null;
			Btn_NoMPCost = null;
			Btn_NoHPCost = null;
			Btn_CreateSpiling = null;
			Btn_GMController_Disable = null;
			Btn_NoCDCost = null;
			Btn_GMController_Enable = null;
			GM = null;
			Par_GMControllerDis = null;
			Part_GMControllerEnable = null;
		}
}
}