#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;
    using System;

#if UNITY_EDITOR
    using UnityEditor;
    using Sirenix.Utilities.Editor;
#endif

    public class DebugSerializationExample : SerializedMonoBehaviour
    {
        // Unity will serialize this field because it's public.
        public int SerializedByUnity;

        // This field will not be serialized by Unity because of the NonSerialized attribute, however,
        // Odin will serialize the field because of the OdinSerialize attribute.
        [OdinSerialize, NonSerialized]
        public int SerializedByOdin;

        // Neither Unity nor Odin will serialize this field because of the NonSerialized attribute.
        // By default, the field does not appear in the inspector because it's not serialized.
        [NonSerialized]
        public int NotSerialized;

        // Neither Unity nor Odin will serialize this field because of the NonSerialized attribute.
        // The SerializeField attribute is ignored.
        // By default, the field does not appear in the inspector because it's not serialized.
        [SerializeField, NonSerialized]
        public int WeirdAttributeCombo;

        // Odin only serialized auto-properties - We have no way of knowing what's actually going on full property implementations.
        // It could easily call functions that were not suppose to be called during serialization.
        // This property will not appear in the inspector either.
        [OdinSerialize]
        public int FullProperty
        {
            get { return 7; }
            set { this.SerializedByOdin = value; }
        }

        [DisplayAsString]
        public DateTime DateTime;

        public Type Type;

        // UnitySupportedClass is marked with [Serializable], and is therefore it and everything within it, is serialized by Unity.
        public UnitySerializedClass ClassSerializedByUnity;

        // This class is not marked with [Serializable] which means Unity will not be serializing it. 
        // Odin detects this and serialzies it instead.
        public OdinSerializedClass ClassSerializedByOdin;

        [Serializable]
        public class UnitySerializedClass
        {
            public int MyValue;
        }

        public class OdinSerializedClass
        {
            public int MyValue;
        }

#if UNITY_EDITOR
        [OnInspectorGUI, PropertyOrder(-10)]
        private void DrawInfoAndAnimatingPointer()
        {
            const string message =
                "You can get an overview of how your scripts are serialized opening the serialization debugger window.\n\n" +
                "You can find the window by opening the context menu for the component and selecting 'Serialization Debugger'. " +
                "You can also find it in Tools > Odin Inspector > Serialization Debugger.";

            EditorGUILayout.HelpBox(message, MessageType.Info);

            GUIHelper.RequestRepaint();

            if (Event.current.type == EventType.Repaint)
            {
                var rect = GUIHelper.GetCurrentLayoutRect();
                Matrix4x4 prev = GUI.matrix;

                GUI.matrix =
                    Matrix4x4.TRS(new Vector3(rect.xMax - 30, rect.yMin - 3, 0f), Quaternion.AngleAxis(20f, new Vector3(0f, 0f, 1f)), Vector3.one) *
                    Matrix4x4.TRS(new Vector3(0f, Mathf.Sin((float)EditorApplication.timeSinceStartup * 5f) * 15f, 0f), Quaternion.identity, Vector3.one);

                GUI.color = Color.red;
                EditorIcons.ArrowUp.Draw(new Rect(0, 0, 40, 40), EditorIcons.ArrowUp.Raw);
                GUI.color = Color.white;
                GUI.matrix = prev;
            }
        }
#endif
    }
}
#endif
