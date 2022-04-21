using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
	public class UnitComponent: Entity
	{
		[BsonElement]
		[BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
		public readonly Dictionary<long, Unit> idUnits = new Dictionary<long, Unit>();
		
		public int Count
		{
			get
			{
				return this.idUnits.Count;
			}
		}
		
		public void Add( Unit unit)
		{
			this.idUnits.Add(unit.Id, unit);
		}

		public Unit Get( long id)
		{
			this.idUnits.TryGetValue(id, out Unit unit);
			return unit;
		}

		public void Remove( long id)
		{
			Unit unit;
			this.idUnits.TryGetValue(id, out unit);
			this.idUnits.Remove(id);
			unit?.Dispose();
		}

		public void RemoveNoDispose( long id)
		{
			this.idUnits.Remove(id);
		}

		public Unit[] GetAll()
		{
			return this.idUnits.Values.ToArray();
		}
		
		public List<Unit> GetAllUnitToSyncToClient()
		{
			List<Unit> allUnitTobeSynced = new List<Unit>();
			foreach (var unit in idUnits)
			{
				if (unit.Value.NeedSyncToClient)
				{
					allUnitTobeSynced.Add(unit.Value);
				}
			}

			return allUnitTobeSynced;
		}
	}
}