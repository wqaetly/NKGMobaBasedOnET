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
		
		public GImage SkillBack;
		public HpProcessBar RedProBar;
		public GTextField RedText;
		public OtherBar BlueProBar;
		public GTextField BlueText;
		public SkillProBar SkillQ_Bar;
		public GTextField SkillQ_Text;
		public GGroup SkillQ;
		public SkillProBar SkillE_Bar;
		public GTextField SkillE_Text;
		public GGroup SkillE;
		public SkillProBar SkillW_Bar;
		public GTextField SkillW_Text;
		public GGroup SkillW;
		public SmallSkillProBar SkillF_Bar;
		public GTextField SkillF_Text;
		public GGroup SkillF;
		public SmallSkillProBar SkillD_Bar;
		public GTextField SkillD_Text;
		public GGroup SkillD;
		public SkillProBar SkillR_Bar;
		public GTextField SkillR_Text;
		public GGroup SkillR;
		public SmallSkillProBar SkillTalent;
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
		public GImage HeroAvatar;
		public GImage AvatarBack;
		public GTextField HeroLevel;
		public ProgressBar1 ExpBar;
		public GGroup Avatar;
		public GImage UIBack;
		public SmallSkillProBar Equ1;
		public SmallSkillProBar Equ2;
		public SmallSkillProBar Equ3;
		public SmallSkillProBar Equ5;
		public SmallSkillProBar Equ6;
		public SmallSkillProBar Equ7;
		public GTextField t1;
		public GTextField t6;
		public GTextField t5;
		public GTextField t3;
		public GTextField t2;
		public GTextField t7;
		public GGroup Zhuangbei;
		public SmallSkillProBar EyeBar;
		public SmallSkillProBar BBar;
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

        public static Task<FUI5V5Map> CreateInstanceAsync()
        {
            TaskCompletionSource<FUI5V5Map> tcs = new TaskCompletionSource<FUI5V5Map>();

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
				SkillBack = (GImage)com.GetChild("SkillBack");
				RedProBar = HpProcessBar.Create(com.GetChild("RedProBar"));
				RedText = (GTextField)com.GetChild("RedText");
				BlueProBar = OtherBar.Create(com.GetChild("BlueProBar"));
				BlueText = (GTextField)com.GetChild("BlueText");
				SkillQ_Bar = SkillProBar.Create(com.GetChild("SkillQ_Bar"));
				SkillQ_Text = (GTextField)com.GetChild("SkillQ_Text");
				SkillQ = (GGroup)com.GetChild("SkillQ");
				SkillE_Bar = SkillProBar.Create(com.GetChild("SkillE_Bar"));
				SkillE_Text = (GTextField)com.GetChild("SkillE_Text");
				SkillE = (GGroup)com.GetChild("SkillE");
				SkillW_Bar = SkillProBar.Create(com.GetChild("SkillW_Bar"));
				SkillW_Text = (GTextField)com.GetChild("SkillW_Text");
				SkillW = (GGroup)com.GetChild("SkillW");
				SkillF_Bar = SmallSkillProBar.Create(com.GetChild("SkillF_Bar"));
				SkillF_Text = (GTextField)com.GetChild("SkillF_Text");
				SkillF = (GGroup)com.GetChild("SkillF");
				SkillD_Bar = SmallSkillProBar.Create(com.GetChild("SkillD_Bar"));
				SkillD_Text = (GTextField)com.GetChild("SkillD_Text");
				SkillD = (GGroup)com.GetChild("SkillD");
				SkillR_Bar = SkillProBar.Create(com.GetChild("SkillR_Bar"));
				SkillR_Text = (GTextField)com.GetChild("SkillR_Text");
				SkillR = (GGroup)com.GetChild("SkillR");
				SkillTalent = SmallSkillProBar.Create(com.GetChild("SkillTalent"));
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
				HeroAvatar = (GImage)com.GetChild("HeroAvatar");
				AvatarBack = (GImage)com.GetChild("AvatarBack");
				HeroLevel = (GTextField)com.GetChild("HeroLevel");
				ExpBar = ProgressBar1.Create(com.GetChild("ExpBar"));
				Avatar = (GGroup)com.GetChild("Avatar");
				UIBack = (GImage)com.GetChild("UIBack");
				Equ1 = SmallSkillProBar.Create(com.GetChild("Equ1"));
				Equ2 = SmallSkillProBar.Create(com.GetChild("Equ2"));
				Equ3 = SmallSkillProBar.Create(com.GetChild("Equ3"));
				Equ5 = SmallSkillProBar.Create(com.GetChild("Equ5"));
				Equ6 = SmallSkillProBar.Create(com.GetChild("Equ6"));
				Equ7 = SmallSkillProBar.Create(com.GetChild("Equ7"));
				t1 = (GTextField)com.GetChild("t1");
				t6 = (GTextField)com.GetChild("t6");
				t5 = (GTextField)com.GetChild("t5");
				t3 = (GTextField)com.GetChild("t3");
				t2 = (GTextField)com.GetChild("t2");
				t7 = (GTextField)com.GetChild("t7");
				Zhuangbei = (GGroup)com.GetChild("Zhuangbei");
				EyeBar = SmallSkillProBar.Create(com.GetChild("EyeBar"));
				BBar = SmallSkillProBar.Create(com.GetChild("BBar"));
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
			SkillBack = null;
			RedProBar.Dispose();
			RedProBar = null;
			RedText = null;
			BlueProBar.Dispose();
			BlueProBar = null;
			BlueText = null;
			SkillQ_Bar.Dispose();
			SkillQ_Bar = null;
			SkillQ_Text = null;
			SkillQ = null;
			SkillE_Bar.Dispose();
			SkillE_Bar = null;
			SkillE_Text = null;
			SkillE = null;
			SkillW_Bar.Dispose();
			SkillW_Bar = null;
			SkillW_Text = null;
			SkillW = null;
			SkillF_Bar.Dispose();
			SkillF_Bar = null;
			SkillF_Text = null;
			SkillF = null;
			SkillD_Bar.Dispose();
			SkillD_Bar = null;
			SkillD_Text = null;
			SkillD = null;
			SkillR_Bar.Dispose();
			SkillR_Bar = null;
			SkillR_Text = null;
			SkillR = null;
			SkillTalent.Dispose();
			SkillTalent = null;
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
			HeroAvatar = null;
			AvatarBack = null;
			HeroLevel = null;
			ExpBar.Dispose();
			ExpBar = null;
			Avatar = null;
			UIBack = null;
			Equ1.Dispose();
			Equ1 = null;
			Equ2.Dispose();
			Equ2 = null;
			Equ3.Dispose();
			Equ3 = null;
			Equ5.Dispose();
			Equ5 = null;
			Equ6.Dispose();
			Equ6 = null;
			Equ7.Dispose();
			Equ7 = null;
			t1 = null;
			t6 = null;
			t5 = null;
			t3 = null;
			t2 = null;
			t7 = null;
			Zhuangbei = null;
			EyeBar.Dispose();
			EyeBar = null;
			BBar.Dispose();
			BBar = null;
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
		}
	}
}