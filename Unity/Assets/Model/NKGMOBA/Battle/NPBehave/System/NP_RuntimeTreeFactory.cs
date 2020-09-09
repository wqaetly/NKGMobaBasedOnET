//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 15:06:15
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ETModel.BBValues;
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
        /// <param name="nPDataId">行为树数据id</param>
        /// <returns></returns>
        public static NP_RuntimeTree CreateNpRuntimeTree(Unit unit, long nPDataId)
        {
            NP_DataSupportor npDataSupportor =
                    Game.Scene.GetComponent<NP_TreeDataRepository>().GetNP_TreeData_DeepCopy(nPDataId);

            NP_RuntimeTree tempTree =
                    ComponentFactory.Create<NP_RuntimeTree, NP_DataSupportor>(npDataSupportor);

            long rootId = npDataSupportor.RootId;

            unit.GetComponent<NP_RuntimeTreeManager>().AddTree(tempTree.Id, rootId, tempTree);

            //Log.Info($"运行时id为{theRuntimeTreeID}");
            //配置节点数据
            foreach (var nodeDateBase in npDataSupportor.NP_DataSupportorDic)
            {
                switch (nodeDateBase.Value.NodeType)
                {
                    case NodeType.Task:
                        try
                        {
                            nodeDateBase.Value.CreateTask(unit.Id, tempTree);
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
                            nodeDateBase.Value.CreateDecoratorNode(unit.Id, tempTree,
                                npDataSupportor.NP_DataSupportorDic[nodeDateBase.Value.LinkedIds[0]].NP_GetNode());
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
                            foreach (var linkedId in nodeDateBase.Value.LinkedIds)
                            {
                                temp.Add(npDataSupportor.NP_DataSupportorDic[linkedId].NP_GetNode());
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

            //配置根结点
            tempTree.SetRootNode(npDataSupportor.NP_DataSupportorDic[rootId].NP_GetNode() as Root);

            //配置Buff数据归属的NP_RuntimeTree实例
            foreach (var buffNodeDataBase in npDataSupportor.BuffDataDic)
            {
                if (buffNodeDataBase.Value is NormalBuffNodeData normalBuffNodeData)
                {
                    normalBuffNodeData.BuffData.BelongToRuntiemTree = tempTree;
                }
            }
            
            //配置黑板数据
            Dictionary<string, ANP_BBValue> bbvaluesManager = tempTree.GetBlackboard().GetDatas();
            foreach (var bbValues in npDataSupportor.NP_BBValueManager)
            {
                bbvaluesManager.Add(bbValues.Key, bbValues.Value);
            }

            return tempTree;
        }
    }
}