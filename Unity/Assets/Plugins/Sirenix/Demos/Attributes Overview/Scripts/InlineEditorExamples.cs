#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    public class InlineEditorExamples : MonoBehaviour
    {
        [DisableInInlineEditors]
        public Vector3 DisabledInInlineEditors;

        [HideInInlineEditors]
        public Vector3 HiddenInInlineEditors;

        [InlineEditor]
        public InlineEditorExamples Self;

        [InlineEditor]
        public Transform InlineComponent;

        [InlineEditor(InlineEditorModes.FullEditor)]
        public Material FullInlineEditor;

        [InlineEditor(InlineEditorModes.GUIAndHeader)]
        public Material InlineMaterial;

        [InlineEditor(InlineEditorModes.SmallPreview)]
        public Material[] InlineMaterialList;

        [InlineEditor(InlineEditorModes.LargePreview)]
        public Mesh InlineMeshPreview;

        [LabelText("Boxed / Default")]
        [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
        public Transform Boxed;

        [LabelText("Foldout")]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public MinMaxSliderExamples Foldout;

        [LabelText("Hide ObjectField")]
        [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
        public Transform CompletelyHidden;

        [LabelText("Show ObjectField if null")]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public Transform OnlyHiddenWhenNotNull;
    }
}
#endif
