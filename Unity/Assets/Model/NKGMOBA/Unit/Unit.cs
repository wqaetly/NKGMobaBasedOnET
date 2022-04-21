using System;
using MongoDB.Bson.Serialization.Attributes;
using UnityEngine;

namespace ET
{
    [BsonIgnoreExtraElements]
    public sealed class Unit : Entity
    {
        public int ConfigId; //配置表id

        /// <summary>
        /// 归属的房间
        /// </summary>
        public Room BelongToRoom;

        public LSF_Component LsfComponent => BelongToRoom.GetComponent<LSF_Component>();
        private Vector3 position; //坐标

        public Vector3 Position
        {
            get => this.position;
            set { this.position = value; }
        }

        [BsonIgnore]
        public Vector3 Forward
        {
            get => this.Rotation * Vector3.forward;
            set => this.Rotation = Quaternion.LookRotation(value, Vector3.up);
        }

        private Quaternion rotation = Quaternion.identity;

        public Quaternion Rotation
        {
            get => this.rotation;
            set { this.rotation = value; }
        }

#if !SERVER

        private Vector3 viewPosition; //坐标

        public Vector3 ViewPosition
        {
            get => this.viewPosition;
            set
            {
                this.viewPosition = value;
                Game.EventSystem.Publish(new EventType.ChangePosition() {Unit = this}).Coroutine();
            }
        }

        private Quaternion viewRotation = Quaternion.identity;

        public Quaternion ViewRotation
        {
            get => this.viewRotation;
            set
            {
                this.viewRotation = value;
                Game.EventSystem.Publish(new EventType.ChangeRotation() {Unit = this}).Coroutine();
            }
        }

#endif
    }
}