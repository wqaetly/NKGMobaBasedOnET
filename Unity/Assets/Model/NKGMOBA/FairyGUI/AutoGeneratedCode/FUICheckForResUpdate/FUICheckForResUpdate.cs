/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETModel
{
    public partial class FUICheckForResUpdate : GComponent
    {
        public GImage n0;
        public CheckForResUpdateBar processbar;
        public GTextField updateInfo;
        public GGroup n4;
        public const string URL = "ui://233k1ld9rfuv0";

        public static FUICheckForResUpdate CreateInstance()
        {
            return (FUICheckForResUpdate)UIPackage.CreateObject("FUICheckForResUpdate", "FUICheckForResUpdate");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            n0 = (GImage)GetChildAt(0);
            processbar = (CheckForResUpdateBar)GetChildAt(1);
            updateInfo = (GTextField)GetChildAt(2);
            n4 = (GGroup)GetChildAt(3);
        }
    }
}