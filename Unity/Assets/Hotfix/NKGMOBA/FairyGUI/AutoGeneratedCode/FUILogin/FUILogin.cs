/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;
using ETHotfix;

namespace ETHotfix.FUILogin
{
    [ObjectSystem]
    public class FUILoginAwakeSystem : AwakeSystem<FUILogin, GObject>
    {
        public override void Awake(FUILogin self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class FUILogin : FUI
	{	
		public const string UIPackageName = "FUILogin";
		public const string UIResName = "FUILogin";
		
		/// <summary>
        /// FUILogin的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
		public GComponent self;
		
		public GImage n0;
		public GImage n9;
		public GTextField n10;
		public loginBtn loginBtn;
		public RegistBtn registBtn;
		public GImage accountInput;
		public GImage passwordInput;
		public GTextInput accountText;
		public GTextField loginInfo;
		public ToTestScene ToTestSceneBtn;
		public GTextInput passwordText;
		public GGroup n16;
		public Transition t0;
		public Transition t1;

		private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
		
		private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }

        public static FUILogin CreateInstance()
		{			
			return ComponentFactory.Create<FUILogin, GObject>(CreateGObject());
		}

        public static Task<FUILogin> CreateInstanceAsync()
        {
            TaskCompletionSource<FUILogin> tcs = new TaskCompletionSource<FUILogin>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<FUILogin, GObject>(go));
            });

            return tcs.Task;
        }

        public static FUILogin Create(GObject go)
		{
			return ComponentFactory.Create<FUILogin, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUILogin GetFormPool(GObject go)
        {
            var fui = go.Get<FUILogin>();

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
				n0 = (GImage)com.GetChild("n0");
				n9 = (GImage)com.GetChild("n9");
				n10 = (GTextField)com.GetChild("n10");
				loginBtn = loginBtn.Create(com.GetChild("loginBtn"));
				registBtn = RegistBtn.Create(com.GetChild("registBtn"));
				accountInput = (GImage)com.GetChild("accountInput");
				passwordInput = (GImage)com.GetChild("passwordInput");
				accountText = (GTextInput)com.GetChild("accountText");
				loginInfo = (GTextField)com.GetChild("loginInfo");
				ToTestSceneBtn = ToTestScene.Create(com.GetChild("ToTestSceneBtn"));
				passwordText = (GTextInput)com.GetChild("passwordText");
				n16 = (GGroup)com.GetChild("n16");
				t0 = com.GetTransition("t0");
				t1 = com.GetTransition("t1");
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
			n0 = null;
			n9 = null;
			n10 = null;
			loginBtn.Dispose();
			loginBtn = null;
			registBtn.Dispose();
			registBtn = null;
			accountInput = null;
			passwordInput = null;
			accountText = null;
			loginInfo = null;
			ToTestSceneBtn.Dispose();
			ToTestSceneBtn = null;
			passwordText = null;
			n16 = null;
			t0 = null;
			t1 = null;
		}
	}
}