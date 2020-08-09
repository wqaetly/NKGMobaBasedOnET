/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETModel
{
    public partial class CheckForResUpdateBar : GProgressBar
    {
        public GImage n0;
        public GImage bar;
        public const string URL = "ui://233k1ld9rfuv4";

        public static CheckForResUpdateBar CreateInstance()
        {
            return (CheckForResUpdateBar)UIPackage.CreateObject("FUICheckForResUpdate", "CheckForResUpdateBar");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            n0 = (GImage)GetChildAt(0);
            bar = (GImage)GetChildAt(1);
        }
    }
}