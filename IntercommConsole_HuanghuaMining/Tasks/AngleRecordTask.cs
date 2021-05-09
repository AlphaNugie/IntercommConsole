using CommonLib.Clients;
using CommonLib.Clients.Tasks;
using CommonLib.Helpers;
using IntercommConsole.Core;
using IntercommConsole.DataUtil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    /// <summary>
    /// 出垛边角度记录任务
    /// </summary>
    public class AngleRecordTask : Task
    {
        private readonly string _filePath = string.Format(@"D:\AngleRecordings\{0}\", Config.MachineName), _fileNameTemplate = "{0:yyyyMMddHHmmss}.csv", _fileNameTemplateExt = "{0:yyyyMMddHHmmss}_full.csv";
        private readonly string _columnHeader = "time,walking,pitch_angle,left_out_angle,left_turn_angle,right_out_angle,right_turn_angle,direction,type", _columnTemplate = "{0},{1},{2},{3},{4},{5},{6},{7},{8}";
        private readonly string _columnHeaderExt = "time,walking,pitch_angle,yaw_angle,angle_left,angle_right,wheel_power,wheel_height,radius_left,radius_right", _columnTemplateExt = "{0:yyyy-MM-dd HH:mm:ss},{1},{2},{3},{4},{5},{6},{7:#.##},{8:#.##},{9:#.##}";
        private string _fileName, _fileNameExt;
        private bool _actionStarted = false, _recording = false; //取料过程是否开始，是否在记录
        private bool _beyondLeftReached = false, _beyondRightReached = false; //左侧、右侧出垛边触发标志
        private TurnSignal _manualTurnSignal = 0; //人工换边标志，0 无变化，1 到达左侧边缘，2 到达右侧边缘
        private readonly double _angleThres = 10; //判断是否在取料区域内的回转角度（绝对值）阈值，大于此值则在取料区域内
        private readonly GenericStorage<double> _yawStorage = new GenericStorage<double>(), _pitchStorage = new GenericStorage<double>(), _walkStorage = new GenericStorage<double>();
        private readonly int _storageLength = 4; //假如队列长度为n，则队首队尾元素相差n-1个循环周期
        private readonly DataService_Machine _dataService = new DataService_Machine();

        private DateTime _fileDate;
        /// <summary>
        /// 记录文件的日期
        /// </summary>
        public DateTime FileDate
        {
            get { return _fileDate; }
            private set
            {
                _fileDate = value;
                _fileName = _filePath + string.Format(_fileNameTemplate, _fileDate);
                _fileNameExt = _filePath + string.Format(_fileNameTemplateExt, _fileDate);
            }
        }

        private bool _inAction = false;
        /// <summary>
        /// 是否在取料过程中
        /// </summary>
        public bool InAction
        {
            get { return _inAction; }
            private set
            {
                _actionStarted = _inAction != value && !InAction;
                _inAction = value;
                _recording = !_inAction ? false : _recording; //假如停止取料，则同时停止记录
            }
        }

        public double WalkingPosition { get; private set; }

        /// <summary>
        /// 俯仰角
        /// </summary>
        public double PitchAngle { get; private set; }

        /// <summary>
        /// 回转角
        /// </summary>
        public double YawAngle { get; private set; }

        private double _yawAngleDiff = 0;
        /// <summary>
        /// 回转角的偏差，用来判断转向动作
        /// </summary>
        public double YawAngleDiff
        {
            get { return _yawAngleDiff; }
            private set
            {
                _manualTurnSignal = TurnSignal.None; //重置方向变化信号
                //假如角度变化小于0.5（不明显，可能误判），不进行记录
                if (Math.Abs(value) < 0.5)
                    return;
                int sign = Math.Sign(_yawAngleDiff * value); //假如为1则代表同正或同负
                _manualTurnSignal = sign == 0 || sign == 1 ? TurnSignal.None : (_yawAngleDiff < 0 ? TurnSignal.Left : TurnSignal.Right); //之前的差值小于0而后一个差值大于0，则代表到达最左侧，相反则到达最右侧
                _yawAngleDiff = value;
            }
        }

        private int _wheelLeftBeyond;
        /// <summary>
        /// 左侧是否出垛边
        /// </summary>
        public int WheelLeftBeyond
        {
            get { return _wheelLeftBeyond; }
            set
            {
                _beyondLeftReached = _wheelLeftBeyond != value && value == 1;
                _wheelLeftBeyond = value;
            }
        }

        private int _wheelRightBeyond;
        /// <summary>
        /// 右侧是否出垛边
        /// </summary>
        public int WheelRightBeyond
        {
            get { return _wheelRightBeyond; }
            set
            {
                _beyondRightReached = _wheelRightBeyond != value && value == 1;
                _wheelRightBeyond = value;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            FileDate = DateTime.Now;
            _yawStorage.MaxCapacity = _storageLength;
            _pitchStorage.MaxCapacity = _storageLength;
        }

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            if (!Config.EnableAngleRecording)
                return;

            YawAngle = Config.AngleRecordingSource == AngleSource.PLC ? Const.OpcDatasource.YawAngle_Plc : Const.GnssInfo.YawAngle;
            PitchAngle = Config.AngleRecordingSource == AngleSource.PLC ? Const.OpcDatasource.PitchAngle_Plc : Const.GnssInfo.PitchAngle;
            WalkingPosition = Config.AngleRecordingSource == AngleSource.PLC ? Const.OpcDatasource.WalkingPosition_Plc : Const.GnssInfo.WalkingPosition;
            InAction = Math.Abs(YawAngle) > _angleThres;
            if (_actionStarted)
                FileDate = DateTime.Now;
            WheelLeftBeyond = Const.OpcDatasource.WheelLeftBeyondStack;
            WheelRightBeyond = Const.OpcDatasource.WheelRightBeyondStack;
            _yawStorage.Push(YawAngle);
            _pitchStorage.Push(PitchAngle);
            _walkStorage.Push(WalkingPosition);
            double first = _yawStorage.First(), last = _yawStorage.Last();
            YawAngleDiff = first - last;

            //假如目录不存在，创建
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);
            //假如文件不存在，则首先写入表头
            if (!File.Exists(_fileName))
                File.AppendAllLines(_fileName, new string[] { _columnHeader });
            if (!File.Exists(_fileNameExt))
                File.AppendAllLines(_fileNameExt, new string[] { _columnHeaderExt });
            ////假如有一边出垛边，则开始记录
            //if (Const.OpcDatasource.WheelLeftBeyondStack == 1 || Const.OpcDatasource.WheelRightBeyondStack == 1)
            //    _recording = true;
            //假如斗轮开始转动，则开始记录
            if (Const.OpcDatasource.WheelTurningBackwards == 1)
                _recording = true;
            //是否记录同时由悬皮状态以及斗轮是否转动决定
            if (_recording && Const.OpcDatasource.BeltStatus == 1 && Const.OpcDatasource.WheelTurningBackwards == 1)
            {
                DateTime now = DateTime.Now;
                string stamp = DateTimeHelper.GetTimeStampBySeconds(), fileDateStr = _fileDate.ToString("yyyyMMddHHmmss");
                //记录所有数据
                File.AppendAllLines(_fileNameExt, new string[] { string.Format(_columnTemplateExt, now, WalkingPosition, PitchAngle, YawAngle, Const.RadarInfo.SurfaceAngleWheelLeft, Const.RadarInfo.SurfaceAngleWheelRight, Const.OpcDatasource.WheelPowerPolished, Const.OpcDatasource.PileHeight, Const.RadarInfo.RadiusAverageLeft, Const.RadarInfo.RadiusAverageRight) });
                if (_beyondLeftReached)
                {
                    File.AppendAllLines(_fileName, new string[] { string.Format(_columnTemplate, stamp, WalkingPosition, PitchAngle, YawAngle, string.Empty, string.Empty, string.Empty, "left", 1) });
                    _dataService.InsertAngleRecord(Config.MachineName, fileDateStr, WalkingPosition, PitchAngle, YawAngle, 1, 1, now);
                }
                if (_beyondRightReached)
                {
                    File.AppendAllLines(_fileName, new string[] { string.Format(_columnTemplate, stamp, WalkingPosition, PitchAngle, string.Empty, string.Empty, YawAngle, string.Empty, "right", 1) });
                    _dataService.InsertAngleRecord(Config.MachineName, fileDateStr, WalkingPosition, PitchAngle, YawAngle, 2, 2, now);
                }
                if (_manualTurnSignal == TurnSignal.Left)
                {
                    File.AppendAllLines(_fileName, new string[] { string.Format(_columnTemplate, stamp, _walkStorage.Last(), _pitchStorage.Last(), string.Empty, last, string.Empty, string.Empty, "left", 2) });
                    _dataService.InsertAngleRecord(Config.MachineName, fileDateStr, WalkingPosition, PitchAngle, YawAngle, 3, 1, now);
                }
                else if (_manualTurnSignal == TurnSignal.Right)
                {
                    File.AppendAllLines(_fileName, new string[] { string.Format(_columnTemplate, stamp, _walkStorage.Last(), _pitchStorage.Last(), string.Empty, string.Empty, string.Empty, last, "right", 2) });
                    _dataService.InsertAngleRecord(Config.MachineName, fileDateStr, WalkingPosition, PitchAngle, YawAngle, 4, 2, now);
                }
            }
        }
    }

    /// <summary>
    /// 方向变化标志
    /// </summary>
    public enum TurnSignal
    {
        /// <summary>
        /// 方向无变化
        /// </summary>
        None = 0,

        /// <summary>
        /// 向左转
        /// </summary>
        Left = 1,

        /// <summary>
        /// 向右转
        /// </summary>
        Right = 2
    }
}
