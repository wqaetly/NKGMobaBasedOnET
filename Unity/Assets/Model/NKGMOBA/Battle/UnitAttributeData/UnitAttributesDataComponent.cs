//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月14日 21:37:53
//------------------------------------------------------------

namespace ET
{
    /// <summary>
    /// 英雄数据组件，负责管理英雄数据
    /// </summary>
    public class UnitAttributesDataComponent: Entity
    {
        public UnitAttributesNodeDataBase UnitAttributesNodeDataBase;

        public NumericComponent NumericComponent;

        public T GetAttributeDataAs<T>() where T : UnitAttributesNodeDataBase
        {
            return UnitAttributesNodeDataBase as T;
        }
        
        public float GetAttribute(NumericType numericType)
        {
            return NumericComponent[numericType];
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            this.UnitAttributesNodeDataBase = null;
            NumericComponent = null;
        }
    }
}