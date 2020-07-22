using CommonLib.Clients;
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
        #region 北斗姿态
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
        #endregion

        #region PLC姿态
        /// <summary>
        /// 左编码器行走位置
        /// </summary>
        public double WalkingPositionLeft_Plc { get; set; }

        /// <summary>
        /// 右编码器行走位置
        /// </summary>
        public double WalkingPositionRight_Plc { get; set; }

        /// <summary>
        /// 存储PLC行走位置的队列
        /// </summary>
        public GenericStorage<double> WalkingQueue_Plc { get; set; }

        /// <summary>
        /// PLC行走速度
        /// </summary>
        public double WalkingSpeed_Plc { get; set; }
        //public double WalkingSpeed_Plc { get { return WalkingQueue_Plc == null ? 0 : WalkingQueue_Plc.ElementAt(2) - WalkingQueue_Plc.ElementAt(1); } }

        /// <summary>
        /// PLC行走加速度
        /// </summary>
        public double WalkingAcce_Plc { get; set; }
        //public double WalkingAcce_Plc { get { return WalkingQueue_Plc == null ? 0 : WalkingQueue_Plc.ElementAt(0) - 2 * WalkingQueue_Plc.ElementAt(1) + WalkingQueue_Plc.ElementAt(2); } }

        /// <summary>
        /// 编码器俯仰角
        /// </summary>
        public double PitchAngle_Plc { get; set; }

        /// <summary>
        /// 编码器回转角
        /// </summary>
        public double YawAngle_Plc { get; set; }

        /// <summary>
        /// 存储PLC回转角的队列
        /// </summary>
        public GenericStorage<double> YawQueue_Plc { get; set; }

        /// <summary>
        /// PLC回转速度
        /// </summary>
        public double YawSpeed_Plc { get; set; }
        //public double YawSpeed_Plc { get { return YawQueue_Plc == null ? 0 : YawQueue_Plc.ElementAt(1) - YawQueue_Plc.ElementAt(0); } }
        #endregion

        #region 惯导姿态
        /// <summary>
        /// 惯导行走加速度
        /// </summary>
        public double WalkingAcce_Ins { get; set; }

        /// <summary>
        /// 惯导俯仰角
        /// </summary>
        public double PitchAngle_Ins { get; set; }

        /// <summary>
        /// 惯导回转速度
        /// </summary>
        public double YawSpeed_Ins { get; set; }
        #endregion

        /// <summary>
        /// 该点煤堆高度
        /// </summary>
        public double PileHeight { get; set; }

        /// <summary>
        /// 皮带是否有料
        /// </summary>
        public bool CoalOnBelt { get; set; }

        /// <summary>
        /// 皮带是否有料（PLC信号）
        /// </summary>
        public int CoalOnBeltPlc { get { return CoalOnBelt ? 1 : 0; } }

        /// <summary>
        /// 瞬时流量
        /// </summary>
        public double StreamPerHour { get; set; }

        /// <summary>
        /// 累计流量
        /// </summary>
        public double StreamTotal { get; set; }

        #region 雷达
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
        public int WheelLeftBeyondStack { get; set; }

        /// <summary>
        /// 斗轮右侧雷达判断出垛边
        /// </summary>
        public int WheelRightBeyondStack { get; set; }

        /// <summary>
        /// 臂架左侧
        /// </summary>
        public double ArmLeftDist { get; set; }

        /// <summary>
        /// 臂架右侧距离
        /// </summary>
        public double ArmRightDist { get; set; }

        /// <summary>
        /// 臂架下方距离
        /// </summary>
        public double ArmBelowDist { get; set; }

        /// <summary>
        /// 配重左侧距离
        /// </summary>
        public double CounterLeftDist { get; set; }

        /// <summary>
        /// 配重右侧距离
        /// </summary>
        public double CounterRightDist { get; set; }

        /// <summary>
        /// 雷达信号
        /// </summary>
        public int RadarStatus { get; set; }
        #endregion

        /// <summary>
        /// 设置左右出料堆状态
        /// </summary>
        /// <param name="left_dist">斗轮左侧雷达测距</param>
        /// <param name="right_dist">斗轮右侧雷达测距</param>
        public void SetWheelBeyondStack(double left_dist, double right_dist)
        {
            //left_out = right_out = false;
            //假如距离为0则改为99，代表没有数据时是打到了远处
            left_dist = left_dist == 0 ? 99 : left_dist;
            right_dist = right_dist == 0 ? 99 : right_dist;
            double diff = left_dist - right_dist; //左右测距的差
            //double abs_diff = Math.Abs(left_dist - right_dist); //斗轮两侧雷达距离差
            //bool on_each_side = (left_dist - Config.BeyondStackBorder) * (right_dist - Config.BeyondStackBorder) < 0; //左侧、右侧雷达测距是否分属分界线两侧
            //斗轮两侧雷达距离差达到阈值，同时左右侧雷达测距分属分界线两侧
            bool _out = Math.Abs(diff) >= Config.BeyondStackThreshold && (left_dist - Config.BeyondStackBorder) * (right_dist - Config.BeyondStackBorder) < 0;
            WheelLeftBeyondStack = _out && diff > 0 ? 1 : 0;
            WheelRightBeyondStack = _out && diff < 0 ? 1 : 0;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public OpcDataSource()
        {
            this.WalkingQueue_Plc = new GenericStorage<double>(3);
            this.YawQueue_Plc = new GenericStorage<double>(2);
            this.WalkingQueue_Plc.FillEmptyShells();
            this.YawQueue_Plc.FillEmptyShells();
            //this.WalkingQueue.Push(0);
            //this.WalkingQueue.Push(0);
            //this.WalkingQueue.Push(0);
            //this.YawQueue.Push(0);
            //this.YawQueue.Push(0);
        }
    }
}
