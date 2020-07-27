using CommonLib.Clients;
using CommonLib.Events;
using CommonLib.Extensions;
using CommonLib.Extensions.Property;
using CommonLib.Function;
using IntercommConsole.Core;
using ProtobufNetLibrary;
using SerializationFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Tasks
{
    public class DataProcessTask : Task
    {
        private readonly DataFilterClient _filterClient = new DataFilterClient(Config.FilterLength, FilterType.Gaussian, Config.Sigma, false);
        private readonly TimerEventRaiser _raiser = new TimerEventRaiser(1000) { RaiseInterval = 3000, RaiseThreshold = (ulong)Math.Round(1000 * Config.BeltSignalDuration) }; //有煤则Click，10秒无Click则无煤
        private readonly List<double> _filterSamples = new List<double>();

        /// <summary>
        /// 构造器
        /// </summary>
        public DataProcessTask() : base() { }

        public override void Init()
        {
            _raiser.ThresholdReached += new TimerEventRaiser.ThresholdReachedEventHandler(Raiser_ThresholdReached);
            _raiser.Clicked += new TimerEventRaiser.ClickedEventHandler(Raiser_Clicked);
            _raiser.Run();

            InitClients();
        }

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            //渐变的雷达距离校正值，当斗轮两侧距离差不大于第1阶阈值时采用（否则为0），形如d'=(1-d/m)n，d为两侧距离差值，m为第1阶阈值，n为距离拟合校正值
            double dist_offset = !Const.RadarInfo.DistWheelDiff.Between(0, Config.DistDiffThres[0]) ? 0 : (1 - Const.RadarInfo.DistWheelDiff / Config.DistDiffThres[0]) * Config.DistOffset;
            //落料口到落料点的高度：落料口雷达平均距离+渐变的雷达距离校正值（取料机此值为0）
            Const.StrategyDataSource.BlankingDistance = Const.IsStacker ? Const.RadarInfo.DistWheelAverage + dist_offset : 0;
            #region 料堆高度高斯过滤
            //煤堆高度=落料口Z坐标 - 落料口到落料点的高度
            //double height = Math.Round(Const.GnssInfo.LocalCoor_Tipz - (Const.IsStacker ? Const.RadarInfo.DistWheelAverage + dist_offset : 0) + Config.HeightOffset + Config.HeightOffset2, 3);
            double height = Math.Round(Const.GnssInfo.LocalCoor_Tipz - Const.StrategyDataSource.BlankingDistance + Config.HeightOffset + Config.HeightOffset2, 3);
            _filterSamples.Add(height);
            if (_filterSamples.Count >= (Config.UseGaussianFilter ? Config.FilterLength : 1))
            {
                Const.StrategyDataSource.MaterialHeight = Const.OpcDatasource.PileHeight = Math.Round(_filterClient.GetGaussianValue(_filterSamples), 3);
                _filterSamples.Clear();
            }
            #endregion
            string result = string.Format("{0:yyyy-MM-dd HH:mm:ss}==>单机{1}落料口XYZ:({2},{3},{4}),落料距离:{5},垛高:{6},行走:{7},俯仰:{8},回转:{9},有料流:{10},料流距离:{11},料流级别:{12},瞬时:{13},PLC行走速度、加速度:({14},{15}),PLC回转速度:{16}", DateTime.Now, Config.MachineName, Const.GnssInfo.LocalCoor_Tipx, Const.GnssInfo.LocalCoor_Tipy, Const.GnssInfo.LocalCoor_Tipz, Const.RadarInfo.DistWheelAverage, Const.OpcDatasource.PileHeight, Const.GnssInfo.WalkingPosition, Const.GnssInfo.PitchAngle, Const.GnssInfo.YawAngle, Const.OpcDatasource.CoalOnBelt, Const.RadarInfo.DistBelt, Const.OpcDatasource.CoalOnBeltLevel, Const.OpcDatasource.StreamPerHour, Const.OpcDatasource.WalkingSpeed_Plc, Const.OpcDatasource.WalkingAcce_Plc, Const.OpcDatasource.YawSpeed_Plc);
            _taskLogsBuffer = new List<string>() { result };
        }

        private void InitClients()
        {
            DerivedTcpClient client1 = new DerivedTcpClient() { ServerIp = "127.0.0.1", ServerPort = Config.RadarPort }, client2 = new DerivedTcpClient() { ServerIp = "127.0.0.1", ServerPort = Config.GnssPort }; //连接雷达和北斗，顺序无所谓
            client1.DataReceived += new DataReceivedEventHandler(Client_DataReceived);
            client2.DataReceived += new DataReceivedEventHandler(Client_DataReceived);
            Const.WriteConsoleLog("等待CLIENT1连接...");
            while (true)
            {
                try { client1.Connect(); }
                catch (Exception) { continue; }
                break;
            }
            Const.WriteConsoleLog(string.Format("{0}已连接\r\n等待CLIENT2连接...", client1.Name));
            while (true)
            {
                try { client2.Connect(); }
                catch (Exception) { continue; }
                break;
            }
            Const.WriteConsoleLog(string.Format("{0}已连接", client2.Name));
        }

        private static void Raiser_Clicked(object sender, ClickedEventArgs e)
        {
            Const.OpcDatasource.CoalOnBelt = true;
        }

        private static void Raiser_ThresholdReached(object sender, ThresholdReachedEventArgs e)
        {
            Const.OpcDatasource.CoalOnBelt = false;
        }

        /// <summary>
        /// 接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void Client_DataReceived(object sender, DataReceivedEventArgs eventArgs)
        {
            byte[] received = eventArgs.ReceivedData;
            ProtoInfoType type = (ProtoInfoType)ProtobufNetWrapper.ReadValueFromByteArray(received, 0);
            _errorMessage = string.Empty;
            try
            {
                switch (type)
                {
                    case ProtoInfoType.GNSS:
                        Const.GnssInfo = ProtobufNetWrapper.DeserializeFromBytes<GnssProtoInfo>(received);
                        Const.GnssInfo.CopyPropertyValueTo(ref Const.OpcDatasource);
                        break;
                    case ProtoInfoType.RADAR:
                        Const.RadarInfo = ProtobufNetWrapper.DeserializeFromBytes<RadarProtoInfo>(received);
                        Const.RadarInfo.CopyPropertyValueTo(ref Const.OpcDatasource);
                        //Const.OpcDatasource.SetWheelBeyondStack(Const.RadarInfo.DistWheelLeft, Const.RadarInfo.DistWheelRight);
                        Const.OpcDatasource.UpdateCoalOnBeltLevel(Const.RadarInfo.DistBelt);
                        //TODO 高于静止无料以及皮带运动无料的级别才判断为有料
                        if (!Config.DistBeltThresholdEnabled || (Config.DistBeltThresholdEnabled && Const.OpcDatasource.CoalOnBeltLevel > 1))
                            _raiser.Click();
                        //if (!Config.DistBeltThresholdEnabled || (Config.DistBeltThresholdEnabled && Const.RadarInfo.DistBelt < Config.DistBeltThreshold))
                        //    _raiser.Click();
                        break;
                }
            }
            catch (Exception e) { _errorMessage = e.Message; }
        }
    }
}
