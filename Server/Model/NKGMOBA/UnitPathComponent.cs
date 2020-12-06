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

        public List<Vector3> Path;

        public CancellationTokenSource CancellationTokenSource;

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

        public void CancelMove()
        {
            CancellationTokenSource?.Cancel();
            this.Entity.GetComponent<StackFsmComponent>().RemoveState("Navigate");
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
        }
    }
}