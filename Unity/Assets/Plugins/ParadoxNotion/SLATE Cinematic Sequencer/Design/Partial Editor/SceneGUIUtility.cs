#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Slate
{

    public static class SceneGUIUtility
    {

        ///In SceneGUI, shows a position handle for target parameter of target keyable
        public static bool DoParameterPositionHandle(IKeyable keyable, AnimatedParameter animParam, TransformSpace space) {
            var originalPos = (Vector3)animParam.GetCurrentValueAsObject();
            var newPos = originalPos;
            if ( DoVectorPositionHandle(keyable, space, ref newPos) ) {
                if ( keyable.IsRootTimeWithinClip() ) {
                    if ( !Event.current.shift ) {
                        animParam.SetCurrentValue(newPos);
                    } else {
                        animParam.OffsetValue(newPos - originalPos);
                    }
                } else {
                    animParam.SetCurrentValue(newPos);
                    animParam.OffsetValue(newPos - originalPos);
                }
                EditorUtility.SetDirty(keyable as Object);
                return true;
            }
            return false;
        }

        ///In SceneGUI, shows a rotation handle for target parameter of target keyable
        public static bool DoParameterRotationHandle(IKeyable keyable, AnimatedParameter animParam, TransformSpace space, Vector3 position) {
            var originalRot = (Vector3)animParam.GetCurrentValueAsObject();
            var newRot = originalRot;
            if ( DoVectorRotationHandle(keyable, space, position, ref newRot) ) {
                animParam.SetCurrentValue(newRot);
                EditorUtility.SetDirty(keyable as Object);
                return true;
            }
            return false;
        }

        ///In SceneGUI, shows a position handle for target vector of target directable (pos handle sin World Space rotation)
        public static bool DoVectorPositionHandle(IDirectable directable, TransformSpace space, ref Vector3 position) {
            return DoVectorPositionHandle(directable, space, null, ref position);
        }

        ///In SceneGUI, shows a position handle for target vector of target directable (pos handle in provided euler rotation)
        public static bool DoVectorPositionHandle(IDirectable directable, TransformSpace space, Vector3? euler, ref Vector3 position) {
            EditorGUI.BeginChangeCheck();
            var pos = directable.TransformPosition(position, space);
            var rot = euler == null ? Quaternion.identity : directable.TransformRotation(euler.Value, space);
            var newPos = Handles.PositionHandle(pos, rot);
            Handles.SphereHandleCap(-10, pos, Quaternion.identity, 0.1f, EventType.Repaint);
            if ( EditorGUI.EndChangeCheck() ) {
                Undo.RecordObject(directable as Object, "Position Change");
                position = directable.InverseTransformPosition(newPos, space);
                EditorUtility.SetDirty(directable as Object);
                return true;
            }
            return false;
        }

        ///In SceneGUI, shows a rotation handle for target vector of target directable
        public static bool DoVectorRotationHandle(IDirectable directable, TransformSpace space, Vector3 position, ref Vector3 euler) {
            EditorGUI.BeginChangeCheck();
            var pos = directable.TransformPosition(position, space);
            var rot = directable.TransformRotation(euler, space);
            var newRot = Handles.RotationHandle(rot, pos);

            Handles.SphereHandleCap(-10, pos, Quaternion.identity, 0.1f, EventType.Repaint);
            if ( EditorGUI.EndChangeCheck() ) {
                Undo.RecordObject(directable as Object, "Rotation Change");
                euler = directable.InverseTransformRotation(newRot, space);
                EditorUtility.SetDirty(directable as Object);
                return true;
            }
            return false;
        }

        /*
                static Vector3 ContinousRotation(Vector3 current, Vector3 newRot){
                    var x = ContinousRotation(current.x, newRot.x);
                    var y = ContinousRotation(current.y, newRot.y);
                    var z = ContinousRotation(current.z, newRot.z);
                    return new Vector3(x, y, z);
                }

                //TODO: make it better!
                static float ContinousRotation(float current, float newValue){

                    var oldValue = Mathf.Repeat(current, 360);
                    if (newValue < 120 && oldValue > 240){
                        return current + newValue + (360 - oldValue);
                    }

                    if (newValue > 240 && oldValue < 120){
                        return current - (360 - newValue) - oldValue;
                    }

                    return current + (newValue - oldValue);
                }
        */

    }
}

#endif