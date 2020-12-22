using System.Collections.Generic;
using System.Threading;
using ETModel.NKGMOBA.Battle.Fsm;
using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETModel
{
    [NumericWatcher(NumericType.Speed)]
    public class SpeedSynced_UnitPathComponent: INumericWatcher
    {
        public void Run(long id, float value)
        {
            Unit unit = UnitComponent.Instance.Get(id);
            unit.GetComponent<UnitPathComponent>().PlayRunAnimationByMoveSpeed();
        }
    }

    public class UnitPathComponent: Component
    {
        public List<Vector3> Path = new List<Vector3>();

        /// <summary>
        /// Unit在服务器上的位置
        /// </summary>
        public Vector3 UnitPosInServer;

        public ETCancellationTokenSource EtCancellationTokenSource;

        /// <summary>
        /// 开始移动
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async ETVoid StartMove(M2C_PathfindingResult message)
        {
            if (!this.Entity.GetComponent<StackFsmComponent>().ChangeState<NavigateState>(StateTypes.Run, "Navigate", 1)) return;
            PlayRunAnimationByMoveSpeed();
            
            // 取消之前的移动协程
            this.EtCancellationTokenSource?.Cancel();
            this.EtCancellationTokenSource = ComponentFactory.Create<ETCancellationTokenSource>();
            this.Path.Clear();

            for (int i = 0; i < message.Xs.Count; ++i)
            {
                this.Path.Add(new Vector3(message.Xs[i], message.Ys[i], message.Zs[i]));
            }

            this.UnitPosInServer = new Vector3(message.X, message.Y, message.Z);
            await this.StartMove_Internal(this.EtCancellationTokenSource.Token);
            this.EtCancellationTokenSource.Dispose();
            this.EtCancellationTokenSource = null;
            this.Entity.GetComponent<StackFsmComponent>().RemoveState("Navigate");
            this.Entity.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
        }

        private async ETTask StartMove_Internal(CancellationToken cancellationToken)
        {
            for (int i = 0; i < this.Path.Count; ++i)
            {
                Vector3 v = this.Path[i];
                this.Entity.GetComponent<TurnComponent>().Turn(v);
                await this.Entity.GetComponent<MoveComponent>().MoveToAsync(v, cancellationToken);
            }
        }

        /// <summary>
        /// 根据移速矫正动画播放
        /// </summary>
        public void PlayRunAnimationByMoveSpeed()
        {
            if (this.Entity.GetComponent<StackFsmComponent>().GetCurrentFsmState().StateTypes != StateTypes.Run)
            {
                return;
            }

            HeroDataComponent heroDataComponent = this.Entity.GetComponent<HeroDataComponent>();
            float animSpeed = heroDataComponent.GetAttribute(NumericType.Speed) / heroDataComponent.GetAttribute(NumericType.SpeedBase);
            this.Entity.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent(0.3f, animSpeed);
        }

        /// <summary>
        /// 取消移动
        /// </summary>
        public void CancelMove()
        {
            this.EtCancellationTokenSource?.Cancel();
            this.Path.Clear();
            this.Entity.GetComponent<StackFsmComponent>().RemoveState("Navigate");
            this.Entity.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            Path.Clear();
            this.UnitPosInServer = Vector3.zero;
            this.EtCancellationTokenSource?.Cancel();
            //TODO Entity相关的操作放到DestroySystem中
            // this.Entity.GetComponent<StackFsmComponent>().RemoveState("Navigate");
            // this.Entity.GetComponent<AnimationComponent>().PlayAnimByStackFsmCurrent();
        }
    }
}