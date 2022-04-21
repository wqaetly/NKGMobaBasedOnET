using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 专门处理状态帧消息
    /// </summary>
    public class LSF_CmdDispatcherComponent : Entity
    {
        public static LSF_CmdDispatcherComponent Instance;
        
        public readonly Dictionary<uint, List<ILockStepStateFrameSyncMessageHandler>> Handlers =
            new Dictionary<uint, List<ILockStepStateFrameSyncMessageHandler>>();
    }
}