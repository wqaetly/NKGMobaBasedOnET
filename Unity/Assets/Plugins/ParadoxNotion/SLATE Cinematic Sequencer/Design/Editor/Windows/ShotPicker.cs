#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Slate
{

    ///A popup window to select a camera shot with a preview
    public class ShotPicker : PopupWindowContent
    {

        private System.Action<ShotCamera> callback;
        private Vector2 scrollPos;
        private IDirector director;

        ///Shows the popup menu at position and with title
        public static void Show(Vector2 pos, IDirector director, System.Action<ShotCamera> callback) {
            PopupWindow.Show(new Rect(pos.x, pos.y, 0, 0), new ShotPicker(director, callback));
        }

        public ShotPicker(IDirector director, System.Action<ShotCamera> callback) {
            this.director = director;
            this.callback = callback;
        }

        public override Vector2 GetWindowSize() { return new Vector2(300, 600); }
        public override void OnGUI(Rect rect) {

            GUILayout.BeginVertical("box");

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
            foreach ( var shot in Object.FindObjectsOfType<ShotCamera>() ) {
                var res = EditorTools.GetGameViewSize();
                var texture = shot.GetRenderTexture((int)res.x, (int)res.y);
                if ( GUILayout.Button(texture, GUILayout.Width(262), GUILayout.Height(120)) ) {
                    callback(shot);
                    editorWindow.Close();
                    return;
                }
                var r = GUILayoutUtility.GetLastRect();
                r.x += 10;
                r.y += 10;
                r.width -= 20;
                r.height = 18;
                GUI.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                GUI.DrawTexture(r, Slate.Styles.whiteTexture);
                GUI.color = Color.white;
                GUI.Label(r, shot.name, Styles.leftLabel);
            }
            EditorGUILayout.EndScrollView();

            GUILayout.Space(10);

            if ( GUILayout.Button("Create Shot") ) {
                callback(ShotCamera.Create(director.context.transform));
                editorWindow.Close();
            }

            GUILayout.Space(10);

            GUILayout.EndVertical();

        }
    }
}

#endif