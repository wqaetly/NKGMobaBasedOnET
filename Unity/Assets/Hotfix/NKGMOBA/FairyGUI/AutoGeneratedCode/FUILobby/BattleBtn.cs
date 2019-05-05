/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;
using ETHotfix;

namespace ETHotfix.FUILobby
{
    [ObjectSystem]
    public class BattleBtnAwakeSystem : AwakeSystem<BattleBtn, GObject>
    {
        public override void Awake(BattleBtn self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class BattleBtn : FUI
	{	
		public const string UIPackageName = "FUILobby";
		public const string UIResName = "BattleBtn";
		
		/// <summary>
        /// BattleBtn的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
		public GButton self;
		
		public Controller button;
		public GGraph n0;
		public GGraph n1;
		public GGraph n2;

		private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
		
		private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }

        public static BattleBtn CreateInstance()
		{			
			return ComponentFactory.Create<BattleBtn, GObject>(CreateGObject());
		}

        public static Task<BattleBtn> CreateInstanceAsync()
        {
            TaskCompletionSource<BattleBtn> tcs = new TaskCompletionSource<BattleBtn>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<BattleBtn, GObject>(go));
            });

            return tcs.Task;
        }

        public static BattleBtn Create(GObject go)
		{
			return ComponentFactory.Create<BattleBtn, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static BattleBtn GetFormPool(GObject go)
        {
            var fui = go.Get<BattleBtn>();

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
			
			self = (GButton)go;
			
			self.Add(this);
			
			var com = go.asCom;
				
			if(com != null)
			{	
				button = com.GetController("button");
				n0 = (GGraph)com.GetChild("n0");
				n1 = (GGraph)com.GetChild("n1");
				n2 = (GGraph)com.GetChild("n2");
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
			button = null;
			n0 = null;
			n1 = null;
			n2 = null;
		}
	}
}