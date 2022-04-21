#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Sirenix.OdinInspector.Editor;

namespace Slate
{
    [CustomEditor(typeof(Cutscene), true)]
    public class CutsceneInspector : OdinEditor
    {
        private bool optionsFold = true;
        private bool actorsFold = false;

        private static Cutscene cutscene;
        private static bool willResample;
        private static bool willDirty;
        private static Dictionary<Object, Editor> directableEditors = new Dictionary<Object, Editor>();
        private static Editor currentDirectableEditor;

        //...
        void OnEnable()
        {
            currentDirectableEditor = null;
            cutscene = (Cutscene) target;
            willResample = false;
            willDirty = false;
        }

        //...
        void OnDisable()
        {
            DestroyImmediate(currentDirectableEditor);
            foreach (var pair in directableEditors)
            {
                DestroyImmediate(pair.Value);
            }

            currentDirectableEditor = null;
            directableEditors.Clear();
            cutscene = null;
            willResample = false;
            willDirty = false;
        }

        //...
        public override void OnInspectorGUI()
        {
            cutscene = (Cutscene) target;

            if (UnityEditor.EditorUtility.IsPersistent(cutscene))
            {
                EditorGUILayout.HelpBox("To edit a cutscene prefab please open it first.", MessageType.Info);
                return;
            }

            var e = Event.current;
            GUI.skin.GetStyle("label").richText = true;

            if (e.rawType == EventType.MouseDown && e.button == 0)
            {
                //generic undo
                Undo.RegisterFullObjectHierarchyUndo(cutscene.groupsRoot.gameObject, "Cutscene Inspector");
                Undo.RecordObject(cutscene, "Cutscene Inspector");
                willDirty = true;
            }

            if (e.rawType == EventType.MouseUp && e.button == 0 || e.rawType == EventType.KeyUp)
            {
                willDirty = true;
                if (CutsceneUtility.selectedObject != null &&
                    CutsceneUtility.selectedObject.startTime <= cutscene.currentTime)
                {
                    willResample = true;
                }
            }

            GUILayout.Space(5);
            if (GUILayout.Button("EDIT IN SLATE"))
            {
                CutsceneEditor.ShowWindow(cutscene);
            }

            GUILayout.Space(5);

            DoCutsceneInspector();
            DoSelectionInspector();

            if (willDirty)
            {
                willDirty = false;
                EditorUtility.SetDirty(cutscene);
                if (CutsceneUtility.selectedObject as UnityEngine.Object != null)
                {
                    EditorUtility.SetDirty((UnityEngine.Object) CutsceneUtility.selectedObject);
                }
            }

            if (willResample)
            {
                //resample after the changes on fresh gui pass
                willResample = false;
                //delaycall so that other gui controls are finalized before resample.
                EditorApplication.delayCall += () =>
                {
                    if (cutscene != null) cutscene.ReSample();
                };
            }

            Repaint();
        }

        //Show cutscene options
        void DoCutsceneInspector()
        {
            GUI.color = new Color(0, 0, 0, 0.2f);
            GUILayout.BeginHorizontal(Slate.Styles.headerBoxStyle);
            GUI.color = Color.white;
            GUILayout.Label(string.Format("<b>{0} Cutscene Settings</b>", optionsFold ? "▼" : "▶"));
            GUILayout.EndHorizontal();

            var lastRect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(lastRect, MouseCursor.Link);
            if (Event.current.type == EventType.MouseDown && lastRect.Contains(Event.current.mousePosition))
            {
                optionsFold = !optionsFold;
                Event.current.Use();
            }

            GUILayout.Space(2);
            if (optionsFold)
            {
                base.OnInspectorGUI();

                DoActorsInspector();
            }
        }

        //Show bound actors
        void DoActorsInspector()
        {
            actorsFold = EditorGUILayout.Foldout(actorsFold, "Affected Group Actors");
            GUI.enabled = cutscene.currentTime == 0;
            if (actorsFold)
            {
                EditorGUI.indentLevel++;
                var exists = false;
                foreach (var group in cutscene.groups.OfType<ActorGroup>())
                {
                    var name = string.IsNullOrEmpty(group.name) ? "(No Name Specified)" : group.name;
                    group.actor =
                        EditorGUILayout.ObjectField(name, group.actor, typeof(GameObject), true) as GameObject;
                    exists = true;
                }

                if (!exists)
                {
                    GUILayout.Label("No Actor Groups");
                }

                EditorGUI.indentLevel--;
            }

            GUI.enabled = true;
        }

        //Show selection inspector
        static void DoSelectionInspector()
        {
            var selection = CutsceneUtility.selectedObject as Object; //cast object
            if (currentDirectableEditor != null && (currentDirectableEditor.target != selection || selection == null))
            {
                var disableMethod = currentDirectableEditor.GetType().GetMethod("OnDisable",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.FlattenHierarchy);
                if (disableMethod != null)
                {
                    disableMethod.Invoke(currentDirectableEditor, null);
                }
            }

            if (selection == null)
            {
                currentDirectableEditor = null;
                return;
            }

            Editor newEditor = null;
            if (!directableEditors.TryGetValue(selection, out newEditor))
            {
                directableEditors[selection] = newEditor = Editor.CreateEditor(selection);
            }

            if (currentDirectableEditor != newEditor)
            {
                var enableMethod = newEditor.GetType().GetMethod("OnEnable",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.FlattenHierarchy);
                if (enableMethod != null)
                {
                    enableMethod.Invoke(newEditor, null);
                }

                currentDirectableEditor = newEditor;
            }

            EditorTools.BoldSeparator();
            GUILayout.Space(4);
            ShowPreliminaryInspector();
            currentDirectableEditor.OnInspectorGUI();
        }

        //Show basic stuff
        static void ShowPreliminaryInspector()
        {
            var type = CutsceneUtility.selectedObject.GetType();
            var nameAtt = type.GetCustomAttributes(typeof(NameAttribute), false).FirstOrDefault() as NameAttribute;
            var name = nameAtt != null ? nameAtt.name : type.Name.SplitCamelCase();
            var withinRange = cutscene.currentTime > 0 &&
                              cutscene.currentTime >= CutsceneUtility.selectedObject.startTime &&
                              cutscene.currentTime <= CutsceneUtility.selectedObject.endTime;
            var keyable = CutsceneUtility.selectedObject is IKeyable &&
                          (CutsceneUtility.selectedObject as IKeyable).animationData != null &&
                          (CutsceneUtility.selectedObject as IKeyable).animationData.isValid;
            var isActive = CutsceneUtility.selectedObject.isActive;

            GUI.color = new Color(0, 0, 0, 0.2f);
            GUILayout.BeginHorizontal(Slate.Styles.headerBoxStyle);
            GUI.color = Color.white;
            GUILayout.Label(string.Format("<b><size=18>{0}{1}</size></b>",
                withinRange && keyable && isActive ? "<color=#eb5b50>●</color> " : "", name));
            GUI.backgroundColor = default(Color);
            var clip = CutsceneUtility.selectedObject as ActionClip;
            if (clip != null && GUILayout.Button(Styles.gearIcon, GUILayout.Width(40)))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Copy Values"), false, () => { CutsceneUtility.CopyClipValues(clip); });
                menu.AddItem(new GUIContent("Paste Values"), false, () => { CutsceneUtility.PasteClipValues(clip); });
                menu.ShowAsContext();
            }

            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            if (Prefs.showDescriptions)
            {
                var descAtt =
                    type.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as
                        DescriptionAttribute;
                var description = descAtt != null ? descAtt.description : null;
                if (!string.IsNullOrEmpty(description))
                {
                    EditorGUILayout.HelpBox(description, MessageType.None);
                }
            }

            GUILayout.Space(2);
        }
    }
}

#endif