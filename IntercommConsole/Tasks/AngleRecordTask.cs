using CommonLib.Clients;
using CommonLib.Clients.Tasks;
using IntercommConsole.Core;
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
        private readonly string _filePath = @"D:\AngleRecordings\", _fileNameTemplate = "{0:yyyyMMddHHmmss}.csv";
        private readonly string _columnHeader = "time,pitch_angle,yaw_angle,direction,type", _columnTemplate = "{0:yyyy-MM-dd HH:mm:ss},{1},{2},{3},{4}";
        private string _fileName;
        private bool _actionStarted = false, _recording = false; //取料过程是否开始，是否在记录
        private bool _beyondLeftReached = false, _beyondRightReached = false; //左侧、右侧出垛边触发标志
        private TurnSignal _manualTurnSignal = 0; //人工换边标志，0 无变化，1 到达左侧边缘，2 到达右侧边缘
        private readonly double _angleThres = 10; //判断是否取料的回转角度（绝对值）阈值，大于此值则在取料
        private readonly GenericStorage<double> _yawStorage = new GenericStorage<double>(), _pitchStorage = new GenericStorage<double>();
        private readonly int _storageLength = 4; //假如队列长度为n，则队首队尾元素相差n-1个循环周期

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

        private double _yawAngle = 0;
        public double YawAngle
        {
            get { return _yawAngle; }
            private set { _yawAngle = value; }
        }

        private double _pitchAngle = 0;
        public double PitchAngle
        {
            get { return _pitchAngle; }
            private set { _pitchAngle = value; }
        }

        private double _yawAngleDiff = 0;
        public double YawAngleDiff
        {
            get { return _yawAngleDiff; }
            private set
            {
                //假如角度变化小于0.5（不明显，可能误判），不进行记录
                if (Math.Abs(value) < 0.5)
                    return;
                int sign = Math.Sign(_yawAngleDiff * value); //假如为1则代表同正或同负
                _manualTurnSignal = sign == 0 || sign == 1 ? TurnSignal.None : (_yawAngleDiff < 0 ? TurnSignal.Left : TurnSignal.Right); //之前的差值小于0而后一个差值大于0，则代表到达最左侧，相反则到达最右侧
                _yawAngleDiff = value;
            }
        }

        private int _wheelLeftBeyond;
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
        public int WheelRightBeyond
        {
            get { return _wheelRightBeyond; }
            set
            {
                _beyondRightReached = _wheelRightBeyond != value && value == 1;
                _wheelRightBeyond = value;
            }
        }

        public override void Init()
        {
            FileDate = DateTime.Now;
            _yawStorage.MaxCapacity = _storageLength;
            _pitchStorage.MaxCapacity = _storageLength;
        }

        public override void LoopContent()
        {
            if (!Config.EnableAngleRecording)
                return;

            YawAngle = Config.AngleRecordingSource == AngleSource.PLC ? Const.OpcDatasource.YawAngle_Plc : Const.GnssInfo.YawAngle;
            PitchAngle = Config.AngleRecordingSource == AngleSource.PLC ? Const.OpcDatasource.PitchAngle_Plc : Const.GnssInfo.PitchAngle;
            InAction = Math.Abs(YawAngle) > _angleThres;
            if (_actionStarted)
                FileDate = DateTime.Now;
            WheelLeftBeyond = Const.OpcDatasource.WheelLeftBeyondStack;
            WheelRightBeyond = Const.OpcDatasource.WheelRightBeyondStack;
            _yawStorage.Push(YawAngle);
            _pitchStorage.Push(PitchAngle);
            double first = _yawStorage.First(), last = _yawStorage.Last();
            YawAngleDiff = first - last;

            //假如目录不存在，创建
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);
            //假如文件不存在，则首先写入表头
            if (!File.Exists(_fileName))
                File.AppendAllLines(_fileName, new string[] { _columnHeader });
            //假如有一边出垛边，则开始记录
            if (Const.OpcDatasource.WheelLeftBeyondStack == 1 || Const.OpcDatasource.WheelRightBeyondStack == 1)
                _recording = true;
            if (_recording)
            {
                if (_beyondLeftReached)
                    File.AppendAllLines(_fileName, new string[] { string.Format(_columnTemplate, DateTime.Now, PitchAngle, YawAngle, "left", 1) });
                if (_beyondRightReached)
                    File.AppendAllLines(_fileName, new string[] { string.Format(_columnTemplate, DateTime.Now, PitchAngle, YawAngle, "right", 1) });
                if (_manualTurnSignal != TurnSignal.None)
                    File.AppendAllLines(_fileName, new string[] { string.Format(_columnTemplate, DateTime.Now, _pitchStorage.Last(), last, _manualTurnSignal == TurnSignal.Left ? "left" : "right", 2) });
            }
        }
    }

    public enum TurnSignal
    {
        None = 0,

        Left = 1,

        Right = 2
    }
}
