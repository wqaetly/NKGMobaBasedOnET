/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;

namespace ETHotfix.FUIDialog
{
    [ObjectSystem]
    public class tow_cancelAwakeSystem : AwakeSystem<tow_cancel, GObject>
    {
        public override void Awake(tow_cancel self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class tow_cancel : FUI
	{	
		public const string UIPackageName = "FUIDialog";
		public const string UIResName = "tow_cancel";
		
		/// <summary>
        /// tow_cancel的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
		public GButton self;
		
		public Controller button;
		public GImage n0;
		public GImage n1;
		public GTextField title;

		private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
		
		private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }

        public static tow_cancel CreateInstance()
		{			
			return ComponentFactory.Create<tow_cancel, GObject>(CreateGObject());
		}

        public static Task<tow_cancel> CreateInstanceAsync()
        {
            TaskCompletionSource<tow_cancel> tcs = new TaskCompletionSource<tow_cancel>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<tow_cancel, GObject>(go));
            });

            return tcs.Task;
        }

        public static tow_cancel Create(GObject go)
		{
			return ComponentFactory.Create<tow_cancel, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static tow_cancel GetFormPool(GObject go)
        {
            var fui = go.Get<tow_cancel>();

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
				n0 = (GImage)com.GetChild("n0");
				n1 = (GImage)com.GetChild("n1");
				title = (GTextField)com.GetChild("title");
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
			title = null;
		}
	}
}