/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;

namespace ETHotfix.FUILogin
{
    [ObjectSystem]
    public class RegistBtnAwakeSystem : AwakeSystem<RegistBtn, GObject>
    {
        public override void Awake(RegistBtn self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class RegistBtn : FUI
	{	
		public const string UIPackageName = "FUILogin";
		public const string UIResName = "RegistBtn";
		
		/// <summary>
        /// RegistBtn的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
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

        public static RegistBtn CreateInstance()
		{			
			return ComponentFactory.Create<RegistBtn, GObject>(CreateGObject());
		}

        public static Task<RegistBtn> CreateInstanceAsync()
        {
            TaskCompletionSource<RegistBtn> tcs = new TaskCompletionSource<RegistBtn>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<RegistBtn, GObject>(go));
            });

            return tcs.Task;
        }

        public static RegistBtn Create(GObject go)
		{
			return ComponentFactory.Create<RegistBtn, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static RegistBtn GetFormPool(GObject go)
        {
            var fui = go.Get<RegistBtn>();

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