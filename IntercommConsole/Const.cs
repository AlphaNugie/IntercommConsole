using CommonLib.Clients;
using CommonLib.Function;
using IntercommConsole.Model;
using SerializationFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole
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

        //private static OpcDatasource _opc_source = new OpcDatasource();
        /// <summary>
        /// OPC数据源
        /// </summary>
        public static OpcDatasource OpcDatasource = new OpcDatasource();
        //{
        //    get { return _opc_source; }
        //    set { _opc_source = value; }
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
