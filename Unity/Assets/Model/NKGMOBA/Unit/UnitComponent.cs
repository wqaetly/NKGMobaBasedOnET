using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public static class UnitComponentSystem
    {
        public static void Add(this UnitComponent self, Unit unit)
        {
            self.idUnits.Add(unit.Id, unit);
        }

        public static Unit Get(this UnitComponent self, long id)
        {
            Unit unit;
            self.idUnits.TryGetValue(id, out unit);
            return unit;
        }

        public static void Remove(this UnitComponent self, long id)
        {
            Unit unit;
            self.idUnits.TryGetValue(id, out unit);
            self.idUnits.Remove(id);
            unit?.Dispose();
        }

        public static void RemoveAll(this UnitComponent self)
        {
            foreach (var unit in self.idUnits)
            {
                unit.Value?.Dispose();
            }

            self.idUnits.Clear();
        }

        public static void RemoveNoDispose(this UnitComponent self, long id)
        {
            self.idUnits.Remove(id);
        }

        public static Unit[] GetAll(this UnitComponent self)
        {
            return self.idUnits.Values.ToArray();
        }
        
        public static void OnLSF_Tick(this UnitComponent self)
        {
            
        }
    }

    public class UnitComponent : Entity
    {
        public Dictionary<long, Unit> idUnits = new Dictionary<long, Unit>();

#if !SERVER
        public Unit MyUnit;
#endif
        public void LSF_Tick()
        {
            this.OnLSF_Tick();
        }
    }
}