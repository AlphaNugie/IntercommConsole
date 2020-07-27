using CommonLib.Clients;
using CommonLib.Extensions;
using IntercommConsole.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Tasks
{
    public class InternalCommTask : Task
    {
        private DerivedUdpClient udp = null;
        private const int CHART_LOCAL_PORT = 43991, CHART_REMOTE_PORT = 43992;

        /// <summary>
        /// 构造器
        /// </summary>
        public InternalCommTask() : base() { }

        public override void Init()
        {
            udp = new DerivedUdpClient(Const.LocalIp, CHART_LOCAL_PORT, true, false);
        }

        public override void LoopContent()
        {
            string result = string.Format("{0},{1:f3},{2:f3},{3:f3},", Config.MachineName, Const.GnssInfo.LocalCoor_Tipx, Const.GnssInfo.LocalCoor_Tipy, Const.OpcDatasource.PileHeight);
            result += result.ToCharArray().Sum(c => (int)c);
            udp.SendString(result, Const.LocalIp, CHART_REMOTE_PORT);

            _taskLogsBuffer = new List<string>() { result };
        }
    }
}
