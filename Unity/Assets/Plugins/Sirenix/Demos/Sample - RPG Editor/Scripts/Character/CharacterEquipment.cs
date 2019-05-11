#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using System;

    [Serializable]
    public class CharacterEquipment
    {
        [ValidateInput("IsMainHand")]
        public EquipableItem MainHand;

        [ValidateInput("IsOffHand")]
        public EquipableItem Offhand;

        [ValidateInput("IsHead")]
        public EquipableItem Head;

        [ValidateInput("IsBody")]
        public EquipableItem Body;

#if UNITY_EDITOR
        private bool IsBody(EquipableItem value)
        {
            return value == null || value.Type == ItemTypes.Body;
        }

        private bool IsHead(EquipableItem value)
        {
            return value == null || value.Type == ItemTypes.Head;
        }

        private bool IsMainHand(EquipableItem value)
        {
            return value == null || value.Type == ItemTypes.MainHand;
        }

        private bool IsOffHand(EquipableItem value)
        {
            return value == null || value.Type == ItemTypes.OffHand;
        }
#endif
    }
}
#endif
