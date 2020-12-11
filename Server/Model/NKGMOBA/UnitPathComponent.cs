using System.Collections.Generic;
using System.Threading;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETModel
{
    public class UnitPathComponent: Component
    {
        public Vector3 Target;

        private RecastPath recastPath;

        public CancellationTokenSource CancellationTokenSource;

        /// <summary>
        /// 目标范围，当自身与目标位置小于等于此范围时，则停止寻路，进入NextState
        /// </summary>
        public float TargetRange;
        
        /// <summary>
        /// 绑定的状态
        /// </summary>
        public AFsmStateBase BindState;

        /// <summary>
        /// 寻路完成后会转移到的状态
        /// </summary>
        public AFsmStateBase NextState;

        public RecastPath RecastPath
        {
            get
            {
                return this.recastPath;
            }
            set
            {
                if (recastPath != null)
                    ReferencePool.Release(recastPath);
                this.recastPath = value;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            if (recastPath != null)
                ReferencePool.Release(recastPath);
            
            CancellationTokenSource?.Cancel();
            CancellationTokenSource = null;
            if (this.BindState != null)
            {
                this.Entity.GetComponent<StackFsmComponent>().RemoveState(this.BindState.StateName);
                this.BindState = null;
            }

            if (this.NextState != null)
            {
                ReferencePool.Release(NextState);
                this.NextState = null;
            }
        }
    }
}