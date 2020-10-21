using System.Collections.Generic;
using System.Threading;
using ETModel.NKGMOBA.Battle.Fsm;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETModel
{
    public class UnitPathComponent: Component
    {
        public List<Vector3> Path = new List<Vector3>();

        public Vector3 ServerPos;

        public ETCancellationTokenSource EtCancellationTokenSource;

        private async ETTask StartMove_Internal(CancellationToken cancellationToken)
        {
            for (int i = 0; i < this.Path.Count; ++i)
            {
                Vector3 v = this.Path[i];

                float speed = 5;

                if (i == 0)
                {
                    // 矫正移动速度
                    Vector3 clientPos = this.GetParent<Unit>().Position;
                    float serverf = (ServerPos - v).magnitude;
                    if (serverf > 0.1f)
                    {
                        float clientf = (clientPos - v).magnitude;
                        speed = clientf / serverf * speed;
                    }
                }

                this.Entity.GetComponent<TurnComponent>().Turn(v);
                await this.Entity.GetComponent<MoveComponent>().MoveToAsync(v, speed, cancellationToken);
            }

            this.Entity.GetComponent<StackFsmComponent>().ChangeState<IdleState>(StateTypes.Idle, "Idle", 1);
            this.Entity.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
        }

        /// <summary>
        /// 开始移动
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async ETVoid StartMove(M2C_PathfindingResult message)
        {
            if (!this.Entity.GetComponent<StackFsmComponent>().ChangeState<NavigateState>(StateTypes.Run, "Run", 1)) return;
            this.Entity.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
            // 取消之前的移动协程
            this.EtCancellationTokenSource?.Cancel();
            this.EtCancellationTokenSource = ComponentFactory.Create<ETCancellationTokenSource>();
            this.Path.Clear();

            for (int i = 0; i < message.Xs.Count; ++i)
            {
                this.Path.Add(new Vector3(message.Xs[i], message.Ys[i], message.Zs[i]));
            }

            ServerPos = new Vector3(message.X, message.Y, message.Z);
            await this.StartMove_Internal(this.EtCancellationTokenSource.Token);
        }

        /// <summary>
        /// 取消移动
        /// </summary>
        public void CancelMove()
        {
            this.EtCancellationTokenSource?.Cancel();
            this.Path.Clear();
            this.Entity.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            Path.Clear();
            this.ServerPos = Vector3.zero;
            this.EtCancellationTokenSource?.Cancel();
        }
    }
}