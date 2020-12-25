//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月21日 18:40:49
//------------------------------------------------------------

using ETHotfix.NKGMOBA.Factory;
using ETModel;
using UnityEngine;

namespace ETHotfix.NKGMOBA.Battle
{
    [NumericWatcher(NumericType.Hp)]
    public class ChangeHP: INumericWatcher
    {
        public void Run(long id, float value)
        {
            MessageHelper.Broadcast(new M2C_SyncUnitAttribute() { UnitId = id, NumericType = (int) NumericType.Hp, FinalValue = value });
        }
    }

    [NumericWatcher(NumericType.Mp)]
    public class ChangeMP: INumericWatcher
    {
        public void Run(long id, float value)
        {
            MessageHelper.Broadcast(new M2C_SyncUnitAttribute() { UnitId = id, NumericType = (int) NumericType.Mp, FinalValue = value });
        }
    }

    [NumericWatcher(NumericType.AttackAdd)]
    public class ChangeAttackAdd: INumericWatcher
    {
        public void Run(long id, float value)
        {
            MessageHelper.Broadcast(new M2C_SyncUnitAttribute() { UnitId = id, NumericType = (int) NumericType.AttackAdd, FinalValue = value });
        }
    }

    [NumericWatcher(NumericType.Attack)]
    public class ChangeAttack: INumericWatcher
    {
        public void Run(long id, float value)
        {
            MessageHelper.Broadcast(new M2C_SyncUnitAttribute() { UnitId = id, NumericType = (int) NumericType.Attack, FinalValue = value });
        }
    }
    
    [NumericWatcher(NumericType.Speed)]
    public class ChangeSpeed: INumericWatcher
    {
        public void Run(long id, float value)
        {
            MessageHelper.Broadcast(new M2C_SyncUnitAttribute() { UnitId = id, NumericType = (int) NumericType.Speed, FinalValue = value });
        }
    }

    [Event(EventIdType.NumericApplyChangeValue)]
    public class SendDamageInfoToClient: AEvent<long, NumericType, float>
    {
        public override void Run(long unitId, NumericType numberType, float changedValue)
        {
            MessageHelper.Broadcast(new M2C_ChangeUnitAttribute() { UnitId = unitId, NumericType = (int) numberType, ChangeValue = changedValue });
        }
    }

    /// <summary>
    /// 向客户端发送事件，一般为特效表现
    /// long:来自Unit的ID
    /// long:归属Unit的ID
    /// string：要传给客户端的事件ID
    /// </summary>
    [Event(EventIdType.SendBuffInfoToClient)]
    public class SendBuffInfoToClient: AEvent<M2C_BuffInfo>
    {
        public override void Run(M2C_BuffInfo c)
        {
            MessageHelper.Broadcast(c);
        }
    }
    
    /// <summary>
    /// 向客户端发送黑板bool类型值
    /// </summary>
    [Event(EventIdType.SendNPBBValue_BoolToClient)]
    public class SendNPBBValue_BoolToClient: AEvent<M2C_SyncNPBehaveBoolData>
    {
        public override void Run(M2C_SyncNPBehaveBoolData a)
        {
            MessageHelper.Broadcast(a);
        }
    }

    [Event(EventIdType.MoveToRandomPos)]
    public class UnitPathComponentInvoke: AEvent<long, Vector3>
    {
        public override void Run(long a, Vector3 b)
        {
            UnitComponent.Instance.Get(a).GetComponent<UnitPathComponent>().CommonNavigate(b);
        }
    }
    
    [Event(EventIdType.RemoveCollider)]
    public class RemoveCollider: AEvent<long>
    {
        public override void Run(long a)
        {
            UnitFactory.RemoveColliderUnit(a);
        }
    }
}