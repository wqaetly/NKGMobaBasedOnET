using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ETModel
{
    /// <summary>
    /// 轻量级Task，不带泛型
    /// </summary>
    [AsyncMethodBuilder(typeof (AsyncETTaskMethodBuilder))]
    public partial struct ETTask: IEquatable<ETTask>
    {
        /// <summary>
        /// 守护者
        /// </summary>
        private readonly IAwaiter awaiter;

        [DebuggerHidden]
        public ETTask(IAwaiter awaiter)
        {
            this.awaiter = awaiter;
        }

        /// <summary>
        /// 设置状态，如果守护者不存在，那么成功
        /// </summary>
        [DebuggerHidden]
        public AwaiterStatus Status => awaiter?.Status ?? AwaiterStatus.Succeeded;

        /// <summary>
        /// 是否成功，值与守护者IsCompleted一致（如果守护者存在的话）
        /// </summary>
        [DebuggerHidden]
        public bool IsCompleted => awaiter?.IsCompleted ?? true;

        /// <summary>
        /// 获取守护者状态
        /// </summary>
        [DebuggerHidden]
        public void GetResult()
        {
            if (awaiter != null)
            {
                awaiter.GetResult();
            }
        }

        public void Coroutine()
        {
        }

        /// <summary>
        /// 获取守护者
        /// </summary>
        /// <returns></returns>
        [DebuggerHidden]
        public Awaiter GetAwaiter()
        {
            return new Awaiter(this);
        }

        /// <summary>
        /// 重写比较函数
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ETTask other)
        {
            if (this.awaiter == null && other.awaiter == null)
            {
                return true;
            }

            if (this.awaiter != null && other.awaiter != null)
            {
                return this.awaiter == other.awaiter;
            }

            return false;
        }

        /// <summary>
        /// 重写HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (this.awaiter == null)
            {
                return 0;
            }

            return this.awaiter.GetHashCode();
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.awaiter == null? "()"
                    : this.awaiter.Status == AwaiterStatus.Succeeded? "()"
                    : "(" + this.awaiter.Status + ")";
        }

        /// <summary>
        /// 守护者结构体
        /// </summary>
        public struct Awaiter: IAwaiter
        {
            private readonly ETTask task;

            [DebuggerHidden]
            public Awaiter(ETTask task)
            {
                this.task = task;
            }

            [DebuggerHidden]
            public bool IsCompleted => task.IsCompleted;

            [DebuggerHidden]
            public AwaiterStatus Status => task.Status;

            [DebuggerHidden]
            public void GetResult()
            {
                task.GetResult();
            }

            [DebuggerHidden]
            public void OnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.OnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }

            [DebuggerHidden]
            public void UnsafeOnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.UnsafeOnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }
        }
    }

    /// <summary>
    /// 轻量级Task，带泛型
    /// </summary>
    [AsyncMethodBuilder(typeof (ETAsyncTaskMethodBuilder<>))]
    public struct ETTask<T>: IEquatable<ETTask<T>>
    {
        private readonly T result;
        private readonly IAwaiter<T> awaiter;

        [DebuggerHidden]
        public ETTask(T result)
        {
            this.result = result;
            this.awaiter = null;
        }

        [DebuggerHidden]
        public ETTask(IAwaiter<T> awaiter)
        {
            this.result = default;
            this.awaiter = awaiter;
        }

        [DebuggerHidden]
        public AwaiterStatus Status => awaiter?.Status ?? AwaiterStatus.Succeeded;

        [DebuggerHidden]
        public bool IsCompleted => awaiter?.IsCompleted ?? true;

        [DebuggerHidden]
        public T Result
        {
            get
            {
                if (awaiter == null)
                {
                    return result;
                }

                return this.awaiter.GetResult();
            }
        }

        public void Coroutine()
        {
        }

        [DebuggerHidden]
        public Awaiter GetAwaiter()
        {
            return new Awaiter(this);
        }

        public bool Equals(ETTask<T> other)
        {
            if (this.awaiter == null && other.awaiter == null)
            {
                return EqualityComparer<T>.Default.Equals(this.result, other.result);
            }

            if (this.awaiter != null && other.awaiter != null)
            {
                return this.awaiter == other.awaiter;
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (this.awaiter == null)
            {
                if (result == null)
                {
                    return 0;
                }

                return result.GetHashCode();
            }

            return this.awaiter.GetHashCode();
        }

        public override string ToString()
        {
            return this.awaiter == null? result.ToString()
                    : this.awaiter.Status == AwaiterStatus.Succeeded? this.awaiter.GetResult().ToString()
                    : "(" + this.awaiter.Status + ")";
        }

        public static implicit operator ETTask(ETTask<T> task)
        {
            if (task.awaiter != null)
            {
                return new ETTask(task.awaiter);
            }

            return new ETTask();
        }

        public struct Awaiter: IAwaiter<T>
        {
            private readonly ETTask<T> task;

            [DebuggerHidden]
            public Awaiter(ETTask<T> task)
            {
                this.task = task;
            }

            [DebuggerHidden]
            public bool IsCompleted => task.IsCompleted;

            [DebuggerHidden]
            public AwaiterStatus Status => task.Status;

            [DebuggerHidden]
            void IAwaiter.GetResult()
            {
                GetResult();
            }

            [DebuggerHidden]
            public T GetResult()
            {
                return task.Result;
            }

            [DebuggerHidden]
            public void OnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.OnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }

            [DebuggerHidden]
            public void UnsafeOnCompleted(Action continuation)
            {
                if (task.awaiter != null)
                {
                    task.awaiter.UnsafeOnCompleted(continuation);
                }
                else
                {
                    continuation();
                }
            }
        }
    }
}