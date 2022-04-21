#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using ShaderUtil = UnityEditor.ShaderUtil;

namespace Slate
{

    [CustomPropertyDrawer(typeof(ShaderPropertyPopupAttribute))]
    public class ShaderPropertyPopupDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) { return -2; }
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent content) {

            var att = (ShaderPropertyPopupAttribute)attribute;

            var directable = prop.serializedObject.targetObject as IDirectable;
            if ( directable != null ) {
                var actor = directable.actor;
                if ( actor != null ) {
                    var renderer = actor.GetComponent<Renderer>();
                    if ( renderer != null ) {
                        var material = renderer.sharedMaterial;
                        if ( material != null ) {
                            var shader = material.shader;
                            var options = new List<string>();
                            for ( var i = 0; i < ShaderUtil.GetPropertyCount(shader); i++ ) {
                                if ( ShaderUtil.IsShaderPropertyHidden(shader, i) ) {
                                    continue;
                                }

                                if ( att.propertyType != null ) {
                                    var type = ShaderUtil.GetPropertyType(shader, i);
                                    if ( att.propertyType == typeof(Color) && type != ShaderUtil.ShaderPropertyType.Color ) { continue; }
                                    if ( att.propertyType == typeof(Texture) && type != ShaderUtil.ShaderPropertyType.TexEnv ) { continue; }
                                    if ( att.propertyType == typeof(float) && type != ShaderUtil.ShaderPropertyType.Float && type != ShaderUtil.ShaderPropertyType.Range ) { continue; }
                                    if ( ( att.propertyType == typeof(Vector2) || att.propertyType == typeof(Vector4) ) && type != ShaderUtil.ShaderPropertyType.Vector ) { continue; }
                                }

                                options.Add(ShaderUtil.GetPropertyName(shader, i));
                            }

                            prop.stringValue = EditorTools.CleanPopup<string>(content.text, prop.stringValue, options);
                            return;
                        }
                    }
                }
            }

            prop.stringValue = EditorGUILayout.TextField(content.text, prop.stringValue);

        }
    }
}

#endif