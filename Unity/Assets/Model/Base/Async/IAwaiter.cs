using System.Runtime.CompilerServices;

namespace ETModel
{
    /// <summary>
    /// 守护者状态
    /// </summary>
    public enum AwaiterStatus
    {
        /// <summary>
        /// 任务状态：操作还没有完成
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 任务状态：操作成功完成
        /// </summary>
        Succeeded = 1,

        /// <summary>
        /// 任务状态：操作以失败收尾
        /// </summary>
        Faulted = 2,

        /// <summary>
        /// 任务状态：操作以取消结尾
        /// </summary>
        Canceled = 3
    }

    /// <summary>
    /// 表示 一个等待操作完成时 所调用的接口
    /// </summary>
    public interface IAwaiter: ICriticalNotifyCompletion
    {
        AwaiterStatus Status { get; }
        bool IsCompleted { get; }
        void GetResult();
    }

    public interface IAwaiter<out T>: IAwaiter
    {
        new T GetResult();
    }

    /// <summary>
    /// 任务状态拓展
    /// </summary>
    public static class AwaiterStatusExtensions
    {
        /// <summary>
        /// 已完成，不论成功与否
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCompleted(this AwaiterStatus status)
        {
            return status != AwaiterStatus.Pending;
        }

        /// <summary>
        /// 成功完成
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCompletedSuccessfully(this AwaiterStatus status)
        {
            return status == AwaiterStatus.Succeeded;
        }

        /// <summary>
        /// 任务取消
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCanceled(this AwaiterStatus status)
        {
            return status == AwaiterStatus.Canceled;
        }

        /// <summary>
        /// 任务失败
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFaulted(this AwaiterStatus status)
        {
            return status == AwaiterStatus.Faulted;
        }
    }
}