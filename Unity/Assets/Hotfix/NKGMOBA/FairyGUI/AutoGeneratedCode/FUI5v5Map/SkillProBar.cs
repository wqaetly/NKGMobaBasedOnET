/** This is an automatically generated class by FairyGUI plugin FGUI2ET. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;
using ETModel;
using ETHotfix;

namespace ETHotfix.FUI5v5Map
{
    [ObjectSystem]
    public class SkillProBarAwakeSystem : AwakeSystem<SkillProBar, GObject>
    {
        public override void Awake(SkillProBar self, GObject go)
        {
            self.Awake(go);
        }
    }
	
	public sealed class SkillProBar : FUI
	{	
		public const string UIPackageName = "FUI5v5Map";
		public const string UIResName = "SkillProBar";
		
		/// <summary>
        /// SkillProBar的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
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

        public static SkillProBar CreateInstance()
		{			
			return ComponentFactory.Create<SkillProBar, GObject>(CreateGObject());
		}

        public static ETTask<SkillProBar> CreateInstanceAsync()
        {
            ETTaskCompletionSource<SkillProBar> tcs = new ETTaskCompletionSource<SkillProBar>();

            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(ComponentFactory.Create<SkillProBar, GObject>(go));
            });

            return tcs.Task;
        }

        public static SkillProBar Create(GObject go)
		{
			return ComponentFactory.Create<SkillProBar, GObject>(go);
		}
		
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static SkillProBar GetFormPool(GObject go)
        {
            var fui = go.Get<SkillProBar>();

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