using CommonLib.Clients;
using CommonLib.Clients.Tasks;
using CommonLib.Function;
using CommonLib.Helpers;
using IntercommConsole.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SocketHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    /// <summary>
    /// 策略工控机数据发送任务
    /// </summary>
    public class StrategyServiceTask : Task
    {
        //private readonly string _path = @"..\..\..\..\DataStorage\", _fileName = "WheelCentreCoors.txt";
        private readonly SocketTcpServer _tcpServer = new SocketTcpServer() { ServerIp = Const.LocalIp, ServerPort = 12999 };

        /// <summary>
        /// 构造器
        /// </summary>
        public StrategyServiceTask() : base() { }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            _tcpServer.Start();
            //_tcpServer = new SocketTcpServer() { ServerIp = Const.LocalIp, ServerPort = 4999 };
        }

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            string info = string.Format("{0:f3},{1:f3},{2},{3},{4}", Const.OpcDatasource.PitchAngle_Plc, Const.OpcDatasource.YawAngle_Plc, Const.GnssInfo.LocalCoor_Tipx, Const.GnssInfo.LocalCoor_Tipy, Const.GnssInfo.LocalCoor_Tipz);
            //info = HexHelper.GetStringSumResult(info);
            _tcpServer.SendData(HexHelper.GetStringSumResult(info) + ';');

            //if (!Directory.Exists(_path))
            //    Directory.CreateDirectory(_path);
            //string filePath = FileSystemHelper.TrimFilePath(_path) + FileSystemHelper.DirSeparator + _fileName;
            //File.WriteAllText(filePath, info);

            //_taskLogsBuffer.Add("已向策略工控机发送：" + result.Length);
        }
    }
}
