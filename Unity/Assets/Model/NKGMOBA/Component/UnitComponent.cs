using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
    [ObjectSystem]
    public class UnitComponentSystem: AwakeSystem<UnitComponent>
    {
        public override void Awake(UnitComponent self)
        {
            self.Awake();
        }
    }

    public class UnitComponent: Component
    {
        public static UnitComponent Instance { get; private set; }

        public Unit MyUnit;

        private readonly Dictionary<long, Unit> m_IdUnits = new Dictionary<long, Unit>();

        public void Awake()
        {
            Instance = this;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            GameObjectPool gameObjectPool = Game.Scene.GetComponent<GameObjectPool>();
            foreach (Unit unit in this.m_IdUnits.Values)
            {
                gameObjectPool.Recycle(unit);
            }
            gameObjectPool.Recycle(MyUnit);
            this.m_IdUnits.Clear();

            Instance = null;
        }

        public void Add(Unit unit)
        {
            this.m_IdUnits.Add(unit.Id, unit);
            unit.Parent = this;
        }

        public Unit Get(long id)
        {
            Unit unit;
            if (this.m_IdUnits.TryGetValue(id, out unit))
            {
                if (unit.IsDisposed)
                {
                    Log.Error("想获得的Unit已经Dispose了");
                    return null;
                }

                return unit;
            }

            foreach (var idUnit in m_IdUnits)
            {
                unit = idUnit.Value.GetComponent<ChildrenUnitComponent>().GetUnit(id);
                if (unit != null)
                {
                    return unit;
                }
            }

            Log.Info($"实在没有找到unit，id为{id}");
            return null;
        }

        public void Remove(long id)
        {
            Unit unit;
            this.m_IdUnits.TryGetValue(id, out unit);
            this.m_IdUnits.Remove(id);
            unit?.Dispose();
        }

        public void RemoveNoDispose(long id)
        {
            this.m_IdUnits.Remove(id);
        }

        public int Count
        {
            get
            {
                return this.m_IdUnits.Count;
            }
        }

        public Unit[] GetAll()
        {
            return this.m_IdUnits.Values.ToArray();
        }
    }
}