using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    public class UnitComponentAwakeSystem : AwakeSystem<UnitComponent>
    {
        public override void Awake(UnitComponent self)
        {
        }
    }

    public class UnitComponentDestroySystem : DestroySystem<UnitComponent>
    {
        public override void Destroy(UnitComponent self)
        {
            foreach (Unit unit in self.idUnits.Values)
            {
                unit.Dispose();
            }

            self.idUnits.Clear();
        }
    }
}