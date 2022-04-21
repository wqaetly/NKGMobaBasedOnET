#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Slate
{

    //Shows a GUI within a popup. The delegate includes the gui calls
    public class QuickPopup : PopupWindowContent
    {

        private System.Action Call;
        private Rect myRect = new Rect(0, 0, 320, 10);

        public static void Show(System.Action Call, Vector2 pos = default(Vector2)) {
            var e = Event.current;
            pos = pos == default(Vector2) ? new Vector2(e.mousePosition.x, e.mousePosition.y) : pos;
            var rect = new Rect(pos.x, pos.y, 0, 0);
            PopupWindow.Show(rect, new QuickPopup(Call));
        }

        public QuickPopup(System.Action Call) { this.Call = Call; }
        public override Vector2 GetWindowSize() { return new Vector2(myRect.xMin + myRect.xMax, myRect.yMin + myRect.yMax); }
        public override void OnGUI(Rect rect) {
            GUILayout.BeginVertical("box");
            Call();
            GUILayout.EndVertical();
            if ( Event.current.type == EventType.Repaint ) {
                myRect.yMax = GUILayoutUtility.GetLastRect().yMax;
            }
        }
    }
}

#endif