/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;

namespace ETHotfix.FUIDialog
{
    [ObjectSystem]
    public class FUIDialogAwakeSystem : AwakeSystem<FUIDialog, GObject>
    {
        public override void Awake(FUIDialog self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class FUIDialog : FUI
	{	
		public const string UIPackageName = "FUIDialog";
		public const string UIResName = "FUIDialog";
		
		/// <summary>
        /// FUIDialog的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
		public GComponent self;
		
		public GImage n0;
		public GTextField Tittle;
		public GTextField Conten;
		public tow_cancel tow_cancel;
		public one_confirm tow_confirm;
		public GGroup towmode;
		public one_confirm one_confirm;
		public GGroup onemode;

		private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
		
		private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }

        public static FUIDialog CreateInstance()
		{			
			return ComponentFactory.Create<FUIDialog, GObject>(CreateGObject());
		}

        public static Task<FUIDialog> CreateInstanceAsync()
        {
            TaskCompletionSource<FUIDialog> tcs = new TaskCompletionSource<FUIDialog>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<FUIDialog, GObject>(go));
            });

            return tcs.Task;
        }

        public static FUIDialog Create(GObject go)
		{
			return ComponentFactory.Create<FUIDialog, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUIDialog GetFormPool(GObject go)
        {
            var fui = go.Get<FUIDialog>();

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
				Tittle = (GTextField)com.GetChild("Tittle");
				Conten = (GTextField)com.GetChild("Conten");
				tow_cancel = tow_cancel.Create(com.GetChild("tow_cancel"));
				tow_confirm = one_confirm.Create(com.GetChild("tow_confirm"));
				towmode = (GGroup)com.GetChild("towmode");
				one_confirm = one_confirm.Create(com.GetChild("one_confirm"));
				onemode = (GGroup)com.GetChild("onemode");
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
			Tittle = null;
			Conten = null;
			tow_cancel.Dispose();
			tow_cancel = null;
			tow_confirm.Dispose();
			tow_confirm = null;
			towmode = null;
			one_confirm.Dispose();
			one_confirm = null;
			onemode = null;
		}
	}
}