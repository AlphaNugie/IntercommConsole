using CommonLib.Clients;
using CommonLib.Clients.Tasks;
using CommonLib.Extensions;
using IntercommConsole.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    public class ModelBuildingServiceTask : Task
    {
        private DerivedUdpClient udp = null;

        /// <summary>
        /// 构造器
        /// </summary>
        public ModelBuildingServiceTask() : base() { }

        public override void Init()
        {
            Const.WriteConsoleLog("初始化建模服务数据发送UDP...");
            udp = new DerivedUdpClient(Const.LocalIp, Config.UdpModelLocalPort, true, false);
            Const.WriteConsoleLog(string.Format("建模服务数据发送UDP{0}已启动", udp.Name));
        }

        public override void LoopContent()
        {
            string result = string.Format("{0},{1:f3},{2:f3},{3:f3},", Config.MachineName, Const.GnssInfo.LocalCoor_Tipx, Const.GnssInfo.LocalCoor_Tipy, Const.OpcDatasource.PileHeight);
            result += result.ToCharArray().Sum(c => (int)c);
            //雷达数据是否合格
            bool is_radar_valid = !Const.IsStacker || ( Const.RadarInfo.DistWheelLeft != 0 &&  Const.RadarInfo.DistWheelRight != 0 &&  Const.RadarInfo.DistWheelAverage != 0 &&  Const.RadarInfo.DistWheelDiff.Between(0, Config.DistDiffThres[1]));
            ////单机姿态数据是否合格
            //bool is_gnss_valid =  Const.GnssInfo.WalkingPosition != 0 ||  Const.GnssInfo.PitchAngle != 0 ||  Const.GnssInfo.YawAngle != 0;
            //附加条件
            bool additional = Const.OpcDatasource.PileHeight != 0;
            //皮带上料流是否符合要求
            bool is_coal_valid = !Const.IsStacker || Const.OpcDatasource.CoalOnBelt;
            //所有条件均符合要求后才发送消息，否则发送随机字符
            result = is_radar_valid && Const.IsGnssValid && additional && is_coal_valid ? result : "#";
            udp.SendString(result, Config.ModelServerIp, Config.UdpModelRemotePort);

            _taskLogsBuffer = new List<string>() { result };
        }
    }
}
