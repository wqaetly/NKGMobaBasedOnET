using UnityEngine;
using System.Collections.Generic;

namespace Slate
{

    [System.Serializable]
    ///A group of blendshapes. An expression
    public class BlendShapeGroup
    {

        [SerializeField]
        private string _UID;
        [SerializeField]
        private string _name = "(rename me)";
        [SerializeField]
        private float _weight;
        [SerializeField]
        private List<BlendShape> _blendShapes = new List<BlendShape>();

        public string UID {
            get { return _UID; }
            private set { _UID = value; }
        }

        public string name {
            get { return _name; }
            set { _name = value; }
        }

        public float weight {
            get { return _weight; }
            set
            {
                _weight = value;
                SetBlendWeights();
            }
        }

        public List<BlendShape> blendShapes {
            get { return _blendShapes; }
        }

        public BlendShapeGroup() {
            UID = System.Guid.NewGuid().ToString();
        }


        void SetBlendWeights() {
            for ( var i = 0; i < blendShapes.Count; i++ ) {
                blendShapes[i].SetRealWeight(weight);
            }
        }

        public override string ToString() {
            return name;
        }
    }
}