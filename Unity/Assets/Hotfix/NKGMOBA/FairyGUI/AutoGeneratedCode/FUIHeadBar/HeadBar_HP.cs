/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;
using ETHotfix;

namespace ETHotfix.FUIHeadBar
{
    [ObjectSystem]
    public class HeadBar_HPAwakeSystem : AwakeSystem<HeadBar_HP, GObject>
    {
        public override void Awake(HeadBar_HP self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class HeadBar_HP : FUI
	{	
		public const string UIPackageName = "FUIHeadBar";
		public const string UIResName = "HeadBar_HP";
		
		/// <summary>
        /// HeadBar_HP的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
		public GProgressBar self;
		
		public GImage bar;

		private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
		
		private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }

        public static HeadBar_HP CreateInstance()
		{			
			return ComponentFactory.Create<HeadBar_HP, GObject>(CreateGObject());
		}

        public static Task<HeadBar_HP> CreateInstanceAsync()
        {
            TaskCompletionSource<HeadBar_HP> tcs = new TaskCompletionSource<HeadBar_HP>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<HeadBar_HP, GObject>(go));
            });

            return tcs.Task;
        }

        public static HeadBar_HP Create(GObject go)
		{
			return ComponentFactory.Create<HeadBar_HP, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static HeadBar_HP GetFormPool(GObject go)
        {
            var fui = go.Get<HeadBar_HP>();

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
			bar = null;
		}
	}
}