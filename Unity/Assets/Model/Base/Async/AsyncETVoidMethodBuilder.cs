using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;

namespace ETModel
{
    public struct AsyncETVoidMethodBuilder
    {
        /// <summary>
        /// 转换到下一状态机的方法（委托）
        /// </summary>
        private Action moveNext;

        /// <summary>
        /// 静态方法创建
        /// </summary>
        /// <returns></returns>
        [DebuggerHidden]
        public static AsyncETVoidMethodBuilder Create()
        {
            AsyncETVoidMethodBuilder builder = new AsyncETVoidMethodBuilder();
            return builder;
        }

        /// <summary>
        /// 类似Task属性
        /// </summary>
        public ETVoid Task => default;

        /// <summary>
        /// 设置异常
        /// </summary>
        /// <param name="exception"></param>
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            Log.Error(exception);
        }

        /// <summary>
        /// 设置结果
        /// </summary>
        [DebuggerHidden]
        public void SetResult()
        {
            // TODO ：Nothing
        }

        /// <summary>
        /// 当前任务完成
        /// </summary>
        /// <param name="awaiter">当前任务守护者</param>
        /// <param name="stateMachine">异步方法生成的状态机</param>
        /// <typeparam name="TAwaiter"></typeparam>
        /// <typeparam name="TStateMachine"></typeparam>
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : INotifyCompletion
                where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine; 
            }

            awaiter.OnCompleted(moveNext);
        }

        /// <summary>
        /// 当前任务不安全完成
        /// </summary>
        /// <param name="awaiter">当前任务守护者</param>
        /// <param name="stateMachine">异步方法生成的状态机</param>
        /// <typeparam name="TAwaiter"></typeparam>
        /// <typeparam name="TStateMachine"></typeparam>
        [DebuggerHidden]
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
                where TAwaiter : ICriticalNotifyCompletion
                where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine; // set after create delegate.
            }

            awaiter.UnsafeOnCompleted(moveNext);
        }

        // 7. Start
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}