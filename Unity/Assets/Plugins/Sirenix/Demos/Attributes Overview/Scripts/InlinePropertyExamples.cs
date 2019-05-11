#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using UnityEngine;

    public class InlinePropertyExamples : MonoBehaviour
    {
        public Vector3 Vector3;

        public Vector3Int Vector3Int;

        [InlineProperty(LabelWidth = 13)]
        public Vector2Int Vector2Int;

    }

    [Serializable]
    [InlineProperty(LabelWidth = 13)]
    public struct Vector3Int
    {
        [HorizontalGroup]
        public int X;

        [HorizontalGroup]
        public int Y;

        [HorizontalGroup]
        public int Z;
    }

    [Serializable]
    public struct Vector2Int
    {
        [HorizontalGroup]
        public int X;

        [HorizontalGroup]
        public int Y;
    }
}
#endif
