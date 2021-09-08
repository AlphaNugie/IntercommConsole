using CommonLib.Clients.Tasks;
using ConnectServerWrapper;
using gprotocol;
using IntercommConsole.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    public class ModelDisplayServiceTask : Task
    {
        private readonly RadioWrapper<Rcms_test> beltWrapper = new RadioWrapper<Rcms_test>();
        private const int _send_interval = 5; //料流数据发送间隔
        private int _counter = 0;

        /// <summary>
        /// 构造器
        /// </summary>
        public ModelDisplayServiceTask() : base() { }

        public override void Init()
        {
            //NetworkGateway.Start(Config.DataServerIp, Config.UserName, Config.Password);
            NetworkGateway.Start(Config.DataServerIp, Config.DataUdpServerIp, Config.UserName, Config.Password);
            Const.WriteConsoleLog(string.Format("已向服务器{0}发起连接请求...", NetworkGateway.ServerIp));
        }

        public override void LoopContent()
        {
            ////皮带料流状态
            //beltWrapper.MachineName = JsonConvert.SerializeObject(new { ID = Config.WrapperId, status = Const.OpcDatasource.BeltStatus }, Formatting.None);
            //状态为3位数的数字，从左至右分别为斗轮逆变状态、悬皮状态以及地面皮带状态
            beltWrapper.MachineName = JsonConvert.SerializeObject(new { ID = Config.WrapperId, status = Const.OpcDatasource.WheelTurningBackwards * 1000 + Const.OpcDatasource.BeltStatus * 100 + Const.OpcDatasource.GroundBeltStatus * 10 + Const.OpcDatasource.CoalOnBeltPlc }, Formatting.None);

            //单机姿态
            //bool is_gnss_valid = Const.GnssInfo.WalkingPosition != 0 || Const.GnssInfo.PitchAngle != 0 || Const.GnssInfo.YawAngle != 0;
            Const.Wrapper.Walking = (float)Const.GnssInfo.WalkingPosition;
            Const.Wrapper.PitchAngle = (float)Const.GnssInfo.PitchAngle;
            Const.Wrapper.YawAngle = (float)Const.GnssInfo.YawAngle;
            Const.Wrapper.PostureStatus = Const.GnssInfo.Working;

            //雷达报警级别
            List<int> levels = Const.RadarInfo.RadarList == null ? null : Const.RadarInfo.RadarList.Select(r => r.ThreatLevel).ToList();
            Const.WrapperAlarm.ThreatLevels = levels;

            //开始发送
            if (++_counter >= _send_interval)
            {
                _counter = 0;
                NetworkGateway.SendProtobufCmd(SendMode.UDP, beltWrapper.MachineType, beltWrapper.Instance);
            }
            try
            {
                if (Const.IsGnssValid)
                {
                    NetworkGateway.SendProtobufCmd(SendMode.UDP, Const.Wrapper.MachineType, Const.Wrapper.Instance);
                    AddLog("已向3维成像服务器发送单机姿态数据");
                    //_taskLogsBuffer.Add("已向3维成像服务器发送单机姿态数据");
                }
                //if (Const.WrapperAlarm.ThreatStatus != 0)
                //{
                //    NetworkGateway.SendProtobufCmd(Const.WrapperAlarm.MachineType, Const.WrapperAlarm.Instance);
                //    _taskLogsBuffer.Add("已向3维成像服务器发送雷达数据");
                //}
            }
            catch (Exception) { }
        }
    }
}
