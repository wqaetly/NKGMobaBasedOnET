/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;
using ETHotfix;

namespace ETHotfix.FUIHeadBar
{
    [ObjectSystem]
    public class HeadBarAwakeSystem : AwakeSystem<HeadBar, GObject>
    {
        public override void Awake(HeadBar self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class HeadBar : FUI
	{	
		public const string UIPackageName = "FUIHeadBar";
		public const string UIResName = "HeadBar";
		
		/// <summary>
        /// HeadBar的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
		public GComponent self;
		
		public GImage BackGround;
		public GImage LevelBack;
		public GImage HPBack;
		public HeadBar_HP HPBar;
		public GImage Other;
		public GList HPGapList;
		public GTextField Level;
		public GTextField n14;

		private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
		
		private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }

        public static HeadBar CreateInstance()
		{			
			return ComponentFactory.Create<HeadBar, GObject>(CreateGObject());
		}

        public static Task<HeadBar> CreateInstanceAsync()
        {
            TaskCompletionSource<HeadBar> tcs = new TaskCompletionSource<HeadBar>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<HeadBar, GObject>(go));
            });

            return tcs.Task;
        }

        public static HeadBar Create(GObject go)
		{
			return ComponentFactory.Create<HeadBar, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static HeadBar GetFormPool(GObject go)
        {
            var fui = go.Get<HeadBar>();

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
				BackGround = (GImage)com.GetChild("BackGround");
				LevelBack = (GImage)com.GetChild("LevelBack");
				HPBack = (GImage)com.GetChild("HPBack");
				HPBar = HeadBar_HP.Create(com.GetChild("HPBar"));
				Other = (GImage)com.GetChild("Other");
				HPGapList = (GList)com.GetChild("HPGapList");
				Level = (GTextField)com.GetChild("Level");
				n14 = (GTextField)com.GetChild("n14");
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
			BackGround = null;
			LevelBack = null;
			HPBack = null;
			HPBar.Dispose();
			HPBar = null;
			Other = null;
			HPGapList = null;
			Level = null;
			n14 = null;
		}
	}
}