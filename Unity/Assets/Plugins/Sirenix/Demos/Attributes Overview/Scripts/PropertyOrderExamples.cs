#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
	using UnityEngine;

	public class PropertyOrderExamples : MonoBehaviour
    {
		[PropertyOrder(1)]
		public int Second;

		[InfoBox("PropertyOrder is used to change the order of properties in the inspector.")]
		[PropertyOrder(-1)]
		public int First;
	}
}
#endif
