#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    [TypeInfoBox("OnValueChanged is used here to create a material for a shader, when the shader is changed.")]
    public class OnValueChangedExamples : MonoBehaviour
    {
        [OnValueChanged("CreateMaterial")]
        public Shader Shader;

        [ReadOnly, InlineEditor(InlineEditorModes.LargePreview)]
        public Material Material;

        private void CreateMaterial()
        {
            if (this.Material != null)
            {
                DestroyImmediate(this.Material);
            }

            if (this.Shader != null)
            {
                this.Material = new Material(this.Shader);
            }
        }
    }
}
#endif
