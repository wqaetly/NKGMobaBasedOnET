namespace ETModel
{
    public static class EventIdType
    {
        public const string NumbericChange = "NumbericChange";
        public const string TestBehavior = "TestBehavior";

        public const string CreateCollider = "CreateCollider";

        public const string SendBuffInfoToClient = "SendBuffInfoToClient";

        //移动到随机位置
        public const string MoveToRandomPos = "MoveToRandomPos";

        //伤害触发
        public const string ExcuteDamage = "ExcuteDamage";
        //治疗触发
        public const string ExcuteTreate = "ExcuteTreate";
        
        //Numeric执行修改
        public const string NumericApplyChangeValue = "NumericApplyChangeValue";
        
        /// <summary>
        /// 取消移动
        /// </summary>
        public const string CancelMove = "CancelMove";
        
        /// <summary>
        /// 取消攻击
        /// </summary>
        public const string CancelAttack = "CancelAttack";

        /// <summary>
        /// 取消攻击但不重置攻击对象
        /// </summary>
        public const string CancelAttackWithOutResetAttackTarget = "CancelAttackWithOutResetAttackTarget";
    }
}