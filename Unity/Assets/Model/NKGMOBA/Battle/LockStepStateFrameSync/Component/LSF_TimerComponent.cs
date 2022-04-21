using System;
using System.Collections.Generic;

namespace ET
{
    public enum LSF_TimerClass
    {
        None,
        OnceWaitTimer,
        OnceTimer,
        RepeatedTimer,
    }

    public class LSF_TimerAction : Entity
    {
        public TimerClass TimerClass;

        public object Callback;

        public uint FrameCount;
    }

    [ObjectSystem]
    public class LSF_TimerComponentAwakeSystem : AwakeSystem<LSF_TimerComponent>
    {
        public override void Awake(LSF_TimerComponent self)
        {
            self.LsfComponent = self.GetParent<Room>().GetComponent<LSF_Component>();
        }
    }

    [ObjectSystem]
    public class LSF_TimerActionAwakeSystem : AwakeSystem<LSF_TimerAction, TimerClass, uint, object>
    {
        public override void Awake(LSF_TimerAction self, TimerClass timerClass, uint frameCount, object callback)
        {
            self.TimerClass = timerClass;
            self.Callback = callback;
            self.FrameCount = frameCount;
        }
    }

    [ObjectSystem]
    public class LSF_TimerActionDestroySystem : DestroySystem<LSF_TimerAction>
    {
        public override void Destroy(LSF_TimerAction self)
        {
            self.Callback = null;
            self.FrameCount = 0;
            self.TimerClass = TimerClass.None;
        }
    }

    /// <summary>
    /// 状态帧同步专用计时器组件
    /// </summary>
    public class LSF_TimerComponent : Entity
    {
        public LSF_Component LsfComponent;

        /// <summary>
        /// key: time, value: timer id
        /// </summary>
        private readonly MultiMap<uint, long> TimeId = new MultiMap<uint, long>();

        private readonly Queue<uint> timeOutTime = new Queue<uint>();

        private readonly Queue<long> timeOutTimerIds = new Queue<long>();

        // 记录最小时间，不用每次都去MultiMap取第一个值
        private uint minFrame;

        public void ResetTime(long timeid, long newtime)
        {
            foreach (KeyValuePair<uint, List<long>> kv in this.TimeId)
            {
                foreach (var timer in kv.Value)
                {
                    if (timer == timeid)
                    {
                        kv.Value.Remove(timer);
                        this.TimeId.Add(
                            this.LsfComponent.CurrentFrame + TimeAndFrameConverter.Frame_Long2Frame(newtime), timeid);
                    }
                }
            }
        }

        public void FixedUpdate()
        {
            if (this.TimeId.Count == 0)
            {
                return;
            }

            uint currentFrame = LsfComponent.CurrentFrame;
            if (currentFrame < this.minFrame)
            {
                return;
            }

            foreach (KeyValuePair<uint, List<long>> kv in this.TimeId)
            {
                uint k = kv.Key;
                if (k > currentFrame)
                {
                    minFrame = k;
                    break;
                }

                this.timeOutTime.Enqueue(k);
            }

            while (this.timeOutTime.Count > 0)
            {
                uint time = this.timeOutTime.Dequeue();
                foreach (long timerId in this.TimeId[time])
                {
                    this.timeOutTimerIds.Enqueue(timerId);
                }

                this.TimeId.Remove(time);
            }

            while (this.timeOutTimerIds.Count > 0)
            {
                long timerId = this.timeOutTimerIds.Dequeue();

                LSF_TimerAction timerAction = this.GetChild<LSF_TimerAction>(timerId);
                if (timerAction == null)
                {
                    continue;
                }

                Run(timerAction);
            }
        }

        private void Run(LSF_TimerAction timerAction)
        {
            switch (timerAction.TimerClass)
            {
                case TimerClass.OnceWaitTimer:
                {
                    ETTask<bool> tcs = timerAction.Callback as ETTask<bool>;
                    this.Remove(timerAction.Id);
                    tcs.SetResult(true);
                    break;
                }
                case TimerClass.OnceTimer:
                {
                    Action action = timerAction.Callback as Action;
                    this.Remove(timerAction.Id);
                    action?.Invoke();
                    break;
                }
                case TimerClass.RepeatedTimer:
                {
                    Action action = timerAction.Callback as Action;
                    uint tillTime = LsfComponent.CurrentFrame + timerAction.FrameCount;
                    this.AddTimer(tillTime, timerAction);
                    action?.Invoke();
                    break;
                }
            }
        }

        private void AddTimer(uint tillFrame, LSF_TimerAction timer)
        {
            this.TimeId.Add(tillFrame, timer.Id);
            if (tillFrame < this.minFrame)
            {
                this.minFrame = tillFrame;
            }
        }

        public async ETTask<bool> WaitFrameAsync(ETCancellationToken cancellationToken = null)
        {
            return await WaitAsync(1, cancellationToken);
        }

        /// <summary>
        /// 等待x毫秒
        /// </summary>
        /// <param name="time"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ETTask<bool> WaitAsync(long time, ETCancellationToken cancellationToken = null)
        {
            if (time == 0)
            {
                return true;
            }

            uint tillFrame = LsfComponent.CurrentFrame + TimeAndFrameConverter.Frame_Long2Frame(time);

            ETTask<bool> tcs = ETTask<bool>.Create(true);

            LSF_TimerAction timer =
                this.AddChild<LSF_TimerAction, TimerClass, uint, object>(TimerClass.OnceWaitTimer, 0, tcs, true);
            this.AddTimer(tillFrame, timer);
            long timerId = timer.Id;

            void CancelAction()
            {
                if (this.Remove(timerId))
                {
                    tcs.SetResult(false);
                }
            }

            bool ret;
            try
            {
                cancellationToken?.Add(CancelAction);
                ret = await tcs;
            }
            finally
            {
                cancellationToken?.Remove(CancelAction);
            }

            return ret;
        }

        public long NewFrameTimer(Action action)
        {
            return NewRepeatedTimerInner(1, action);
        }

        /// <summary>
        /// 创建一个RepeatedTimer
        /// </summary>
        private long NewRepeatedTimerInner(long time, Action action)
        {
            uint frameCount = TimeAndFrameConverter.Frame_Long2Frame(time);
            uint tillFrame = LsfComponent.CurrentFrame + frameCount;
            LSF_TimerAction timer =
                this.AddChild<LSF_TimerAction, TimerClass, uint, object>(TimerClass.RepeatedTimer, frameCount, action, true);
            this.AddTimer(tillFrame, timer);
            return timer.Id;
        }

        public long NewRepeatedTimer(long time, Action action)
        {
            return NewRepeatedTimerInner(time, action);
        }

        public bool Remove(long id)
        {
            if (id == 0)
            {
                return false;
            }

            LSF_TimerAction timerAction = this.GetChild<LSF_TimerAction>(id);
            if (timerAction == null)
            {
                return false;
            }

            timerAction.Dispose();
            return true;
        }
    }
}