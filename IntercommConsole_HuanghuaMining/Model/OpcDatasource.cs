using CommonLib.Clients;
using IntercommConsole.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OpcDataSource
    {
        /// <summary>
        /// PLC是否可读
        /// </summary>
        public bool PlcReadable { get; set; }

        /// <summary>
        /// PLC是否可写
        /// </summary>
        public bool PlcWritable { get; set; }

        /// <summary>
        /// PLC出现故障
        /// </summary>
        [JsonProperty]
        public bool PlcErrorOccured { get; set; }

        #region 北斗姿态
        /// <summary>
        /// 北斗定位是否为固定解，1 是，0 否
        /// </summary>
        public int IsFixed { get { return Const.GnssInfo.IsFixed ? 1 : 0; } }

        /// <summary>
        /// 是否接收到真北航向角，1 是，0 否
        /// </summary>
        public int TrackDirection_Received { get { return Const.GnssInfo.TrackDirection_Received ? 1 : 0; } }

        /// <summary>
        /// 行走位置
        /// </summary>
        public double WalkingPosition { get; set; }

        /// <summary>
        /// 俯仰角
        /// </summary>
        public double PitchAngle { get; set; }

        private double _yawAngle;
        /// <summary>
        /// 回转角（PLC内部的值与实际值的符号相反）
        /// </summary>
        public double YawAngle
        {
            get { return _yawAngle; }
            set { _yawAngle = value * -1; }
        }
        #endregion

        #region PLC姿态
        /// <summary>
        /// 编码器行走位置
        /// </summary>
        public double WalkingPosition_Plc { get; set; }

        /// <summary>
        /// 存储PLC行走位置的队列
        /// </summary>
        public GenericStorage<double> WalkingQueue_Plc { get; set; }

        ///// <summary>
        ///// PLC行走速度
        ///// </summary>
        //public double WalkingSpeed_Plc { get; set; }

        private double _walkSpeedPlc = 0;
        /// <summary>
        /// 回转角速度（PLC）
        /// </summary>
        public double WalkingSpeed_Plc
        {
            get { return _walkSpeedPlc; }
            //变频器最大值为15000，对应最大走行速度为0.5米/秒（30米/分钟）
            set { _walkSpeedPlc = value / 15000 * 0.5; }
        }

        /// <summary>
        /// PLC行走加速度
        /// </summary>
        public double WalkingAcce_Plc { get; set; }

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

        ///// <summary>
        ///// PLC回转速度
        ///// </summary>
        //public double YawSpeed_Plc { get; set; }

        private double _yawSpeedPlc = 0;
        /// <summary>
        /// 回转角速度（PLC）
        /// </summary>
        public double YawSpeed_Plc
        {
            get { return _yawSpeedPlc; }
            //变频器最大值为14500，对应最大转速为0.13rpm，转换为°/s
            set { _yawSpeedPlc = value / 14500 * 0.13 * 360 / 60; }
        }
        #endregion

        #region 惯导姿态
        /// <summary>
        /// 惯导行走加速度
        /// </summary>
        public double WalkingAcce_Ins { get; set; }

        private const int PITCH_QUEUE_MAXCOUNT = 5;
        private readonly Queue<double> _pitch_queue = new Queue<double>(PITCH_QUEUE_MAXCOUNT);
        private double _pitch_ins;
        /// <summary>
        /// 惯导俯仰角
        /// </summary>
        public double PitchAngle_Ins
        {
            get { return _pitch_ins; }
            set
            {
                _pitch_queue.Enqueue(_pitch_ins);
                if (_pitch_queue.Count > PITCH_QUEUE_MAXCOUNT)
                    _pitch_queue.Dequeue();
                _pitch_ins = value;
                Const.IsPlcInsValid = _pitch_ins != _pitch_queue.Average();
            }
        }

        /// <summary>
        /// 惯导回转速度
        /// </summary>
        public double YawSpeed_Ins { get; set; }
        #endregion

        #region 校正后姿态
        /// <summary>
        /// 校正后行走位置
        /// </summary>
        public double WalkingPositionCorr { get; set; }

        /// <summary>
        /// 校正后俯仰角
        /// </summary>
        public double PitchAngleCorr { get; set; }

        /// <summary>
        /// 校正后回转角
        /// </summary>
        public double YawAngleCorr { get; set; }

        /// <summary>
        /// 校正后单击姿态的状态，由9位2进制数构成，分别对应行走俯仰回转是否可用，以及（假如可用）在三个定位系统中具体哪个数据不可用：0 均可用，1 北斗，2 编码器，3 惯导
        /// </summary>
        public int PostureStates { get; set; }
        #endregion

        /// <summary>
        /// 是否正在自动作业，0 否，1 是
        /// </summary>
        public int AutoControl { get; set; }

        /// <summary>
        /// 出模型边界角度1
        /// </summary>
        public double ModelBoundAngle1 { get; set; }

        /// <summary>
        /// 出模型边界角度2
        /// </summary>
        public double ModelBoundAngle2 { get; set; }

        /// <summary>
        /// 斗轮是否逆向转动，0 逆向转动，1 非逆向转动
        /// </summary>
        public int WheelTurningBackwards { get; set; }

        /// <summary>
        /// 悬皮是否启动，0 未启动，1 启动
        /// </summary>
        public int BeltStatus { get; set; }

        /// <summary>
        /// 地面皮带是否启动，0 未启动，1 启动
        /// </summary>
        public int GroundBeltStatus { get; set; }

        /// <summary>
        /// 该点煤堆高度
        /// </summary>
        public double PileHeight { get; set; }

        /// <summary>
        /// 皮带是否有料
        /// </summary>
        public bool CoalOnBelt { get; set; }

        /// <summary>
        /// 皮带上料流的大小等级
        /// </summary>
        public int CoalOnBeltLevel { get; set; }

        /// <summary>
        /// 皮带是否有料（PLC信号），1 有料，0 无料
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

        /// <summary>
        /// 斗轮功率（原始数据）
        /// </summary>
        public double WheelPowerRaw { get; set; }

        /// <summary>
        /// 斗轮功率（经过平滑处理）
        /// </summary>
        public double WheelPowerPolished { get; set; }

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
        /// 斗轮左侧雷达判断出垛边，0 未出垛边，1 出垛边
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
        /// 左前距离
        /// </summary>
        public double DistLeftFront { get; set; }

        /// <summary>
        /// 左中距离
        /// </summary>
        public double DistLeftMiddle { get; set; }

        /// <summary>
        /// 左后距离
        /// </summary>
        public double DistLeftBack { get; set; }

        /// <summary>
        /// 右前距离
        /// </summary>
        public double DistRightFront { get; set; }

        /// <summary>
        /// 右中距离
        /// </summary>
        public double DistRightMiddle { get; set; }

        /// <summary>
        /// 右后距离
        /// </summary>
        public double DistRightBack { get; set; }

        /// <summary>
        /// 左前距离级别
        /// </summary>
        public double LevelLeftFront { get; set; }

        /// <summary>
        /// 左中距离级别
        /// </summary>
        public double LevelLeftMiddle { get; set; }

        /// <summary>
        /// 左后距离级别
        /// </summary>
        public double LevelLeftBack { get; set; }

        /// <summary>
        /// 右前距离级别
        /// </summary>
        public double LevelRightFront { get; set; }

        /// <summary>
        /// 右中距离级别
        /// </summary>
        public double LevelRightMiddle { get; set; }

        /// <summary>
        /// 右后距离级别
        /// </summary>
        public double LevelRightBack { get; set; }

        /// <summary>
        /// 雷达信号
        /// </summary>
        public int RadarStatus { get; set; }
        #endregion

        /// <summary>
        /// 根据皮带料流雷达距离判断料流等级
        /// </summary>
        /// <param name="dist_belt"></param>
        public void UpdateCoalOnBeltLevel(double dist_belt)
        {
            int level = 0;
            foreach (var dist in Config.DistBeltLevels)
            {
                if (dist_belt >= dist)
                    break;
                level++;
            }
            CoalOnBeltLevel = level;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public OpcDataSource()
        {
            PlcReadable = true;
            PlcWritable = true;
            WalkingQueue_Plc = new GenericStorage<double>(3);
            YawQueue_Plc = new GenericStorage<double>(2);
            WalkingQueue_Plc.FillEmptyShells();
            YawQueue_Plc.FillEmptyShells();
        }
    }
}
