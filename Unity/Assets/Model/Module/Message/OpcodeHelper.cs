using System.Collections.Generic;

namespace ET
{
    public static class OpcodeHelper
    {
        private static readonly HashSet<ushort> ignoreDebugLogMessageSet = new HashSet<ushort>
        {
            OuterOpcode_Gate.C2G_Ping,
            OuterOpcode_Gate.G2C_Ping,
        };

        private static bool IsNeedLogMessage(ushort opcode)
        {
            if (ignoreDebugLogMessageSet.Contains(opcode))
            {
                return false;
            }

            return true;
        }

        public static bool IsOuterMessage(ushort opcode)
        {
            return opcode >= 20000;
        }

        public static bool IsInnerMessage(ushort opcode)
        {
            return opcode < 20000;
        }

        public static void LogMsg(int zone, ushort opcode, object message)
        {
            if (!IsNeedLogMessage(opcode))
            {
                return;
            }

            GlobalDefine.ILog.Debug("zone: {0} {1} {2}", zone, message.GetType(), message);
        }

        public static void LogMsg(ushort opcode, long actorId, object message)
        {
            if (!IsNeedLogMessage(opcode))
            {
                return;
            }

            GlobalDefine.ILog.Debug("actorId: {0} {1}", actorId, message);
        }
    }
}