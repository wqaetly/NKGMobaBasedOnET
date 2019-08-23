//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 15:06:15
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;

namespace ETModel
{
    public class NP_RuntimeTreeFactory
    {
        /// <summary>
        /// 创建一个行为树实例
        /// </summary>
        /// <param name="id">行为树id</param>
        /// <returns></returns>
        public static NP_RuntimeTree CreateNpRuntimeTree(long id)
        {
            NP_DataSupportor npDataSupportor = Game.Scene.GetComponent<NP_RuntimeTreeRepository>().GetNPRuntimeTree(id);

            //先要把叶子结点都实例化好，这是基础
            foreach (var VARIABLE in npDataSupportor.mNP_DataSupportorDic)
            {
                if (VARIABLE.Value.NodeType == NodeType.Task)
                {
                    VARIABLE.Value.CreateTask();
                }
            }

            //然后开始处理非叶子结点
            foreach (var VARIABLE in npDataSupportor.mNP_DataSupportorDic)
            {
                switch (VARIABLE.Value.NodeType)
                {
                    case NodeType.Decorator:
                        VARIABLE.Value.CreateDecoratorNode(npDataSupportor.mNP_DataSupportorDic[VARIABLE.Value.linkedID[0]].NP_GetNode());
                        break;
                    case NodeType.Composite:
                        List<Node> temp = new List<Node>();
                        foreach (var VARIABLE1 in VARIABLE.Value.linkedID)
                        {
                            temp.Add(npDataSupportor.mNP_DataSupportorDic[VARIABLE1].NP_GetNode());
                        }

                        VARIABLE.Value.CreateComposite(temp.ToArray());
                        break;
                }
            }

            return ComponentFactory.Create<NP_RuntimeTree, Root>((Root) npDataSupportor.mNP_DataSupportorDic[npDataSupportor.RootId].NP_GetNode());
        }
    }
}