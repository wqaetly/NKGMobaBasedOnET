//此文件格式由工具自动生成

using System.Threading;
using UnityEngine;

namespace ET
{
    public class CommonAttackComponent_View : Entity
    {
        #region 私有成员

        public AnimationComponent m_AnimationComponent;
        public UserInputComponent m_UserInputComponent;
        public MouseTargetSelectorComponent m_MouseTargetSelectorComponent;
        public StackFsmComponent m_StackFsmComponent;

        /// <summary>
        /// 上次选中的Unit，用于自动攻击
        /// </summary>
        public Unit m_CachedUnit;

        #endregion

        #region 公有成员

        #endregion

        #region 生命周期函数

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();

            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
            m_UserInputComponent = null;
            m_StackFsmComponent = null;
            m_AnimationComponent = null;
        }

        #endregion
    }
}