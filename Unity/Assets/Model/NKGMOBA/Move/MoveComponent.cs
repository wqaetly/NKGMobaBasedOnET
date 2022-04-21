using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class MoveComponent : Entity
    {
        public Vector3 PreTarget
        {
            get { return this.Targets[this.NextPointIndex - 1]; }
        }

        public Vector3 NextTarget
        {
            get { return this.Targets[this.NextPointIndex]; }
        }

        // 开启一次移动协程的时间点
        public long BeginTime;

        /// <summary>
        /// 两点之间寻路的累加计时器
        /// </summary>
        public long AccumulateTime;

        /// <summary>
        /// 目标的范围
        /// </summary>
        public float TargetRange = 0;

        // 每个点的开始移动的时间点
        public long StartTime { get; set; }

        // 开启移动时的Unit的位置
        public Vector3 StartPos;

        private long needTime;

        public long NeedTime
        {
            get { return this.needTime; }
            set { this.needTime = value; }
        }

        public float Speed; // m/s

        public Action<bool> Callback;

        public List<Vector3> Targets = new List<Vector3>();

        /// <summary>
        /// 维护一个历史运动轨迹，一旦发现模拟结果同服务器不一致就进行回滚，也用于服务端的脏数据检查
        /// </summary>
        /// <returns></returns>
        public Dictionary<uint, LSF_MoveCmd> HistroyMoveStates = new Dictionary<uint, LSF_MoveCmd>();

        public Vector3 FinalTarget
        {
            get
            {
                if (this.Targets.Count > 0)
                {
                    return this.Targets[this.Targets.Count - 1];
                }

                return Vector3.zero;
            }
        }

        public bool ShouldMove = false;
        public bool StartMoveCurrentFrame = false;

        /// <summary>
        /// 下一个路径点的索引值
        /// </summary>
        public int NextPointIndex;

        public int TurnTime;

        public bool IsTurnHorizontal;

        public Quaternion From;

        public Quaternion To;
    }
}