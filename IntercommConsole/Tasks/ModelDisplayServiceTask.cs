using ConnectServerWrapper;
using gprotocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Tasks
{
    public class ModelDisplayServiceTask : Task
    {
        private readonly RadioWrapper<Rcms_test> beltWrapper = new RadioWrapper<Rcms_test>();
        private const string _format = @"[ 'ID': {1}, 'status': {0}]";
        private const int _send_interval = 5; //料流数据发送间隔
        private int _counter = 0;

        /// <summary>
        /// 构造器
        /// </summary>
        public ModelDisplayServiceTask() : base() { }

        public override void Init()
        {
            NetworkGateway.Start(Config.DataServerIp, Config.UserName, Config.Password);
            Const.WriteConsoleLog(string.Format("已向服务器{0}发起连接请求...", NetworkGateway.ServerIp));
        }

        public override void LoopContent()
        {
            bool is_gnss_valid = Const.GnssInfo.WalkingPosition != 0 || Const.GnssInfo.PitchAngle != 0 || Const.GnssInfo.YawAngle != 0;
            if (++_counter >= _send_interval)
            {
                _counter = 0;
                beltWrapper.MachineName = string.Format(_format, Const.OpcDatasource.CoalOnBelt ? 1 : 0, Config.WrapperId).Replace('[', '{').Replace(']', '}');
                NetworkGateway.SendProtobufCmd(beltWrapper.MachineType, beltWrapper.Instance);
            }
            if (is_gnss_valid)
            {
                try
                {
                    Const.Wrapper.Walking = (float)Const.GnssInfo.WalkingPosition;
                    Const.Wrapper.PitchAngle = (float)Const.GnssInfo.PitchAngle;
                    Const.Wrapper.YawAngle = (float)Const.GnssInfo.YawAngle;
                    NetworkGateway.SendProtobufCmd(Const.Wrapper.MachineType, Const.Wrapper.Instance);
                    _taskLogs = new List<string>() { "已向3维成像服务器发送数据..." };
                    //Console.WriteLine("已向3维成像服务器发送数据...");
                }
                catch (Exception) { }
            }
        }
    }
}
