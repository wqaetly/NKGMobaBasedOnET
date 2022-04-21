using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{
    [Category("Sprites")]
    [Description("Change sorting layer and order for sprite renderer.")]
    public class SetSortingLayer : ActorActionClip<SpriteRenderer>
    {
        public int sortingOrder;
        [SortingLayer]
        public int sortingLayerID;

        private string _lastLayer;
        private int _lastOrder;

        public override string info {
            get
            {
                return string.Format("Set Sorting Layer\n{0} ({1})", SortingLayer.IsValid(sortingLayerID) ?
              SortingLayer.IDToName(sortingLayerID) : SortingLayer.layers[0].name, sortingOrder);
            }
        }

        protected override void OnEnter() {
            _lastLayer = actor.sortingLayerName;
            _lastOrder = actor.sortingOrder;

            actor.sortingLayerID = sortingLayerID;
            actor.sortingOrder = sortingOrder;
        }

        protected override void OnReverse() {
            actor.sortingLayerName = _lastLayer;
            actor.sortingOrder = _lastOrder;
        }
    }
}
