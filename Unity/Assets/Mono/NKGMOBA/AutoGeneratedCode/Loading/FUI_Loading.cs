/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ET
{
    public partial class FUI_Loading : GComponent
    {
        public GImage m_n1;
        public Transition m_t0;
        public const string URL = "ui://enltropwpxxk0";

        public static FUI_Loading CreateInstance()
        {
            return (FUI_Loading)UIPackage.CreateObject("Loading", "Loading", typeof(FUI_Loading));
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            m_n1 = (GImage)GetChildAt(0);
            m_t0 = GetTransitionAt(0);
        }
    }
}