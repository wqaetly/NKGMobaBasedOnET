#if !UNITY_2017_1_OR_NEWER

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Slate.ActionClips;

using UnityEngine.Experimental.Director;

namespace Slate{

	[UniqueElement]
	[Description("The Animator Track works with an 'Animator' Component attached on the actor, but does not require or use the Controller assigned. Instead animation clips can be played directly. The 'Base Animation Clip' will be played along the whole track length when no other animation clip is playing. This can usualy be something like an Idle.")]
	[Icon(typeof(Animator))]
	[Attachable(typeof(ActorGroup))]
	partial class AnimatorTrack : CutsceneTrack {

		const int ROOTMOTION_FRAMERATE = 30;

		public AnimationClip baseAnimationClip;
		[Range(0.1f, 2)]
		public float basePlaybackSpeed = 1f;
		public bool useRootMotion = true;

		private Dictionary<PlayAnimatorClip, int> ports;
		private int activeClips;

		private PlayableGraph graph;
		private AnimationPlayableOutput animationOutput;
		private PlayableHandle mixerPlayableHandle;
		private PlayableHandle baseClipPlayableHandle;

		private bool useBakedRootMotion;
		private List<Vector3> rmPositions;
		private List<Quaternion> rmRotations;

		private Dictionary<AnimatorControllerParameter, object> wasAnimatorParameters;
		private RuntimeAnimatorController wasController;
		private AnimatorCullingMode wasCullingMode;
		private bool wasRootMotion;
		private bool wasEnabled;

		public Animator animator{get; private set;}
		public bool isMasterTrack{get {return true;}}

		public override string info{
			get {return string.Format("Base Clip: {0} {1}", baseAnimationClip? baseAnimationClip.name : "NONE", useRootMotion? " | RM: Enabled" : "");}
		}

		//...
		protected override bool OnInitialize(){
			animator = actor.GetComponentInChildren<Animator>();
			if (animator == null){
				Debug.LogError("Animator Track requires that the actor has the Animator Component attached.", actor);
				return false;
			}

			return true;
		}

		protected override void OnEnter(){

			animator = actor.GetComponentInChildren<Animator>(); //re-get to fetch from virtual actor ref instance if any
			if (animator == null){
				return;
			}

			StoreSet();
			var wasActive = animator.gameObject.activeSelf;
			animator.gameObject.SetActive(true);
			CreateAndPlayTree();
			if (useRootMotion){
				BakeRootMotion();
			}
			animator.gameObject.SetActive(wasActive);
		}

		protected override void OnUpdate(float time, float previousTime){

			if (animator == null || !animator.gameObject.activeInHierarchy){
				return;
			}

			if (!graph.IsValid()){
				return;
			}

			baseClipPlayableHandle.time = time * basePlaybackSpeed;
			graph.Evaluate(0);

			if (useRootMotion && useBakedRootMotion){
				ApplyBakedRootMotion(time);
			}
		}

		protected override void OnReverseEnter(){

			animator = actor.GetComponentInChildren<Animator>(); //re-get to fetch from virtual actor ref instance if any
			if (animator == null){
				return;
			}

			StoreSet();
			CreateAndPlayTree();
			//DO NOT Re-Bake root motion
		}

		protected override void OnExit(){
			Restore();
			if (useRootMotion){
				ApplyBakedRootMotion(endTime - startTime);
			}
		}
		protected override void OnReverse(){
			Restore();
			if (useRootMotion){
				ApplyBakedRootMotion(0);
			}
		}


		public void EnableClip(PlayAnimatorClip playAnimClip){

			if (animator == null){
				return;
			}

			if (!graph.IsValid()){
				return;
			}

			activeClips++;
			var index = ports[playAnimClip];
			var weight = playAnimClip.GetClipWeight();

			mixerPlayableHandle.SetInputWeight(0, activeClips == 2? 0 : 1 - weight);
			mixerPlayableHandle.SetInputWeight(index, weight);
		}

		public void UpdateClip(PlayAnimatorClip playAnimClip, float clipTime, float clipPrevious, float weight){

			if (animator == null){
				return;
			}

			if (!graph.IsValid()){
				return;
			}

			var index = ports[playAnimClip];

			var clipPlayable = mixerPlayableHandle.GetInput(index);
			clipPlayable.time = clipTime;
			mixerPlayableHandle.SetInputWeight(index, weight);
			mixerPlayableHandle.SetInputWeight(0, activeClips == 2? 0 : 1 - weight);
		}

		public void DisableClip(PlayAnimatorClip playAnimClip){

			if (animator == null){
				return;
			}

			if (!graph.IsValid()){
				return;
			}

			activeClips--;
			var index = ports[playAnimClip];

			mixerPlayableHandle.SetInputWeight(0, activeClips == 0? 1 : 0);
			mixerPlayableHandle.SetInputWeight(index, 0);			
		}
		

		void StoreSet(){

			wasController  = animator.runtimeAnimatorController;
			wasRootMotion  = animator.applyRootMotion;
			wasCullingMode = animator.cullingMode;
			wasEnabled     = animator.enabled;

			animator.applyRootMotion = false;
			animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;			
		}


		void Restore(){

			if (animator != null){
				animator.runtimeAnimatorController = wasController;
				animator.applyRootMotion = wasRootMotion;
				animator.cullingMode = wasCullingMode;
				animator.enabled = wasEnabled;		
			}

			if (graph.IsValid()){
				graph.Destroy();
			}
		}



		//Create playable tree
		void CreateAndPlayTree(){
			var clipActions = clips.OfType<PlayAnimatorClip>().ToList();
			var inputCount = 1 + clipActions.Count;
			ports = new Dictionary<PlayAnimatorClip, int>();
			graph = PlayableGraph.CreateGraph();
			mixerPlayableHandle = graph.CreateAnimationMixerPlayable(inputCount, true);
			mixerPlayableHandle.SetInputWeight(0, 1f);
			baseClipPlayableHandle = graph.CreateAnimationClipPlayable(baseAnimationClip);
			baseClipPlayableHandle.playState = PlayState.Paused;
			graph.Connect(baseClipPlayableHandle, 0, mixerPlayableHandle, 0);

			var index = 1; //0 is baseclip
			foreach(var playAnimClip in clipActions){
				var clipPlayableHandle = graph.CreateAnimationClipPlayable(playAnimClip.animationClip);
				graph.Connect(clipPlayableHandle, 0, mixerPlayableHandle, index);
				mixerPlayableHandle.SetInputWeight(index, 0f);
				ports[playAnimClip] = index;
				clipPlayableHandle.playState = PlayState.Paused;
				index++;
			}

			animationOutput = graph.CreateAnimationOutput("Animation", animator);
			animationOutput.sourcePlayable = mixerPlayableHandle;
			mixerPlayableHandle.playState = PlayState.Paused;
			graph.Play();

			// GraphVisualizerClient.Show(graph, animator.name);
		}


		//The root motion must be baked if required.
		void BakeRootMotion(){
			useBakedRootMotion = false;
			animator.applyRootMotion = true;
			rmPositions = new List<Vector3>();
			rmRotations = new List<Quaternion>();
			var lastTime = -1f;
			var updateInterval = (1f/ROOTMOTION_FRAMERATE);
			var tempActiveClips = 0;
			for (var i = startTime; i <= endTime + updateInterval; i += updateInterval){
				foreach(var clip in (this as IDirectable).children){

					if (i >= clip.startTime && lastTime < clip.startTime){
						tempActiveClips++;
						clip.Enter();
					}

					if (i >= clip.startTime && i <= clip.endTime){
						clip.Update(i - clip.startTime, i - clip.startTime - updateInterval);
					}

					if ( (i > clip.endTime || i >= this.endTime) && lastTime <= clip.endTime){
						tempActiveClips--;
						clip.Exit();
					}
				}

				if (tempActiveClips > 0){
					graph.Evaluate(updateInterval);
				}

				rmPositions.Add(animator.transform.localPosition);
				rmRotations.Add(animator.transform.localRotation);
				lastTime = i;
			}
			animator.applyRootMotion = false;
			useBakedRootMotion = true;
		}

		//Apply baked root motion by lerping between stored frames.
		void ApplyBakedRootMotion(float time){
			var frame = Mathf.FloorToInt( time * ROOTMOTION_FRAMERATE );
			var nextFrame = frame + 1;
			nextFrame = nextFrame < rmPositions.Count? nextFrame : rmPositions.Count - 1;

			var tNow = frame * (1f/ROOTMOTION_FRAMERATE);
			var tNext = nextFrame * (1f/ROOTMOTION_FRAMERATE);
		
			var posNow = rmPositions[frame];
			var posNext = rmPositions[nextFrame];
			var pos = Vector3.Lerp(posNow, posNext, Mathf.InverseLerp(tNow, tNext, time) );
			animator.transform.localPosition = pos;

			var rotNow = rmRotations[frame];
			var rotNext = rmRotations[nextFrame];
			var rot = Quaternion.Lerp(rotNow, rotNext, Mathf.InverseLerp(tNow, tNext, time) );
			animator.transform.localRotation = rot;
		}
	}
}

#endif