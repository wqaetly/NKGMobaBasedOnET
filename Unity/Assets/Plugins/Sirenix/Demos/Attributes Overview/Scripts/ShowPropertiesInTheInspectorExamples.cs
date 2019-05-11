#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class ShowPropertiesInTheInspectorExamples : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private int evenNumber;

        [ShowInInspector]
        public int EvenNumber
        {
            get { return this.evenNumber; }
            set { this.evenNumber = value + value % 2; }
        }
    }
}
#endif
