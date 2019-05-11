#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class OnInspectorGUIExamples : MonoBehaviour
    {
        [OnInspectorGUI("DrawPreview", append: true)]
        public Texture2D Texture;

        private void DrawPreview()
        {
            if (this.Texture == null) return;

            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(this.Texture);
            GUILayout.EndVertical();
        }

#if UNITY_EDITOR

        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("On Inspector GUI can also be used on both methods and properties", UnityEditor.MessageType.Info);
        }

#endif
    }
}
#endif
