using CommonLib.Clients;
using IntercommConsole.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Tasks
{
    /// <summary>
    /// 策略工控机数据发送任务
    /// </summary>
    public class StrategyServiceTask : Task
    {
        private DerivedUdpClient udp = null;
        //序列化设置
        private JsonSerializerSettings _jsonSetting = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy() //大小写格式，lowerCamelCase
            },
            Formatting = Formatting.None //无特殊格式，一行输出
            //Formatting = Formatting.Indented //带缩进多行输出
        };

        /// <summary>
        /// 构造器
        /// </summary>
        public StrategyServiceTask() : base() { }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            Const.WriteConsoleLog("初始化策略工控机数据发送UDP...");
            udp = new DerivedUdpClient(Const.LocalIp, Config.UdpStrategyLocalPort, true, false);
            Const.WriteConsoleLog(string.Format("策略工控机数据发送UDP{0}已启动", udp.Name));
        }

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            //if (Const.IsGnssValid)
            //{
            //    Const.StrategyDataSource.RunningPosition = Const.GnssInfo.WalkingPosition;
            //    Const.StrategyDataSource.PitchAngle = Const.GnssInfo.PitchAngle;
            //    Const.StrategyDataSource.RotationAngle = Const.GnssInfo.YawAngle;
            //}
            Const.StrategyDataSource.RunningPosition = Const.OpcDatasource.WalkingPositionLeft_Plc;
            Const.StrategyDataSource.PitchAngle = Const.OpcDatasource.PitchAngle_Plc;
            Const.StrategyDataSource.RotationAngle = Const.OpcDatasource.YawAngle_Plc;
            Const.StrategyDataSource.CollisionInfo = Const.RadarInfo.RadarList == null ? string.Empty : string.Join(string.Empty, Const.RadarInfo.RadarList.Select(r => Convert.ToString(r.ThreatLevel, 2).PadLeft(2, '0')));
            Const.StrategyDataSource.WheelLeftDist = Const.RadarInfo.DistWheelLeft;
            Const.StrategyDataSource.WheelRightDist = Const.RadarInfo.DistWheelRight;
            string result = JsonConvert.SerializeObject(Const.StrategyDataSource, _jsonSetting);
            udp.SendString(result, Config.StrategyIPCIp, Config.UdpStrategyRemotePort);

            _taskLogsBuffer.Add("已向策略工控机发送：" + result.Length);
            //_taskLogsBuffer = new List<string>() { result };
        }
    }
}
