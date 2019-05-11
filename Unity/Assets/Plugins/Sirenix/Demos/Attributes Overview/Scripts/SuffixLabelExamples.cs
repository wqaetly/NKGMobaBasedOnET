#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
	using UnityEngine;

    [TypeInfoBox(
            "The SuffixLabel attribute draws a label at the end of a property. " +
            "It's useful for conveying intend about a property.")]
    public class SuffixLabelExamples : MonoBehaviour
	{
		[SuffixLabel("Prefab")]
		public GameObject GameObject;

		[Space(15)]
		[InfoBox(
            "Using the Overlay property, the suffix label will be drawn on top of the property instead of behind it.\n" +
			"Use this for a neat inline look.")]
		[SuffixLabel("ms", Overlay = true)]
		public float Speed;

		[SuffixLabel("radians", Overlay = true)]
		public float Angle;

		[Space(15)]
		[InfoBox("The SuffixAttribute also supports referencing a member string field, property, or method by using $.")]
		[SuffixLabel("$Suffix", Overlay = true)]
		public string Suffix = "Dynamic suffix label";
	}
}
#endif
