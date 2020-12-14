//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月13日 12:57:29
//------------------------------------------------------------

namespace ETModel
{
    public class DamageData: IReference
    {
        public BuffDamageTypes BuffDamageTypes;
        public float DamageValue;
        public Unit AttackCaster;
        public Unit AttackReceiver;

        public DamageData InitData(BuffDamageTypes buffDamageTypes,float damageValue,Unit attackCaster,Unit attackReceiver)
        {
            BuffDamageTypes = buffDamageTypes;
            DamageValue = damageValue;
            AttackCaster = attackCaster;
            AttackReceiver = attackReceiver;
            return this;
        }
        
        public void Clear()
        {
            BuffDamageTypes = BuffDamageTypes.None;
            DamageValue = 0;
            AttackCaster = null;
            AttackReceiver = null;
        }
    }
}