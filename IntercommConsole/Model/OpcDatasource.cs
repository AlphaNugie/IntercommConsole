using IntercommConsole.Core;
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

        /// <summary>
        /// 斗轮左侧测距（假如为堆料机则为落料口左侧测距）
        /// </summary>
        public double WheelLeftDist { get; set; }

        /// <summary>
        /// 斗轮右侧测距（假如为堆料机则为落料口右侧测距）
        /// </summary>
        public double WheelRightDist { get; set; }

        /// <summary>
        /// 斗轮左侧雷达判断出垛边
        /// </summary>
        public bool WheelLeftBeyondStack { get; set; }

        /// <summary>
        /// 斗轮右侧雷达判断出垛边
        /// </summary>
        public bool WheelRightBeyondStack { get; set; }

        /// <summary>
        /// 设置左右出料堆状态
        /// </summary>
        /// <param name="left_dist">斗轮左侧雷达测距</param>
        /// <param name="right_dist">斗轮右侧雷达测距</param>
        public void SetWheelBeyondStack(double left_dist, double right_dist)
        {
            //left_out = right_out = false;
            //假如距离为0则改为99，代表没有数据时是打到了远处
            left_dist = left_dist == 0 ? 99 : 0;
            right_dist = right_dist == 0 ? 99 : 0;
            double diff = left_dist - right_dist; //左右测距的差
            //double abs_diff = Math.Abs(left_dist - right_dist); //斗轮两侧雷达距离差
            //bool on_each_side = (left_dist - Config.BeyondStackBorder) * (right_dist - Config.BeyondStackBorder) < 0; //左侧、右侧雷达测距是否分属分界线两侧
            //斗轮两侧雷达距离差达到阈值，同时左右侧雷达测距分属分界线两侧
            bool _out = Math.Abs(diff) >= Config.BeyondStackThreshold && (left_dist - Config.BeyondStackBorder) * (right_dist - Config.BeyondStackBorder) < 0;
            WheelLeftBeyondStack = _out && diff > 0;
            WheelRightBeyondStack = _out && diff < 0;
        }
    }
}
