using System;
using MongoDB.Bson.Serialization.Attributes;
using UnityEngine;

namespace ET
{
    public class  UnitDestroySystem: DestroySystem<Unit>
    {
        public override void Destroy(Unit self)
        {
            self.ConfigId = 0;
            self.BelongToPlayer = null;
            self.NeedSyncToClient = false;
        }
    }
    

    [BsonIgnoreExtraElements]
    public sealed class Unit: Entity
    {
        public int ConfigId; //配置表id

        /// <summary>
        /// 归属的玩家
        /// </summary>
        public Player BelongToPlayer;

        /// <summary>
        /// 归属的房间
        /// </summary>
        public Room BelongToRoom;

        public LSF_Component LsfComponent => BelongToRoom.GetComponent<LSF_Component>();

        /// <summary>
        /// 是否需要同步到客户端
        /// </summary>
        public bool NeedSyncToClient = false;

        private Vector3 position; //坐标

        public Vector3 Position
        {
            get => this.position;
            set
            {
                this.position = value;
                Game.EventSystem.Publish(new EventType.ChangePosition() { Unit = this }).Coroutine();
            }
        }

        [BsonIgnore]
        public Vector3 Forward
        {
            get => this.Rotation * Vector3.forward;
            set => this.Rotation = Quaternion.LookRotation(value, Vector3.up);
        }

        private Quaternion rotation;
        public Quaternion Rotation
        {
            get => this.rotation;
            set
            {
                this.rotation = value;
                Game.EventSystem.Publish(new EventType.ChangeRotation() {Unit = this}).Coroutine();
            }
        }
    }
}