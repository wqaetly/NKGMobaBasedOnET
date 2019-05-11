#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using System;
    using UnityEngine;

    // 
    // These StatValues are used by StatLists, and are setup so the StatType cannot be changed after it has been added to the list.
    // This is done by giving the Type a HideInInspector attribute, and we then rename the Value label to be the name of the type,
    // and set the LabelWidth to make it a bit more compact.
    // 

    [Serializable]
    public struct StatValue : IEquatable<StatValue>
    {
        [HideInInspector]
        public StatType Type;

        [Range(-100, 100)]
        [LabelWidth(70)]
        [LabelText("$Type")]
        public float Value;

        public StatValue(StatType type, float value)
        {
            this.Type = type;
            this.Value = value;
        }

        public StatValue(StatType type)
        {
            this.Type = type;
            this.Value = 0;
        }

        public bool Equals(StatValue other)
        {
            return this.Type == other.Type && this.Value == other.Value;
        }
    }
}
#endif
