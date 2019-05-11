#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    // Note that you can also reference methods and properties. You are not limited to fields.
    public class EnableAndDisableIfExamples : MonoBehaviour
    {
        public UnityEngine.Object SomeObject;

        [EnumToggleButtons]
        public InfoMessageType SomeEnum;

        public bool IsToggled;

        [EnableIf("SomeEnum", InfoMessageType.Info)]
        public Vector2 Info;

        [EnableIf("SomeEnum", InfoMessageType.Error)]
        public Vector2 Error;

        [EnableIf("SomeEnum", InfoMessageType.Warning)]
        public Vector2 Warning;

        [EnableIf("IsToggled")]
        public int EnableIfToggled;

        [DisableIf("IsToggled")]
        public int DisableIfToggled;

        [DisableIf("SomeObject")]
        public Vector3 EnableWhenNull;

        [EnableIf("SomeObject")]
        public Vector3 DisableWhenNull;
    }
}
#endif
