using System.Runtime.CompilerServices;

namespace ETModel
{
    /// <summary>
    /// 进行下一个异步方法生成的状态机
    /// </summary>
    /// <typeparam name="TStateMachine">表示为异步方法生成的状态机。此类型仅供编译器使用。</typeparam>
    internal class MoveNextRunner<TStateMachine> where TStateMachine : IAsyncStateMachine
    {
        public TStateMachine StateMachine;

        /// <summary>
        /// 从当前状态机转换到下一个状态机
        /// </summary>
        public void Run()
        {
            StateMachine.MoveNext();
        }
    }
}