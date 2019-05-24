/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;

namespace ETHotfix.FUILobby
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
        /// FUILobby的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
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
		public GImage n43;
		public GImage n31;
		public GTextField pointInfo;
		public GImage n32;
		public GGroup point;
		public GImage n34;
		public GImage n42;
		public GTextField n65;
		public GImage n41;
		public GGroup golden;
		public GImage n37;
		public GImage n44;
		public GTextField n66;
		public GImage n40;
		public GGroup gem;
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

        public static Task<FUILobby> CreateInstanceAsync()
        {
            TaskCompletionSource<FUILobby> tcs = new TaskCompletionSource<FUILobby>();

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
				n1 = (GImage)com.GetChild("n1");
				n2 = (GImage)com.GetChild("n2");
				shop = BottomBtn.Create(com.GetChild("shop"));
				team = BottomBtn.Create(com.GetChild("team"));
				backpacket = BottomBtn.Create(com.GetChild("backpacket"));
				achievemen = BottomBtn.Create(com.GetChild("achievemen"));
				hero = BottomBtn.Create(com.GetChild("hero"));
				prebattle = BottomBtn.Create(com.GetChild("prebattle"));
				friend = BottomBtn.Create(com.GetChild("friend"));
				watch = BottomBtn.Create(com.GetChild("watch"));
				activity = ActityHead.Create(com.GetChild("activity"));
				Bottom = (GGroup)com.GetChild("Bottom");
				n23 = (GImage)com.GetChild("n23");
				n43 = (GImage)com.GetChild("n43");
				n31 = (GImage)com.GetChild("n31");
				pointInfo = (GTextField)com.GetChild("pointInfo");
				n32 = (GImage)com.GetChild("n32");
				point = (GGroup)com.GetChild("point");
				n34 = (GImage)com.GetChild("n34");
				n42 = (GImage)com.GetChild("n42");
				n65 = (GTextField)com.GetChild("n65");
				n41 = (GImage)com.GetChild("n41");
				golden = (GGroup)com.GetChild("golden");
				n37 = (GImage)com.GetChild("n37");
				n44 = (GImage)com.GetChild("n44");
				n66 = (GTextField)com.GetChild("n66");
				n40 = (GImage)com.GetChild("n40");
				gem = (GGroup)com.GetChild("gem");
				mail = (GImage)com.GetChild("mail");
				setting = (GImage)com.GetChild("setting");
				RightTop = (GGroup)com.GetChild("RightTop");
				n48 = (GImage)com.GetChild("n48");
				n51 = (GImage)com.GetChild("n51");
				n61 = (GTextField)com.GetChild("n61");
				normalPVPBtn = BattleBtn.Create(com.GetChild("normalPVPBtn"));
				nomalpvp = (GGroup)com.GetChild("nomalpvp");
				n49 = (GImage)com.GetChild("n49");
				n53 = (GImage)com.GetChild("n53");
				n62 = (GTextField)com.GetChild("n62");
				pveBtn = BattleBtn.Create(com.GetChild("pveBtn"));
				pve = (GGroup)com.GetChild("pve");
				n50 = (GImage)com.GetChild("n50");
				n63 = (GTextField)com.GetChild("n63");
				n52 = (GImage)com.GetChild("n52");
				SerPVPBtn = BattleBtn.Create(com.GetChild("SerPVPBtn"));
				seriviouspvp = (GGroup)com.GetChild("seriviouspvp");
				Right = (GGroup)com.GetChild("Right");
				UserAvatar = (GImage)com.GetChild("UserAvatar");
				userName = (GTextField)com.GetChild("userName");
				UserLevel = (GTextField)com.GetChild("UserLevel");
				n71 = (GImage)com.GetChild("n71");
				LeftTop = (GGroup)com.GetChild("LeftTop");
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
			shop.Dispose();
			shop = null;
			team.Dispose();
			team = null;
			backpacket.Dispose();
			backpacket = null;
			achievemen.Dispose();
			achievemen = null;
			hero.Dispose();
			hero = null;
			prebattle.Dispose();
			prebattle = null;
			friend.Dispose();
			friend = null;
			watch.Dispose();
			watch = null;
			activity.Dispose();
			activity = null;
			Bottom = null;
			n23 = null;
			n43 = null;
			n31 = null;
			pointInfo = null;
			n32 = null;
			point = null;
			n34 = null;
			n42 = null;
			n65 = null;
			n41 = null;
			golden = null;
			n37 = null;
			n44 = null;
			n66 = null;
			n40 = null;
			gem = null;
			mail = null;
			setting = null;
			RightTop = null;
			n48 = null;
			n51 = null;
			n61 = null;
			normalPVPBtn.Dispose();
			normalPVPBtn = null;
			nomalpvp = null;
			n49 = null;
			n53 = null;
			n62 = null;
			pveBtn.Dispose();
			pveBtn = null;
			pve = null;
			n50 = null;
			n63 = null;
			n52 = null;
			SerPVPBtn.Dispose();
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