#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    [TypeInfoBox("Specify a different label text for your properties.")]
    public class LabelTextExamples : MonoBehaviour
    {
        [LabelText("1")]
        public int MyInt1;

        [LabelText("2")]
        public int MyInt2;

        [LabelText("3")]
        public int MyInt3;

		[InfoBox("Use $ to refer to a member string.")]
		[LabelText("$MyInt3")]
		public string LabelText = "Dynamic label text";
    }
}
#endif
