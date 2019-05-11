#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
	using UnityEngine;

    [TypeInfoBox(
        "VerticalGroup, similar to HorizontalGroup, groups properties together vertically in the inspector.\n" +
        "By itself it doesn't do much, but combined with other groups, like HorizontalGroup, it can be very useful. It can also be used in TableLists to create columns.")]
	public class VerticalGroupExamples : MonoBehaviour
	{
		[HorizontalGroup("Split")]
		[VerticalGroup("Split/Left")]
		public InfoMessageType First;

		[VerticalGroup("Split/Left")]
		public InfoMessageType Second;

		[HideLabel]
		[VerticalGroup("Split/Right")]
		public int A;

		[HideLabel]
		[VerticalGroup("Split/Right")]
		public int B;
	}
}
#endif
