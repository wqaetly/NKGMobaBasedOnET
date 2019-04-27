/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETModel.FUICheckForResUpdate
{
	public partial class CheckForResUpdateBar : GProgressBar
	{
		public GImage m_background;
		public GImage bar;

		public const string URL = "ui://8l6m4xsyot4fk";

		public static CheckForResUpdateBar CreateInstance()
		{
			return (CheckForResUpdateBar)UIPackage.CreateObject("FUICheckForResUpdate","CheckForResUpdateBar");
		}

		public CheckForResUpdateBar()
		{
		}

		public override void ConstructFromXML(XML xml)
		{
			base.ConstructFromXML(xml);

			m_background = (GImage)this.GetChildAt(0);
			bar = (GImage)this.GetChildAt(1);
		}
	}
}