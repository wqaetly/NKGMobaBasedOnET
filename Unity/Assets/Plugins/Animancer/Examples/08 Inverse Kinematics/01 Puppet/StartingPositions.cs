// Animancer // Copyright 2019 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using UnityEngine;

namespace Animancer.Examples
{
    /// <summary>
    /// Records the positions of a set of objects so they can be returned later on.
    /// </summary>
    [AddComponentMenu("Animancer/Examples/Starting Positions")]
    [HelpURL(AnimancerPlayable.APIDocumentationURL + ".Examples/StartingPositions")]
    public sealed class StartingPositions : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private Transform[] _Transforms;

        [NonSerialized]
        private Vector3[] _StartingPositions;

        /************************************************************************************************************************/

        private void Awake()
        {
            _StartingPositions = new Vector3[_Transforms.Length];
            for (int i = 0; i < _StartingPositions.Length; i++)
            {
                _StartingPositions[i] = _Transforms[i].localPosition;
            }
        }

        /************************************************************************************************************************/

        public void ReturnToStartingPositions()
        {
            for (int i = 0; i < _StartingPositions.Length; i++)
            {
                _Transforms[i].localPosition = _StartingPositions[i];
            }
        }

        /************************************************************************************************************************/
    }
}
