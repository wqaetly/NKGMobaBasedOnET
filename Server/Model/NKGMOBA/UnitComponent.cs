using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

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
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private readonly Dictionary<long, Unit> idUnits = new Dictionary<long, Unit>();

        private static UnitComponent m_Instance;

        public static UnitComponent Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    Log.Error("请先注册UnitComponent到Game.Scene中");
                    
                    return null;
                }
                else
                {
                    return m_Instance;
                }

            }
        }

        
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (Unit unit in this.idUnits.Values)
            {
                unit.Dispose();
            }

            this.idUnits.Clear();
        }

        public void Add(Unit unit)
        {
            this.idUnits.Add(unit.Id, unit);
        }

        /// <summary>
        /// 先查找所有父物体，如果没有找到再找子物体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Unit Get(long id)
        {
            Unit unit;
            if (this.idUnits.TryGetValue(id, out unit))
            {
                return unit;
            }

            foreach (var VARIABLE in idUnits)
            {
                
                unit = VARIABLE.Value.GetComponent<ChildrenUnitComponent>().GetUnit(id);
                if (unit != null)
                {
                    Log.Info($"尝试从子实体取得id为{id}的实体");
                    return unit;
                }
            }

            Log.Info($"实在没有找到unit，id为{id}");
            return null;
        }

        public void Remove(long id)
        {
            Unit unit;
            this.idUnits.TryGetValue(id, out unit);
            this.idUnits.Remove(id);
            unit?.Dispose();
        }

        public void RemoveNoDispose(long id)
        {
            this.idUnits.Remove(id);
        }

        public int Count
        {
            get
            {
                return this.idUnits.Count;
            }
        }

        public Unit[] GetAll()
        {
            return this.idUnits.Values.ToArray();
        }

        public void Awake()
        {
            m_Instance = this;
        }
    }
}