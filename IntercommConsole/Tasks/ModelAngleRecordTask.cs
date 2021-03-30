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
    public class ModelAngleRecordTask : Task
    {
        private readonly string _filePath = string.Format(@"D:\ModelAngleRecordings\{0}\", Config.MachineName), _fileNameTemplate = "{0:yyyyMMddHHmmss}.csv";
        private readonly string _columnHeader = "time,auto,angle1,angle2", _columnTemplate = "{0:yyyy-MM-dd HH:mm:ss},{1},{2},{3}";
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

        public double ModelAngle1 { get; private set; }

        public double ModelAngle2 { get; private set; }

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
            ModelAngle1 = Const.OpcDatasource.ModelBoundAngle1;
            ModelAngle2 = Const.OpcDatasource.ModelBoundAngle2;
            InAction = Const.OpcDatasource.AutoControl == 1;
            if (_actionStarted)
                FileDate = DateTime.Now;

            //假如目录不存在，创建
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);
            //假如文件不存在，则首先写入表头
            if (!File.Exists(_fileName))
                File.AppendAllLines(_fileName, new string[] { _columnHeader });
            if (_recording)
            {
                File.AppendAllLines(_fileName, new string[] { string.Format(_columnTemplate, DateTime.Now, _inAction, ModelAngle1, ModelAngle2) });
            }
        }
    }
}
