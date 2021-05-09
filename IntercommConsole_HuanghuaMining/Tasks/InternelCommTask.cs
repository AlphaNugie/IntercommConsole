using CommonLib.Clients;
using CommonLib.Clients.Tasks;
using CommonLib.Extensions;
using CommonLib.Function;
using IntercommConsole.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    public class InternalCommTask : Task
    {
        private DerivedUdpClient _udp = null;
        private readonly Dictionary<int, string> _dict = new Dictionary<int, string>();
        //本地UDP端口，远程端口1（目的地未知），远程端口2（ChartDisplay.FormBlockDistChart），远程端口3（RadarDataGrabber）
        private const int CHART_LOCAL_PORT = 43991, CHART_REMOTE_PORT1 = 43992, CHART_REMOTE_PORT2 = 43993, CHART_REMOTE_PORT3 = 43994;
        private string _fileName = @"D:\MachinePostures\" + Config.MachineName + ".txt";

        /// <summary>
        /// 构造器
        /// </summary>
        public InternalCommTask() : base() { }

        public override void Init()
        {
            _udp = new DerivedUdpClient(Const.LocalIp, CHART_LOCAL_PORT, true, false);
        }

        public override void LoopContent()
        {
            _dict.Clear();
            _dict.Add(CHART_REMOTE_PORT1, string.Format("{0},{1:f3},{2:f3},{3:f3}", Config.MachineName, Const.GnssInfo.LocalCoor_Tipx, Const.GnssInfo.LocalCoor_Tipy, Const.StrategyDataSource.MaterialHeight));
            _dict.Add(CHART_REMOTE_PORT2, string.Format("{0},{1:f3},{2:f3},{3:f3},{4:f3},{5:f3},{6:f3}", Config.MachineName, Const.RadarInfo.DistLeftFront, Const.RadarInfo.DistLeftMiddle, Const.RadarInfo.DistLeftBack, Const.RadarInfo.DistRightFront, Const.RadarInfo.DistRightMiddle, Const.RadarInfo.DistRightBack));
            _dict.Add(CHART_REMOTE_PORT3, string.Format("{0},{1:f3},{2:f3},{3:f3},{4:f3},{5:f3},{6:f3}", Config.MachineName, Const.GnssInfo.LocalCoor_Tipx, Const.GnssInfo.LocalCoor_Tipy, Const.GnssInfo.LocalCoor_Tipz, Const.GnssInfo.LocalCoor_Centrex, Const.GnssInfo.LocalCoor_Centrey, Const.GnssInfo.LocalCoor_Centrez));
            List<int> ports = _dict.Keys.Cast<int>().ToList();
            ports.ForEach(port =>
            {
                _dict[port] = HexHelper.GetStringSumResult(_dict[port]);
                try { _udp.SendString(_dict[port], Const.LocalIp, port); }
                catch (Exception) { }
            });
        }
    }
}
