//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月20日 19:21:49
//------------------------------------------------------------

using System.Numerics;
using Box2DSharp.Dynamics;

namespace ETModel
{
    [ObjectSystem]
    public class B2S_WorldComponentAwakeSystem: AwakeSystem<B2S_WorldComponent>
    {
        public override void Awake(B2S_WorldComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class B2S_WorldComponentFixedUpdateSystem: FixedUpdateSystem<B2S_WorldComponent>
    {
        public override void FixedUpdate(B2S_WorldComponent self)
        {
            self.FixedUpdate();
        }
    }

    /// <summary>
    /// 物理世界组件，代表一个物理世界
    /// </summary>
    public class B2S_WorldComponent: Component
    {
        private World m_World;
        public const int VelocityIteration = 10;
        public const int PositionIteration = 10;

        public void Awake()
        {
            this.m_World = B2S_WorldUtility.CreateWorld(new Vector2(0, 0));
        }

        public void FixedUpdate()
        {
            this.m_World.Step(EventSystem.FixedUpdateTimeDelta, VelocityIteration, PositionIteration);
        }

        public World GetWorld()
        {
            return this.m_World;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
            this.m_World.Dispose();
        }
    }
}