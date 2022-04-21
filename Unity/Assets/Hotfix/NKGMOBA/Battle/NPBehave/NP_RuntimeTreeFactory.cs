//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 15:06:15
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using NPBehave;
using Exception = NPBehave.Exception;

namespace ET
{
    public class NP_RuntimeTreeFactory
    {
        public static Dictionary<Type, NodeType> NPNodeRegister = new Dictionary<Type, NodeType>()
        {
            {typeof(NP_RootNodeData), NodeType.Decorator},
            {typeof(NP_ParallelNodeData), NodeType.Composite},
            {typeof(NP_SelectorNodeData), NodeType.Composite},
            {typeof(NP_SequenceNodeData), NodeType.Composite},

            {typeof(NP_BlackboardConditionNodeData), NodeType.Decorator},
            {typeof(NP_BlackboardMultipleConditionsNodeData), NodeType.Decorator},
            {typeof(NP_RepeaterNodeData), NodeType.Decorator},
            {typeof(NP_ServiceNodeData), NodeType.Decorator},

            {typeof(NP_ActionNodeData), NodeType.Task},
            {typeof(NP_WaitNodeData), NodeType.Task},
            {typeof(NP_WaitUntilStoppedData), NodeType.Task},
        };

        /// <summary>
        /// 创建一个行为树实例,默认存入Unit的NP_RuntimeTreeManager中
        /// </summary>
        /// <param name="unit">行为树所归属unit</param>
        /// <param name="nPDataId">行为树数据id</param>
        /// <returns></returns>
        public static NP_RuntimeTree CreateNpRuntimeTree(Unit unit, long nPDataId)
        {
            NP_DataSupportor npDataSupportor = unit.DomainScene().GetComponent<NP_TreeDataRepositoryComponent>()
                .GetNP_TreeData_DeepCopyBBValuesOnly(nPDataId);

            NP_RuntimeTreeManager npRuntimeTreeManager = unit.GetComponent<NP_RuntimeTreeManager>();
            long rootId = npDataSupportor.NpDataSupportorBase.NPBehaveTreeDataId;

            NP_RuntimeTree tempTree =
                npRuntimeTreeManager.AddChildWithId<NP_RuntimeTree, NP_DataSupportor, NP_SyncComponent, Unit>(
                    rootId + unit.Id, npDataSupportor,
                    unit.GetComponent<NP_SyncComponent>(), unit);


            npRuntimeTreeManager.AddTree(tempTree.Id, rootId, tempTree);

            //Log.Info($"运行时id为{theRuntimeTreeID}");
            //配置节点数据
            foreach (var nodeDateBase in npDataSupportor.NpDataSupportorBase.NP_DataSupportorDic)
            {
                switch (NPNodeRegister[nodeDateBase.Value.GetType()])
                {
                    case NodeType.Task:
                        try
                        {
                            nodeDateBase.Value.CreateTask(unit, tempTree);
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
                            nodeDateBase.Value.CreateDecoratorNode(unit, tempTree,
                                npDataSupportor.NpDataSupportorBase.NP_DataSupportorDic[nodeDateBase.Value.LinkedIds[0]]
                                    .NP_GetNode());
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
                                temp.Add(npDataSupportor.NpDataSupportorBase.NP_DataSupportorDic[linkedId]
                                    .NP_GetNode());
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
            tempTree.SetRootNode(npDataSupportor.NpDataSupportorBase.NP_DataSupportorDic[rootId].NP_GetNode() as Root);

            //配置黑板数据
            Dictionary<string, ANP_BBValue> bbvaluesManager = tempTree.GetBlackboard().GetDatas();
            foreach (var bbValues in npDataSupportor.NpDataSupportorBase.NP_BBValueManager)
            {
                bbvaluesManager.Add(bbValues.Key, bbValues.Value);
            }

            return tempTree;
        }

        /// <summary>
        /// 创建一个技能树实例,默认存入Unit的SkillCanvasManagerComponentComponent中
        /// </summary>
        /// <param name="unit">行为树所归属unit</param>
        /// <param name="nPDataId">行为树数据id</param>
        /// <param name="belongToSkillId">归属的SkillId,一般来说需要从excel表中读取</param>
        /// <returns></returns>
        public static NP_RuntimeTree CreateSkillNpRuntimeTree(Unit unit, long nPDataId, long belongToSkillId)
        {
            NP_RuntimeTree result = CreateNpRuntimeTree(unit, nPDataId);
            unit.GetComponent<SkillCanvasManagerComponent>()
                .AddSkillCanvas(belongToSkillId, result);
            return result;
        }
    }
}