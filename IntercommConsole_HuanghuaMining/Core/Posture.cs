using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Core
{
    /// <summary>
    /// 校正后单机姿态数据
    /// </summary>
    public static class Posture
    {
        /// <summary>
        /// 校正后行走位置
        /// </summary>
        public static double WalkingPosition { get; set; }

        /// <summary>
        /// 校正后行走位置是否可用
        /// </summary>
        public static bool WalkingValid { get; set; }

        /// <summary>
        /// 行走位置校正后（假如可用）哪个来源的数据不可用
        /// </summary>
        public static PostureInvalidResource WalkingInvalidResource { get; set; }

        /// <summary>
        /// 校正后俯仰角度
        /// </summary>
        public static double PitchAngle { get; set; }

        /// <summary>
        /// 校正后俯仰角是否可用
        /// </summary>
        public static bool PitchValid { get; set; }

        /// <summary>
        /// 俯仰角校正后（假如可用）哪个来源的数据不可用
        /// </summary>
        public static PostureInvalidResource PitchInvalidResource { get; set; }

        /// <summary>
        /// 校正后回转角度
        /// </summary>
        public static double YawAngle { get; set; }

        /// <summary>
        /// 校正后回转角是否可用
        /// </summary>
        public static bool YawValid { get; set; }

        /// <summary>
        /// 回转角校正后（假如可用）哪个来源的数据不可用
        /// </summary>
        public static PostureInvalidResource YawInvalidResource { get; set; }

        /// <summary>
        /// 定位数据所采用的定位系统类型
        /// </summary>
        public static PostureType Type { get; set; }
    }

    /// <summary>
    /// 定位系统采用的定位类型
    /// </summary>
    public enum PostureType
    {
        /// <summary>
        /// GPS
        /// </summary>
        GPS = 1,

        /// <summary>
        /// 编码器
        /// </summary>
        PLC = 2,

        /// <summary>
        /// 自动判断
        /// </summary>
        AUTOMATIC = 3
    }

    /// <summary>
    /// 姿态数据（行走俯仰回转）可用的情况下哪个来的数据不可用
    /// </summary>
    public enum PostureInvalidResource
    {
        /// <summary>
        /// 均可用
        /// </summary>
        OK = 0,

        /// <summary>
        /// 北斗数据不可用
        /// </summary>
        GPS = 1,

        /// <summary>
        /// 编码器数据不可用
        /// </summary>
        PLC = 2,

        /// <summary>
        /// 惯导数据不可用
        /// </summary>
        INS = 3
    }
}
