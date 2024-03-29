﻿using System.Diagnostics;
using ET;

namespace NPBehave
{
    public class Action : Task
    {
        public enum Result
        {
            SUCCESS,
            FAILED,
            BLOCKED,
            PROGRESS
        }

        public enum Request
        {
            START,
            UPDATE,
            CANCEL,
        }

        public System.Func<bool> singleFrameFunc = null;
        public System.Func<bool, Result> multiFrameFunc = null;
        public System.Func<Request, Result> multiFrameFunc2 = null;
        public System.Action action = null;
        public bool bWasBlocked = false;
        private long TimerId;

        public Action() : base("Action")
        {
        }

        public Action(System.Action action) : base("Action")
        {
            this.action = action;
        }

        public Action(System.Func<bool, Result> multiframeFunc) : base("Action")
        {
            this.multiFrameFunc = multiframeFunc;
        }

        public Action(System.Func<Request, Result> multiframeFunc2) : base("Action")
        {
            this.multiFrameFunc2 = multiframeFunc2;
        }

        public Action(System.Func<bool> singleFrameFunc) : base("Action")
        {
            this.singleFrameFunc = singleFrameFunc;
        }

        protected override void DoStart()
        {
            if (this.action != null)
            {
                this.action.Invoke();
                this.Stopped(true);
            }
            else if (this.multiFrameFunc != null)
            {
                Result result = this.multiFrameFunc.Invoke(false);
                if (result == Result.PROGRESS)
                {
                    TimerId = this.RootNode.Clock.AddTimer(1, OnUpdateFunc, -1);
                }
                else if (result == Result.BLOCKED)
                {
                    this.bWasBlocked = true;
                    TimerId = this.RootNode.Clock.AddTimer(1, OnUpdateFunc, -1);
                }
                else
                {
                    this.Stopped(result == Result.SUCCESS);
                }
            }
            else if (this.multiFrameFunc2 != null)
            {
                Result result = this.multiFrameFunc2.Invoke(Request.START);
                if (result == Result.PROGRESS)
                {
                    TimerId = this.RootNode.Clock.AddTimer(1, OnUpdateFunc2, -1);
                }
                else if (result == Result.BLOCKED)
                {
                    this.bWasBlocked = true;
                    TimerId = this.RootNode.Clock.AddTimer(1, OnUpdateFunc2, -1);
                }
                else
                {
                    this.Stopped(result == Result.SUCCESS);
                }
            }
            else if (this.singleFrameFunc != null)
            {
                this.Stopped(this.singleFrameFunc.Invoke());
            }
        }

        private void OnUpdateFunc()
        {
            Result result = this.multiFrameFunc.Invoke(false);
            if (result != Result.PROGRESS && result != Result.BLOCKED)
            {
                this.RootNode.Clock.RemoveTimer(TimerId);
                this.Stopped(result == Result.SUCCESS);
            }
        }

        private void OnUpdateFunc2()
        {
            Result result = this.multiFrameFunc2.Invoke(bWasBlocked ? Request.START : Request.UPDATE);

            if (result == Result.BLOCKED)
            {
                bWasBlocked = true;
            }
            else if (result == Result.PROGRESS)
            {
                bWasBlocked = false;
            }
            else
            {
                this.RootNode.Clock.RemoveTimer(TimerId);
                this.Stopped(result == Result.SUCCESS);
            }
        }

        protected override void DoCancel()
        {
            if (this.multiFrameFunc != null)
            {
                Result result = this.multiFrameFunc.Invoke(true);
                Debug.Assert(result != Result.PROGRESS,
                    $"The Task has to return Result.SUCCESS, Result.FAILED/BLOCKED after beeing cancelled!  PATH: {GetPath()}");
                this.RootNode.Clock.RemoveTimer(TimerId);
                this.Stopped(result == Result.SUCCESS);
            }
            else if (this.multiFrameFunc2 != null)
            {
                Result result = this.multiFrameFunc2.Invoke(Request.CANCEL);
                Debug.Assert(result != Result.PROGRESS,
                    $"The Task has to return Result.SUCCESS or Result.FAILED/BLOCKED after beeing cancelled!  PATH: {GetPath()}");
                this.RootNode.Clock.RemoveTimer(TimerId);
                this.Stopped(result == Result.SUCCESS);
            }
            else
            {
                Debug.Assert(false, $"DoStop called for a single frame action on " + this + $"  PATH: {GetPath()}");
            }
        }
    }
}