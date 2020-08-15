//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 15:06:15
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using NPBehave;
using Exception = NPBehave.Exception;

namespace ETModel
{
    public class NP_RuntimeTreeFactory
    {
        /// <summary>
        /// 创建一个行为树实例
        /// </summary>
        /// <param name="unit">行为树所归属unit</param>
        /// <param name="NPDataId">行为树数据id</param>
        /// <returns></returns>
        public static NP_RuntimeTree CreateNpRuntimeTree(Unit unit, long NPDataId)
        {
            NP_DataSupportor npDataSupportor =
                Game.Scene.GetComponent<NP_TreeDataRepository>().GetNP_TreeData_DeepCopy(NPDataId);

            long theRuntimeTreeID = IdGenerater.GenerateId();

            //Log.Info($"运行时id为{theRuntimeTreeID}");
            foreach (var nodeDateBase in npDataSupportor.mNP_DataSupportorDic)
            {
                switch (nodeDateBase.Value.NodeType)
                {
                    case NodeType.Task:
                        try
                        {
                            nodeDateBase.Value.CreateTask(unit.Id, theRuntimeTreeID);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"{e}-----{nodeDateBase.Value.NodeDes}");
                            throw;
                        }

                        break;
                    case NodeType.Decorator:
                        try
                        {
                            nodeDateBase.Value.CreateDecoratorNode(unit.Id, theRuntimeTreeID,
                                npDataSupportor.mNP_DataSupportorDic[nodeDateBase.Value.linkedID[0]].NP_GetNode());
                        }
                        catch (Exception e)
                        {
                            Log.Error($"{e}-----{nodeDateBase.Value.NodeDes}");
                            throw;
                        }

                        break;
                    case NodeType.Composite:
                        try
                        {
                            List<Node> temp = new List<Node>();
                            foreach (var VARIABLE1 in nodeDateBase.Value.linkedID)
                            {
                                temp.Add(npDataSupportor.mNP_DataSupportorDic[VARIABLE1].NP_GetNode());
                            }
                            nodeDateBase.Value.CreateComposite(temp.ToArray());
                        }
                        catch (Exception e)
                        {
                            Log.Error($"{e}-----{nodeDateBase.Value.NodeDes}");
                            throw;
                        }

                        break;
                }
            }

            NP_RuntimeTree tempTree = ComponentFactory.CreateWithId<NP_RuntimeTree, Root, NP_DataSupportor>(
                theRuntimeTreeID,
                (Root) npDataSupportor.mNP_DataSupportorDic[npDataSupportor.RootId].NP_GetNode(), npDataSupportor);

            unit.GetComponent<NP_RuntimeTreeManager>().AddTree(tempTree.Id, npDataSupportor.RootId, tempTree);
            
            //把提前缓存的数据注入黑板
            unit.GetComponent<NP_InitCacheComponent>()?.AddCacheDatas2RuntimeTree(tempTree);
            return tempTree;
        }
    }
}