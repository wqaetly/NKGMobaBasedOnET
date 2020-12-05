using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public class GizmosDebug: MonoBehaviour
    {
        public static GizmosDebug Instance { get; private set; }

        public Dictionary<long, List<Vector3>> Path = new Dictionary<long, List<Vector3>>();

        private void Awake()
        {
            Instance = this;
        }

        private void OnDrawGizmos()
        {
            foreach (var VARIABLE in Path)
            {
                for (int i = 0; i < VARIABLE.Value.Count - 1; ++i)
                {
                    if (VARIABLE.Value.Count < 2)
                    {
                        return;
                    }
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(VARIABLE.Value[i], VARIABLE.Value[i + 1]);
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(VARIABLE.Value[i + 1], 0.3f);
                }
            }
        }

        public void AddData(long unitId, Vector3 point)
        {
            if (this.Path.TryGetValue(unitId, out var list))
            {
                list.Add(point);
            }
            else
            {
                this.Path.Add(unitId, new List<Vector3>() { point });
            }
        }

        public void ClearData(long unitId)
        {
            if (this.Path.ContainsKey(unitId))
            {
                this.Path.Remove(unitId);
            }
        }
    }
}