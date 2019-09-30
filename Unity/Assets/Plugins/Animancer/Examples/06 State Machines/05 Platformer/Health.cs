// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Keeps track of the health of an object.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Health")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/Health")]
    [DefaultExecutionOrder(-5000)]// Initialise the CurrentHealth earlier than anything else will use it.
    public sealed class Health : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private int _MaxHealth;
        public int MaxHealth
        {
            get { return _MaxHealth; }
            set
            {
                _CurrentHealth = value;
                if (OnHealthChanged != null)
                    OnHealthChanged();
            }
        }

        /************************************************************************************************************************/

        private int _CurrentHealth;
        public int CurrentHealth
        {
            get { return _CurrentHealth; }
            set
            {
                _CurrentHealth = Mathf.Clamp(value, 0, _MaxHealth);
                if (OnHealthChanged != null)
                    OnHealthChanged();
            }
        }

        public event Action OnHealthChanged;

        /************************************************************************************************************************/

        private void Awake()
        {
            CurrentHealth = _MaxHealth;
        }

        /************************************************************************************************************************/
    }
}
