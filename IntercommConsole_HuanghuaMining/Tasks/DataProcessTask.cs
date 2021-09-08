using CommonLib.Clients;
using CommonLib.Clients.Tasks;
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

        private readonly KalmanFilter _kalmanFilter = new KalmanFilter(0.3, 0.7);
        private readonly double _distRadarCentre = 1.2; //落料口雷达距落料口距离

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            //渐变的雷达距离校正值，当斗轮两侧距离差不大于第1阶阈值时采用（否则为0），形如d'=(1-d/m)n，d为两侧距离差值，m为第1阶阈值，n为距离拟合校正值
            double dist_offset = !Const.RadarInfo.DistWheelDiff.Between(0, Config.DistDiffThres[0]) ? 0 : (1 - Const.RadarInfo.DistWheelDiff / Config.DistDiffThres[0]) * Config.DistOffset;
            #region 计算落料点距离
            //余弦定理：根据2边以及2边夹角算出第3边长度=>根据雷达-落料口距离、雷达测算距离、雷达测算角度换算落料口到下方的测距值
            //double d1 = _distRadarCentre, d2 = Const.RadarInfo.DistWheelAverage, a = Const.RadarInfo.AngleWheelAverage;
            //double distCorr = Math.Sqrt(Math.Pow(d1, 2) + Math.Pow(d2, 2) - 2 * d1 * d2 * Math.Cos(90 + a));
            double d1 = _distRadarCentre, d2 = Const.RadarInfo.DistWheelAverage;
            double distCorr = Math.Sqrt(Math.Pow(d2, 2) - Math.Pow(d1, 2));
            //double distCorr = Const.RadarInfo.DistWheelAverage;
            #region 落料口距料堆距离的卡尔曼滤波
            //double value = Const.IsStacker ? Const.RadarInfo.DistWheelAverage + dist_offset : 0;
            double value = Const.IsStacker ? distCorr + dist_offset : 0;
            _kalmanFilter.SetValue(ref value, 0);
            Const.StrategyDataSource.BlankingDistance = value;
            #endregion
            ////落料口到落料点的高度：落料口雷达平均距离+渐变的雷达距离校正值（取料机此值为0）
            //Const.StrategyDataSource.BlankingDistance = Const.IsStacker ? Const.RadarInfo.DistWheelAverage + dist_offset : 0;
            ////落料口到落料点的高度：落料口雷达最小距离+渐变的雷达距离校正值（取料机此值为0）
            //Const.StrategyDataSource.BlankingDistance = Const.IsStacker ? Const.RadarInfo.DistWheelMin + dist_offset : 0;
            Const.OpcDatasource.PileDist = Const.StrategyDataSource.BlankingDistance;
            #endregion
            #region 料堆高度高斯过滤
            //煤堆高度=落料口Z坐标 - 落料口到落料点的高度 + 校正值
            double height = Math.Round(Const.GnssInfo.LocalCoor_Tipz - Const.StrategyDataSource.BlankingDistance + Config.HeightOffset + Config.HeightOffset2, 3);
            _filterSamples.Add(height);
            if (_filterSamples.Count >= (Config.UseGaussianFilter ? Config.FilterLength : 1))
            {
                //估算出的料堆高度
                Const.StrategyDataSource.MaterialHeight = Math.Round(_filterClient.GetGaussianValue(_filterSamples), 3);
                //给PLC传送的高度在估算出的料堆高度上进行修正
                //假如修正类型为固定值，或地形数据为空，或坐标位置超出地形数据索引，则使用固定值修正，否则用对应行走位置的地面高度修正
                int index = (int)Const.GnssInfo.LocalCoor_Tipx - 1; //根据落料口位置X坐标计算索引
                index = index < 0 ? 0 : index;
                double corr = Config.PileHeightCorrType == PileHeightCorrType.SolidValue || Config.GroundHeightValues == null || Config.GroundHeightValues.Count == 0 || index >= Config.GroundHeightValues.Count ? Config.PileHeightCorr : -1 * Config.GroundHeightValues[index];
                Const.OpcDatasource.PileHeight = Const.StrategyDataSource.MaterialHeight + corr;
                //落料口下沿距地面高度=落料口Z坐标 + 地形高度修正 + 校正值
                Const.OpcDatasource.TipHeight = Math.Round(Const.GnssInfo.LocalCoor_Tipz + corr + Config.HeightOffset + Config.HeightOffset2, 3);
                _filterSamples.Clear();
            }
            #endregion
            string result = string.Format("{0:yyyy-MM-dd HH:mm:ss}==>单机{1}落料口XYZ:({2:f3},{3:f3},{4:f3}),落料距离:{5:f2},估算垛高:{6:f2},PLC垛高:{7:f2},行走:{8:f3},俯仰:{9:f3},回转:{10:f3},固定解:{19},收到航向:{28},斗轮左右角度:({11:f2},{12:f2}),左右半径:({23:f2},{24:f2}),左右出垛边:({25},{26}),斗轮逆变:{13},悬皮启动:{14},地面皮带:{27},落料口左右测角({15:f2},{16:f2}),落料口测角均值:{17:f2},斗轮功率:{18:f2},PLC行走、回转速度:({20:f2},{22:f2}),PLC错误:{21}", DateTime.Now, Config.MachineName, Const.GnssInfo.LocalCoor_Tipx, Const.GnssInfo.LocalCoor_Tipy, Const.GnssInfo.LocalCoor_Tipz, Const.StrategyDataSource.BlankingDistance, Const.StrategyDataSource.MaterialHeight, Const.OpcDatasource.PileHeight, Const.GnssInfo.WalkingPosition, Const.GnssInfo.PitchAngle, Const.GnssInfo.YawAngle, Const.RadarInfo.SurfaceAngleWheelLeft, Const.RadarInfo.SurfaceAngleWheelRight, Const.OpcDatasource.WheelTurningBackwards, Const.OpcDatasource.BeltStatus, Const.RadarInfo.AngleWheelLeft, Const.RadarInfo.AngleWheelRight, Const.RadarInfo.AngleWheelAverage, Const.OpcDatasource.WheelPowerPolished, Const.OpcDatasource.IsFixed, Const.OpcDatasource.WalkingSpeed_Plc, Const.OpcDatasource.PlcErrorOccured, Const.OpcDatasource.YawSpeed_Plc, Const.RadarInfo.RadiusAverageLeft, Const.RadarInfo.RadiusAverageRight, Const.OpcDatasource.WheelLeftBeyondStack, Const.OpcDatasource.WheelRightBeyondStack, Const.OpcDatasource.GroundBeltStatus, Const.OpcDatasource.TrackDirection_Received);
            AddLog(result);
            //_taskLogsBuffer = new List<string>() { result };
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
                        //if (!Config.DistBeltThresholdEnabled || (Config.DistBeltThresholdEnabled && Const.OpcDatasource.CoalOnBeltLevel > 1))
                        if (Const.OpcDatasource.CoalOnBeltLevel > 1)
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
