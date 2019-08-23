//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 22:10:29
//------------------------------------------------------------

using System;

namespace ETModel
{
    public class NP_BlackBoardDataForCompare: IEquatable<NP_BlackBoardDataForCompare>
    {
        public float _float;

        public long _long;

        public int _int;

        public bool _bool;

        public string _string;

        public bool Equals(NP_BlackBoardDataForCompare other)
        {
            if (other == null) return false;
            if (Math.Abs(this._float - other._float) < 0.000001f && this._long == other._long && this._int == other._int &&
                this._bool == other._bool &&
                this._string == other._string)
            {
                return true;
            }

            return false;
        }

        public static bool operator >=(NP_BlackBoardDataForCompare first, NP_BlackBoardDataForCompare other)
        {
            if (first == null || other == null) return false;
            if (first._float >= other._float && first._long >= other._long && first._int >= other._int && first._bool == other._bool ||
                first._string == other._string)
            {
                return true;
            }

            return false;
        }
        
        public static bool operator > (NP_BlackBoardDataForCompare first, NP_BlackBoardDataForCompare other)
        {
            if (first == null || other == null) return false;
            if (first._float > other._float && first._long > other._long && first._int > other._int && first._bool == other._bool ||
                first._string == other._string)
            {
                return true;
            }

            return false;
        }

        public static bool operator <=(NP_BlackBoardDataForCompare first, NP_BlackBoardDataForCompare other)
        {
            if (first == null || other == null) return false;
            if (first._float <= other._float && first._long <= other._long && first._int <= other._int && first._bool == other._bool ||
                first._string == other._string)
            {
                return true;
            }

            return false;
        }
        
        public static bool operator < (NP_BlackBoardDataForCompare first, NP_BlackBoardDataForCompare other)
        {
            if (first == null || other == null) return false;
            if (first._float < other._float && first._long < other._long && first._int < other._int && first._bool == other._bool ||
                first._string == other._string)
            {
                return true;
            }

            return false;
        }
        
    }
}