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
        /// 校正后俯仰角度
        /// </summary>
        public static double PitchAngle { get; set; }

        /// <summary>
        /// 校正后回转角度
        /// </summary>
        public static double YawAngle { get; set; }

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
}
