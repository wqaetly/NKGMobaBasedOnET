using UnityEngine;
using System.Collections;

namespace Slate
{

    [System.Serializable]
    ///Parameters targeting a blendshape to be used within a BlendShapeGroup
    public class BlendShape
    {

        [SerializeField]
        private SkinnedMeshRenderer _skin;
        [SerializeField]
        private string _name;
        [SerializeField]
        private float _weight;

        public SkinnedMeshRenderer skin {
            get { return _skin; }
            set { _skin = value; }
        }

        public string name {
            get { return _name; }
            set { _name = value; }
        }

        public float weight {
            get { return _weight; }
            set { _weight = value; }
        }

        public void SetRealWeight(float modWeight) {
            if ( skin == null ) {
                return;
            }

            var index = skin.GetBlendShapeIndex(name);
            if ( index == -1 ) {
                return;
            }

            skin.SetBlendShapeWeight(index, this.weight * modWeight * 100);
        }

        public override string ToString() {
            return name;
        }
    }
}