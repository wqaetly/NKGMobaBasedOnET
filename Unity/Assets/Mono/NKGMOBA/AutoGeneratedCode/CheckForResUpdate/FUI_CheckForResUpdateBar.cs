/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET
{
    public partial class FUI_CheckForResUpdateBar : GProgressBar
    {
        public GImage m_n0;
        public GImage m_bar;
        public const string URL = "ui://233k1ld9rfuv4";

        public static FUI_CheckForResUpdateBar CreateInstance()
        {
            return (FUI_CheckForResUpdateBar)UIPackage.CreateObject("CheckForResUpdate", "CheckForResUpdateBar", typeof(FUI_CheckForResUpdateBar));
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_n0 = (GImage)GetChildAt(0);
            m_bar = (GImage)GetChildAt(1);
        }
    }
}