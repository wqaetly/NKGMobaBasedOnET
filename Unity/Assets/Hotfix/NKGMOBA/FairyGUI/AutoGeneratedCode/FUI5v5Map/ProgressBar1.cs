/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;

namespace ETHotfix.FUI5v5Map
{
    [ObjectSystem]
    public class ProgressBar1AwakeSystem : AwakeSystem<ProgressBar1, GObject>
    {
        public override void Awake(ProgressBar1 self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class ProgressBar1 : FUI
	{	
		public const string UIPackageName = "FUI5v5Map";
		public const string UIResName = "ProgressBar1";
		
		/// <summary>
        /// ProgressBar1的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
		public GProgressBar self;
		
		public GGraph n0;
		public GImage bar;

		private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
		
		private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }

        public static ProgressBar1 CreateInstance()
		{			
			return ComponentFactory.Create<ProgressBar1, GObject>(CreateGObject());
		}

        public static Task<ProgressBar1> CreateInstanceAsync()
        {
            TaskCompletionSource<ProgressBar1> tcs = new TaskCompletionSource<ProgressBar1>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<ProgressBar1, GObject>(go));
            });

            return tcs.Task;
        }

        public static ProgressBar1 Create(GObject go)
		{
			return ComponentFactory.Create<ProgressBar1, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static ProgressBar1 GetFormPool(GObject go)
        {
            var fui = go.Get<ProgressBar1>();

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
			
			self = (GProgressBar)go;
			
			self.Add(this);
			
			var com = go.asCom;
				
			if(com != null)
			{	
				n0 = (GGraph)com.GetChild("n0");
				bar = (GImage)com.GetChild("bar");
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
			bar = null;
		}
	}
}