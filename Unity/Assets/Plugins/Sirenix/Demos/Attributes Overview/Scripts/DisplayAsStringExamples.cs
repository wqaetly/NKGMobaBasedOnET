#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class DisplayAsStringExamples : MonoBehaviour
    {
        [InfoBox(
            "Instead of disabling values in the inspector in order to show some information or debug a value. " +
            "You can use DisplayAsString to show the value as text, instead of showing it in a disabled drawer")]
        [DisplayAsString]
        public Color SomeColor;

        [BoxGroup("SomeBox")]
        [DisplayAsString]
        [HideLabel]
        public string SomeText = "Lorem Ipsum";

		[InfoBox("The DisplayAsString attribute can also be configured to enable or disable overflowing to multiple lines.")]
		[DisplayAsString]
		[HideLabel]
		public string Overflow = "A very very very very very very very very very long string that has been configured to overflow.";

		[DisplayAsString(false)]
		[HideLabel]
		public string DisplayAllOfIt = "A very very very very very very very very long string that has been configured to not overflow.";
    }
}
#endif
