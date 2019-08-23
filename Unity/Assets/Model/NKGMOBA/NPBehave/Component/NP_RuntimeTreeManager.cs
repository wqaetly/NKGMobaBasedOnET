//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 14:54:26
//------------------------------------------------------------

using System.Collections.Generic;

namespace ETModel
{
    public class NP_RuntimeTreeManager: Component
    {
        public Dictionary<long, NP_RuntimeTree> RuntimeTrees = new Dictionary<long, NP_RuntimeTree>();

        public void AddTree(long id, NP_RuntimeTree npRuntimeTree)
        {
            RuntimeTrees.Add(id, npRuntimeTree);
        }

        public NP_RuntimeTree GetTree(long id)
        {
            if (RuntimeTrees.ContainsKey(id))
            {
                return RuntimeTrees[id];
            }

            Log.Error($"请求的ID不存在，id是{id}");
            return null;
        }

        public void RemoveTree(long id)
        {
            if (RuntimeTrees.ContainsKey(id))
            {
                RuntimeTrees.Remove(id);
            }
            else
            {
                Log.Error($"请求删除的ID不存在，id是{id}");
            }
        }
    }
}