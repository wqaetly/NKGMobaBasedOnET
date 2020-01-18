/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;
using ETHotfix;

namespace ETHotfix.FUI5v5Map
{
    [ObjectSystem]
    public class Btn_GMController_DisableAwakeSystem : AwakeSystem<Btn_GMController_Disable, GObject>
    {
        public override void Awake(Btn_GMController_Disable self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class Btn_GMController_Disable : FUI
	{	
		public const string UIPackageName = "FUI5v5Map";
		public const string UIResName = "Btn_GMController_Disable";
		
		/// <summary>
        /// Btn_GMController_Disable的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
		public GButton self;
		
		public Controller button;
		public GImage n0;
		public GImage n1;
		public GImage n2;

		private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
		
		private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }

        public static Btn_GMController_Disable CreateInstance()
		{			
			return ComponentFactory.Create<Btn_GMController_Disable, GObject>(CreateGObject());
		}

        public static ETTask<Btn_GMController_Disable> CreateInstanceAsync()
        {
            ETTaskCompletionSource<Btn_GMController_Disable> tcs = new ETTaskCompletionSource<Btn_GMController_Disable>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<Btn_GMController_Disable, GObject>(go));
            });

            return tcs.Task;
        }

        public static Btn_GMController_Disable Create(GObject go)
		{
			return ComponentFactory.Create<Btn_GMController_Disable, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static Btn_GMController_Disable GetFormPool(GObject go)
        {
            var fui = go.Get<Btn_GMController_Disable>();

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
				n2 = (GImage)com.GetChild("n2");
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