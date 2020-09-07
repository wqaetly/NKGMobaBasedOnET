//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月1日 10:04:05
//------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class M5V5GameComponentAwakeSystem: AwakeSystem<M5V5GameComponent, M5V5Game>
    {
        public override void Awake(M5V5GameComponent self, M5V5Game a)
        {
            self.Awake(a);
        }
    }

    public class M5V5GameComponent: Component
    {
        public M5V5Game m_5V5Game { get; set; }

        public Dictionary<long, HotfixUnit> m_HotfixUnits = new Dictionary<long, HotfixUnit>();

        public void Awake(M5V5Game m5V5Game)
        {
            this.m_5V5Game = m5V5Game;
        }

        public void AddHotfixUnit(long id, HotfixUnit hotfixUnit)
        {
            this.m_HotfixUnits.Add(id, hotfixUnit);
        }

        public HotfixUnit GetHotfixUnit(long id)
        {
            HotfixUnit hotfixUnit;
            this.m_HotfixUnits.TryGetValue(id, out hotfixUnit);
            return hotfixUnit;
        }

        public void RemoveHotfixUnit(long id)
        {
            HotfixUnit hotfixUnit;
            this.m_HotfixUnits.TryGetValue(id, out hotfixUnit);
            this.m_HotfixUnits.Remove(id);
            hotfixUnit?.Dispose();
        }

        public void RemoveNoDispose(long id)
        {
            this.m_HotfixUnits.Remove(id);
        }

        public int Count
        {
            get
            {
                return this.m_HotfixUnits.Count;
            }
        }

        public HotfixUnit[] GetAllHotfixUnits()
        {
            return this.m_HotfixUnits.Values.ToArray();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            foreach (KeyValuePair<long, HotfixUnit> hotfixUnit in this.m_HotfixUnits)
            {
                hotfixUnit.Value.Dispose();
            }

            this.m_5V5Game.Dispose();
        }
    }
}