//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:39:43
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    public abstract class ABuffSystemBase
    {
        /// <summary>
        /// Buff当前状态
        /// </summary>
        public BuffState BuffState;

        /// <summary>
        /// 当前叠加数
        /// </summary>
        public int CurrentOverlay;

        /// <summary>
        /// 最多持续到什么时候
        /// </summary>
        public long MaxLimitTime;
        
        /// <summary>
        /// Buff数据
        /// </summary>
        public BuffDataBase BuffData;

        /// <summary>
        /// 来自哪个Unit
        /// </summary>
        [DisableInEditorMode]
        public Unit TheUnitFrom;

        /// <summary>
        /// 寄生于哪个Unit
        /// </summary>
        [DisableInEditorMode]
        public Unit TheUnitBelongto;
        
        /// <summary>
        /// 初始化buff数据
        /// </summary>
        /// <param name="buffData">Buff数据</param>
        /// <param name="theUnitFrom">来自哪个Unit</param>
        /// <param name="theUnitBelongto">寄生于哪个Unit</param>
        public abstract void OnInit(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto);

        /// <summary>
        /// Buff触发
        /// </summary>
        public abstract void OnExecute();

        /// <summary>
        /// Buff持续
        /// </summary>
        public virtual void OnUpdate()
        {
        }

        /// <summary>
        /// 重置Buff用
        /// </summary>
        public abstract void OnFinished();

        /// <summary>
        /// 刷新，用于刷新Buff状态
        /// </summary>
        public virtual void OnRefresh()
        {
            
        }
    }
}