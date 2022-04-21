using System;
using System.Collections.Generic;
using System.Diagnostics;
using ET.EventType;
using NPBehave_Core;
using UnityEngine;

#if !SERVER
using UnityEngine.Profiling;

#endif

namespace ET
{
    public class LockStepStateFrameSyncComponentAwakeSystem : AwakeSystem<LSF_Component>
    {
        public override void Awake(LSF_Component self)
        {
        }
    }

    public class LockStepStateFrameSyncComponentUpdateSystem : UpdateSystem<LSF_Component>
    {
        public override void Update(LSF_Component self)
        {
            if (!self.StartSync)
            {
                return;
            }

            // 将FixedUpdate Tick放在此处，这样可以防止框架层FixedUpdate帧率小于帧同步FixedUpdate帧率而导致的一些问题
            self.FixedUpdate.Tick();

#if !SERVER
            self.ClientHandleExceptionNet().Coroutine();
            self.LSF_TickBattleView(TimeAndFrameConverter.MS_Float2Long(Time.deltaTime));
#endif
        }
    }

    public class LockStepStateFrameSyncComponentFixedUpdateSystem : FixedUpdateSystem<LSF_Component>
    {
        public override void FixedUpdate(LSF_Component self)
        {
            if (!self.StartSync)
            {
                return;
            }

#if !SERVER
            // 本地也跑一个服务器模拟帧数
            self.ServerCurrentFrame++;
#endif
        }
    }

    public class LockStepStateFrameSyncComponentDestroySystem : DestroySystem<LSF_Component>
    {
        public override void Destroy(LSF_Component self)
        {
            self.FixedUpdate.UpdateCallback = null;
            self.FixedUpdate = null;
        }
    }
}