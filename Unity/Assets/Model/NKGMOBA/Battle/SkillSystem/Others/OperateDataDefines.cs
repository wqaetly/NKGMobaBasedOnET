//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月13日 12:57:29
//------------------------------------------------------------

namespace ET
{
    public abstract class OperateData: IReference
    {
        /// <summary>
        /// 操作发起者
        /// </summary>
        public Unit OperateCaster;

        /// <summary>
        /// 操作承受者
        /// </summary>
        public Unit OperateTaker;

        public virtual void Clear()
        {
            OperateCaster = null;
            OperateTaker = null;
        }
    }

    /// <summary>
    /// 伤害数据定义
    /// </summary>
    public class DamageData: OperateData
    {
        public BuffDamageTypes BuffDamageTypes;
        public float DamageValue;

        /// <summary>
        /// 自定义数据标记，比如标识此伤害来自于W技能
        /// </summary>
        public string CustomData;

        public DamageData InitData(BuffDamageTypes buffDamageTypes, float damageValue, Unit attackCaster, Unit attackReceiver,
        string customData = null)
        {
            BuffDamageTypes = buffDamageTypes;
            DamageValue = damageValue;
            this.OperateCaster = attackCaster;
            this.OperateTaker = attackReceiver;
            CustomData = customData;
            return this;
        }

        public override void Clear()
        {
            base.Clear();
            BuffDamageTypes = BuffDamageTypes.None;
            DamageValue = 0;
            CustomData = null;
        }
    }
}