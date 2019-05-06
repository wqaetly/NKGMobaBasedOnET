/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETModel.FUICheckForResUpdate
{
	public partial class UI_FUICheckForResUpdate : GComponent
	{
		public GImage m_n0;
		public UI_CheckForResUpdateBar m_processbar;
		public GTextField m_updateInfo;
		public GGroup m_n4;

		public const string URL = "ui://233k1ld9rfuv0";

		public static UI_FUICheckForResUpdate CreateInstance()
		{
			return (UI_FUICheckForResUpdate)UIPackage.CreateObject("FUICheckForResUpdate","FUICheckForResUpdate");
		}

		public UI_FUICheckForResUpdate()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_n0 = (GImage)this.GetChildAt(0);
			m_processbar = (UI_CheckForResUpdateBar)this.GetChildAt(1);
			m_updateInfo = (GTextField)this.GetChildAt(2);
			m_n4 = (GGroup)this.GetChildAt(3);
		}
	}
}