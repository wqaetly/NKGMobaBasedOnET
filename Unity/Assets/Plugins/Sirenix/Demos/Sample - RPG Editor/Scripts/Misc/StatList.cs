#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

#if UNITY_EDITOR
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using System.Collections;
#endif

    // 
    // The StatList is a dictionary-like list of StatValues, which holds a StatType and a value.
    // This could be used by many things throughout the system. In this case, StatLists are used
    // by the Character and items to define requirements and modifiers. But one could imagine
    // that many things in a game could have StatLists.
    // 
    // The reason for it being a list instead of a dictioanry is, that most often StatLists doesn't 
    // contain very many stats. For instance, a shield might add some defences, and a few other random bonuses,
    // and iterating over a dozen values, is actually faster than making a dictionary lookup if optimized.
    // 
    // The StatList is then customized with the ValueDropdown attribute, where we override how elements 
    // are added and provide the user with a list of types to choose from using OdinSelectors. 
    // Checkout the CustomAddStatsButton at the bottom of this script.
    // 

    [Serializable]
    public class StatList
    {
        [SerializeField]
        [ValueDropdown("CustomAddStatsButton", IsUniqueList = true, DrawDropdownForListElements = false, DropdownTitle = "Modify Stats")]
        [ListDrawerSettings(DraggableItems = false, Expanded = true)]
        private List<StatValue> stats = new List<StatValue>();

        public StatValue this[int index]
        {
            get { return this.stats[index]; }
            set { this.stats[index] = value; }
        }

        public int Count
        {
            get { return this.stats.Count; }
        }

        public float this[StatType type]
        {
            get
            {
                for (int i = 0; i < this.stats.Count; i++)
                {
                    if (this.stats[i].Type == type)
                    {
                        return this.stats[i].Value;
                    }
                }

                return 0;
            }
            set
            {
                for (int i = 0; i < this.stats.Count; i++)
                {
                    if (this.stats[i].Type == type)
                    {
                        var val = this.stats[i];
                        val.Value = value;
                        this.stats[i] = val;
                        return;
                    }
                }

                this.stats.Add(new StatValue(type, value));
            }
        }

#if UNITY_EDITOR
        // Finds all available stat-types and excludes the types that the statList already contains, so we don't get multiple entries of the same type.
        private IEnumerable CustomAddStatsButton()
        {
            return Enum.GetValues(typeof(StatType)).Cast<StatType>()
                .Except(this.stats.Select(x => x.Type))
                .Select(x => new StatValue(x))
                .AppendWith(this.stats)
                .Select(x => new ValueDropdownItem(x.Type.ToString(), x));
        }
#endif
    }

#if UNITY_EDITOR

    // 
    // Since the StatList is just a class that contains a list, all StatLists would contain an extra 
    // label with a foldout in the inspector, which we don't want.
    // 
    // So with this drawer, we simply take the label of the member that holds the StatsList, and render the 
    // actual list using that label.
    //
    // So instead of the "private List<StatValue> stats" field getting a label named "Stats"
    // It now gets the label of whatever member holds the actual StatsList
    // 
    // If this confuses you, try out commenting the drawer below, and take a look at an item in the RPGEditor to see 
    // the difference.
    // 

    internal class StatListValueDrawer : OdinValueDrawer<StatList>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // This would be the "private List<StatValue> stats" field.
            this.Property.Children[0].Draw(label);
        }
    }

#endif
}
#endif
