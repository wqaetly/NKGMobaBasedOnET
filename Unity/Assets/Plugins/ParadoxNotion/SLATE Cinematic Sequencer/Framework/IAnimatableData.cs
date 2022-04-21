using UnityEngine;
using System.Collections;

namespace Slate
{

    ///Used to interface with AnimatedParameter and AnimationDataCollection the same,
    ///since AnimatedParameters are contained within an AnimationDataCollection.
    public interface IAnimatableData
    {

        bool isValid { get; }
        AnimationCurve[] GetCurves();

        void Validate(IKeyable keyable);
        void SetVirtualTransformParent(Transform virtualTransformParent);
        void SetSnapshot();
        void Evaluate(float time, float previousTime, float weight);
        void RestoreSnapshot();

        void SetKeyCurrent(float time);
        bool TryAutoKey(float time);
        bool TryKeyIdentity(float time);
        void RemoveKey(float time);
        bool HasKey(float time);
        bool HasAnyKey();
        bool HasChanged();
        float GetKeyNext(float time);
        float GetKeyPrevious(float time);
        string GetKeyLabel(float time);
        void SetPreWrapMode(WrapMode mode);
        void SetPostWrapMode(WrapMode mode);
        void Reset();
    }
}