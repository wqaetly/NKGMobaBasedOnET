#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate
{

    [CustomEditor(typeof(Character))]
    public class CharacterInspector : Editor
    {

        private Dictionary<BlendShapeGroup, bool> foldStates = new Dictionary<BlendShapeGroup, bool>();

        private SerializedProperty neckProp;
        private SerializedProperty headProp;
        private SerializedProperty upVectorProp;
        private SerializedProperty rotationOffsetProp;

        private Character character {
            get { return (Character)target; }
        }

        void OnEnable() {
            neckProp = serializedObject.FindProperty("_neckTransform");
            headProp = serializedObject.FindProperty("_headTransform");
            upVectorProp = serializedObject.FindProperty("_upVector");
            rotationOffsetProp = serializedObject.FindProperty("_rotationOffset");
            SetWireframeHidden(true);
            if ( !Application.isPlaying && character != null ) {
                character.ResetExpressions();
            }
        }

        void OnDisable() {
            SetWireframeHidden(false);
            if ( !Application.isPlaying && character != null ) {
                character.ResetExpressions();
            }
        }

        void SetWireframeHidden(bool active) {
#if UNITY_5_5_OR_NEWER

            return;


#else
			if (character != null){
				foreach(var renderer in character.GetComponentsInChildren<Renderer>(false)){
					EditorUtility.SetSelectedWireframeHidden(renderer, active);
				}
			}
#endif
        }

        public override void OnInspectorGUI() {

            GUILayout.Space(10);

            serializedObject.Update();
            EditorTools.Header("Head Look At");
            EditorGUILayout.PropertyField(neckProp);
            EditorGUILayout.PropertyField(headProp);
            EditorGUILayout.PropertyField(upVectorProp);
            EditorGUILayout.PropertyField(rotationOffsetProp);
            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(10);

            EditorTools.Header("Expressions");
            var skins = character.GetComponentsInChildren<SkinnedMeshRenderer>().Where(s => s.sharedMesh.blendShapeCount > 0).ToList();
            if ( skins == null || skins.Count == 0 ) {
                EditorGUILayout.HelpBox("There are no Skinned Mesh Renderers with blend shapes within the actor's GameObject hierarchy.", MessageType.Warning);
                return;
            }

            if ( GUILayout.Button("Create New Expression") ) {
                Undo.RecordObject(character, "Add Expression");
                character.expressions.Add(new BlendShapeGroup());
            }

            GUILayout.Space(5);


            EditorGUI.indentLevel++;
            foreach ( var expression in character.expressions.ToArray() ) {

                var foldState = false;
                if ( !foldStates.TryGetValue(expression, out foldState) ) {
                    foldStates[expression] = false;
                }

                GUI.backgroundColor = new Color(0, 0, 0, 0.3f);
                GUILayout.BeginVertical(Slate.Styles.headerBoxStyle);
                GUI.backgroundColor = Color.white;

                GUILayout.BeginHorizontal();
                foldStates[expression] = EditorGUILayout.Foldout(foldStates[expression], expression.name);
                if ( GUILayout.Button("X", GUILayout.Width(18)) ) {
                    Undo.RecordObject(character, "Remove Expression");
                    expression.weight = 0;
                    character.expressions.Remove(expression);
                }
                GUILayout.EndHorizontal();

                if ( foldStates[expression] ) {

                    EditorGUI.BeginChangeCheck();
                    var expName = EditorGUILayout.TextField("Name", expression.name);
                    var expWeight = EditorGUILayout.Slider("Debug Weight", expression.weight, 0, 1);
                    if ( EditorGUI.EndChangeCheck() ) {
                        Undo.RecordObject(character, "Expression Changed");
                        expression.name = expName;
                        expression.weight = expWeight;
                    }


                    foreach ( var shape in expression.blendShapes.ToArray() ) {
                        GUILayout.BeginHorizontal("box");

                        GUILayout.BeginVertical();
                        var skin = shape.skin;
                        var name = shape.name;
                        var weight = shape.weight;
                        EditorGUI.BeginChangeCheck();
                        skin = EditorTools.Popup<SkinnedMeshRenderer>("Skin", skin, skins);
                        if ( skin != null ) {
                            name = EditorTools.Popup<string>("Shape", name, skin.GetBlendShapeNames().ToList());
                            weight = EditorGUILayout.Slider("Weight", weight, -1, 2);
                        }
                        GUILayout.EndVertical();

                        if ( skin != shape.skin || name != shape.name ) {
                            shape.SetRealWeight(0);
                        }

                        if ( EditorGUI.EndChangeCheck() ) {
                            Undo.RecordObject(character, "Expression Changed");
                            shape.skin = skin;
                            shape.name = name;
                            shape.weight = weight;
                        }

                        if ( GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(50)) ) {
                            Undo.RecordObject(character, "Remove Expression Blend Shape");
                            shape.SetRealWeight(0);
                            expression.blendShapes.Remove(shape);
                        }

                        GUILayout.EndHorizontal();
                        GUILayout.Space(5);
                    }

                    if ( GUILayout.Button("Add Blend Shape") ) {
                        Undo.RecordObject(character, "Add Expression Blend Shape");
                        expression.blendShapes.Add(new BlendShape());
                    }

                    GUILayout.Space(5);
                }

                GUILayout.EndVertical();
                GUILayout.Space(5);
            }

            EditorGUI.indentLevel--;


            if ( GUI.changed ) {
                EditorUtility.SetDirty(character);
            }
        }
    }
}

#endif