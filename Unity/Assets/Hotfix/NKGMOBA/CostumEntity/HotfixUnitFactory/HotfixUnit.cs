//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月1日 11:03:11
//------------------------------------------------------------

using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class HotfixUnitAwakeSystem: AwakeSystem<HotfixUnit, Unit>
    {
        public override void Awake(HotfixUnit self, Unit modelUnit)
        {
            self.Awake(modelUnit);
        }
    }

    public class HotfixUnit: Entity
    {
        public Unit m_ModelUnit;

        public void Awake(Unit modelUnit)
        {
            this.m_ModelUnit = modelUnit;
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            this.m_ModelUnit.Dispose();
        }
    }
}