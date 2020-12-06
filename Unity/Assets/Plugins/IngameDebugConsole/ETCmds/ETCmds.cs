//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月16日 18:03:51
//------------------------------------------------------------

using ETModel;
using IngameDebugConsole;

namespace Plugins.IngameDebugConsole.ETCmds
{
    public class ETCmds
    {
        [ConsoleMethod("测试常规Log","这是一个测试常规的Log")]
        public static void TestCommonLog()
        {
            Log.Info("这是一个测试常规的Log");
        }
        
        [ConsoleMethod("测试await/async的Log","这是一条测试await/async的Log")]
        public static async void TestAsyncLog()
        {
            Log.Info("这是一个测试常规的Log--一秒后会Log加密通话");
            await TimerComponent.Instance.WaitAsync(1000);
            Log.Info("这是一个测试常规的Log--别比别比，别比巴伯");
        }
    }
}