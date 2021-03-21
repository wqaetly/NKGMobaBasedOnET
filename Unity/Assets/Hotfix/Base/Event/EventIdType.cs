namespace ETHotfix
{
    public static class EventIdType
    {
        /// <summary>
        /// 登录完毕
        /// </summary>
        public const string LoginFinish = "LoginFinish";

        /// <summary>
        /// 显示战斗UI
        /// </summary>
        public const string Show5v5MapUI = "Show5v5MapUI";

        /// <summary>
        /// 关闭战斗UI
        /// </summary>
        public const string Close5v5MapUI = "Close5v5MapUI";

        /// <summary>
        /// 显示掉线对话框UI
        /// </summary>
        public const string ShowOfflineDialogUI = "ShowOfflineDialogUI";

        /// <summary>
        /// 关闭对话框UI
        /// </summary>
        public const string CloseDialogUI = "CloseDialogUI";

        /// <summary>
        /// 进入地图完毕
        /// </summary>
        public const string EnterMapFinish = "EnterMapFinish";

        /// <summary>
        /// 大厅界面的所有数据加载完毕
        /// </summary>
        public const string LobbyUIAllDataLoadComplete = "LobbyUIAllDataLoadComplete";

        /// <summary>
        /// 显示登录UI
        /// </summary>
        public const string ShowLoginUI = "ShowLoginUI";

        /// <summary>
        /// 显示登录信息
        /// </summary>
        public const string ShowLoginInfo = "ShowLoginInfo";

        /// <summary>
        /// 显示加载中的UI
        /// </summary>
        public const string ShowLoadingProcess = "ShowLoadingPorcess";

        /// <summary>
        /// 点击小地图
        /// </summary>
        public const string ClickSmallMap = "ClickSmallMap";
        
        public const string OfflineToLoginUI = "OfflineToLoginUI";

        #region 血条相关事件

        /// <summary>
        /// 创建人物头部UI
        /// </summary>
        public const string CreateHeadBar = "CreateHeadBar";

        #endregion

        /// <summary>
        /// 设置自身的英雄Unit
        /// </summary>
        public const string SetSelfHero = "SetSelfHero";

        /// <summary>
        /// 修改对象的属性，用于处理具体的改变数值
        /// 例如服务端发送了一条扣血（50）消息
        /// Numeric处理当前血量（例如当前血量为100 - 50 = 50）事件
        /// 而这个事件则处理改变了50这个事件，比如出现50飘血字样
        /// </summary>
        public const string ChangeUnitAttribute = "ChangeUnitAttribute";
    }
}