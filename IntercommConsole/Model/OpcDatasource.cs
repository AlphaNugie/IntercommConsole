using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Model
{
    public class OpcDataSource
    {
        /// <summary>
        /// 行走位置
        /// </summary>
        public double WalkingPosition { get; set; }

        /// <summary>
        /// 俯仰角
        /// </summary>
        public double PitchAngle { get; set; }

        /// <summary>
        /// 回转角
        /// </summary>
        public double YawAngle { get; set; }

        /// <summary>
        /// 左编码器行走位置
        /// </summary>
        public double WalkingPositionLeft_Plc { get; set; }

        /// <summary>
        /// 右编码器行走位置
        /// </summary>
        public double WalkingPositionRight_Plc { get; set; }

        /// <summary>
        /// 编码器俯仰角
        /// </summary>
        public double PitchAngle_Plc { get; set; }

        /// <summary>
        /// 编码器回转角
        /// </summary>
        public double YawAngle_Plc { get; set; }

        /// <summary>
        /// 该点煤堆高度
        /// </summary>
        public double PileHeight { get; set; }

        /// <summary>
        /// 皮带是否有料
        /// </summary>
        public bool CoalOnBelt { get; set; }

        /// <summary>
        /// 瞬时流量
        /// </summary>
        public double StreamPerHour { get; set; }

        /// <summary>
        /// 累计流量
        /// </summary>
        public double StreamTotal { get; set; }
    }
}
