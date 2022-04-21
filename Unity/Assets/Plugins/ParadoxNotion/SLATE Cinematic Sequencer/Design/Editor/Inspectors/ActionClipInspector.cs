#if UNITY_EDITOR

using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Slate
{
    public class ActionClipInspector<T> : ActionClipInspector where T : ActionClip
    {
        protected T action
        {
            get { return (T) target; }
        }
    }

    [CustomEditor(typeof(ActionClip), true)]
    public class ActionClipInspector : OdinEditor
    {
        private ActionClip action
        {
            get { return (ActionClip) target; }
        }

        public override void OnInspectorGUI()
        {
            ShowCommonInspector();
            ShowAnimatableParameters();
            if (GUI.changed)
            {
                action.Validate();
            }
        }

        protected void ShowCommonInspector(bool showBaseInspector = true)
        {
            ShowErrors();
            ShowInOutControls();
            ShowBlendingControls();
            if (showBaseInspector)
            {
                base.OnInspectorGUI();
            }

            ShowSubClipGUI();
        }

        //In case this is ISubClipContainable shows button to set length at subclip length
        protected void ShowSubClipGUI()
        {
            var subContainable = action as ISubClipContainable;
            if (subContainable != null && subContainable.subClipLength > 0)
            {
                GUILayout.Space(10);

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Match Previous Loop", EditorStyles.miniButtonLeft))
                {
                    action.TryMatchPreviousSubClipLoop();
                }

                if (GUILayout.Button("Match Clip Length", EditorStyles.miniButtonMid))
                {
                    action.TryMatchSubClipLength();
                }

                if (GUILayout.Button("Match Next Loop", EditorStyles.miniButtonRight))
                {
                    action.TryMatchNexSubClipLoop();
                }

                GUILayout.EndHorizontal();
            }
        }

        //Shows all animatable parameters of the clip
        protected void ShowAnimatableParameters()
        {
            if (action.hasParameters)
            {
                foreach (var animParam in action.animationData.animatedParameters)
                {
                    //field based parameters are shown through AnimatableParameterDrawer
                    if (animParam.isProperty || animParam.isExternal)
                    {
                        AnimatableParameterEditor.ShowParameter(animParam, action);
                    }
                }

                GUILayout.Space(5);
            }
        }

        //Shows possible errors
        void ShowErrors()
        {
            if (action.actor == null)
            {
                EditorGUILayout.HelpBox("The target Actor is null.", MessageType.Error);
                GUILayout.Space(5);
                return;
            }

            if (!action.isValid)
            {
                var type = action.GetType();
                while (type != null && type != typeof(ActionClip))
                {
                    var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                    if (typeof(Slate.ActionClips.ActorActionClip<>) == cur)
                    {
                        var requiredType = type.GetGenericArguments()[0];
                        if (action.actor.GetComponent(requiredType) == null)
                        {
                            EditorGUILayout.HelpBox(
                                string.Format("This clip requires the actor to have the '{0}' Component",
                                    requiredType.Name), MessageType.Error);
                            GUILayout.Space(5);
                        }

                        return;
                    }

                    type = type.BaseType;
                }

                EditorGUILayout.HelpBox(
                    "The clip is currently invalid. Please make sure the required parameters are set.",
                    MessageType.Error);
                GUILayout.Space(5);
                return;
            }
        }

        //Shows clip in/out controls
        void ShowInOutControls()
        {
            var previousClip = action.GetPreviousSibling();
            var previousTime = previousClip != null ? previousClip.endTime : action.parent.startTime;
            if (action.CanCrossBlend(previousClip))
            {
                previousTime -= Mathf.Min(action.length / 2, (previousClip.endTime - previousClip.startTime) / 2);
            }

            var nextClip = action.GetNextSibling();
            var nextTime = nextClip != null ? nextClip.startTime : action.parent.endTime;
            if (action.CanCrossBlend(nextClip))
            {
                nextTime += Mathf.Min(action.length / 2, (nextClip.endTime - nextClip.startTime) / 2);
            }

            var canScale = action.CanScale();
            var doFrames = Prefs.timeStepMode == Prefs.TimeStepMode.Frames;

            GUILayout.BeginVertical((GUIStyle) "box");
            GUILayout.BeginHorizontal();

            var _in = action.startTime;
            var _length = action.length;
            var _out = action.endTime;

            if (canScale)
            {
                GUILayout.Label("IN", GUILayout.Width(30));
                if (doFrames)
                {
                    _in *= Prefs.frameRate;
                    _in = (int) EditorGUILayout.DelayedIntField((int) _in, GUILayout.Width(80));
                    _in = _in * (1f / Prefs.frameRate);
                }
                else
                {
                    _in = EditorGUILayout.DelayedFloatField(_in, GUILayout.Width(80));
                }

                GUILayout.FlexibleSpace();
                GUILayout.Label("◄");
                if (doFrames)
                {
                    _length *= Prefs.frameRate;
                    _length = (int) EditorGUILayout.DelayedIntField((int) _length, GUILayout.Width(80));
                    _length = _length * (1f / Prefs.frameRate);
                }
                else
                {
                    _length = EditorGUILayout.DelayedFloatField(_length, GUILayout.Width(80));
                }

                GUILayout.Label("►");
                GUILayout.FlexibleSpace();

                GUILayout.Label("OUT", GUILayout.Width(30));
                if (doFrames)
                {
                    _out *= Prefs.frameRate;
                    _out = (int) EditorGUILayout.DelayedIntField((int) _out, GUILayout.Width(80));
                    _out = _out * (1f / Prefs.frameRate);
                }
                else
                {
                    _out = EditorGUILayout.DelayedFloatField(_out, GUILayout.Width(80));
                }
            }

            GUILayout.EndHorizontal();

            if (canScale)
            {
                if (_in >= action.parent.startTime && _out <= action.parent.endTime)
                {
                    if (_out > _in)
                    {
                        EditorGUILayout.MinMaxSlider(ref _in, ref _out, previousTime, nextTime);
                    }
                    else
                    {
                        _in = EditorGUILayout.Slider(_in, previousTime, nextTime);
                        _out = _in;
                    }
                }
            }
            else
            {
                GUILayout.Label("IN", GUILayout.Width(30));
                _in = EditorGUILayout.Slider(_in, 0, action.parent.endTime);
                _out = _in;
            }


            if (GUI.changed)
            {
                if (_length != action.length)
                {
                    _out = _in + _length;
                }

                _in = Mathf.Round(_in / Prefs.snapInterval) * Prefs.snapInterval;
                _out = Mathf.Round(_out / Prefs.snapInterval) * Prefs.snapInterval;

                _in = Mathf.Clamp(_in, previousTime, _out);
                _out = Mathf.Clamp(_out, _in, nextClip != null ? nextTime : float.PositiveInfinity);

                // var deltaMove = action.startTime - _in;
                // foreach ( var curve in action.GetCurvesAll() ) { curve.OffsetCurveTime(deltaMove); }
                // CutsceneUtility.RefreshAllAnimationEditorsOf(action.animationData);
                // if ( action is ISubClipContainable ) { ( action as ISubClipContainable ).subClipOffset += deltaMove; }

                action.startTime = _in;
                action.endTime = _out;
            }

            if (_in > action.parent.endTime)
            {
                EditorGUILayout.HelpBox("Clip is outside of playable range", MessageType.Warning);
            }
            else
            {
                if (_out > action.parent.endTime)
                {
                    EditorGUILayout.HelpBox("Clip end time is outside of playable range", MessageType.Warning);
                }
            }

            if (_out < action.parent.startTime)
            {
                EditorGUILayout.HelpBox("Clip is outside of playable range", MessageType.Warning);
            }
            else
            {
                if (_in < action.parent.startTime)
                {
                    EditorGUILayout.HelpBox("Clip start time is outside of playable range", MessageType.Warning);
                }
            }

            GUILayout.EndVertical();
        }

        //Show blending in/out controls
        void ShowBlendingControls()
        {
            var canBlendIn = action.CanBlendIn();
            var canBlendOut = action.CanBlendOut();
            if ((canBlendIn || canBlendOut) && action.length > 0)
            {
                GUILayout.BeginVertical((GUIStyle) "box");
                GUILayout.BeginHorizontal();
                if (canBlendIn)
                {
                    GUILayout.BeginVertical();
                    GUILayout.Label("Blend In");
                    var max = action.length - action.blendOut;
                    action.blendIn = EditorGUILayout.Slider(action.blendIn, 0, max);
                    action.blendIn = Mathf.Clamp(action.blendIn, 0, max);
                    GUILayout.EndVertical();
                }

                if (canBlendOut)
                {
                    GUILayout.BeginVertical();
                    GUILayout.Label("Blend Out");
                    var max = action.length - action.blendIn;
                    action.blendOut = EditorGUILayout.Slider(action.blendOut, 0, max);
                    action.blendOut = Mathf.Clamp(action.blendOut, 0, max);
                    GUILayout.EndVertical();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
        }
    }
}

#endif