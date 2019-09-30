// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

#pragma warning disable IDE0018 // Inline variable declaration.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only]
    /// Various utilities used throughout <see cref="Animancer"/>.
    /// </summary>
    public static partial class AnimancerEditorUtilities
    {
        /************************************************************************************************************************/
        #region GUI
        /************************************************************************************************************************/

        /// <summary>The highlight color used for fields showing a warning.</summary>
        public static readonly Color
            WarningFieldColor = new Color(1, 0.9f, 0.6f);

        /// <summary>The highlight color used for fields showing an error.</summary>
        public static readonly Color
            ErrorFieldColor = new Color(1, 0.6f, 0.6f);

        /************************************************************************************************************************/

        /// <summary><see cref="GUILayout.ExpandWidth"/> set to false.</summary>
        public static readonly GUILayoutOption[]
            DontExpandWidth = { GUILayout.ExpandWidth(false) };

        /************************************************************************************************************************/

        private static float _IndentSize = -1;

        /// <summary>
        /// The number of pixels of indentation for each <see cref="EditorGUI.indentLevel"/> increment.
        /// </summary>
        public static float IndentSize
        {
            get
            {
                if (_IndentSize < 0)
                {
                    var indentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 1;
                    _IndentSize = EditorGUI.IndentedRect(new Rect()).x;
                    EditorGUI.indentLevel = indentLevel;
                }

                return _IndentSize;
            }
        }

        /************************************************************************************************************************/

        /// <summary>Used by <see cref="TempContent"/>.</summary>
        private static GUIContent _TempContent;

        /// <summary>
        /// Creates and returns a <see cref="GUIContent"/> with the specified parameters on the first call and then
        /// simply returns the same one with new parameters on each subsequent call.
        /// </summary>
        public static GUIContent TempContent(string text = null, string tooltip = null)
        {
            if (_TempContent == null)
                _TempContent = new GUIContent();

            _TempContent.text = text;
            _TempContent.tooltip = tooltip;
            return _TempContent;
        }

        /************************************************************************************************************************/

        /// <summary>Gets a <see cref="GUILayout"/> rect occupying a single standard line.</summary>
        public static Rect GetRect(bool useStandardVerticalSpacing = false)
        {
            if (useStandardVerticalSpacing)
            {
                var spacing = EditorGUIUtility.standardVerticalSpacing;
                var rect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight + spacing);
                rect.yMin += spacing;
                return rect;
            }
            else
            {
                return GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Subtracts the 'width' from the 'area' and returns a new <see cref="Rect"/> occupying the removed section.
        /// </summary>
        public static Rect StealWidth(ref Rect area, float width)
        {
            area.width -= width;
            return new Rect(area.xMax, area.y, width, area.height);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="GUIStyle.CalcMinMaxWidth"/> and returns the max width.
        /// </summary>
        public static float CalculateWidth(this GUIStyle style, GUIContent content)
        {
            float _, width;
            style.CalcMinMaxWidth(content, out _, out width);
            return width;
        }

        /// <summary>
        /// Calls <see cref="GUIStyle.CalcMinMaxWidth"/> and returns the max width.
        /// <para></para>
        /// This method uses the <see cref="TempContent"/>.
        /// </summary>
        public static float CalculateWidth(this GUIStyle style, string content)
        {
            return style.CalculateWidth(TempContent(content));
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Checks if the current event is a mouse up event inside the last GUI Layout rect that was drawn.
        /// </summary>
        public static bool TryUseClickInLastRect()
        {
            var currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseUp &&
                GUILayoutUtility.GetLastRect().Contains(currentEvent.mousePosition))
            {
                currentEvent.Use();
                return true;
            }
            else return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns true and uses the <see cref="Event.current"/> if it was a <see cref="EventType.ContextClick"/>
        /// inside the 'area'. Otherwise returns false.
        /// </summary>
        public static bool TryUseContextClick(Rect area)
        {
            var currentEvent = Event.current;

            if (currentEvent.rawType != EventType.ContextClick ||
                !area.Contains(currentEvent.mousePosition))
                return false;

            currentEvent.Use();
            return true;
        }

        /************************************************************************************************************************/

        private static GUIStyle _WeightLabelStyle;
        private static float _WeightValueWidth;

        private static readonly string[] CommonWeightLabels =
            { "0.0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1.0" };

        /// <summary>
        /// Draws a label showing the 'weight' aligned to the right side of the 'position' and reduces its
        /// <see cref="Rect.width"/> to remove that label from its area.
        /// </summary>
        public static void DoWeightLabelGUI(ref Rect area, float weight)
        {
            if (_WeightLabelStyle == null)
            {
                _WeightLabelStyle = new GUIStyle(GUI.skin.label);
                _WeightValueWidth = GUI.skin.label.CalculateWidth(CommonWeightLabels[0]);
            }

            area.width -= _WeightValueWidth;

            var weightPosition = new Rect(area.xMax, area.y, _WeightValueWidth, area.height);

            string label;
            if (weight < 0 || weight > 1)
            {
                label = weight.ToString("F1");
            }
            else
            {
                var index = (int)((weight + 0.05f) * 10);
                label = CommonWeightLabels[index];
            }

            _WeightLabelStyle.normal.textColor = Color.Lerp(Color.grey, Color.black, weight);
            _WeightLabelStyle.fontStyle = Mathf.Approximately(weight * 10, (int)(weight * 10)) ?
                FontStyle.Normal : FontStyle.Italic;

            GUI.Label(weightPosition, label, _WeightLabelStyle);
        }

        /************************************************************************************************************************/

        /// <summary>The <see cref="EditorGUIUtility.labelWidth"/> from before <see cref="BeginTightLabel"/>.</summary>
        private static float _TightLabelWidth;

        /// <summary>Stores the <see cref="EditorGUIUtility.labelWidth"/> and changes it to the exact width of the 'label'.</summary>
        public static string BeginTightLabel(string label)
        {
            _TightLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = GUI.skin.label.CalculateWidth(label) + EditorGUI.indentLevel * IndentSize;
            return GetNarrowText(label);
        }

        /// <summary>Reverts <see cref="EditorGUIUtility.labelWidth"/> to its previous value.</summary>
        public static void EndTightLabel()
        {
            EditorGUIUtility.labelWidth = _TightLabelWidth;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Divides the given 'position' such that the fields associated with both labels will have equal space
        /// remaining after the labels themselves.
        /// </summary>
        public static void SplitHorizontally(Rect area, string label0, string label1,
             out float width0, out float width1, out Rect rect0, out Rect rect1)
        {
            width0 = GUI.skin.label.CalculateWidth(label0);
            width1 = GUI.skin.label.CalculateWidth(label1);

            const float Padding = 1;

            rect0 = rect1 = area;

            float remainingWidth = area.width - width0 - width1 - Padding;
            rect0.width = width0 + remainingWidth * 0.5f;
            rect1.xMin = rect0.xMax + Padding;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Invokes 'onDrop' if the <see cref="Event.current"/> is a drag and drop event inside the 'dropArea'.
        /// </summary>
        public static void HandleDragAndDrop<T>(Rect dropArea, Func<T, bool> validate, Action<T> onDrop) where T : class
        {
            if (!dropArea.Contains(Event.current.mousePosition))
                return;

            bool isDrop;
            switch (Event.current.type)
            {
                case EventType.DragUpdated:
                    isDrop = false;
                    break;

                case EventType.DragPerform:
                    isDrop = true;
                    break;

                default:
                    return;
            }

            var dragging = DragAndDrop.objectReferences;
            TryDrop(dragging, validate, onDrop, isDrop);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Updates the <see cref="DragAndDrop.visualMode"/> of calls 'onDrop' for each of the 'objects'.
        /// </summary>
        private static void TryDrop<T>(IEnumerable objects, Func<T, bool> validate, Action<T> onDrop, bool isDrop) where T : class
        {
            if (objects == null)
                return;

            var droppedAny = false;

            foreach (var obj in objects)
            {
                var t = obj as T;

                if (t != null && (validate == null || validate(t)))
                {
                    if (!isDrop)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        break;
                    }
                    else
                    {
                        onDrop(t);
                        droppedAny = true;
                    }
                }
            }

            if (droppedAny)
                GUIUtility.ExitGUI();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Uses <see cref="HandleDragAndDrop"/> to deal with drag and drop operations involving
        /// <see cref="AnimationClip"/>s of <see cref="IAnimationClipSource"/>s.
        /// </summary>
        public static void HandleDragAndDropAnimations(Rect dropArea, Action<AnimationClip> onDrop)
        {
            HandleDragAndDrop(dropArea, (clip) => !clip.legacy, onDrop);

            HandleDragAndDrop<IAnimationClipSource>(dropArea, null, (source) =>
            {
                var clips = new List<AnimationClip>();
                source.GetAnimationClips(clips);
                TryDrop(clips, (clip) => !clip.legacy, onDrop, true);
            });
        }

        /************************************************************************************************************************/

        private static Dictionary<string, string> _StringsWithoutSpaces;

        /// <summary>
        /// Returns the 'text' without any spaces if <see cref="EditorGUIUtility.wideMode"/> is false.
        /// Otherwise simply returns the 'text' without any changes.
        /// </summary>
        public static string GetNarrowText(string text)
        {
            if (EditorGUIUtility.wideMode)
                return text;

            if (_StringsWithoutSpaces == null)
                _StringsWithoutSpaces = new Dictionary<string, string>();

            string spaceless;
            if (!_StringsWithoutSpaces.TryGetValue(text, out spaceless))
            {
                spaceless = text.Replace(" ", "");
                _StringsWithoutSpaces.Add(text, spaceless);
            }

            return spaceless;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Begins a vertical layout group using the given style and decreases the
        /// <see cref="EditorGUIUtility.labelWidth"/> to compensate for the indentation.
        /// </summary>
        public static void BeginVerticalBox(GUIStyle style)
        {
            GUILayout.BeginVertical(style);
            EditorGUIUtility.labelWidth -= style.padding.left;
        }

        /// <summary>
        /// Ends a layout group started by <see cref="BeginVerticalBox"/> and restores the
        /// <see cref="EditorGUIUtility.labelWidth"/>.
        /// </summary>
        public static void EndVerticalBox(GUIStyle style)
        {
            EditorGUIUtility.labelWidth += style.padding.left;
            GUILayout.EndVertical();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// <see cref="GUIStyle"/>s used by this window. They are located in a nested class so they don't get
        /// initialised before they are referenced (because they can't be until the first <see cref="OnGUI"/> call).
        /// </summary>
        public static class Styles
        {
            /// <summary>
            /// The standard <see cref="GUISkin.label"/> with the alignment to the right.
            /// </summary>
            public static readonly GUIStyle RightAlignedLabel = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleRight
            };

            /// <summary>
            /// A more compact <see cref="EditorStyles.miniButton"/> with a fixed size as a tiny box.
            /// </summary>
            public static readonly GUIStyle MiniButton = new GUIStyle(EditorStyles.miniButton)
            {
                margin = new RectOffset(0, 0, 2, 0),
                padding = new RectOffset(2, 3, 2, 2),
                alignment = TextAnchor.MiddleCenter,
                fixedHeight = EditorGUIUtility.singleLineHeight,
                fixedWidth = EditorGUIUtility.singleLineHeight - 1
            };
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Context Menus
        /************************************************************************************************************************/

        /// <summary>
        /// Adds a menu function which is disabled if 'isEnabled' is false.
        /// </summary>
        public static void AddMenuItem(GenericMenu menu, string label, bool isEnabled, GenericMenu.MenuFunction func)
        {
            if (!isEnabled)
            {
                menu.AddDisabledItem(new GUIContent(label));
                return;
            }

            menu.AddItem(new GUIContent(label), false, func);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Adds a menu function which passes the result of <see cref="CalculateEditorFadeDuration"/> into 'startFade'.
        /// </summary>
        public static void AddFadeFunction(GenericMenu menu, string label, bool isEnabled, AnimancerNode node, Action<float> startFade)
        {
            // Fade functions need to be delayed twice since the context menu itself causes the next frame delta
            // time to be unreasonably high (which would skip the start of the fade).
            AddMenuItem(menu, label, isEnabled,
                () => EditorApplication.delayCall +=
                () => EditorApplication.delayCall +=
                () =>
                {
                    startFade(node.CalculateEditorFadeDuration());
                });
        }

        /// <summary>
        /// Returns the duration of the 'node's current fade (if any), otherwise returns the 'defaultDuration'.
        /// </summary>
        public static float CalculateEditorFadeDuration(this AnimancerNode node, float defaultDuration = 1)
        {
            return node.FadeSpeed > 0 ? 1 / node.FadeSpeed : defaultDuration;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Adds a menu function to open a web page. If the 'linkSuffix' starts with a '/' then it will be relative to
        /// the <see cref="AnimancerPlayable.APIDocumentationURL"/>.
        /// </summary>
        public static void AddDocumentationLink(GenericMenu menu, string label, string linkSuffix)
        {
            menu.AddItem(new GUIContent(label), false, () =>
            {
                if (linkSuffix[0] == '/')
                    linkSuffix = AnimancerPlayable.APIDocumentationURL + linkSuffix;

                EditorUtility.OpenWithDefaultApp(linkSuffix);
            });
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Adds a menu item to call the specified 'function' for each of the 'property's target objects.
        /// </summary>
        public static void AddPropertyModifierFunction(GenericMenu menu, SerializedProperty property, string label,
            Action<SerializedProperty> function)
        {
            menu.AddItem(new GUIContent(label), false, () =>
            {
                ForEachTarget(property, function);
            });
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Creates a new <see cref="SerializedProperty"/> targeting the same field in each of the target objects of
        /// the specified 'property' and calls the 'function' for each of them, then calls
        /// <see cref="SerializedObject.ApplyModifiedProperties"/>.
        /// </summary>
        public static void ForEachTarget(SerializedProperty property, Action<SerializedProperty> function)
        {
            var targets = property.serializedObject.targetObjects;

            if (targets.Length == 1)
            {
                function(property);
                property.serializedObject.ApplyModifiedProperties();
            }
            else
            {
                var path = property.propertyPath;
                for (int i = 0; i < targets.Length; i++)
                {
                    using (var serializedObject = new SerializedObject(targets[i]))
                    {
                        property = serializedObject.FindProperty(path);
                        function(property);
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Swaps the <see cref="AnimationClip.legacy"/> flag between true and false.
        /// </summary>
        [MenuItem("CONTEXT/AnimationClip/Toggle Legacy")]
        private static void ToggleLegacy(MenuCommand command)
        {
            var clip = (AnimationClip)command.context;
            clip.legacy = !clip.legacy;
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Misc
        /************************************************************************************************************************/

        /// <summary>
        /// Tries to find a <see cref="T"/> component on the 'gameObject' or its parents or children (in that order).
        /// </summary>
        public static T GetComponentInHierarchy<T>(GameObject gameObject) where T : class
        {
            var component = gameObject.GetComponentInParent<T>();
            if (component != null)
                return component;

            return gameObject.GetComponentInChildren<T>();
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Assets cannot reference scene objects.
        /// </summary>
        public static bool ShouldAllowReference(Object obj, Object reference)
        {
            return obj == null || reference == null ||
                !EditorUtility.IsPersistent(obj) ||
                EditorUtility.IsPersistent(reference);
        }

        /************************************************************************************************************************/

        /// <summary>Wraps <see cref="UnityEditorInternal.InternalEditorUtility.GetIsInspectorExpanded"/>.</summary>
        public static bool GetIsInspectorExpanded(Object obj)
        {
            return UnityEditorInternal.InternalEditorUtility.GetIsInspectorExpanded(obj);
        }

        /// <summary>Wraps <see cref="UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded"/>.</summary>
        public static void SetIsInspectorExpanded(Object obj, bool isExpanded)
        {
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(obj, isExpanded);
        }

        /// <summary>Calls <see cref="SetIsInspectorExpanded"/> on all 'objects'.</summary>
        public static void SetIsInspectorExpanded(Object[] objects, bool isExpanded)
        {
            for (int i = 0; i < objects.Length; i++)
                SetIsInspectorExpanded(objects[i], isExpanded);
        }

        /************************************************************************************************************************/

        private static Dictionary<Type, Dictionary<string, MethodInfo>> _TypeToMethodNameToMethod;

        /// <summary>
        /// Tries to find a method with the specified name on the 'target' object and invoke it.
        /// </summary>
        public static object Invoke(object target, string methodName)
        {
            return Invoke(target.GetType(), target, methodName);
        }

        /// <summary>
        /// Tries to find a method with the specified name on the 'target' object and invoke it.
        /// </summary>
        public static object Invoke(Type type, object target, string methodName)
        {
            if (_TypeToMethodNameToMethod == null)
                _TypeToMethodNameToMethod = new Dictionary<Type, Dictionary<string, MethodInfo>>();

            Dictionary<string, MethodInfo> nameToMethod;
            if (!_TypeToMethodNameToMethod.TryGetValue(type, out nameToMethod))
            {
                nameToMethod = new Dictionary<string, MethodInfo>();
                _TypeToMethodNameToMethod.Add(type, nameToMethod);
            }

            MethodInfo method;
            if (!nameToMethod.TryGetValue(methodName, out method))
            {
                method = type.GetMethod(methodName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                nameToMethod.Add(methodName, method);

                if (method == null)
                    RegisterNonCriticalMissingMember(type.FullName, methodName);
            }

            if (method != null)
                return method.Invoke(target, null);

            return null;
        }

        /************************************************************************************************************************/

        private static List<Action<StringBuilder>> _NonCriticalIssues;

        /// <summary>
        /// Registers a delegate that can construct a description of an issue at a later time so that it doesn't waste
        /// the user's time on unimportant issues.
        /// </summary>
        public static void RegisterNonCriticalIssue(Action<StringBuilder> describeIssue)
        {
            if (_NonCriticalIssues == null)
                _NonCriticalIssues = new List<Action<StringBuilder>>();

            _NonCriticalIssues.Add(describeIssue);
        }

        /// <summary>
        /// Calls <see cref="RegisterNonCriticalIssue"/> with an issue indicating that a particular member was not
        /// found by reflection.
        /// </summary>
        public static void RegisterNonCriticalMissingMember(string type, string name)
        {
            RegisterNonCriticalIssue((text) => text
                .Append("[Reflection] Unable to find member '")
                .Append(name)
                .Append("' in type '")
                .Append(type)
                .Append("'"));
        }

        /// <summary>
        /// Appends all issues given to <see cref="RegisterNonCriticalIssue"/> to the 'text'.
        /// </summary>
        public static void AppendNonCriticalIssues(StringBuilder text)
        {
            if (_NonCriticalIssues == null)
                return;

            text.Append("\n\nThe following non-critical issues have also been found" +
                " (in Animancer generally, not specifically this object):\n\n");

            for (int i = 0; i < _NonCriticalIssues.Count; i++)
            {
                text.Append(" - ");
                _NonCriticalIssues[i](text);
                text.Append("\n\n");
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Gets the value of the 'parameter' in the 'animator'.
        /// </summary>
        public static object GetParameterValue(Animator animator, AnimatorControllerParameter parameter)
        {
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Float:
                    return animator.GetFloat(parameter.nameHash);

                case AnimatorControllerParameterType.Int:
                    return animator.GetInteger(parameter.nameHash);

                case AnimatorControllerParameterType.Bool:
                case AnimatorControllerParameterType.Trigger:
                    return animator.GetBool(parameter.nameHash);

                default:
                    throw new ArgumentException("Unhandled AnimatorControllerParameterType: " + parameter.type);
            }
        }

        /// <summary>
        /// Sets the value of the 'parameter' in the 'animator'.
        /// </summary>
        public static void SetParameterValue(Animator animator, AnimatorControllerParameter parameter, object value)
        {
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(parameter.nameHash, (float)value);
                    break;

                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(parameter.nameHash, (int)value);
                    break;

                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(parameter.nameHash, (bool)value);
                    break;

                case AnimatorControllerParameterType.Trigger:
                    if ((bool)value)
                        animator.SetTrigger(parameter.nameHash);
                    else
                        animator.ResetTrigger(parameter.nameHash);
                    break;

                default:
                    throw new ArgumentException("Unhandled AnimatorControllerParameterType: " + parameter.type);
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region IAnimancerClipSource
        /************************************************************************************************************************/

        /// <summary>Objects already being gathered (to prevent recursion).</summary>
        private static readonly HashSet<object>
            AlreadyGathering = new HashSet<object>();

        /************************************************************************************************************************/

        /// <summary>
        /// Gathers all animations linked to the target <see cref="IAnimancerComponent"/> by objects under the same
        /// <see cref="Transform.root"/> as it.
        /// </summary>
        public static void GatherAnimationClips<TSource>(IAnimancerComponent animancer, List<AnimationClip> clips,
            TSource[] sources, Func<int, IAnimancerComponent> sourceIndexToAnimancer)
        {
            if (AlreadyGathering.Contains(animancer))
                return;

            AlreadyGathering.Add(animancer);

            int i = 0;

            GatherClips:
            try
            {
                for (; i < sources.Length; i++)
                {
                    if (sourceIndexToAnimancer(i) == animancer)
                        GatherAnimationClips(sources[i], clips, typeof(TSource));
                }
            }
            catch (NullReferenceException)
            {
                // If something throws a null exception, just ignore it and go to the next object.
                i++;
                goto GatherClips;
            }

            AlreadyGathering.Remove(animancer);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Gathers all animations from the 'source's fields.
        /// </summary>
        public static void GatherAnimationClips(object source, List<AnimationClip> clips, Type iAnimancerClipSource)
        {
            if (AlreadyGathering.Contains(source))
                return;

            AlreadyGathering.Add(source);

            var customSource = source as IAnimationClipSource;
            if (customSource != null)
                customSource.GetAnimationClips(clips);
            else
                GatherClipsFromFields(source, clips, iAnimancerClipSource);

            AlreadyGathering.Remove(source);
        }

        /************************************************************************************************************************/

        /// <summary>Types mapped to a delegate that can automatically gather their clips.</summary>
        private static readonly Dictionary<Type, Action<object, List<AnimationClip>>>
            TypeToGatherClips = new Dictionary<Type, Action<object, List<AnimationClip>>>();

        /// <summary>
        /// Uses reflection to gather <see cref="AnimationClip"/>s from fields on the 'source' object.
        /// </summary>
        private static void GatherClipsFromFields(object source, List<AnimationClip> clips, Type iAnimancerClipSource)
        {
            var type = source.GetType();
            Action<object, List<AnimationClip>> gatherClips;

            if (!TypeToGatherClips.TryGetValue(type, out gatherClips))
            {
                gatherClips = BuildClipGatherer(type, iAnimancerClipSource);
                TypeToGatherClips.Add(type, gatherClips);
            }

            if (gatherClips != null)
                gatherClips(source, clips);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Creates a delegate to gather <see cref="AnimationClip"/>s from all relevant fields in a given 'type'.
        /// </summary>
        private static Action<object, List<AnimationClip>> BuildClipGatherer(Type type, Type iAnimancerClipSource)
        {
            Action<object, List<AnimationClip>> gatherer = null;

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];

                if (field.FieldType == typeof(AnimationClip))
                {
                    gatherer += (obj, clips) =>
                    {
                        var clip = (AnimationClip)field.GetValue(obj);
                        if (clip != null)
                            clips.Add(clip);
                    };
                }
                else if (typeof(IAnimationClipSource).IsAssignableFrom(field.FieldType))
                {
                    gatherer += (obj, clips) =>
                    {
                        var source = (IAnimationClipSource)field.GetValue(obj);
                        if (source != null)
                            source.GetAnimationClips(clips);
                    };
                }
                else if (iAnimancerClipSource.IsAssignableFrom(field.FieldType))
                {
                    gatherer += (obj, clips) =>
                    {
                        var source = field.GetValue(obj);
                        if (source != null)
                            GatherAnimationClips(source, clips, iAnimancerClipSource);
                    };
                }
            }

            return gatherer;
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}

#endif
