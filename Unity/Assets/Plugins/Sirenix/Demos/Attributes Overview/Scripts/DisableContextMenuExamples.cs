#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class DisableContextMenuExamples : MonoBehaviour
    {
        [InfoBox("DisableContextMenu disables all right-click context menus provided by Odin. It does not disable Unity's context menu.", InfoMessageType.Warning)]
        [DisableContextMenu]
        public int[] NoRightClickList;

        [DisableContextMenu(disableForMember: false, disableCollectionElements: true)]
        public int[] NoRightClickListOnListElements;

        [DisableContextMenu(disableForMember: true, disableCollectionElements: true)]
        public int[] DisableRightClickCompletely;

        [DisableContextMenu]
        public int NoRightClickField;
    }
}
#endif
