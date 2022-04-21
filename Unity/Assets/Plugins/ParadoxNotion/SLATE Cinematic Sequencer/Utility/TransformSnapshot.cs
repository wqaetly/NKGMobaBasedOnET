using UnityEngine;
using System.Collections.Generic;

namespace Slate
{

    ///Can store a complete hierarchy transform pose
    public class TransformSnapshot
    {

        public enum StoreMode
        {
            All,
            RootOnly,
            ChildrenOnly
        }

        struct TransformData
        {
            public Transform transform;
            public Transform parent;
            public Vector3 pos;
            public Quaternion rot;
            public Vector3 scale;
            public TransformData(Transform transform, Transform parent, Vector3 pos, Quaternion rot, Vector3 scale) {
                this.transform = transform;
                this.parent = parent;
                this.pos = pos;
                this.rot = rot;
                this.scale = scale;
            }
        }

        private List<TransformData> data;

        public TransformSnapshot(GameObject root, StoreMode mode) {
            Store(root, mode);
        }

        public void Store(GameObject root, StoreMode mode) {
            if ( root == null ) return;
            data = new List<TransformData>();

            if ( mode == StoreMode.RootOnly ) {
                var transform = root.transform;
                data.Add(new TransformData(transform, transform.parent, transform.localPosition, transform.localRotation, transform.localScale));
                return;
            }

            var allTransforms = root.GetComponentsInChildren<Transform>(true);
            for ( var i = 0; i < allTransforms.Length; i++ ) {
                var transform = allTransforms[i];
                if ( transform != root.transform || mode == StoreMode.All ) {
                    data.Add(new TransformData(transform, transform.parent, transform.localPosition, transform.localRotation, transform.localScale));
                }
            }
        }

        public void Restore() {

            for ( var i = 0; i < data.Count; i++ ) {
                var d = data[i];
                if ( d.transform == null ) {
                    continue;
                }

                d.transform.SetParent(d.parent, d.transform is RectTransform ? false : true);
                d.transform.localPosition = d.pos;
                d.transform.localRotation = d.rot;
                d.transform.localScale = d.scale;
            }
        }
    }
}