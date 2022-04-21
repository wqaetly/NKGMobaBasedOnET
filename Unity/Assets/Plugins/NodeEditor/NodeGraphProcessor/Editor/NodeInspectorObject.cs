using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Object = UnityEngine.Object;

namespace GraphProcessor
{
    // /// <summary>
    // /// Custom editor of the node inspector, you can inherit from this class to customize your node inspector.
    // /// </summary>
    // [CustomEditor(typeof(NodeInspectorObject))]
    // public class NodeInspectorObjectEditor : Editor
    // {
    //     NodeInspectorObject inspector;
    //     protected VisualElement root;
    //     protected VisualElement selectedNodeList;
    //     protected VisualElement placeholder;
    //
    //     Dictionary<BaseNode, VisualElement> nodeInspectorCache = new Dictionary<BaseNode, VisualElement>();
    //
    //     protected virtual void OnEnable()
    //     {
    //         inspector = target as NodeInspectorObject;
    //         inspector.nodeSelectionUpdated += UpdateNodeInspectorList;
    //         root = new VisualElement();
    //         selectedNodeList = new VisualElement();
    //         selectedNodeList.styleSheets.Add(Resources.Load<StyleSheet>("GraphProcessorStyles/InspectorView"));
    //         root.Add(selectedNodeList);
    //         placeholder = new Label("Select a node to show it's settings in the inspector");
    //         placeholder.AddToClassList("PlaceHolder");
    //         UpdateNodeInspectorList();
    //     }
    //
    //     protected virtual void OnDisable()
    //     {
    //         inspector.nodeSelectionUpdated -= UpdateNodeInspectorList;
    //     }
    //
    //     public override VisualElement CreateInspectorGUI() => root;
    //
    //     protected virtual void UpdateNodeInspectorList()
    //     {
    //         selectedNodeList.Clear();
    //
    //         if (inspector.selectedNodes.Count == 0)
    //             selectedNodeList.Add(placeholder);
    //
    //         foreach (var nodeView in inspector.selectedNodes)
    //             selectedNodeList.Add(CreateNodeBlock(nodeView));
    //     }
    //
    //     protected VisualElement CreateNodeBlock(BaseNodeView nodeView)
    //     {
    //         var view = new VisualElement();
    //
    //         view.Add(new Label(nodeView.nodeTarget.name));
    //
    //         var tmp = nodeView.controlsContainer;
    //         nodeView.controlsContainer = view;
    //         nodeView.Enable(true);
    //         nodeView.controlsContainer.AddToClassList("NodeControls");
    //         var block = nodeView.controlsContainer;
    //         nodeView.controlsContainer = tmp;
    //         
    //         return block;
    //     }
    // }

    /// <summary>
    /// Node inspector object, you can inherit from this class to customize your node inspector.
    /// </summary>
    public class NodeInspectorObject : SerializedScriptableObject
    {
        /// <summary>List of currently selected nodes</summary>
        public List<BaseNodeView> selectedNodes = new List<BaseNodeView>();

        public virtual void NodeViewRemoved(BaseNodeView view)
        {
            selectedNodes.Remove(view);
        }
    }

    /// <summary>
    /// 手动清理NodeInspectorObject，防止出现编辑已经非法的数据，因为NodeView是每次重新编译/PlayMode后重新构建的
    /// 所以如果这里不做清理在重新编译/PlayMode后编辑的就是已经失效的数据
    /// </summary>
    public class ResetSelectNodeInfo : Editor
    {
        [InitializeOnLoadMethod]
        public static void _ResetSelectNodeInfo()
        {
            if (Selection.activeObject is NodeInspectorObject nodeInspectorObject)
            {
                nodeInspectorObject.selectedNodes.Clear();
            }
        }
    }
}