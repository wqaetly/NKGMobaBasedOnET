using System;
using ET;
using GraphProcessor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public class SkillGraphWindow: UniversalGraphWindow
    {
        protected override void InitializeWindow(BaseGraph graph)
        {
            graphView = new NPBehaveGraphView(this);

            m_MiniMap = new MiniMap() { anchored = true };
            graphView.Add(m_MiniMap);

            m_ToolbarView = new SkillToolbarView(graphView, m_MiniMap, graph);
            graphView.Add(m_ToolbarView);

            SetCurrentBlackBoardDataManager();
        }

        private void OnFocus()
        {
            SetCurrentBlackBoardDataManager();
        }

        private void SetCurrentBlackBoardDataManager()
        {
            SkillGraph npBehaveGraph = (this.graph as SkillGraph);
            if (npBehaveGraph == null)
            {
                //因为OnFocus执行时机比较诡异，在OnEnable后，或者执行一些操作后都会执行，但这时Graph可能为空，所以做判断
                return;
            }
            NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager = (this.graph as SkillGraph).NpBlackBoardDataManager;
        }
    }
}