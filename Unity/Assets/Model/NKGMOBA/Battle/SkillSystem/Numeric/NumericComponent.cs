using System;
using System.Collections.Generic;

namespace ETModel
{
    public class NumericComponent: Component
    {
        public Dictionary<int, float> NumericDic = new Dictionary<int, float>();

        public Dictionary<int, float> OriNumericDic;

        /// <summary>
        /// 初始化初始值字典，用于值回退，比如一个buff加50ad，buff移除后要减去这个50ad，就需要用到OriNumericDic里的值
        /// </summary>
        public void InitOriNumerDic()
        {
            OriNumericDic = new Dictionary<int, float>(NumericDic);
        }

        public float this[NumericType numericType]
        {
            get
            {
                return this.GetByKey((int) numericType);
            }
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

        public Dictionary<int, float> GetOriNum()
        {
            return this.OriNumericDic;
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

            //如果不是直接操作最终值，需要发送两次事件，一次是修改的值，一次是最终值
            if (numericType > NumericType.Min)
            {
                final = (int) numericType / 10;
                int bas = final * 10 + 1;
                int add = final * 10 + 2;

                //取得最终值，由基础xxx+额外xxx值组成
                float finalResult = this.GetByKey(bas) + this.GetByKey(add);
                //更新最终值
                this[(NumericType) final] = this.Entity.GetComponent<DataModifierComponent>().BaptismData(numericType.ToString(), finalResult);
            }

            //将改变的值以事件的形式发送出去
            Game.EventSystem.Run(EventIdType.NumbericChange, this.Entity.Id, numericType, result);
        }
    }
}