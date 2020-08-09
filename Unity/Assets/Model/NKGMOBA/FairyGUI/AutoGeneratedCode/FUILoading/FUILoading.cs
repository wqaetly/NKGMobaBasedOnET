/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ETModel
{
    public partial class FUILoading : GComponent
    {
        public GImage n1;
        public Transition t0;
        public const string URL = "ui://enltropwpxxk0";

        public static FUILoading CreateInstance()
        {
            return (FUILoading)UIPackage.CreateObject("FUILoading", "FUILoading");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            n1 = (GImage)GetChildAt(0);
            t0 = GetTransitionAt(0);
        }
    }
}