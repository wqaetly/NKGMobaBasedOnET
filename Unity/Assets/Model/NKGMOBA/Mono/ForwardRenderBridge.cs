//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月12日 18:39:13
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ETModel
{
    public class ForwardRenderBridge: MonoSingleton<ForwardRenderBridge>
    {
        public ForwardRendererData CurrentForwardRendererData;

        private Dictionary<string, ScriptableRendererFeature> m_ScriptableRendererFeatures = new Dictionary<string, ScriptableRendererFeature>();

        public void Init()
        {
            foreach (var scriptableRendererFeature in CurrentForwardRendererData.rendererFeatures)
            {
                m_ScriptableRendererFeatures.Add(scriptableRendererFeature.name, scriptableRendererFeature);
            }
        }

        public void SetCurrentForwardRendererData(ForwardRendererData forwardRendererData)
        {
            this.CurrentForwardRendererData = forwardRendererData;
            this.Init();
        }

        public void SetScriptableRendererFeatureState(string renderFeatureName,bool state)
        {
            if (m_ScriptableRendererFeatures.TryGetValue(renderFeatureName, out var forwardRendererData))
            {
                forwardRendererData.SetActive(state);
            }
            else
            {
                Log.Error($"未找到名为{renderFeatureName}的RenderFeature");
            }
        }
    }
}