/*
using UnityEngine;
using System.Collections;

namespace Slate.ActionClips{

	[Category("Rendering")]
	[Description("Displays a texture overlay")]
	public class OverlaySprite : DirectorActionClip {

		[SerializeField] [HideInInspector]
		private float _length = 2;
		[SerializeField] [HideInInspector]
		private float _blendIn = 0.25f;
		[SerializeField] [HideInInspector]
		private float _blendOut = 0.25f;

		public Sprite sprite;
		[AnimatableParameter]
		public Color color = Color.white;
		[AnimatableParameter]
		public Vector2 scale = Vector2.one;
		[AnimatableParameter]
		public Vector2 position;
		public EaseType interpolation = EaseType.QuadraticInOut;

		public override string info{
			get {return string.Format("Overlay '{0}'", sprite != null? sprite.name : "NONE");}
		}

		public override float length{
			get {return _length;}
			set {_length = value;}
		}

		public override float blendIn{
			get {return _blendIn;}
			set {_blendIn = value;}
		}

		public override float blendOut{
			get {return _blendOut;}
			set {_blendOut = value;}
		}

		protected override void OnUpdate(float deltaTime){
			var lerpColor = color;
			lerpColor.a = Easing.Ease(interpolation, 0, color.a, GetClipWeight(deltaTime));
			DirectorGUI.UpdateOverlaySprite(sprite, lerpColor, scale, position);
		}
	}
}
*/