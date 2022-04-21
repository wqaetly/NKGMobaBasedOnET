//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月15日 13:12:44
//------------------------------------------------------------

using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 连接ET和Mono的桥梁
    /// </summary>
    public class MonoBridge: MonoBehaviour
    {
        /// <summary>
        /// 自定义Tag
        /// </summary>
        [LabelText("自定义Tag")]
        [InfoBox("作用同GameObject.Tag")]
        public string CustomTag;

        /// <summary>
        /// 归属UnitId
        /// </summary>
        public long BelongToUnitId;

        private void OnCollisionEnter2D(Collision2D other)
        {
        }

        private void OnCollisionStay(Collision other)
        {
        }

        private void OnCollisionExit(Collision other)
        {
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
        }

        private void OnTriggerStay(Collider other)
        {
        }

        private void OnTriggerExit(Collider other)
        {
        }

#if UNITY_EDITOR
        private string m_GameObjectTag;

        [ShowIf("CustomTagNotEqualGameObjectName")]
        [InfoBox("注意，当前自定义Tag与游戏物体名称不一致", InfoMessageType.Warning)]
        [Button("点击此按钮可重置自定义Tag"), GUIColor(0.5f, 0.4f, 0.8f)]
        public void ResetCustomTagEqualGameObjectName()
        {
            Reset();
        }

        public bool CustomTagNotEqualGameObjectName()
        {
            return CustomTag == m_GameObjectTag? false : true;
        }

        private void Reset()
        {
            CustomTag = this.gameObject.name;
            m_GameObjectTag = this.gameObject.name;
        }
#endif
    }
}