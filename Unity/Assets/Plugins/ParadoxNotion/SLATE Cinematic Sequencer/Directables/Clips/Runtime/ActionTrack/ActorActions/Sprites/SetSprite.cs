using UnityEngine;
using System.Collections;

namespace Slate.ActionClips
{

    [Category("Sprites")]
    public class SetSprite : ActorActionClip<SpriteRenderer>
    {

        [Header("Basic")]
        public Sprite sprite;
        public Color color = Color.white;
        public bool flipX;
        public bool flipY;

        [Header("Sorting")]
        public bool changeSorting;
        [SortingLayer]
        public int sortingLayerID;
        public int sortingOrder;

        private Sprite lastSprite;
        private Color lastColor;
        private bool lastFlipX;
        private bool lastFlipY;

        private int lastSortingLayerID;
        private int lastSortingOrder;

        public override string info {
            get { return sprite == null ? "No Sprite Set" : string.Empty; }
        }

        protected override void OnEnter() {
            lastSprite = actor.sprite;
            lastColor = actor.color;
            lastFlipX = actor.flipX;
            lastFlipY = actor.flipY;
            lastSortingLayerID = actor.sortingLayerID;
            lastSortingOrder = actor.sortingOrder;

            actor.sprite = sprite;
            actor.color = color;
            actor.flipX = flipX;
            actor.flipY = flipY;
            if ( changeSorting ) {
                actor.sortingLayerID = sortingLayerID;
                actor.sortingOrder = sortingOrder;
            }
        }

        protected override void OnReverse() {
            actor.sprite = lastSprite;
            actor.color = lastColor;
            actor.flipX = lastFlipX;
            actor.flipY = lastFlipY;

            actor.sortingLayerID = lastSortingLayerID;
            actor.sortingOrder = lastSortingOrder;
        }

        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnClipGUIExternal(Rect left, Rect right) {
            if ( sprite != null ) {
                var t = sprite.texture;
                var tr = sprite.textureRect;
                var r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);
                var viewRect = new Rect(right.x, right.y, right.height, right.height);
                if ( flipX ) {
                    var min = viewRect.xMin;
                    viewRect.xMin = viewRect.xMax;
                    viewRect.xMax = min;
                }
                if ( flipY ) {
                    var min = viewRect.yMin;
                    viewRect.yMin = viewRect.yMax;
                    viewRect.yMax = min;
                }
                GUI.color = color;
                GUI.DrawTextureWithTexCoords(viewRect, t, r);
                GUI.color = Color.white;
            }
        }

#endif
    }
}