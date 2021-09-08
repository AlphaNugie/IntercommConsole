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
    public class ModelWebDisplayServiceTask : Task
    {
        //private readonly RadioWrapper<Rcms_test> beltWrapper = new RadioWrapper<Rcms_test>();
        //private const int _send_interval = 5; //数据发送间隔
        //private int _counter = 0;

        /// <summary>
        /// 构造器
        /// </summary>
        public ModelWebDisplayServiceTask() : base() { }

        public override void Init()
        {
            NetworkDisplayGateway.Start(Config.DataDisplayServerIp, Config.DataDisplayServerPort, Config.UserNameDisplay, Config.PasswordDisplay);
            //NetworkDisplayGateway.Start(Config.DataDisplayServerIp, Config.DataDisplayServerPort, Config.UserName, Config.Password);
            Const.WriteConsoleLog(string.Format("已向展示服务器{0}发起连接请求...", NetworkDisplayGateway.ServerIp));
        }

        public override void LoopContent()
        {
            ////雷达报警级别
            //List<int> levels = Const.RadarInfo.RadarList == null ? null : Const.RadarInfo.RadarList.Select(r => r.ThreatLevel).ToList();
            //Const.WrapperAlarm.ThreatLevels = levels;

            ////开始发送
            //if (++_counter >= _send_interval)
            //{
            //    _counter = 0;
            //}
            if (Const.IsGnssValid)
            {
                try { NetworkDisplayGateway.SendMachineMovements(Config.MachineName, Const.OpcDatasource.CoalOnBeltPlc, Const.GnssInfo.WalkingPosition, Const.GnssInfo.PitchAngle, Const.GnssInfo.YawAngle, Const.StrategyDataSource.MaterialHeight); }
                catch (Exception) { }
            }
            //try { NetworkDisplayGateway.SendMachineWorkStatus(Config.MachineName, Const.OpcDatasource.WheelTurningBackwards, Const.OpcDatasource.BeltStatus, Const.OpcDatasource.GroundBeltStatus, Const.OpcDatasource.CoalOnBeltPlc); }
            try { NetworkDisplayGateway.SendMachineWorkStatus(Config.MachineName, Const.OpcDatasource.WheelTurningBackwards, Const.OpcDatasource.BeltStatus, Const.OpcDatasource.GroundBeltStatus, Const.IsCoalValid ? 1 : 0); }
            catch (Exception) { }
        }
    }
}
