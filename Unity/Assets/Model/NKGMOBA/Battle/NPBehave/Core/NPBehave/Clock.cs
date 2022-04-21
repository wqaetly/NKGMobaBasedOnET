using System;
using System.Collections.Generic;
using System.Diagnostics;
using ET;
using NPBehave_Core;
using Log = ET.Log;

namespace NPBehave
{
    public class Clock
    {
        private bool isInUpdate = false;

        public LSF_Component LsfComponent;

        public Clock(LSF_Component lsfComponent)
        {
            LsfComponent = lsfComponent;
        }

        /// <summary>
        /// 将要被添加的帧事件
        /// </summary>
        private Dictionary<long, FrameAction> ToBeAddedFrameActions = new Dictionary<long, FrameAction>();

        /// <summary>
        /// 将要被移除的帧事件
        /// </summary>
        private List<long> ToBeRemovedFrameActions = new List<long>();

        /// <summary>
        /// 所有的帧事件
        /// </summary>
        private Dictionary<long, FrameAction> AllFrameActions = new Dictionary<long, FrameAction>();

        /// <summary>Register a timer function, 因为这个函数可能会在回滚的时候调用，所以要强行传递一个currentFrame</summary>
        /// <param name="intervalFrame">time in Frame</param>
        /// <param name="repeat">number of times to repeat, set to -1 to repeat until unregistered.</param>
        /// <param name="action">method to invoke</param>
        public long AddTimer(uint intervalFrame, System.Action action, int repeat = 1)
        {
            FrameAction frameAction = ReferencePool.Acquire<FrameAction>();
            frameAction.Id = action.GetHashCode();
            frameAction.Action = action;
            frameAction.RepeatTime = repeat;
            frameAction.IntervalFrame = intervalFrame;

            CalculateTimerFrame(frameAction);
            AddTimer(frameAction);

            return frameAction.Id;
        }

        private void AddTimer(FrameAction frameAction)
        {
            if (!isInUpdate)
            {
                if (AllFrameActions.TryGetValue(frameAction.Id, out var result))
                {
                    if (result.TargetTickFrame > frameAction.TargetTickFrame)
                    {
                        result.TargetTickFrame = frameAction.TargetTickFrame;
                    }
                }
                else
                {
                    AllFrameActions[frameAction.Id] = frameAction;
                }
            }
            else
            {
                if (ToBeAddedFrameActions.TryGetValue(frameAction.Id, out var result))
                {
                    if (result.TargetTickFrame > frameAction.TargetTickFrame)
                    {
                        result.TargetTickFrame = frameAction.TargetTickFrame;
                    }
                }
                else
                {
                    this.ToBeAddedFrameActions[frameAction.Id] = frameAction;
                }
            }
        }

        public void RemoveTimer(long id)
        {
            if (!isInUpdate)
            {
                if (this.AllFrameActions.TryGetValue(id, out var frameAction))
                {
                    this.AllFrameActions.Remove(id);
                }
            }
            else
            {
                this.ToBeRemovedFrameActions.Add(id);
            }
        }

        public void Update()
        {
            this.isInUpdate = true;

            foreach (var frameActionPair in AllFrameActions)
            {
                FrameAction frameAction = frameActionPair.Value;
                if (frameAction.TargetTickFrame <= LsfComponent.CurrentFrame && !this.ToBeRemovedFrameActions.Contains(frameAction.Id))
                {
                    frameAction.Action.Invoke();

                    if (frameAction.RepeatTime != -1 && --frameAction.RepeatTime <= 0)
                    {
                        RemoveTimer(frameAction.Id);
                    }
                    else
                    {
                        CalculateTimerFrame(frameAction);
                    }
                }
            }

            this.isInUpdate = false;

            foreach (var frameActionId in this.ToBeRemovedFrameActions)
            {
                RemoveTimer(frameActionId);
            }

            foreach (var frameActionPair in this.ToBeAddedFrameActions)
            {
                AddTimer(frameActionPair.Value);
            }

            this.ToBeAddedFrameActions.Clear();
            this.ToBeRemovedFrameActions.Clear();
        }

        /// <summary>
        /// 支持传递一个自定义currentFrame进来，用于回滚时精确调用
        /// </summary>
        /// <param name="frameAction"></param>
        /// <param name="currentFrame"></param>
        private void CalculateTimerFrame(FrameAction frameAction)
        {
            frameAction.TargetTickFrame = LsfComponent.CurrentFrame + frameAction.IntervalFrame;
        }
    }
}