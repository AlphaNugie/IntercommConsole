using CommonLib.Clients;
using CommonLib.Function;
using IntercommConsole.DataUtil;
using IntercommConsole.Model;
using SerializationFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Core
{
    /// <summary>
    /// 变量
    /// </summary>
    public static partial class Const
    {
        private static readonly LogClient _log = new LogClient("logs", "intercomm", "executable.log", false, true);

        /// <summary>
        /// 日志
        /// </summary>
        public static LogClient Log { get { return _log; } }

        private static DataService_Sqlite _dataService = new DataService_Sqlite();
        /// <summary>
        /// Sqlite数据库服务
        /// </summary>
        public static DataService_Sqlite DataServiceSqlite
        {
            get { return _dataService; }
            set { _dataService = value; }
        }

        /// <summary>
        /// 本地IP
        /// </summary>
        public static string LocalIp { get { return Functions.GetIPAddressV4(); } }

        /// <summary>
        /// 是否为堆料机
        /// </summary>
        public static bool IsStacker { get { return Config.MachineType == MachineType.Stacker; } }

        private static RadarProtoInfo _radarInfo = new RadarProtoInfo();
        /// <summary>
        /// 雷达消息
        /// </summary>
        public static RadarProtoInfo RadarInfo
        {
            get { return _radarInfo; }
            set { _radarInfo = value; }
        }

        private static GnssProtoInfo _gnssInfo = new GnssProtoInfo();
        /// <summary>
        /// GNSS消息
        /// </summary>
        public static GnssProtoInfo GnssInfo
        {
            get { return _gnssInfo; }
            set { _gnssInfo = value; }
        }

        /// <summary>
        /// OPC数据源
        /// </summary>
        public static OpcDataSource OpcDatasource = new OpcDataSource();

        /// <summary>
        /// 策略工控机数据源
        /// </summary>
        public static StrategyDataSource StrategyDataSource = new StrategyDataSource();

        /// <summary>
        /// RCMS数据源
        /// </summary>
        public static RcmsDataSource RcmsDataSource = new RcmsDataSource();

        /// <summary>
        /// 北斗单机姿态数据是否可用（行走位置、俯仰角、回转角不全为空）
        /// </summary>
        public static bool IsGnssValid { get { return GnssInfo.WalkingPosition != 0 || GnssInfo.PitchAngle != 0 || GnssInfo.YawAngle != 0; } }

        /// <summary>
        /// PLC单机姿态数据是否可用（行走位置、俯仰角、回转角不全为空）
        /// </summary>
        public static bool IsPlcPostureValid { get { return OpcDatasource.WalkingPositionLeft_Plc != 0 || OpcDatasource.PitchAngle_Plc != 0 || OpcDatasource.YawAngle_Plc != 0; } }

        /// <summary>
        /// 从PLC获取的惯导数据是否可用
        /// </summary>
        public static bool IsPlcInsValid { get; set; }

        /// <summary>
        /// 写入日志同时在控制台输出
        /// </summary>
        /// <param name="info"></param>
        public static void WriteConsoleLog(string info)
        {
            Log.WriteLogsToFile(info);
            Console.WriteLine(info);
        }
    }
}
