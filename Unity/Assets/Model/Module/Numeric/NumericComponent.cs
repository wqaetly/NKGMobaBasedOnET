using System;
using System.Collections.Generic;
using ET;

namespace ET
{
    public class NumericComponentDestroySystem : DestroySystem<NumericComponent>
    {
        public override void Destroy(NumericComponent self)
        {
            self.NumericDic.Clear();
            self.OriNumericDic.Clear();
        }
    }

    public class NumericComponent : Entity
    {
#if SERVER 
        /// <summary>
        /// 每帧Attribute的结果
        /// </summary>
        public Dictionary<uint, Dictionary<int, float>> AttributeReusltFrameSnap =
            new Dictionary<uint, Dictionary<int, float>>();
        
        /// <summary>
        /// 每帧变化量
        /// </summary>
        public Dictionary<uint, Dictionary<int, float>> AttributeChangeFrameSnap =
            new Dictionary<uint, Dictionary<int, float>>();  
#endif

        public Dictionary<int, float> NumericDic = new Dictionary<int, float>();

        public Dictionary<int, float> OriNumericDic = new Dictionary<int, float>();

        public void SetValueWithoutBroadCast(NumericType numericType, float value)
        {
            NumericDic[(int) numericType] = value;
        }
        
        public float this[NumericType numericType]
        {
            get { return this.GetByKey((int) numericType); }
            set
            {
                float v = this.GetByKey((int) numericType);
                if (Math.Abs(v - value) <= 0.00001f)
                {
                    return;
                }

                NumericDic[(int) numericType] = value;
                Update(numericType);
            }
        }

        private float GetByKey(int key)
        {
            float value = 0;
            this.NumericDic.TryGetValue(key, out value);
            return value;
        }

        public void Update(NumericType numericType)
        {
            int final = (int) numericType;
            float result = this.NumericDic[final];
            
#if SERVER
            uint currentFrame = this.GetParent<Unit>().BelongToRoom.GetComponent<LSF_Component>().CurrentFrame;
            this.AttributeReusltFrameSnap[currentFrame][(int)numericType] = result;
#endif
            
            //如果不是直接操作最终值，需要发送两次事件，一次是修改的值，一次是最终值
            if (numericType > NumericType.Min)
            {
                final = (int) numericType / 10;
                int bas = final * 10 + 1;
                int add = final * 10 + 2;

                //取得最终值，由基础xxx+额外xxx值组成
                float finalResult = this.GetByKey(bas) + this.GetByKey(add);
                //更新最终值
                this[(NumericType) final] = this.Parent.GetComponent<DataModifierComponent>()
                    .BaptismData(numericType.ToString(), finalResult);
            }

            //将改变的值以事件的形式发送出去
            Game.EventSystem.Publish(new EventType.NumericChange()
                {NumericComponent = this, NumericType = numericType, Result = result}).Coroutine();
        }

        public Dictionary<int, float> GetOriNum()
        {
            return this.OriNumericDic;
        }
    }
}