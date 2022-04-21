using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Slate.ActionClips
{

    [Category("Sprites")]
    [Description("Animate a sprite object by altering between different sprites in order")]
    public class SpriteFlipbook : ActorActionClip<SpriteRenderer>
    {

        [SerializeField]
        [HideInInspector]
        private float _length = 1;

        [Min(1)]
        public int loops = 1;
        public List<Sprite> sprites = new List<Sprite>();
        public bool endWithPrevious = true;

        private Sprite lastSprite;

        public override string info {
            get { return isValid ? string.Empty : ( actor == null ? "No SpriteRenderer on Actor" : "No Sprites Set" ); }
        }

        public override float length {
            get { return _length; }
            set { _length = value; }
        }

        public override bool isValid {
            get { return actor != null && sprites.Count > 0; }
        }

        protected override void OnEnter() {
            lastSprite = actor.sprite;
        }

        protected override void OnUpdate(float deltaTime) {
            if ( length == 0 ) {
                return;
            }

            if ( deltaTime >= length ) {
                actor.sprite = sprites[sprites.Count - 1];
                return;
            }

            var norm = ( deltaTime / length ) * loops;
            norm = Mathf.Repeat(norm, 1);
            var index = Mathf.FloorToInt(Mathf.Lerp(0, sprites.Count, norm));
            if ( index < sprites.Count ) {
                actor.sprite = sprites[index];
            }
        }

        protected override void OnExit() {
            if ( endWithPrevious ) {
                actor.sprite = lastSprite;
            }
        }

        protected override void OnReverse() {
            actor.sprite = lastSprite;
        }


        ////////////////////////////////////////
        ///////////GUI AND EDITOR STUFF/////////
        ////////////////////////////////////////
#if UNITY_EDITOR

        protected override void OnClipGUI(Rect rect) {
            var previousX = float.NegativeInfinity;
            for ( var _i = 0; _i < sprites.Count * loops; _i++ ) {
                var i = (int)Mathf.Repeat(_i, sprites.Count);

                if ( sprites[i] == null ) {
                    continue;
                }

                var t = sprites[i].texture;
                var tr = sprites[i].textureRect;
                var r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);

                var posX = Mathf.Lerp(0, rect.xMax, ( _i / (float)sprites.Count ) / loops);
                var viewRect = new Rect(posX, rect.y, rect.height, rect.height);
                if ( posX > previousX + viewRect.width ) {
                    GUI.DrawTextureWithTexCoords(viewRect, t, r);
                    previousX = posX;
                }

                if ( i == 0 ) {
                    UnityEditor.Handles.color = new Color(0, 0, 0, 0.5f);
                    UnityEditor.Handles.DrawLine(new Vector2(posX, rect.y), new Vector2(posX, rect.yMax));
                }
            }
        }

#endif
    }
}