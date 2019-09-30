// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only] Draws the inspector GUI for a <see cref="AnimancerState.Serializable{TState}"/>.</summary>
    [CustomPropertyDrawer(typeof(AnimancerState.Serializable<>), true)]
    public class AnimancerStateSerializableDrawer : PropertyDrawer
    {
        /************************************************************************************************************************/

        /// <summary>The visual state of a drawer.</summary>
        private enum Mode
        {
            Uninitialised,
            Normal,
            Expanded,
        }

        /// <summary>The current state of this drawer.</summary>
        private Mode _Mode;

        /************************************************************************************************************************/

        /// <summary>
        /// If set, the field with this name will be drawn with the foldout arrow instead of in its default place.
        /// </summary>
        protected readonly string MainPropertyName;

        /// <summary>
        /// "." + <see cref="MainPropertyName"/> (to avoid creating garbage repeatedly).
        /// </summary>
        protected readonly string MainPropertyPathSuffix;

        /************************************************************************************************************************/

        /// <summary>Constructs a new <see cref="AnimancerStateSerializableDrawer"/>.</summary>
        protected AnimancerStateSerializableDrawer() { }

        /// <summary>
        /// Constructs a new <see cref="AnimancerStateSerializableDrawer"/> and sets the
        /// <see cref="MainPropertyName"/>.
        /// </summary>
        protected AnimancerStateSerializableDrawer(string mainPropertyName)
        {
            MainPropertyName = mainPropertyName;
            MainPropertyPathSuffix = "." + mainPropertyName;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the property specified by the <see cref="MainPropertyName"/>.
        /// </summary>
        private SerializedProperty GetMainProperty(SerializedProperty property)
        {
            if (MainPropertyName == null)
                return null;
            else
                return property.FindPropertyRelative(MainPropertyName);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calculates the number of vertical pixels the 'property' will occupy when it is drawn.
        /// </summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            InitialiseMode(property.serializedObject);

            if (_Mode == Mode.Expanded)
            {
                property.isExpanded = true;
            }

            var height = EditorGUI.GetPropertyHeight(property, label, true);

            if (property.isExpanded || _Mode == Mode.Expanded)
            {
                var mainProperty = GetMainProperty(property);
                if (mainProperty != null)
                    height -= EditorGUI.GetPropertyHeight(mainProperty) + EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// If the <see cref="_Mode"/> is <see cref="Mode.Uninitialised"/>, this method determines how it should start
        /// based on the number of properties in the 'serializedObject'. If the only serialized field is an
        /// <see cref="AnimancerState.Serializable{TState}"/> then it should start expanded.
        /// </summary>
        private void InitialiseMode(SerializedObject serializedObject)
        {
            if (_Mode != Mode.Uninitialised)
                return;

            _Mode = Mode.Expanded;

            var iterator = serializedObject.GetIterator();
            iterator.Next(true);

            int count = 0;
            do
            {
                switch (iterator.propertyPath)
                {
                    case "m_ObjectHideFlags":
                    case "m_Script":
                        break;

                    default:
                        count++;
                        if (count > 1)
                        {
                            _Mode = Mode.Normal;
                            return;
                        }
                        break;
                }
            }
            while (iterator.NextVisible(false));
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Draws the root 'property' GUI and calls <see cref="DoPropertyGUI"/> for each of its children.
        /// </summary>
        public override void OnGUI(Rect area, SerializedProperty property, GUIContent label)
        {
            var mainProperty = GetMainProperty(property);
            if (mainProperty != null)
            {
                DoPropertyGUI(ref area, property, mainProperty, label);

                if (_Mode != Mode.Expanded)
                    property.isExpanded = EditorGUI.Foldout(area, property.isExpanded, GUIContent.none, true);
            }
            else
            {
                area.height = EditorGUI.GetPropertyHeight(property, label, false);
                if (_Mode != Mode.Expanded)
                {
                    EditorGUI.PropertyField(area, property, label, false);
                }
                else
                {
                    label = EditorGUI.BeginProperty(area, label, property);
                    EditorGUI.LabelField(area, label);
                    EditorGUI.EndProperty();
                }
            }

            if (property.isExpanded || _Mode == Mode.Expanded)
            {
                area.y += area.height + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.indentLevel++;

                var baseProperty = property.Copy();

                var depth = property.depth;
                property.NextVisible(true);
                while (property.depth > depth)
                {
                    if (MainPropertyPathSuffix == null || !property.propertyPath.EndsWith(MainPropertyPathSuffix))
                    {
                        label.text = property.displayName;
                        label.tooltip = property.tooltip;
                        DoPropertyGUI(ref area, baseProperty, property, label);
                        if (area.height > 0)
                            area.y += area.height + EditorGUIUtility.standardVerticalSpacing;
                    }

                    if (!property.NextVisible(false))
                        break;
                }

                EditorGUI.indentLevel--;
            }

            CheckContextMenu(area, property);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Draws the 'property' GUI in relation to the 'rootProperty' which was passed into <see cref="OnGUI"/>.
        /// </summary>
        protected virtual void DoPropertyGUI(ref Rect area, SerializedProperty rootProperty, SerializedProperty property, GUIContent label)
        {
            area.height = EditorGUI.GetPropertyHeight(property, label, true);
            EditorGUI.PropertyField(area, property, label, true);

            if (property.propertyPath.EndsWith("._FadeDuration"))
            {
                if (property.floatValue < 0)
                    property.floatValue = 0;
            }
        }

        /************************************************************************************************************************/
        #region Context Menu
        /************************************************************************************************************************/

        /// <summary>
        /// If the <see cref="Event.current"/> is a <see cref="EventType.ContextClick"/> inside the 'clickArea', this
        /// method opens a context menu which is filled by <see cref="BuildContextMenu"/>.
        /// </summary>
        private void CheckContextMenu(Rect clickArea, SerializedProperty rootProperty)
        {
            if (!AnimancerEditorUtilities.TryUseContextClick(clickArea))
                return;

            var menu = new GenericMenu();
            BuildContextMenu(menu, rootProperty);
            menu.ShowAsContext();
        }

        /// <summary>
        /// Fills the 'menu' with functions relevant to the 'rootProperty'.
        /// </summary>
        protected virtual void BuildContextMenu(GenericMenu menu, SerializedProperty rootProperty) { }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}

#endif
