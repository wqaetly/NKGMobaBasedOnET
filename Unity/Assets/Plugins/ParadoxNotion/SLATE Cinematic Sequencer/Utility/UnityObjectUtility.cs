using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace Slate
{

    public static class UnityObjectUtility
    {

        private static MethodInfo _getLocalEulerAngles;
        private static MethodInfo _setLocalEulerAngles;
        private static PropertyInfo _rotationOrder;
        private static object[] _rotationOrderArgGet;
        private static object[] _rotationOrderArgSet;

        //initialize reflection
        static UnityObjectUtility() {
            var t = typeof(Transform);
            _getLocalEulerAngles = t.GetMethod("GetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
            _setLocalEulerAngles = t.GetMethod("SetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
            _rotationOrder = t.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
            _rotationOrderArgGet = new object[1];
            _rotationOrderArgSet = new object[2];
        }

        ///Ensures rotation continuety. All this just because Unity is being weird about it.
        public static Vector3 GetLocalEulerAngles(this Transform transform) {
            if ( !Application.isPlaying ) {
                _rotationOrderArgGet[0] = _rotationOrder.GetValue(transform, null);
                return (Vector3)_getLocalEulerAngles.Invoke(transform, _rotationOrderArgGet);
            }
            return transform.localEulerAngles;
        }

        ///Ensures rotation continuety. All this just because Unity is being weird about it.
        public static void SetLocalEulerAngles(this Transform transform, Vector3 value) {
            if ( !Application.isPlaying ) {
                _rotationOrderArgSet[0] = value;
                _rotationOrderArgSet[1] = _rotationOrder.GetValue(transform, null);
                _setLocalEulerAngles.Invoke(transform, _rotationOrderArgSet);
                return;
            }
            transform.localEulerAngles = value;
        }

        ///----------------------------------------------------------------------------------------------
#if UNITY_2018_3_OR_NEWER && UNITY_EDITOR
        //Returns whether the gameobject can safely be deleted in regards to the prefab system
        public static bool IsSafePrefabDelete(this GameObject go) {
            var isPrefab = UnityEditor.PrefabUtility.IsPartOfAnyPrefab(go);
            var isOveride = UnityEditor.PrefabUtility.IsAddedGameObjectOverride(go);
            return !isPrefab || isOveride;
        }
#endif
        ///----------------------------------------------------------------------------------------------

        ///Returns a path from root to child
        public static string CalculateTransformPath(this Transform root, Transform child) {
            var path = new List<string>();
            var curr = child;
            while ( curr != root && curr != null ) {
                path.Add(curr.name);
                curr = curr.parent;
            }
            path.Reverse();
            return string.Join("/", path.ToArray());
        }

        ///Returns the Transform from path
        public static Transform ResolveTransformPath(this Transform root, string path) {
            var transform = root;
            var split = path.Split('/');
            for ( var i = 0; i < split.Length; i++ ) {
                var tName = split[i];
                transform = transform.Find(tName);
                if ( transform == null ) {
                    return null;
                }
            }
            return transform;
        }

        ///Find transform with name within all children of root, irelevant of path
        public static Transform FindInChildren(this Transform root, string name, bool includeHidden) {
            if ( root == null || string.IsNullOrEmpty(name) ) { return root; }
            return root.GetComponentsInChildren<Transform>(includeHidden).FirstOrDefault(t => t.name.ToUpper() == name.ToUpper());
        }

        ///Quickly resets local pos/rot/scale of transform
        public static void ResetLocalCoords(this Transform transform, bool includeScale = true) {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            if ( includeScale ) { transform.localScale = Vector3.one; }
        }

        ///Quickly copy local pos/rot/scale from one transform to another
        public static void SetLocalCoordsFrom(this Transform transform, Transform source) {
            transform.localPosition = source.localPosition;
            transform.localRotation = source.localRotation;
            transform.localScale = source.localScale;
        }

        ///Insert parent between transform and it's current parent
        public static Transform InsertParentInChain(this Transform transform, string name) {
            var newParent = new GameObject(name).transform;
            newParent.SetParent(transform.parent, false);
            newParent.SetLocalCoordsFrom(transform);
            transform.SetParent(newParent, true);
            return newParent;
        }

        ///----------------------------------------------------------------------------------------------

        ///Gets existing T component or adds new one if not exists
        public static T GetAddComponent<T>(this GameObject go) where T : Component {
            return GetAddComponent(go, typeof(T)) as T;
        }

        ///Gets existing T component or adds new one if not exists
        public static T GetAddComponent<T>(this Component comp) where T : Component {
            return GetAddComponent(comp.gameObject, typeof(T)) as T;
        }

        ///Gets existing component or adds new one if not exists
        public static Component GetAddComponent(this GameObject go, System.Type type) {
            var result = go.GetComponent(type);
            if ( result == null ) {
                result = go.AddComponent(type);
            }
            return result;
        }

        ///----------------------------------------------------------------------------------------------

        ///Get all shape names of a skinned mesh
        public static List<string> GetBlendShapeNames(this SkinnedMeshRenderer skinnedMesh) {
            var result = new List<string>();
            if ( skinnedMesh == null ) return result;
            for ( int i = 0; i < skinnedMesh.sharedMesh.blendShapeCount; i++ ) {
                result.Add(skinnedMesh.sharedMesh.GetBlendShapeName(i));
            }
            return result;
        }

        ///Get the index of a shape name
        public static int GetBlendShapeIndex(this SkinnedMeshRenderer skinnedMesh, string shapeName) {
            if ( skinnedMesh == null ) return -1;
            for ( int i = 0; i < skinnedMesh.sharedMesh.blendShapeCount; i++ ) {
                if ( skinnedMesh.sharedMesh.GetBlendShapeName(i) == shapeName ) {
                    return i;
                }
            }
            return -1;
        }

    }
}