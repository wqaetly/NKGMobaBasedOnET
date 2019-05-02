/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETModel.FUICheckForResUpdate
{
	public partial class UI_CheckForResUpdateBar : GProgressBar
	{
		public GImage m_n0;
		public GImage m_bar;

		public const string URL = "ui://233k1ld9rfuv4";

		public static UI_CheckForResUpdateBar CreateInstance()
		{
			return (UI_CheckForResUpdateBar)UIPackage.CreateObject("FUICheckForResUpdate","CheckForResUpdateBar");
		}

		public UI_CheckForResUpdateBar()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_n0 = (GImage)this.GetChildAt(0);
			m_bar = (GImage)this.GetChildAt(1);
		}
	}
}