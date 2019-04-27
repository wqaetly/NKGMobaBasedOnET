/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETModel.FUICheckForResUpdate
{
	public partial class FUICheckForResUpdate : GComponent
	{
		public GLoader m_bg;
		public CheckForResUpdateBar m_bar;
		public GTextField m_text;

		public const string URL = "ui://8l6m4xsypiue1";

		public static FUICheckForResUpdate CreateInstance()
		{
			return (FUICheckForResUpdate)UIPackage.CreateObject("FUICheckForResUpdate","FUICheckForResUpdate");
		}

		public FUICheckForResUpdate()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_bg = (GLoader)this.GetChildAt(0);
			m_bar = (CheckForResUpdateBar)this.GetChildAt(1);
			m_text = (GTextField)this.GetChildAt(2);
		}
	}
}