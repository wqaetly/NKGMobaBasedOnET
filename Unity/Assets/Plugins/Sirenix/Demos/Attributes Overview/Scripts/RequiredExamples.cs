#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    [TypeInfoBox("Required displays an error when objects are missing.")]
    public class RequiredExamples : MonoBehaviour
    {
        [Required]
        public GameObject MyGameObject;

        [Required("Custom error message.")]
        public Rigidbody MyRigidbody;

		[InfoBox("Use $ to indicate a member string as message.")]
		[Required("$DynamicMessage")]
		public GameObject GameObject;

		public string DynamicMessage = "Dynamic error message";
    }
}
#endif
