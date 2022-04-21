/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_Battle_MainAwakeSystem : AwakeSystem<FUI_Battle_Main, GObject>
    {
        public override void Awake(FUI_Battle_Main self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_Battle_Main : FUI
    {	
        public const string UIPackageName = "BattleMain";
        public const string UIResName = "Battle_Main";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public GImage m_SkillAndStateBackGround;
    	public FUI_HpProcessBar m_RedProBar;
    	public GTextField m_RedText;
    	public FUI_OtherBar m_BlueProBar;
    	public GTextField m_BlueText;
    	public GLoader m_SkillTalent_Loader;
    	public FUI_SkillSmallProBar m_SkillTalent_Bar;
    	public GTextField m_SkillTalent_CDInfo;
    	public GGroup m_SkillTalent;
    	public GLoader m_SkillQ_Loader;
    	public FUI_SkillProBar m_SkillQ_Bar;
    	public GTextField m_SkillQ_CDInfo;
    	public GTextField m_SkillQ_Text;
    	public GGroup m_SkillQ;
    	public GLoader m_SkillW_Loader;
    	public FUI_SkillProBar m_SkillW_Bar;
    	public GTextField m_SkillW_CDInfo;
    	public GTextField m_SkillW_Text;
    	public GGroup m_SkillW;
    	public GLoader m_SkillE_Loader;
    	public FUI_SkillProBar m_SkillE_Bar;
    	public GTextField m_SkillE_CDInfo;
    	public GTextField m_SkillE_Text;
    	public GGroup m_SkillE;
    	public GLoader m_SkillR_Loader;
    	public FUI_SkillProBar m_SkillR_Bar;
    	public GTextField m_SkillR_CDInfo;
    	public GTextField m_SkillR_Text;
    	public GGroup m_SkillR;
    	public GLoader m_SkillD_Loader;
    	public FUI_SkillSmallProBar m_SkillD_Bar;
    	public GTextField m_SkillD_CDInfo;
    	public GTextField m_SkillD_Text;
    	public GGroup m_SkillD;
    	public GLoader m_SkillF_Loader;
    	public FUI_SkillSmallProBar m_SkillF_Bar;
    	public GTextField m_SkillF_CDInfo;
    	public GTextField m_SkillF_Text;
    	public GGroup m_SkillF;
    	public GGroup m_SkillAndState;
    	public GImage m_n96;
    	public GImage m_Attack;
    	public GImage m_Magic;
    	public GImage m_Armorpenetration;
    	public GImage m_Magicpenetration;
    	public GImage m_Armor;
    	public GImage m_SpellResistance;
    	public GImage m_AttackSpeed;
    	public GImage m_ExtraMagic;
    	public GImage m_ExtraAttack;
    	public GImage m_SkillCD;
    	public GImage m_Criticalstrike;
    	public GImage m_MoveSpeed;
    	public GTextField m_AttackInfo;
    	public GTextField m_SkillCDInfo;
    	public GTextField m_MagicpenetrationInfo;
    	public GTextField m_ArmorpenetrationInfo;
    	public GTextField m_ExtraMagicInfo;
    	public GTextField m_ExtraAttackInfo;
    	public GTextField m_CriticalstrikeInfo;
    	public GTextField m_AttackSpeedInfo;
    	public GTextField m_SpellResistanceInfo;
    	public GTextField m_ArmorInfo;
    	public GTextField m_MagicInfo;
    	public GTextField m_MoveSpeedInfo;
    	public GGroup m_DetailData;
    	public GLoader m_HeroAvatarLoader;
    	public GImage m_AvatarBack;
    	public FUI_ProgressBar1 m_ExpBar;
    	public GTextField m_HeroLevel;
    	public GGroup m_Avatar;
    	public GImage m_UIBack;
    	public GTextField m_t1;
    	public GTextField m_t6;
    	public GTextField m_t5;
    	public GTextField m_t3;
    	public GTextField m_t2;
    	public GTextField m_t7;
    	public GGroup m_Zhuangbei;
    	public GTextField m_t4;
    	public GTextField m_tB;
    	public GGroup m_BasicSkills;
    	public FUI_GoldenToShopBtn m_n58;
    	public GTextField m_GoldenCount;
    	public GGroup m_Golden;
    	public GGroup m_ShopAndBack;
    	public GGroup m_Bottom;
    	public GList m_BuffList;
    	public GList m_DeBuffList;
    	public GLoader m_SmallMapSprite;
    	public GImage m_SmallMapFrame;
    	public GGroup m_SmallMap;
    	public GImage m_GM_BackGround;
    	public FUI_Btn_GMItem m_Btn_NoMPCost;
    	public GTextField m_n198;
    	public FUI_Btn_GMItem m_Btn_NoHPCost;
    	public GTextField m_n200;
    	public FUI_Btn_GMItem m_Btn_NoCDCost;
    	public GTextField m_n201;
    	public FUI_Btn_GMItem m_Btn_CreateSpiling;
    	public GTextField m_n202;
    	public FUI_Btn_GMController_Disable m_Btn_GMController_Enable;
    	public FUI_Btn_GMController_Disable m_Btn_GMController_Disable;
    	public GGroup m_GM;
    	public GTextField m_Text_C2GPingDes;
    	public GTextField m_Text_C2GPinginfo;
    	public GTextField m_Text_C2GPingDes_2;
    	public GTextField m_Text_C2MPinginfo;
    	public GGroup m_Right_Top;
    	public Transition m_Par_GMControllerDis;
    	public Transition m_Part_GMControllerEnable;
    	public const string URL = "ui://9sdz56q4rdf51";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_Battle_Main CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_Battle_Main, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_Battle_Main> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_Battle_Main> tcs = ETTask<FUI_Battle_Main>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_Battle_Main, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_Battle_Main Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_Battle_Main, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_Battle_Main GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_Battle_Main>();
        
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
                
    			m_SkillAndStateBackGround = (GImage)com.GetChildAt(0);
    			m_RedProBar = FUI_HpProcessBar.Create(this, com.GetChildAt(1));
    			m_RedText = (GTextField)com.GetChildAt(2);
    			m_BlueProBar = FUI_OtherBar.Create(this, com.GetChildAt(3));
    			m_BlueText = (GTextField)com.GetChildAt(4);
    			m_SkillTalent_Loader = (GLoader)com.GetChildAt(5);
    			m_SkillTalent_Bar = FUI_SkillSmallProBar.Create(this, com.GetChildAt(6));
    			m_SkillTalent_CDInfo = (GTextField)com.GetChildAt(7);
    			m_SkillTalent = (GGroup)com.GetChildAt(8);
    			m_SkillQ_Loader = (GLoader)com.GetChildAt(9);
    			m_SkillQ_Bar = FUI_SkillProBar.Create(this, com.GetChildAt(10));
    			m_SkillQ_CDInfo = (GTextField)com.GetChildAt(11);
    			m_SkillQ_Text = (GTextField)com.GetChildAt(12);
    			m_SkillQ = (GGroup)com.GetChildAt(13);
    			m_SkillW_Loader = (GLoader)com.GetChildAt(14);
    			m_SkillW_Bar = FUI_SkillProBar.Create(this, com.GetChildAt(15));
    			m_SkillW_CDInfo = (GTextField)com.GetChildAt(16);
    			m_SkillW_Text = (GTextField)com.GetChildAt(17);
    			m_SkillW = (GGroup)com.GetChildAt(18);
    			m_SkillE_Loader = (GLoader)com.GetChildAt(19);
    			m_SkillE_Bar = FUI_SkillProBar.Create(this, com.GetChildAt(20));
    			m_SkillE_CDInfo = (GTextField)com.GetChildAt(21);
    			m_SkillE_Text = (GTextField)com.GetChildAt(22);
    			m_SkillE = (GGroup)com.GetChildAt(23);
    			m_SkillR_Loader = (GLoader)com.GetChildAt(24);
    			m_SkillR_Bar = FUI_SkillProBar.Create(this, com.GetChildAt(25));
    			m_SkillR_CDInfo = (GTextField)com.GetChildAt(26);
    			m_SkillR_Text = (GTextField)com.GetChildAt(27);
    			m_SkillR = (GGroup)com.GetChildAt(28);
    			m_SkillD_Loader = (GLoader)com.GetChildAt(29);
    			m_SkillD_Bar = FUI_SkillSmallProBar.Create(this, com.GetChildAt(30));
    			m_SkillD_CDInfo = (GTextField)com.GetChildAt(31);
    			m_SkillD_Text = (GTextField)com.GetChildAt(32);
    			m_SkillD = (GGroup)com.GetChildAt(33);
    			m_SkillF_Loader = (GLoader)com.GetChildAt(34);
    			m_SkillF_Bar = FUI_SkillSmallProBar.Create(this, com.GetChildAt(35));
    			m_SkillF_CDInfo = (GTextField)com.GetChildAt(36);
    			m_SkillF_Text = (GTextField)com.GetChildAt(37);
    			m_SkillF = (GGroup)com.GetChildAt(38);
    			m_SkillAndState = (GGroup)com.GetChildAt(39);
    			m_n96 = (GImage)com.GetChildAt(40);
    			m_Attack = (GImage)com.GetChildAt(41);
    			m_Magic = (GImage)com.GetChildAt(42);
    			m_Armorpenetration = (GImage)com.GetChildAt(43);
    			m_Magicpenetration = (GImage)com.GetChildAt(44);
    			m_Armor = (GImage)com.GetChildAt(45);
    			m_SpellResistance = (GImage)com.GetChildAt(46);
    			m_AttackSpeed = (GImage)com.GetChildAt(47);
    			m_ExtraMagic = (GImage)com.GetChildAt(48);
    			m_ExtraAttack = (GImage)com.GetChildAt(49);
    			m_SkillCD = (GImage)com.GetChildAt(50);
    			m_Criticalstrike = (GImage)com.GetChildAt(51);
    			m_MoveSpeed = (GImage)com.GetChildAt(52);
    			m_AttackInfo = (GTextField)com.GetChildAt(53);
    			m_SkillCDInfo = (GTextField)com.GetChildAt(54);
    			m_MagicpenetrationInfo = (GTextField)com.GetChildAt(55);
    			m_ArmorpenetrationInfo = (GTextField)com.GetChildAt(56);
    			m_ExtraMagicInfo = (GTextField)com.GetChildAt(57);
    			m_ExtraAttackInfo = (GTextField)com.GetChildAt(58);
    			m_CriticalstrikeInfo = (GTextField)com.GetChildAt(59);
    			m_AttackSpeedInfo = (GTextField)com.GetChildAt(60);
    			m_SpellResistanceInfo = (GTextField)com.GetChildAt(61);
    			m_ArmorInfo = (GTextField)com.GetChildAt(62);
    			m_MagicInfo = (GTextField)com.GetChildAt(63);
    			m_MoveSpeedInfo = (GTextField)com.GetChildAt(64);
    			m_DetailData = (GGroup)com.GetChildAt(65);
    			m_HeroAvatarLoader = (GLoader)com.GetChildAt(66);
    			m_AvatarBack = (GImage)com.GetChildAt(67);
    			m_ExpBar = FUI_ProgressBar1.Create(this, com.GetChildAt(68));
    			m_HeroLevel = (GTextField)com.GetChildAt(69);
    			m_Avatar = (GGroup)com.GetChildAt(70);
    			m_UIBack = (GImage)com.GetChildAt(71);
    			m_t1 = (GTextField)com.GetChildAt(72);
    			m_t6 = (GTextField)com.GetChildAt(73);
    			m_t5 = (GTextField)com.GetChildAt(74);
    			m_t3 = (GTextField)com.GetChildAt(75);
    			m_t2 = (GTextField)com.GetChildAt(76);
    			m_t7 = (GTextField)com.GetChildAt(77);
    			m_Zhuangbei = (GGroup)com.GetChildAt(78);
    			m_t4 = (GTextField)com.GetChildAt(79);
    			m_tB = (GTextField)com.GetChildAt(80);
    			m_BasicSkills = (GGroup)com.GetChildAt(81);
    			m_n58 = FUI_GoldenToShopBtn.Create(this, com.GetChildAt(82));
    			m_GoldenCount = (GTextField)com.GetChildAt(83);
    			m_Golden = (GGroup)com.GetChildAt(84);
    			m_ShopAndBack = (GGroup)com.GetChildAt(85);
    			m_Bottom = (GGroup)com.GetChildAt(86);
    			m_BuffList = (GList)com.GetChildAt(87);
    			m_DeBuffList = (GList)com.GetChildAt(88);
    			m_SmallMapSprite = (GLoader)com.GetChildAt(89);
    			m_SmallMapFrame = (GImage)com.GetChildAt(90);
    			m_SmallMap = (GGroup)com.GetChildAt(91);
    			m_GM_BackGround = (GImage)com.GetChildAt(92);
    			m_Btn_NoMPCost = FUI_Btn_GMItem.Create(this, com.GetChildAt(93));
    			m_n198 = (GTextField)com.GetChildAt(94);
    			m_Btn_NoHPCost = FUI_Btn_GMItem.Create(this, com.GetChildAt(95));
    			m_n200 = (GTextField)com.GetChildAt(96);
    			m_Btn_NoCDCost = FUI_Btn_GMItem.Create(this, com.GetChildAt(97));
    			m_n201 = (GTextField)com.GetChildAt(98);
    			m_Btn_CreateSpiling = FUI_Btn_GMItem.Create(this, com.GetChildAt(99));
    			m_n202 = (GTextField)com.GetChildAt(100);
    			m_Btn_GMController_Enable = FUI_Btn_GMController_Disable.Create(this, com.GetChildAt(101));
    			m_Btn_GMController_Disable = FUI_Btn_GMController_Disable.Create(this, com.GetChildAt(102));
    			m_GM = (GGroup)com.GetChildAt(103);
    			m_Text_C2GPingDes = (GTextField)com.GetChildAt(104);
    			m_Text_C2GPinginfo = (GTextField)com.GetChildAt(105);
    			m_Text_C2GPingDes_2 = (GTextField)com.GetChildAt(106);
    			m_Text_C2MPinginfo = (GTextField)com.GetChildAt(107);
    			m_Right_Top = (GGroup)com.GetChildAt(108);
    			m_Par_GMControllerDis = com.GetTransitionAt(0);
    			m_Part_GMControllerEnable = com.GetTransitionAt(1);
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
            
    		m_SkillAndStateBackGround = null;
    		m_RedProBar.Dispose();
    		m_RedProBar = null;
    		m_RedText = null;
    		m_BlueProBar.Dispose();
    		m_BlueProBar = null;
    		m_BlueText = null;
    		m_SkillTalent_Loader = null;
    		m_SkillTalent_Bar.Dispose();
    		m_SkillTalent_Bar = null;
    		m_SkillTalent_CDInfo = null;
    		m_SkillTalent = null;
    		m_SkillQ_Loader = null;
    		m_SkillQ_Bar.Dispose();
    		m_SkillQ_Bar = null;
    		m_SkillQ_CDInfo = null;
    		m_SkillQ_Text = null;
    		m_SkillQ = null;
    		m_SkillW_Loader = null;
    		m_SkillW_Bar.Dispose();
    		m_SkillW_Bar = null;
    		m_SkillW_CDInfo = null;
    		m_SkillW_Text = null;
    		m_SkillW = null;
    		m_SkillE_Loader = null;
    		m_SkillE_Bar.Dispose();
    		m_SkillE_Bar = null;
    		m_SkillE_CDInfo = null;
    		m_SkillE_Text = null;
    		m_SkillE = null;
    		m_SkillR_Loader = null;
    		m_SkillR_Bar.Dispose();
    		m_SkillR_Bar = null;
    		m_SkillR_CDInfo = null;
    		m_SkillR_Text = null;
    		m_SkillR = null;
    		m_SkillD_Loader = null;
    		m_SkillD_Bar.Dispose();
    		m_SkillD_Bar = null;
    		m_SkillD_CDInfo = null;
    		m_SkillD_Text = null;
    		m_SkillD = null;
    		m_SkillF_Loader = null;
    		m_SkillF_Bar.Dispose();
    		m_SkillF_Bar = null;
    		m_SkillF_CDInfo = null;
    		m_SkillF_Text = null;
    		m_SkillF = null;
    		m_SkillAndState = null;
    		m_n96 = null;
    		m_Attack = null;
    		m_Magic = null;
    		m_Armorpenetration = null;
    		m_Magicpenetration = null;
    		m_Armor = null;
    		m_SpellResistance = null;
    		m_AttackSpeed = null;
    		m_ExtraMagic = null;
    		m_ExtraAttack = null;
    		m_SkillCD = null;
    		m_Criticalstrike = null;
    		m_MoveSpeed = null;
    		m_AttackInfo = null;
    		m_SkillCDInfo = null;
    		m_MagicpenetrationInfo = null;
    		m_ArmorpenetrationInfo = null;
    		m_ExtraMagicInfo = null;
    		m_ExtraAttackInfo = null;
    		m_CriticalstrikeInfo = null;
    		m_AttackSpeedInfo = null;
    		m_SpellResistanceInfo = null;
    		m_ArmorInfo = null;
    		m_MagicInfo = null;
    		m_MoveSpeedInfo = null;
    		m_DetailData = null;
    		m_HeroAvatarLoader = null;
    		m_AvatarBack = null;
    		m_ExpBar.Dispose();
    		m_ExpBar = null;
    		m_HeroLevel = null;
    		m_Avatar = null;
    		m_UIBack = null;
    		m_t1 = null;
    		m_t6 = null;
    		m_t5 = null;
    		m_t3 = null;
    		m_t2 = null;
    		m_t7 = null;
    		m_Zhuangbei = null;
    		m_t4 = null;
    		m_tB = null;
    		m_BasicSkills = null;
    		m_n58.Dispose();
    		m_n58 = null;
    		m_GoldenCount = null;
    		m_Golden = null;
    		m_ShopAndBack = null;
    		m_Bottom = null;
    		m_BuffList = null;
    		m_DeBuffList = null;
    		m_SmallMapSprite = null;
    		m_SmallMapFrame = null;
    		m_SmallMap = null;
    		m_GM_BackGround = null;
    		m_Btn_NoMPCost.Dispose();
    		m_Btn_NoMPCost = null;
    		m_n198 = null;
    		m_Btn_NoHPCost.Dispose();
    		m_Btn_NoHPCost = null;
    		m_n200 = null;
    		m_Btn_NoCDCost.Dispose();
    		m_Btn_NoCDCost = null;
    		m_n201 = null;
    		m_Btn_CreateSpiling.Dispose();
    		m_Btn_CreateSpiling = null;
    		m_n202 = null;
    		m_Btn_GMController_Enable.Dispose();
    		m_Btn_GMController_Enable = null;
    		m_Btn_GMController_Disable.Dispose();
    		m_Btn_GMController_Disable = null;
    		m_GM = null;
    		m_Text_C2GPingDes = null;
    		m_Text_C2GPinginfo = null;
    		m_Text_C2GPingDes_2 = null;
    		m_Text_C2MPinginfo = null;
    		m_Right_Top = null;
    		m_Par_GMControllerDis = null;
    		m_Part_GMControllerEnable = null;
    	}
    }
}