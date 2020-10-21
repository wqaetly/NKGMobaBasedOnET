using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public class UnitPathComponent: Component
    {
        public Vector3 Target;

        private ABPathWrap abPath;
        
        public List<Vector3> Path;

        public ETCancellationTokenSource ETCancellationTokenSource;

        public ABPathWrap ABPath
        {
            get
            {
                return this.abPath;
            }
            set
            {
                this.abPath?.Dispose();
                this.abPath = value;
            }
        }

        public void CancelMove()
        {
            ETCancellationTokenSource?.Cancel();;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
            
            this.abPath?.Dispose();
        }
    }
}