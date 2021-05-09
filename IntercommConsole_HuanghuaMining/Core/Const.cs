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
        public static string LocalIp { get { return Functions.GetIPAddressV4("172.18."); } }

        /// <summary>
        /// 是否为堆料机
        /// </summary>
        public static bool IsStacker { get { return Config.MachineType == MachineType.Stacker; } }

        /// <summary>
        /// 雷达应用是否连接
        /// </summary>
        public static bool RadarClientConnected { get; set; }

        private static RadarProtoInfo _radarInfo = new RadarProtoInfo();
        /// <summary>
        /// 雷达消息
        /// </summary>
        public static RadarProtoInfo RadarInfo
        {
            get { return _radarInfo; }
            set { _radarInfo = value; }
        }

        /// <summary>
        /// GNSS应用是否连接
        /// </summary>
        public static bool GnssClientConnected { get; set; }

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
        public static bool IsGnssValid { get { return GnssInfo.IsFixed && GnssInfo.TrackDirection_Received && (GnssInfo.WalkingPosition != 0 || GnssInfo.PitchAngle != 0 || GnssInfo.YawAngle != 0); } }

        /// <summary>
        /// PLC单机姿态数据是否可用（行走位置、俯仰角、回转角不全为空）
        /// </summary>
        public static bool IsPlcPostureValid { get { return OpcDatasource.WalkingPosition_Plc != 0 || OpcDatasource.PitchAngle_Plc != 0 || OpcDatasource.YawAngle_Plc != 0; } }

        /// <summary>
        /// 从PLC获取的惯导数据是否可用
        /// </summary>
        public static bool IsPlcInsValid { get; set; }

        /// <summary>
        /// 悬皮料流是否符合要求
        /// </summary>
        public static bool IsCoalValid
        {
            get
            {
                if (IsStacker)
                {
                    switch (Config.CoalValidMode)
                    {
                        case CoalValidMode.AlwaysValid:
                            return true;
                        case CoalValidMode.RadarDist:
                            return OpcDatasource.CoalOnBelt;
                        case CoalValidMode.BeltStatus:
                            return OpcDatasource.BeltStatus == 1;
                        default:
                            return false;
                    }
                }
                else
                    return true;
            }
        }

        ///// <summary>
        ///// 是否位于底层
        ///// </summary>
        //public static bool OnBottomLevel { get { return OpcDatasource.PileHeight < 2; } }

        ///// <summary>
        ///// 判断是否有料流
        ///// </summary>
        ///// <returns></returns>
        //public static bool IsCoalValid()
        //{
        //    bool is_coal_valid = !IsStacker; //假如不是堆料机，则料流状态始终为true
        //    //堆料机进行额外判断
        //    if (IsStacker)
        //    {
        //        switch (Config.CoalValidMode)
        //        {
        //            case CoalValidMode.AlwaysValid:
        //                is_coal_valid = true;
        //                break;
        //            case CoalValidMode.RadarDist:
        //                is_coal_valid = OpcDatasource.CoalOnBelt;
        //                break;
        //            case CoalValidMode.BeltStatus:
        //                is_coal_valid = OpcDatasource.BeltStatus == 1;
        //                break;
        //        }
        //    }
        //    return is_coal_valid;
        //}

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
