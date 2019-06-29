/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;
using ETHotfix;

namespace ETHotfix.FUI5v5Map
{
    [ObjectSystem]
    public class SkillSmallProBarAwakeSystem : AwakeSystem<SkillSmallProBar, GObject>
    {
        public override void Awake(SkillSmallProBar self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class SkillSmallProBar : FUI
	{	
		public const string UIPackageName = "FUI5v5Map";
		public const string UIResName = "SkillSmallProBar";
		
		/// <summary>
        /// SkillSmallProBar的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
		public GProgressBar self;
		
		public GImage n3;
		public GImage bar;

		private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
		
		private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }

        public static SkillSmallProBar CreateInstance()
		{			
			return ComponentFactory.Create<SkillSmallProBar, GObject>(CreateGObject());
		}

        public static Task<SkillSmallProBar> CreateInstanceAsync()
        {
            TaskCompletionSource<SkillSmallProBar> tcs = new TaskCompletionSource<SkillSmallProBar>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<SkillSmallProBar, GObject>(go));
            });

            return tcs.Task;
        }

        public static SkillSmallProBar Create(GObject go)
		{
			return ComponentFactory.Create<SkillSmallProBar, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static SkillSmallProBar GetFormPool(GObject go)
        {
            var fui = go.Get<SkillSmallProBar>();

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
				n3 = (GImage)com.GetChild("n3");
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
			n3 = null;
			bar = null;
		}
	}
}