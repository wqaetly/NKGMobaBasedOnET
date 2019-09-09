// Animancer // Copyright 2019 Kybernetik //

#if UNITY_EDITOR

#pragma warning disable IDE0018 // Inline variable declaration.
#pragma warning disable IDE0031 // Use null propagation.

using UnityEditor;
using UnityEngine;

namespace Animancer.Editor
{
    /// <summary>[Editor-Only]
    /// A simple wrapper around <see cref="EditorPrefs"/> to get and set a bool.
    /// <para></para>
    /// If you are interested in a more comprehensive pref wrapper that supports more types, check out
    /// <see href="https://assetstore.unity.com/packages/tools/gui/inspector-gadgets-lite-82896">
    /// Inspector Gadgets</see>.
    /// </summary>
    public sealed class BoolPref
    {
        /************************************************************************************************************************/

        /// <summary>The identifier with which this pref will be saved.</summary>
        public readonly string Key;

        /// <summary>The label to use when adding a function to toggle this pref to a menu.</summary>
        public readonly string MenuItem;

        /// <summary>The starting value to use for this pref if none was previously saved.</summary>
        public readonly bool DefaultValue;

        /************************************************************************************************************************/

        private bool _HasValue;
        private bool _Value;

        /// <summary>The current value of this pref.</summary>
        public bool Value
        {
            get
            {
                if (!_HasValue)
                {
                    _HasValue = true;
                    _Value = EditorPrefs.GetBool(Key, DefaultValue);
                }

                return _Value;
            }
            set
            {
                _Value = value;
                EditorPrefs.SetBool(Key, value);
                _HasValue = true;
            }
        }

        /// <summary>Returns the current value of the 'pref'.</summary>
        public static implicit operator bool(BoolPref pref)
        {
            return pref.Value;
        }

        /************************************************************************************************************************/

        /// <summary>Constructs a new <see cref="BoolPref"/>.</summary>
        public BoolPref(string menuItem, bool defaultValue)
        {
            MenuItem = menuItem;
            Key = "Animancer - " + menuItem;
            DefaultValue = defaultValue;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Adds a menu function to toggle the <see cref="Value"/> of this pref.
        /// </summary>
        public void AddToggleFunction(GenericMenu menu)
        {
            menu.AddItem(new GUIContent(MenuItem), _Value, () =>
            {
                Value = !Value;
            });
        }

        /************************************************************************************************************************/
    }
}

#endif
