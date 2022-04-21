/*
using UnityEngine;
using System.Collections;

namespace Slate.ActionClips{

	[Category("Transform")]
	[Description("Pin the actor transform to the target object for the duration of the scene. Pin is like parenting but witout actual parenting.")]
	public class PinToTarget : ActorActionClip {

		[SerializeField] [HideInInspector]
		private float _length = 1;

		public Transform target;

		private Vector3 originalPos;
		private Quaternion originalRot;
		private Vector3 originalScale;

		private Vector3 localPos;
		private Vector3 localRot;

		public override bool isValid{
			get {return actor != null && target != null;}
		}

		public override string info{
			get {return string.Format("Pin To\n'{0}'", target? target.name : "null");}
		}

		public override float length{
			get {return _length;}
			set {_length = value;}
		}

		protected override void OnEnter(){
			originalPos = actor.transform.position;
			originalRot = actor.transform.rotation;
			originalScale = actor.transform.localScale;

			localPos = target.InverseTransformPoint(originalPos);
			localRot = actor.transform.eulerAngles - target.eulerAngles;
		}

		protected override void OnUpdate(float time){
			actor.transform.position = target.TransformPoint(localPos);
			actor.transform.rotation = Quaternion.Euler( target.eulerAngles + localRot );
		}

		protected override void OnReverse(){
			actor.transform.position = originalPos;
			actor.transform.rotation = originalRot;
			actor.transform.localScale = originalScale;
		}
	}
}
*/