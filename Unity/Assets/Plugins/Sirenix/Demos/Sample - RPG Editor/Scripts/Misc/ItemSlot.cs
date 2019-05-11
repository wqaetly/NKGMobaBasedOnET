#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using System;

    [Serializable]
    public struct ItemSlot
    {
        public int ItemCount;
        public Item Item;
    }
}
#endif
