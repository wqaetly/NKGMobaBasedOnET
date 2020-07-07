//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 12:18:03
//------------------------------------------------------------

using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 子实体组件，用于管理unit产生的子实体
    /// 一般用于，召唤物，有关联的实体
    /// 以及那些可能并不需要网络通讯的子实体，训练营的那个木桩就是最好的例子
    /// </summary>
    public class ChildrenUnitComponent: Component
    {
        /// <summary>
        /// 用于管理子实体
        /// </summary>
        public List<Unit> ChildrenUnit = new List<Unit>();

        public void AddUnit(Unit unit)
        {
            this.ChildrenUnit.Add(unit);
        }

        public void RemoveUnit(Unit unit)
        {
            this.ChildrenUnit.Remove(unit);
        }

        public Unit GetUnit(long id)
        {
            return this.ChildrenUnit.Find(s => s.Id.Equals(id));
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
#if SERVER
            foreach (var VARIABLE in ChildrenUnit)
            {
                VARIABLE.Dispose();
            }
#elif !SERVER
                        GameObjectPool<Unit> gameObjectPool = Game.Scene.GetComponent<GameObjectPool<Unit>>();
            foreach (var VARIABLE in ChildrenUnit)
            {
                gameObjectPool.Recycle(VARIABLE);
            }
#endif

            ChildrenUnit.Clear();
        }
    }
}