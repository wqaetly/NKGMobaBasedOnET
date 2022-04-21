//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:14:44
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using NPBehave;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [BsonDeserializerRegister]
    public abstract class NP_NodeDataBase
    {
        /// <summary>
        /// 此结点ID
        /// </summary>
        [LabelText("此结点ID")]
        [HideInEditorMode]
        public long id;

        /// <summary>
        /// 与此结点相连的ID
        /// </summary>
        [HideInEditorMode]
        public List<long> LinkedIds = new List<long>();

        [BoxGroup("结点信息描述")]
        [TextArea(2, 2)]
        [HideLabel]
        [BsonIgnore]
        public string NodeDes;

        /// <summary>
        /// 获取结点
        /// </summary>
        /// <returns></returns>
        public abstract Node NP_GetNode();

        /// <summary>
        /// 创建组合结点
        /// </summary>
        /// <returns></returns>
        public virtual Composite CreateComposite(Node[] nodes)
        {
            return null;
        }

        /// <summary>
        /// 创建装饰结点
        /// </summary>
        /// <param name="unitId">行为树归属的Unit</param>
        /// <param name="runtimeTree">运行时归属的行为树</param>
        /// <param name="node">所装饰的结点</param>
        /// <returns></returns>
        public virtual Decorator CreateDecoratorNode(Unit unit, NP_RuntimeTree runtimeTree, Node node)
        {
            return null;
        }

        /// <summary>
        /// 创建任务节点
        /// </summary>
        /// <param name="unitId">行为树归属的Unit</param>
        /// <param name="runtimeTree">运行时归属的行为树</param>
        /// <returns></returns>
        public virtual Task CreateTask(Unit unit, NP_RuntimeTree runtimeTree)
        {
            return null;
        }
    }
}