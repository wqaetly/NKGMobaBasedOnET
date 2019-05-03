/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETModel.FUILoading
{
	public partial class UI_FUILoading : GComponent
	{
		public GImage m_n1;
		public Transition m_t0;

		public const string URL = "ui://enltropwpxxk0";

		public static UI_FUILoading CreateInstance()
		{
			return (UI_FUILoading)UIPackage.CreateObject("FUILoading","FUILoading");
		}

		public UI_FUILoading()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_n1 = (GImage)this.GetChildAt(0);
			m_t0 = this.GetTransitionAt(0);
		}
	}
}