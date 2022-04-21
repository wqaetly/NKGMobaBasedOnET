using UnityEngine;

namespace Slate
{

    public interface ITransformRefParameter
    {
        Transform transform { get; }
        TransformSpace space { get; }
        bool useAnimation { get; }
    }
}