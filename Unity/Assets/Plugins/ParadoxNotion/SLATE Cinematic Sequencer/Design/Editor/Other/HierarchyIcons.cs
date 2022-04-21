#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Slate
{

    [InitializeOnLoad]
    public static class HierarchyIcons
    {

        static HierarchyIcons() {
            EditorApplication.hierarchyWindowItemOnGUI += ShowIcons;
            Styles.cutsceneIcon = (Texture2D)Resources.Load("Cutscene Icon"); //ensure
        }

        static void ShowIcons(int ID, Rect r) {
            var go = EditorUtility.InstanceIDToObject(ID) as GameObject;
            if ( go == null ) {
                return;
            }

            if ( go.GetComponent<Cutscene>() != null ) {
                r.x = r.xMax - 16;
                r.width = 16;
                GUI.DrawTexture(r, Styles.cutsceneIcon);
            }

            if ( go.GetComponent(typeof(IDirectableCamera)) != null ) {
                r.x = r.xMax - 16;
                r.width = 16;
                GUI.DrawTexture(r, Styles.cameraIcon);
            }
        }
    }
}

#endif