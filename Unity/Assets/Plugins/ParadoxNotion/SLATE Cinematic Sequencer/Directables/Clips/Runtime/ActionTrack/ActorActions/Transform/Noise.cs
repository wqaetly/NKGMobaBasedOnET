using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slate.ActionClips{

	[Category("Transform")]
	public class Noise : ActorActionClip {

		const float _fbmNorm = 1 / 0.75f;

		[SerializeField] [HideInInspector]
		private float _length = 2f;
		[SerializeField] [HideInInspector]
		private float _blendIn = 1f;
		[SerializeField] [HideInInspector]
		private float _blendOut = 1f;

		public Vector3 seed;
		public float frequency = 1;
		public int octaves = 2;
		public Vector3 amplitude = Vector3.one;

		private Vector3 wasPosition;

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

		protected override void OnCreate(){
			seed = new Vector3( Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100) );
		}

		protected override void OnEnter(){
			wasPosition = actor.transform.position;
		}

		protected override void OnUpdate(float time, float previousTime){
			var pos = actor.transform.position;
			pos.x = Perlin.Fbm(seed.x + time * frequency, octaves);
			pos.y = Perlin.Fbm(seed.y + time * frequency, octaves);
			pos.z = Perlin.Fbm(seed.z + time * frequency, octaves);
			
			pos = Vector3.Scale(pos, amplitude) * _fbmNorm;
			actor.transform.position = Vector3.Lerp( wasPosition, wasPosition + pos, GetClipWeight(time) );
		}
	}
}