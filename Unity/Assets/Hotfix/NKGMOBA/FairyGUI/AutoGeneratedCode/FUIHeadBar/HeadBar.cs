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
		
		public GImage n16;
		public GTextField Tex_Level;
		public GTextField Tex_PlayerName;
		public GImage n15;
		public Bar_HP Bar_HP;
		public GList HPGapList;
		public Bar_MP Bar_MP;

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

        public static ETTask<HeadBar> CreateInstanceAsync()
        {
            ETTaskCompletionSource<HeadBar> tcs = new ETTaskCompletionSource<HeadBar>();

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
				n16 = (GImage)com.GetChild("n16");
				Tex_Level = (GTextField)com.GetChild("Tex_Level");
				Tex_PlayerName = (GTextField)com.GetChild("Tex_PlayerName");
				n15 = (GImage)com.GetChild("n15");
				Bar_HP = Bar_HP.Create(com.GetChild("Bar_HP"));
				HPGapList = (GList)com.GetChild("HPGapList");
				Bar_MP = Bar_MP.Create(com.GetChild("Bar_MP"));
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
			n16 = null;
			Tex_Level = null;
			Tex_PlayerName = null;
			n15 = null;
			Bar_HP.Dispose();
			Bar_HP = null;
			HPGapList = null;
			Bar_MP.Dispose();
			Bar_MP = null;
		}
	}
}