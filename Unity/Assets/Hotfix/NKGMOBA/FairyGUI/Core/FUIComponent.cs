using ETModel;
using FairyGUI;

namespace ETHotfix
{
    [ObjectSystem]
	public class FUIComponentAwakeSystem : AwakeSystem<FUIComponent>
	{
		public override void Awake(FUIComponent self)
		{
			self.Root = ComponentFactory.Create<FUI, GObject>(GRoot.inst);
		}
	}

	/// <summary>
	/// 管理所有顶层UI, 顶层UI都是GRoot的孩子
	/// </summary>
	public class FUIComponent: Component
	{
		public FUI Root;
		
		public override void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			base.Dispose();

            Root.Dispose();
            Root = null;
		}

		public void Add(FUI ui, bool asChildGObject)
		{
			Root?.Add(ui, asChildGObject);
		}
		
		public void Remove(string name)
		{
			Root?.Remove(name);
		}
		
		/// <summary>
		/// 通过名字获得FUI
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public FUI Get(string name)
		{
			return Root?.Get(name);
        }
		
		/// <summary>
		/// 通过ID获得FUI
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public FUI Get(long id)
		{
			return Root?.Get(id.ToString());
		}

        public FUI[] GetAll()
        {
            return Root?.GetAll();
        }

        public void Clear()
        {
            var childrens = GetAll();

            if(childrens != null)
            {
                foreach (var fui in childrens)
                {
                    Remove(fui.Name);
                }
            }
        }
	}
}