#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Linq;

namespace Slate
{

    [CustomEditor(typeof(ActionClips.AnimateBlendShape))]
    public class AnimateBlendShapeInspector : ActionClipInspector<ActionClips.AnimateBlendShape>
    {


        public override void OnInspectorGUI() {

            base.ShowCommonInspector();

            GUI.enabled = action.root.currentTime <= 0;

            if ( action.actor == null ) {
                action.skinName = EditorGUILayout.TextField("Skin", action.skinName);
                action.shapeName = EditorGUILayout.TextField("Shape", action.shapeName);
                return;
            }

            var skins = action.actor.GetComponentsInChildren<SkinnedMeshRenderer>().Where(s => s.sharedMesh.blendShapeCount > 0).ToList();
            if ( skins == null || skins.Count == 0 ) {
                EditorGUILayout.HelpBox("There are no Skinned Mesh Renderers with blend shapes within the actor's GameObject hierarchy.", MessageType.Warning);
                return;
            }

            action.skinName = EditorTools.Popup<string>("Skin", action.skinName, skins.Select(s => s.name).ToList());
            if ( action.skinName != string.Empty ) {
                var skin = skins.ToList().Find(s => s.name == action.skinName);
                if ( skin != null ) {
                    action.shapeName = EditorTools.Popup<string>("Shape", action.shapeName, skin.GetBlendShapeNames().ToList());
                }
            }

            base.ShowAnimatableParameters();
        }
    }
}

#endif