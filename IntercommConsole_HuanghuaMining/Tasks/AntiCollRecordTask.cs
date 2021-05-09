using CommonLib.Clients;
using CommonLib.Clients.Tasks;
using CommonLib.Helpers;
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
    public class AntiCollRecordTask : Task
    {
        private readonly string _filePath = string.Format(@"D:\AntiCollRecordings\{0}\", Config.MachineName), _fileNameTemplate = "{0:yyyyMMdd}.csv";
        private readonly string _columnHeader = "time,walk,pitch,yaw,level_lf,level_lm,level_lb,level_rf,level_rm,level_rb,dist_lf,dist_lm,dist_lb,dist_rf,dist_rm,dist_rb", _columnTemplate = "{0:yyyy-MM-dd HH:mm:ss},{1:f2},{2:f2},{3:f2},{4},{5},{6},{7},{8},{9},{10:f2},{11:f2},{12:f2},{13:f2},{14:f2},{15:f2}";
        private string _fileName;
        private bool _actionStarted = false, _recording = false; //取料过程是否开始，是否在记录

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
        /// 是否在自动取料过程中
        /// </summary>
        public bool InAction
        {
            get { return _inAction; }
            private set
            {
                _actionStarted = _inAction != value && !InAction;
                _inAction = value;
                //_recording = !_inAction ? false : _recording; //假如停止取料，则同时停止记录
                _recording = _inAction;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            FileDate = DateTime.Now;
        }

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            InAction = true;
            //if (_actionStarted)
            FileDate = DateTime.Now;

            //假如目录不存在，创建
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);
            //假如文件不存在，则首先写入表头
            if (!File.Exists(_fileName))
                File.AppendAllLines(_fileName, new string[] { _columnHeader });
            //假如报警级别不全为0
            if (Const.RadarInfo.LevelLeftFront + Const.RadarInfo.LevelLeftMiddle + Const.RadarInfo.LevelLeftBack + Const.RadarInfo.LevelRightFront + Const.RadarInfo.LevelRightMiddle + Const.RadarInfo.LevelRightBack > 0)
            {
                File.AppendAllLines(_fileName, new string[] { string.Format(_columnTemplate, DateTime.Now, Const.OpcDatasource.WalkingPosition_Plc, Const.OpcDatasource.PitchAngle_Plc, Const.OpcDatasource.YawAngle_Plc, Const.RadarInfo.LevelLeftFront, Const.RadarInfo.LevelLeftMiddle, Const.RadarInfo.LevelLeftBack, Const.RadarInfo.LevelRightFront, Const.RadarInfo.LevelRightMiddle, Const.RadarInfo.LevelRightBack, Const.RadarInfo.DistLeftFront, Const.RadarInfo.DistLeftMiddle, Const.RadarInfo.DistLeftBack, Const.RadarInfo.DistRightFront, Const.RadarInfo.DistRightMiddle, Const.RadarInfo.DistRightBack) });
            }
        }
    }
}
