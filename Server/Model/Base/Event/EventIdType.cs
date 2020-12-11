namespace ETModel
{
    public static class EventIdType
    {
        public const string NumbericChange = "NumbericChange";
        public const string TestBehavior = "TestBehavior";

        public const string CreateCollider = "CreateCollider";

        public const string ChangeHP = "ChangeHP";
        public const string ChangeMP = "ChangeMP";

        public const string SendBuffInfoToClient = "SendBuffInfoToClient";

        //移动到随机位置
        public const string MoveToRandomPos = "MoveToRandomPos";

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