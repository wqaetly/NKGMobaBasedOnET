//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月9日 20:02:37
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using ETModel;
using ETModel.BBValues;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.NPBehaveNodes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.NodeEditor.Editor.Canvas
{
    /// <summary>
    /// 行为树基类，继承此类即可自行拓展基于行为树的逻辑图
    /// 必须实现的有以下几点
    /// 1.继承NP_DataSupportorBase的数据基类
    /// 2.自动配置所有结点数据：AddAllNodeData
    /// 3.保存行为树信息为二进制文件：Save
    /// 4.自定义的的额外数据块配置
    /// 需要注意的点
    /// 要在NPBehaveNodes文件夹下面的除了NP_NodeBase之外的所有Node的Node特性的type里加上自定义的Canvas的Type，不然创建不了行为树组件
    /// 推荐的按钮样式：[Button("XXX", 25), GUIColor(0.4f, 0.8f, 1)]
    /// </summary>
    public abstract class NPBehaveCanvasBase: NodeCanvas
    {
        public override string canvasName => Name;

        [Title("本Canvas所有数据整理部分")]
        [LabelText("保存文件名"), GUIColor(0.9f, 0.7f, 1)]
        public string Name;

        [LabelText("保存路径"), GUIColor(0.1f, 0.7f, 1)]
        [FolderPath]
        public string SavePath;

        /// <summary>
        /// 黑板数据管理器
        /// </summary>
        [HideInInspector]
        public NPBehaveCanvasDataManager npBehaveCanvasDataManager;

        public NPBehaveCanvasDataManager GetCurrentCanvasDatas()
        {
            if (this.npBehaveCanvasDataManager == null)
            {
                Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(this.savePath);
                foreach (var subAsset in subAssets)
                {
                    if (subAsset is NPBehaveCanvasDataManager npBbDataManager)
                    {
                        return npBbDataManager;
                    }
                }

                this.npBehaveCanvasDataManager = CreateInstance<NPBehaveCanvasDataManager>();
                //Log.Info("新建数据仓库");
                this.npBehaveCanvasDataManager.name = "当前Canvas数据库";
                NodeEditorSaveManager.AddSubAsset(this.npBehaveCanvasDataManager, this);
            }

            return this.npBehaveCanvasDataManager;
        }

        /// <summary>
        /// 自动配置当前图所有数据（结点，黑板）
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        public virtual void AutoSetCanvasDatas(NP_DataSupportorBase npDataSupportorBase)
        {
            this.AutoSetNP_NodeData(npDataSupportorBase);
            this.AutoSetNP_BBDatas(npDataSupportorBase);
        }

        /// <summary>
        /// 保存当前所有结点信息为二进制文件
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        public virtual void Save(NP_DataSupportorBase npDataSupportorBase)
        {
            if (string.IsNullOrEmpty(SavePath) || string.IsNullOrEmpty(Name))
            {
                Log.Error("保存路径或文件名不能为空，请检查配置");
                return;
            }

            using (FileStream file = File.Create($"{SavePath}/{this.Name}.bytes"))
            {
                BsonSerializer.Serialize(new BsonBinaryWriter(file), npDataSupportorBase);
            }

            Debug.Log($"保存 {SavePath}/{this.Name}.bytes 成功");
        }

        /// <summary>
        /// 测试反序列化
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        public virtual T TestDe<T>(T npDataSupportorBase) where T : NP_DataSupportorBase
        {
            byte[] mfile = File.ReadAllBytes($"{SavePath}/{this.Name}.bytes");

            if (mfile.Length == 0) Debug.Log("没有读取到文件");

            try
            {
                npDataSupportorBase = BsonSerializer.Deserialize<T>(mfile);
                return npDataSupportorBase;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        /// <summary>
        /// 自动配置所有行为树结点
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        private void AutoSetNP_NodeData(NP_DataSupportorBase npDataSupportorBase)
        {
            npDataSupportorBase.NP_DataSupportorDic.Clear();

            //当前Canvas所有NP_Node
            List<NP_NodeBase> allNodes = new List<NP_NodeBase>();

            foreach (var node in this.nodes)
            {
                if (node is NP_NodeBase mNode)
                {
                    allNodes.Add(mNode);
                }
            }

            //排序
            allNodes.Sort((x, y) => -x.position.y.CompareTo(y.position.y));

            //配置每个节点Id
            foreach (var node in allNodes)
            {
                node.NP_GetNodeData().id = IdGenerater.GenerateId();
            }

            //设置根结点Id
            npDataSupportorBase.RootId = allNodes[allNodes.Count - 1].NP_GetNodeData().id;

            foreach (var node in allNodes)
            {
                //获取结点对应的NPData
                NP_NodeDataBase mNodeData = node.NP_GetNodeData();
                if (mNodeData.LinkedIds == null)
                {
                    mNodeData.LinkedIds = new List<long>();
                }
                mNodeData.LinkedIds.Clear();

                //出结点连接的Nodes
                List<NP_NodeBase> theNodesConnectedToOutNode = new List<NP_NodeBase>();

                List<ValueConnectionKnob> valueConnectionKnobs = node.GetNextNodes()?.connections;

                if (valueConnectionKnobs != null)
                {
                    foreach (var valueConnectionKnob in valueConnectionKnobs)
                    {
                        theNodesConnectedToOutNode.Add((NP_NodeBase) valueConnectionKnob.body);
                    }

                    //对所连接的节点们进行排序
                    theNodesConnectedToOutNode.Sort((x, y) => x.position.x.CompareTo(y.position.x));

                    //配置连接的Id，运行时实时构建行为树
                    foreach (var npNodeBase in theNodesConnectedToOutNode)
                    {
                        mNodeData.LinkedIds.Add(npNodeBase.NP_GetNodeData().id);
                    }
                }

                //将此结点数据写入字典
                npDataSupportorBase.NP_DataSupportorDic.Add(mNodeData.id, mNodeData);
            }
        }

        /// <summary>
        /// 自动配置黑板数据
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        private void AutoSetNP_BBDatas(NP_DataSupportorBase npDataSupportorBase)
        {
            npDataSupportorBase.NP_BBValueManager.Clear();
            //设置黑板数据
            foreach (var bbvalues in this.GetCurrentCanvasDatas().BBValues)
            {
                npDataSupportorBase.NP_BBValueManager.Add(bbvalues.Key, bbvalues.Value);
            }
        }
    }
}