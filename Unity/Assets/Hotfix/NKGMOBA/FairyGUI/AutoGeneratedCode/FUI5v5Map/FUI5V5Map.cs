/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;
using ETHotfix;

namespace ETHotfix.FUI5v5Map
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
        /// FUI5V5Map的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
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

        public static ETTask<FUI5V5Map> CreateInstanceAsync()
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
				SkillAndStateBackGround = (GImage)com.GetChild("SkillAndStateBackGround");
				RedProBar = HpProcessBar.Create(com.GetChild("RedProBar"));
				RedText = (GTextField)com.GetChild("RedText");
				BlueProBar = OtherBar.Create(com.GetChild("BlueProBar"));
				BlueText = (GTextField)com.GetChild("BlueText");
				SkillQ_Loader = (GLoader)com.GetChild("SkillQ_Loader");
				SkillQ_Bar = SkillProBar.Create(com.GetChild("SkillQ_Bar"));
				SkillQ_CDInfo = (GTextField)com.GetChild("SkillQ_CDInfo");
				SkillQ_Text = (GTextField)com.GetChild("SkillQ_Text");
				SkillQ = (GGroup)com.GetChild("SkillQ");
				SkillE_Loader = (GLoader)com.GetChild("SkillE_Loader");
				SkillE_Bar = SkillProBar.Create(com.GetChild("SkillE_Bar"));
				SkillE_Text = (GTextField)com.GetChild("SkillE_Text");
				SkillE_CDInfo = (GTextField)com.GetChild("SkillE_CDInfo");
				SkillE = (GGroup)com.GetChild("SkillE");
				SkillW_Loader = (GLoader)com.GetChild("SkillW_Loader");
				SkillW_Bar = SkillProBar.Create(com.GetChild("SkillW_Bar"));
				SkillW_Text = (GTextField)com.GetChild("SkillW_Text");
				SkillW_CDInfo = (GTextField)com.GetChild("SkillW_CDInfo");
				SkillW = (GGroup)com.GetChild("SkillW");
				SkillR_Loader = (GLoader)com.GetChild("SkillR_Loader");
				SkillR_Bar = SkillProBar.Create(com.GetChild("SkillR_Bar"));
				SkillR_Text = (GTextField)com.GetChild("SkillR_Text");
				SkillR_CDInfo = (GTextField)com.GetChild("SkillR_CDInfo");
				SkillR = (GGroup)com.GetChild("SkillR");
				SkillTalent_Loader = (GLoader)com.GetChild("SkillTalent_Loader");
				SkillTalent_Bar = SkillSmallProBar.Create(com.GetChild("SkillTalent_Bar"));
				SkillTalent_CDInfo = (GTextField)com.GetChild("SkillTalent_CDInfo");
				SkillTalent = (GGroup)com.GetChild("SkillTalent");
				SkillD_Loader = (GLoader)com.GetChild("SkillD_Loader");
				SkillD_Bar = SkillSmallProBar.Create(com.GetChild("SkillD_Bar"));
				SkillD_CDInfo = (GTextField)com.GetChild("SkillD_CDInfo");
				SkillD_Text = (GTextField)com.GetChild("SkillD_Text");
				SkillD = (GGroup)com.GetChild("SkillD");
				SkillF_Loader = (GLoader)com.GetChild("SkillF_Loader");
				SkillF_Bar = SkillSmallProBar.Create(com.GetChild("SkillF_Bar"));
				SkillF_CDInfo = (GTextField)com.GetChild("SkillF_CDInfo");
				SkillF_Text = (GTextField)com.GetChild("SkillF_Text");
				SkillF = (GGroup)com.GetChild("SkillF");
				SkillAndState = (GGroup)com.GetChild("SkillAndState");
				n96 = (GImage)com.GetChild("n96");
				Attack = (GImage)com.GetChild("Attack");
				Magic = (GImage)com.GetChild("Magic");
				Armorpenetration = (GImage)com.GetChild("Armorpenetration");
				Magicpenetration = (GImage)com.GetChild("Magicpenetration");
				Armor = (GImage)com.GetChild("Armor");
				SpellResistance = (GImage)com.GetChild("SpellResistance");
				AttackSpeed = (GImage)com.GetChild("AttackSpeed");
				ExtraMagic = (GImage)com.GetChild("ExtraMagic");
				ExtraAttack = (GImage)com.GetChild("ExtraAttack");
				SkillCD = (GImage)com.GetChild("SkillCD");
				Criticalstrike = (GImage)com.GetChild("Criticalstrike");
				AttackInfo = (GTextField)com.GetChild("AttackInfo");
				MoveSpeed = (GImage)com.GetChild("MoveSpeed");
				SkillCDInfo = (GTextField)com.GetChild("SkillCDInfo");
				MagicpenetrationInfo = (GTextField)com.GetChild("MagicpenetrationInfo");
				ArmorpenetrationInfo = (GTextField)com.GetChild("ArmorpenetrationInfo");
				ExtraMagicInfo = (GTextField)com.GetChild("ExtraMagicInfo");
				ExtraAttackInfo = (GTextField)com.GetChild("ExtraAttackInfo");
				CriticalstrikeInfo = (GTextField)com.GetChild("CriticalstrikeInfo");
				AttackSpeedInfo = (GTextField)com.GetChild("AttackSpeedInfo");
				SpellResistanceInfo = (GTextField)com.GetChild("SpellResistanceInfo");
				ArmorInfo = (GTextField)com.GetChild("ArmorInfo");
				MagicInfo = (GTextField)com.GetChild("MagicInfo");
				MoveSpeedInfo = (GTextField)com.GetChild("MoveSpeedInfo");
				DetailData = (GGroup)com.GetChild("DetailData");
				HeroAvatarLoader = (GLoader)com.GetChild("HeroAvatarLoader");
				AvatarBack = (GImage)com.GetChild("AvatarBack");
				HeroLevel = (GTextField)com.GetChild("HeroLevel");
				ExpBar = ProgressBar1.Create(com.GetChild("ExpBar"));
				Avatar = (GGroup)com.GetChild("Avatar");
				UIBack = (GImage)com.GetChild("UIBack");
				t1 = (GTextField)com.GetChild("t1");
				t6 = (GTextField)com.GetChild("t6");
				t5 = (GTextField)com.GetChild("t5");
				t3 = (GTextField)com.GetChild("t3");
				t2 = (GTextField)com.GetChild("t2");
				t7 = (GTextField)com.GetChild("t7");
				Zhuangbei = (GGroup)com.GetChild("Zhuangbei");
				t4 = (GTextField)com.GetChild("t4");
				tB = (GTextField)com.GetChild("tB");
				BasicSkills = (GGroup)com.GetChild("BasicSkills");
				n58 = GoldenToShopBtn.Create(com.GetChild("n58"));
				GoldenCount = (GTextField)com.GetChild("GoldenCount");
				Golden = (GGroup)com.GetChild("Golden");
				ShopAndBack = (GGroup)com.GetChild("ShopAndBack");
				Bottom = (GGroup)com.GetChild("Bottom");
				BuffList = (GList)com.GetChild("BuffList");
				DeBuffList = (GList)com.GetChild("DeBuffList");
				SmallMapSprite = (GLoader)com.GetChild("SmallMapSprite");
				SmallMapFrame = (GImage)com.GetChild("SmallMapFrame");
				SmallMap = (GGroup)com.GetChild("SmallMap");
				GM_BackGround = (GImage)com.GetChild("GM_BackGround");
				Btn_NoMPCost = Btn_NoMPCost.Create(com.GetChild("Btn_NoMPCost"));
				Btn_NoHPCost = Btn_NoHPCost.Create(com.GetChild("Btn_NoHPCost"));
				Btn_CreateSpiling = Btn_CreateSpiling.Create(com.GetChild("Btn_CreateSpiling"));
				Btn_GMController_Disable = Btn_GMController_Disable.Create(com.GetChild("Btn_GMController_Disable"));
				Btn_NoCDCost = Btn_NoCDCost.Create(com.GetChild("Btn_NoCDCost"));
				Btn_GMController_Enable = Btn_GMController_Disable.Create(com.GetChild("Btn_GMController_Enable"));
				GM = (GGroup)com.GetChild("GM");
				Par_GMControllerDis = com.GetTransition("Par_GMControllerDis");
				Part_GMControllerEnable = com.GetTransition("Part_GMControllerEnable");
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
			RedProBar.Dispose();
			RedProBar = null;
			RedText = null;
			BlueProBar.Dispose();
			BlueProBar = null;
			BlueText = null;
			SkillQ_Loader = null;
			SkillQ_Bar.Dispose();
			SkillQ_Bar = null;
			SkillQ_CDInfo = null;
			SkillQ_Text = null;
			SkillQ = null;
			SkillE_Loader = null;
			SkillE_Bar.Dispose();
			SkillE_Bar = null;
			SkillE_Text = null;
			SkillE_CDInfo = null;
			SkillE = null;
			SkillW_Loader = null;
			SkillW_Bar.Dispose();
			SkillW_Bar = null;
			SkillW_Text = null;
			SkillW_CDInfo = null;
			SkillW = null;
			SkillR_Loader = null;
			SkillR_Bar.Dispose();
			SkillR_Bar = null;
			SkillR_Text = null;
			SkillR_CDInfo = null;
			SkillR = null;
			SkillTalent_Loader = null;
			SkillTalent_Bar.Dispose();
			SkillTalent_Bar = null;
			SkillTalent_CDInfo = null;
			SkillTalent = null;
			SkillD_Loader = null;
			SkillD_Bar.Dispose();
			SkillD_Bar = null;
			SkillD_CDInfo = null;
			SkillD_Text = null;
			SkillD = null;
			SkillF_Loader = null;
			SkillF_Bar.Dispose();
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
			ExpBar.Dispose();
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
			n58.Dispose();
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
			Btn_NoMPCost.Dispose();
			Btn_NoMPCost = null;
			Btn_NoHPCost.Dispose();
			Btn_NoHPCost = null;
			Btn_CreateSpiling.Dispose();
			Btn_CreateSpiling = null;
			Btn_GMController_Disable.Dispose();
			Btn_GMController_Disable = null;
			Btn_NoCDCost.Dispose();
			Btn_NoCDCost = null;
			Btn_GMController_Enable.Dispose();
			Btn_GMController_Enable = null;
			GM = null;
			Par_GMControllerDis = null;
			Part_GMControllerEnable = null;
		}
	}
}