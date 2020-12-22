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
            self.Awake();
        }
    }
    
    [ObjectSystem]
    public class MoveComponentUpdateSystem: UpdateSystem<MoveComponent>
    {
        public override void Update(MoveComponent self)
        {
            self.Update();
        }
    }
    
    [NumericWatcher(NumericType.Speed)]
    public class SpeedSynced_MoveComponent: INumericWatcher
    {
        public void Run(long id, float value)
        {
            Unit unit = UnitComponent.Instance.Get(id);
            unit.GetComponent<MoveComponent>().CorrectMoveSpeed();
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

        public ETTaskCompletionSource moveTcs;

        private UnitPathComponent m_UnitPathComponent;

        public void Awake()
        {
            m_UnitPathComponent = this.Entity.GetComponent<UnitPathComponent>();
        }
        
        public void Update()
        {
            if (this.moveTcs == null)
            {
                return;
            }

            Unit unit = this.GetParent<Unit>();
            long timeNow = TimeHelper.Now();

            if (timeNow - this.StartTime >= this.needTime)
            {
                unit.Position = this.Target;
                ETTaskCompletionSource tcs = this.moveTcs;
                this.moveTcs = null;
                tcs.SetResult();
                return;
            }

            float amount = (timeNow - this.StartTime) * 1f / this.needTime;
            unit.Position = Vector3.Lerp(this.StartPos, this.Target, amount);
        }

        public ETTask MoveToAsync(Vector3 target, CancellationToken cancellationToken)
        {
            if ((target - this.Target).magnitude < 0.1f)
            {
                return ETTask.CompletedTask;
            }

            this.Target = target;

            CorrectMoveSpeed();

            this.moveTcs = new ETTaskCompletionSource();

            cancellationToken.Register(() =>
            {
                this.moveTcs = null;
            });
            return this.moveTcs.Task;
        }

        /// <summary>
        /// 矫正移动速度，纠正needTime
        /// </summary>
        public void CorrectMoveSpeed()
        {
            this.StartPos = this.GetParent<Unit>().Position;
            
            float speed = this.Entity.GetComponent<HeroDataComponent>().GetAttribute(NumericType.Speed) / 100;
            float distance = (this.Target - this.StartPos).magnitude;
            
            this.StartTime = TimeHelper.Now();
            this.needTime = (long) (distance / speed * 1000);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
            this.needTime = 0;
            this.Target = Vector3.zero;
            this.StartPos = Vector3.zero;
            this.m_UnitPathComponent = null;
        }
    }
}