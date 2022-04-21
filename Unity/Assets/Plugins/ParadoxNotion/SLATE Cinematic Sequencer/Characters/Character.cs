using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate
{

    ///This is a component that is required for a few action clips, as to centralize some properties to the actor rather than the clip
    [AddComponentMenu("SLATE/Character")]
    public class Character : MonoBehaviour
    {

        [SerializeField]
        private List<BlendShapeGroup> _expressions = new List<BlendShapeGroup>();
        [SerializeField]
        private Transform _neckTransform;
        [SerializeField]
        private Transform _headTransform;
        [SerializeField]
        private Vector3 _upVector = new Vector3(0, 1, 0);
        [SerializeField]
        private Vector3 _rotationOffset = new Vector3(0, 0, 0);

        public Transform neck {
            get { return _neckTransform; }
            set { _neckTransform = value; }
        }

        public Transform head {
            get { return _headTransform; }
            set { _headTransform = value; }
        }

        public Vector3 upVector {
            get { return _upVector; }
            set { _upVector = value; }
        }

        public Vector3 rotationOffset {
            get { return _rotationOffset; }
            set { _rotationOffset = value; }
        }

        public List<BlendShapeGroup> expressions {
            get { return _expressions; }
        }

        public BlendShapeGroup FindExpressionByName(string name) {
            return expressions.Find(x => x != null && x.name == name);
        }

        public BlendShapeGroup FindExpressionByUID(string UID) {
            return expressions.Find(x => x != null && x.UID == UID);
        }

        public void SetExpressionWeightByName(string name, float weight) {
            var exp = FindExpressionByName(name);
            if ( exp != null ) {
                exp.weight = weight;
            }
        }

        public void SetExpressionWeightByUID(string UID, float weight) {
            var exp = FindExpressionByUID(UID);
            if ( exp != null ) {
                exp.weight = weight;
            }
        }

        public void ResetExpressions() {
            for ( var i = 0; i < expressions.Count; i++ ) {
                expressions[i].weight = 0;
            }
        }
    }
}