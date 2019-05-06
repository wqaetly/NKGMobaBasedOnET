using System.Collections.Generic;

namespace ETModel
{
	/// <summary>
	/// UI栈
	/// </summary>
	public class FUIStackComponent: Component
	{
		private readonly Stack<FUI> uis = new Stack<FUI>();

		public int Count
		{
			get
			{
				return uis.Count;
			}
		}

		public void Push(FUI fui)
		{
			uis.Peek().Visible = false;
			uis.Push(fui);
		}

		public void Pop()
		{
			FUI fui = uis.Pop();
			fui.Dispose();
			if (uis.Count > 0)
			{
				uis.Peek().Visible = true;
			}
		}

		public void Clear()
		{
			while (uis.Count > 0)
			{
				FUI fui = uis.Pop();
				fui.Dispose();
			}
		}
	}
}