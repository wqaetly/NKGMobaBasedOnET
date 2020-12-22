using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class MoveComponentAwakeSystem: AwakeSystem<MoveComponent>
    {
        public override void Awake(MoveComponent self)
        {
            self.Speed = self.Entity.GetComponent<HeroDataComponent>().GetAttribute(NumericType.Speed) / 100;
        }
    }
    
    public class MoveComponent: Component
    {
        public Vector3 Target;

        // 开启移动协程的时间
        public long StartTime;

        // 开启移动协程的Unit的位置
        public Vector3 StartPos;

        public long needTime;

        // 当前的移动速度
        public float Speed;
        
        public async ETTask MoveToAsync(Vector3 target, CancellationToken cancellationToken)
        {
            // 新目标点离旧目标点太近，不设置新的
            if ((target - this.Target).sqrMagnitude < 0.0001f)
            {
                return;
            }

            // 距离当前位置太近
            if ((this.GetParent<Unit>().Position - target).sqrMagnitude < 0.0001f)
            {
                return;
            }

            this.Target = target;
            Unit unit = this.GetParent<Unit>();
            unit.Rotation = Quaternion.LookRotation(target - unit.Position, Vector3.up);//同步旋转信息
            //Log.Info($"当前旋转值为{Quaternion.QuaternionToEuler(unit.Rotation)}");
            
            // 开启协程移动,每100毫秒移动一次，并且协程取消的时候会计算玩家真实移动
            // 比方说玩家移动了250毫秒,玩家有新的目标,这时旧的移动协程结束,将计算250毫秒移动的位置，而不是300毫秒移动的位置
            this.StartPos = unit.Position;
            this.StartTime = TimeHelper.Now();
            float distance = (this.Target - this.StartPos).magnitude;
            if (Math.Abs(distance) < 0.0001f)
            {
                return;
            }

            this.needTime = (long) (distance / this.Speed * 1000);
            // 协程如果取消，将算出玩家的真实位置，赋值给玩家
            cancellationToken.Register(() =>
            {
                long timeNow = TimeHelper.Now();
                if (timeNow - this.StartTime >= this.needTime)
                {
                    unit.Position = this.Target;
                }
                else
                {
                    float amount = (timeNow - this.StartTime) * 1f / this.needTime;
                    unit.Position = Vector3.Lerp(this.StartPos, this.Target, amount);
                }
            });

            while (true)
            {
                await TimerComponent.Instance.WaitAsync(10, cancellationToken);
                this.needTime = (long) (distance / this.Speed * 1000);
                
                long timeNow = TimeHelper.Now();

                if (timeNow - this.StartTime >= this.needTime)
                {
                    unit.Position = this.Target;
                    break;
                }

                float amount = (timeNow - this.StartTime) * 1f / this.needTime;
                unit.Position = Vector3.Lerp(this.StartPos, this.Target, amount);
            }
        }

        public override void Dispose()
        {
            if(this.IsDisposed)
                return;
            base.Dispose();
            this.needTime = 0;
            this.Speed = 0;
            this.Target = Vector3.zero;
            this.StartPos = Vector3.zero;
            this.StartTime = 0;
        }
    }
}