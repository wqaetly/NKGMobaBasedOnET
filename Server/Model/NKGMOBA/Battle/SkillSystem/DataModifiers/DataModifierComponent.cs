//此文件格式由工具自动生成

using System.Collections.Generic;

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class DataModifierComponentAwakeSystem: AwakeSystem<DataModifierComponent>
    {
        public override void Awake(DataModifierComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class DataModifierComponentUpdateSystem: UpdateSystem<DataModifierComponent>
    {
        public override void Update(DataModifierComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class DataModifierComponentFixedUpdateSystem: FixedUpdateSystem<DataModifierComponent>
    {
        public override void FixedUpdate(DataModifierComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class DataModifierComponentDestroySystem: DestroySystem<DataModifierComponent>
    {
        public override void Destroy(DataModifierComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    public class DataModifierComponent: Component
    {
        #region 私有成员

        /// <summary>
        /// 所有的数据修改器
        /// Key为分组名称，其中如果和NumericComponent有联系，则必须使用NumericType对应String作为Key，例如NumericType.HP对应String就是HP
        /// Value为此装饰器分组中所有的装饰器
        /// </summary>
        private Dictionary<string, List<ADataModifier>> AllModifiers = new Dictionary<string, List<ADataModifier>>();

        #endregion

        #region 公有成员

        /// <summary>
        /// 新增一个数据修改器
        /// </summary>
        /// <param name="modifierName">所归属修改器集合名称</param>
        /// <param name="dataModifier">要添加的修改器</param>
        /// <param name="numericType">如果不为Min说明需要直接去更新属性</param>
        public void AddDataModifier(string modifierName, ADataModifier dataModifier, NumericType numericType = NumericType.Min)
        {
            if (AllModifiers.TryGetValue(modifierName, out var modifiers))
            {
                modifiers.Add(dataModifier);
            }
            else
            {
                AllModifiers.Add(modifierName, new List<ADataModifier>() { dataModifier });
            }

            if (numericType == NumericType.Min)
                return;
            this.Entity.GetComponent<NumericComponent>()[numericType] =
                    this.BaptismData(modifierName, this.Entity.GetComponent<NumericComponent>().GetOriNum()[(int)numericType]);
        }

        /// <summary>
        /// 移除一个数据修改器
        /// </summary>
        /// <param name="modifierName">所归属修改器集合名称</param>
        /// <param name="dataModifier">要移除的修改器</param>
        /// <param name="numericType">如果不为Min说明需要直接去更新属性</param>
        public void RemoveDataModifier(string modifierName, ADataModifier dataModifier, NumericType numericType = NumericType.Min)
        {
            if (AllModifiers.TryGetValue(modifierName, out var modifiers))
            {
                if (modifiers.Remove(dataModifier))
                {
                    if (numericType == NumericType.Min)
                        return;
                    this.Entity.GetComponent<NumericComponent>()[numericType] =
                            this.BaptismData(modifierName, this.Entity.GetComponent<NumericComponent>().GetOriNum()[(int) numericType]);
                    return;
                }
            }

            Log.Error($"目前数据修改器集合中没有名为：{modifierName}的集合");
        }

        /// <summary>
        /// 洗礼一个数值
        /// </summary>
        /// <param name="targetModifierName">目标修改器集合名称</param>
        /// <param name="targetData">将要修改的值</param>
        public float BaptismData(string targetModifierName, float targetData)
        {
            if (AllModifiers.TryGetValue(targetModifierName, out var modifiers))
            {
                float constantValue = 0;
                float percentageValue = 0;
                foreach (var modify in modifiers)
                {
                    if (modify.ModifierType == ModifierType.Constant)
                    {
                        constantValue += modify.GetModifierValue();
                    }
                    else
                    {
                        percentageValue += modify.GetModifierValue();
                    }
                }

                targetData = (targetData + constantValue) * (1 + percentageValue);
            }

            return targetData;
        }

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
        }

        public void Update()
        {
            //此处填写Update逻辑
        }

        public void FixedUpdate()
        {
            //此处填写FixedUpdate逻辑
        }

        public void Destroy()
        {
            //此处填写Destroy逻辑
            AllModifiers.Clear();
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
            this.AllModifiers.Clear();
        }

        #endregion
    }
}