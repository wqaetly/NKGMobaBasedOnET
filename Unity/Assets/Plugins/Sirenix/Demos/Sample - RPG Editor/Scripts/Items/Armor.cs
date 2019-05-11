#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    public class Armor : EquipableItem
    {
        [BoxGroup(STATS_BOX_GROUP)]
        public float BaseArmor;

        [BoxGroup(STATS_BOX_GROUP)]
        public float MovementSpeed;

        public override ItemTypes[] SupportedItemTypes
        {
            get
            {
                return new ItemTypes[] 
                {
                    ItemTypes.Body,
                    ItemTypes.Head,
                    ItemTypes.Boots,
                    ItemTypes.OffHand
                };
            }
        }
    }
}
#endif
