using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;

namespace Slate
{
    [Attachable(typeof(ActionTrack))]
    ///Clips are added in CutsceneTracks to make stuff happen
    abstract public class ActionClip : SerializedMonoBehaviour, IDirectable, IKeyable
    {
        [SerializeField] [HideInInspector] private float _startTime;
        [SerializeField] [HideInInspector] private AnimationDataCollection _animationData;

        public IDirector root
        {
            get { return parent != null ? parent.root : null; }
        }

        public IDirectable parent { get; private set; }

        public GameObject actor
        {
            get { return parent != null ? parent.actor : null; }
        }

        IEnumerable<IDirectable> IDirectable.children
        {
            get { return null; }
        }

        ///All animated parameters are stored within this collection object
        public AnimationDataCollection animationData
        {
            get { return _animationData; }
            private set { _animationData = value; }
        }

        ///...
        public float startTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    _startTime = Mathf.Max(value, 0);
                    blendIn = Mathf.Clamp(blendIn, 0, length - blendOut);
                    blendOut = Mathf.Clamp(blendOut, 0, length - blendIn);
                }
            }
        }

        ///...
        public float endTime
        {
            get { return startTime + length; }
            set
            {
                if (startTime + length != value)
                {
                    length = Mathf.Max(value - startTime, 0);
                    blendOut = Mathf.Clamp(blendOut, 0, length - blendIn);
                    blendIn = Mathf.Clamp(blendIn, 0, length - blendOut);
                }
            }
        }

        ///...
        public bool isActive
        {
            get { return parent != null ? parent.isActive && isValid : false; }
        }

        ///...
        public bool isCollapsed
        {
            get { return parent != null ? parent.isCollapsed : false; }
        }

        //...
        public bool isLocked
        {
            get { return parent != null ? parent.isLocked : false; }
        }

        ///The length of the clip.
        ///Override for scalable clips.
        virtual public float length
        {
            get { return 0; }
            set { }
        }

        ///The blend in value of the clip. A value of zero means instant.
        ///Override for blendable in clips.
        virtual public float blendIn
        {
            get { return 0; }
            set { }
        }

        ///The blend out value of the clip. A value of zero means instant.
        ///Override for blendable out clips.
        virtual public float blendOut
        {
            get { return 0; }
            set { }
        }

        ///Should the clip be able to cross-blend between other clips of the same type?
        virtual public bool canCrossBlend
        {
            get { return false; }
        }

        ///A short summary.
        ///Overide this to show something specific in the action clip in the editor.
        virtual public string info
        {
            get
            {
                var nameAtt = this.GetType().RTGetAttribute<NameAttribute>(true);
                if (nameAtt != null)
                {
                    return nameAtt.name;
                }

                return this.GetType().Name.SplitCamelCase();
            }
        }

        ///Is everything ok for the clip to work?
        virtual public bool isValid
        {
            get { return actor != null; }
        }

        virtual public TransformSpace defaultTransformSpace
        {
            get { return TransformSpace.CutsceneSpace; }
        }

        //An array of property/field paths that will be possible to animate.
        //By default all properties/fields in the actionclip class with an [AnimatableParameter] attribute will be used.
        [System.NonSerialized] private string[] _cachedAnimParamPaths;

        private string[] animatedParameterPaths
        {
            get
            {
                return _cachedAnimParamPaths != null
                    ? _cachedAnimParamPaths
                    : _cachedAnimParamPaths = AnimationDataUtility.GetAnimatableMemberPaths(this);
            }
        }

        //If the params target is not this, registration of parameters should be handled manually
        private bool handleParametersRegistrationManually
        {
            get { return !ReferenceEquals(animatedParametersTarget, this); }
        }

        ///The target instance of the animated properties/fields.
        ///By default the instance of THIS action clip is used.
        ///Do NOT override if you don't know why! :)
        virtual public object animatedParametersTarget
        {
            get { return this; }
        }

        ///The interpolation to use when blending parameters. Only relevant when useWeightInParameters is true.
        virtual public EaseType animatedParametersInterpolation
        {
            get { return EaseType.Linear; }
        }

        ///Whether or not clip weight will be used in parameters automatically.
        virtual public bool useWeightInParameters
        {
            get { return false; }
        }

        ///Does the clip has any parameters?
        public bool hasParameters
        {
            get { return animationData != null && animationData.isValid; }
        }

        ///Does the clip has any active parameters?
        public bool hasActiveParameters
        {
            get
            {
                if (!hasParameters || !isValid)
                {
                    return false;
                }

                for (var i = 0; i < animationData.animatedParameters.Count; i++)
                {
                    if (animationData.animatedParameters[i].enabled)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        bool IDirectable.Initialize()
        {
            return OnInitialize();
        }

        void IDirectable.Enter()
        {
            SetAnimParamsSnapshot();
            OnEnter();
        }

        void IDirectable.Update(float time, float previousTime)
        {
            UpdateAnimParams(time, previousTime);
            OnUpdate(time, previousTime);
        }

        void IDirectable.Exit()
        {
            OnExit();
        }

        void IDirectable.ReverseEnter()
        {
            OnReverseEnter();
        }

        void IDirectable.Reverse()
        {
            RestoreAnimParamsSnapshot();
            OnReverse();
        }

        void IDirectable.RootEnabled()
        {
            OnRootEnabled();
        }

        void IDirectable.RootDisabled()
        {
            OnRootDisabled();
        }

        void IDirectable.RootUpdated(float time, float previousTime)
        {
            OnRootUpdated(time, previousTime);
        }

        void IDirectable.RootDestroyed()
        {
            OnRootDestroyed();
        }


#if UNITY_EDITOR
        void IDirectable.DrawGizmos(bool selected)
        {
            if (selected && actor != null && isValid)
            {
                OnDrawGizmosSelected();
            }
        }

        private Dictionary<MemberInfo, Attribute[]> paramsAttributes = new Dictionary<MemberInfo, Attribute[]>();

        void IDirectable.SceneGUI(bool selected)
        {
            if (!selected || actor == null || !isValid)
            {
                return;
            }

            if (hasParameters)
            {
                for (var i = 0; i < animationData.animatedParameters.Count; i++)
                {
                    var animParam = animationData.animatedParameters[i];
                    if (!animParam.isValid || animParam.animatedType != typeof(Vector3))
                    {
                        continue;
                    }

                    var m = animParam.GetMemberInfo();
                    Attribute[] attributes = null;
                    if (!paramsAttributes.TryGetValue(m, out attributes))
                    {
                        attributes = (Attribute[]) m.GetCustomAttributes(false);
                        paramsAttributes[m] = attributes;
                    }

                    ITransformRefParameter link = null;
                    var animAtt =
                        attributes.FirstOrDefault(a => a is AnimatableParameterAttribute) as
                            AnimatableParameterAttribute;
                    if (animAtt != null)
                    {
                        //only in case parameter has been added manualy. Probably never.
                        if (!string.IsNullOrEmpty(animAtt.link))
                        {
                            try
                            {
                                link = (GetType().GetField(animAtt.link).GetValue(this) as ITransformRefParameter);
                            }
                            catch (Exception exc)
                            {
                                Debug.LogException(exc);
                            }
                        }
                    }

                    if (link == null || link.useAnimation)
                    {
                        var space = link != null ? link.space : defaultTransformSpace;

                        var posHandleAtt =
                            attributes.FirstOrDefault(a => a is PositionHandleAttribute) as PositionHandleAttribute;
                        if (posHandleAtt != null)
                        {
                            DoParameterPositionHandle(animParam, space);
                        }

                        var rotHandleAtt =
                            attributes.FirstOrDefault(a => a is RotationHandleAttribute) as RotationHandleAttribute;
                        if (rotHandleAtt != null)
                        {
                            var posProp = this.GetType().RTGetFieldOrProp(rotHandleAtt.positionPropertyName);
                            var posVal = posProp != null
                                ? (Vector3) posProp.RTGetFieldOrPropValue(this)
                                : default(Vector3);
                            DoParameterRotationHandle(animParam, space, posVal);
                        }

                        var trajAtt =
                            attributes.FirstOrDefault(a => a is ShowTrajectoryAttribute) as ShowTrajectoryAttribute;
                        if (trajAtt != null && animParam.enabled)
                        {
                            CurveEditor3D.Draw3DCurve(animParam, this, GetSpaceTransform(space), length / 2, length);
                        }
                    }
                }
            }

            OnSceneGUI();
        }

        protected bool DoParameterPositionHandle(AnimatedParameter animParam, TransformSpace space)
        {
            return SceneGUIUtility.DoParameterPositionHandle(this, animParam, space);
        }

        protected bool DoParameterRotationHandle(AnimatedParameter animParam, TransformSpace space, Vector3 position)
        {
            return SceneGUIUtility.DoParameterRotationHandle(this, animParam, space, position);
        }

        protected bool DoVectorPositionHandle(TransformSpace space, ref Vector3 position)
        {
            return SceneGUIUtility.DoVectorPositionHandle(this, space, ref position);
        }

        protected bool DoVectorRotationHandle(TransformSpace space, Vector3 position, ref Vector3 euler)
        {
            return SceneGUIUtility.DoVectorRotationHandle(this, space, position, ref euler);
        }

#endif

        ///After creation
        public void PostCreate(IDirectable parent)
        {
            this.parent = parent;
            CreateAnimationDataCollection();
            OnCreate();
        }

        //Validate the clip
        public void Validate()
        {
            OnAfterValidate();
        }

        public void Validate(IDirector root, IDirectable parent)
        {
            this.parent = parent;
            hideFlags = HideFlags.HideInHierarchy;
            ValidateAnimParams();
            OnAfterValidate();
        }


        ///HOOKS
        ///----------------------------------------------------------------------------------------------
        virtual protected bool OnInitialize()
        {
            return true;
        }

        virtual protected void OnEnter()
        {
        }

        virtual protected void OnUpdate(float time, float previousTime)
        {
            OnUpdate(time);
        }

        virtual protected void OnUpdate(float time)
        {
        }

        virtual protected void OnExit()
        {
        }

        virtual protected void OnReverse()
        {
        }

        virtual protected void OnReverseEnter()
        {
        }

        virtual protected void OnDrawGizmosSelected()
        {
        }

        virtual protected void OnSceneGUI()
        {
        }

        virtual protected void OnCreate()
        {
        }

        virtual protected void OnAfterValidate()
        {
        }

        virtual protected void OnRootEnabled()
        {
        }

        virtual protected void OnRootDisabled()
        {
        }

        virtual protected void OnRootUpdated(float time, float previousTime)
        {
        }

        virtual protected void OnRootDestroyed()
        {
        }

        ///----------------------------------------------------------------------------------------------
        ///SHORTCUTS
        ///----------------------------------------------------------------------------------------------
        ///Is the root time within clip time range? A helper method.
        public bool RootTimeWithinRange()
        {
            return IDirectableExtensions.IsRootTimeWithinClip(this);
        }

        ///Transforms a point in specified space
        public Vector3 TransformPosition(Vector3 point, TransformSpace space)
        {
            return IDirectableExtensions.TransformPosition(this, point, space);
        }

        ///Inverse Transforms a point in specified space
        public Vector3 InverseTransformPosition(Vector3 point, TransformSpace space)
        {
            return IDirectableExtensions.InverseTransformPosition(this, point, space);
        }

        ///Transform an euler rotation in specified space and into a quaternion
        public Quaternion TransformRotation(Vector3 euler, TransformSpace space)
        {
            return IDirectableExtensions.TransformRotation(this, euler, space);
        }

        ///Trasnform a quaternion rotation in specified space and into an euler rotation
        public Vector3 InverseTransformRotation(Quaternion rot, TransformSpace space)
        {
            return IDirectableExtensions.InverseTransformRotation(this, rot, space);
        }

        ///Returns the final actor position in specified Space (InverseTransform Space)
        public Vector3 ActorPositionInSpace(TransformSpace space)
        {
            return IDirectableExtensions.ActorPositionInSpace(this, space);
        }

        ///Returns the transform object used for specified Space transformations. Null if World Space.
        public Transform GetSpaceTransform(TransformSpace space, GameObject actorOverride = null)
        {
            return IDirectableExtensions.GetSpaceTransform(this, space, actorOverride);
        }

        ///Returns the previous clip in parent track
        public ActionClip GetPreviousClip()
        {
            return this.GetPreviousSibling<ActionClip>();
        }

        ///Returns the next clip in parent track
        public ActionClip GetNextClip()
        {
            return this.GetNextSibling<ActionClip>();
        }

        ///The current clip weight based on blend properties and based on root current time.
        public float GetClipWeight()
        {
            return GetClipWeight(root.currentTime - startTime);
        }

        ///The weight of the clip at specified local time based on its blend properties.
        public float GetClipWeight(float time)
        {
            return GetClipWeight(time, this.blendIn, this.blendOut);
        }

        ///The weight of the clip at specified local time based on provided override blend in/out properties
        public float GetClipWeight(float time, float blendInOut)
        {
            return GetClipWeight(time, blendInOut, blendInOut);
        }

        public float GetClipWeight(float time, float blendIn, float blendOut)
        {
            return this.GetWeight(time, blendIn, blendOut);
        }

        ///----------------------------------------------------------------------------------------------
        public void TryMatchSubClipLength()
        {
            if (this is ISubClipContainable)
            {
                length = (this as ISubClipContainable).subClipLength / (this as ISubClipContainable).subClipSpeed;
            }
        }

        ///Try set the clip length to match previous subclip loop if this contains a subclip at all.
        public void TryMatchPreviousSubClipLoop()
        {
            if (this is ISubClipContainable)
            {
                length = (this as ISubClipContainable).GetPreviousLoopLocalTime();
            }
        }

        ///Try set the clip length to match next subclip loop if this contains a subclip at all.
        public void TryMatchNexSubClipLoop()
        {
            if (this is ISubClipContainable)
            {
                var targetLength = (this as ISubClipContainable).GetNextLoopLocalTime();
                var nextClip = GetNextClip();
                if (nextClip == null || startTime + targetLength <= nextClip.startTime)
                {
                    length = targetLength;
                }
            }
        }

        //...
        string GetParameterName<T, TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> func)
        {
            return ReflectionTools.GetMemberPath(func);
        }

        ///Get the AnimatedParameter of name. The name is usually the same as the field/property name that [AnimatableParameter] is used on.
        public AnimatedParameter GetParameter<T, TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> func)
        {
            return GetParameter(GetParameterName(func));
        }

        ///Get the AnimatedParameter of name. The name is usually the same as the field/property name that [AnimatableParameter] is used on.
        public AnimatedParameter GetParameter(string paramName)
        {
            return animationData != null ? animationData.GetParameterOfName(paramName) : null;
        }

        ///Enable/Disable an AnimatedParameter of name
        public void SetParameterEnabled<T, TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> func,
            bool enabled)
        {
            SetParameterEnabled(GetParameterName(func), enabled);
        }

        ///Enable/Disable an AnimatedParameter of name
        public void SetParameterEnabled(string paramName, bool enabled)
        {
            var animParam = GetParameter(paramName);
            if (animParam != null)
            {
                animParam.SetEnabled(enabled, root.currentTime - startTime);
            }
        }

        ///Re-Init/Reset all existing animated parameters
        public void ResetAnimatedParameters()
        {
            if (animationData != null)
            {
                animationData.Reset();
            }
        }

        //Creates the animation data collection out of the fields/properties marked with [AnimatableParameter] attribute
        void CreateAnimationDataCollection()
        {
            if (handleParametersRegistrationManually)
            {
                return;
            }

            if (animatedParameterPaths != null && animatedParameterPaths.Length != 0)
            {
                animationData = new AnimationDataCollection(this, this.GetType(), animatedParameterPaths, null);
            }
        }

        //Validate the animation parameters vs the animation data collection to be synced, adding or removing as required.
        void ValidateAnimParams()
        {
            if (animationData != null)
            {
                animationData.Validate(this);
            }

            //we don't need validation in runtime
            if (Application.isPlaying)
            {
                return;
            }

            if (handleParametersRegistrationManually)
            {
                return;
            }

            if (animatedParameterPaths == null || animatedParameterPaths.Length == 0)
            {
                animationData = null;
                return;
            }

            //try append new
            for (var i = 0; i < animatedParameterPaths.Length; i++)
            {
                var memberPath = animatedParameterPaths[i];
                if (!string.IsNullOrEmpty(memberPath))
                {
                    animationData.TryAddParameter(this, this.GetType(), memberPath, null);
                }
            }

            //cleanup
            foreach (var animParam in animationData.animatedParameters.ToArray())
            {
                if (!animParam.isValid)
                {
                    animationData.RemoveParameter(animParam);
                    continue;
                }

                if (!animatedParameterPaths.Contains(animParam.parameterName))
                {
                    animationData.RemoveParameter(animParam);
                    continue;
                }
            }
        }

        //Set an animation snapshot for all parameters
        void SetAnimParamsSnapshot()
        {
            if (hasParameters)
            {
                animationData.SetVirtualTransformParent(GetSpaceTransform(defaultTransformSpace));
                animationData.SetSnapshot();
            }
        }

        //Update the animation parameters, setting their evaluated values
        void UpdateAnimParams(float time, float previousTime)
        {
            if (hasParameters)
            {
                var ease = 1f;
                if (useWeightInParameters)
                {
                    var clipWeight = GetClipWeight(time);
                    ease = animatedParametersInterpolation == EaseType.Linear
                        ? clipWeight
                        : Easing.Ease(animatedParametersInterpolation, 0, 1, clipWeight);
                }

                animationData.Evaluate(time, previousTime, ease);
            }
        }

        //Restore the animation snapshot on all parameters
        void RestoreAnimParamsSnapshot()
        {
            if (hasParameters)
            {
                animationData.RestoreSnapshot();
            }
        }


        ///----------------------------------------------------------------------------------------------
        ///---------------------------------------UNITY EDITOR-------------------------------------------
#if UNITY_EDITOR

        //Unity callback.
        protected void OnValidate()
        {
            OnEditorValidate();
        }

        ///Show clip GUI contents
        public void ShowClipGUI(Rect rect)
        {
            OnClipGUI(rect);
        }

        ///This is called outside of the clip for UI on the the left/right sides of the clip.
        public void ShowClipGUIExternal(Rect left, Rect right)
        {
            OnClipGUIExternal(left, right);
        }

        ///Override for extra clip GUI contents.
        virtual protected void OnClipGUI(Rect rect)
        {
        }

        ///Override for extra clip GUI contents outside of clip.
        virtual protected void OnClipGUIExternal(Rect left, Rect right)
        {
        }

        ///Override to validate things in editor.
        virtual protected void OnEditorValidate()
        {
        }

        ///----------------------------------------------------------------------------------------------

#endif
    }
}