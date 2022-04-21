/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET
{
    public partial class FUI_CheckForResUpdate : GComponent
    {
        public GImage m_n0;
        public FUI_CheckForResUpdateBar m_processbar;
        public GTextField m_updateInfo;
        public GGroup m_n4;
        public const string URL = "ui://233k1ld9rfuv0";

        public static FUI_CheckForResUpdate CreateInstance()
        {
            return (FUI_CheckForResUpdate)UIPackage.CreateObject("CheckForResUpdate", "CheckForResUpdate", typeof(FUI_CheckForResUpdate));
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_n0 = (GImage)GetChildAt(0);
            m_processbar = (FUI_CheckForResUpdateBar)GetChildAt(1);
            m_updateInfo = (GTextField)GetChildAt(2);
            m_n4 = (GGroup)GetChildAt(3);
        }
    }
}