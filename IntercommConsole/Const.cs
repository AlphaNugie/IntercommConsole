using CommonLib.Clients;
using CommonLib.Function;
using ConnectServerWrapper;
using gprotocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole
{
    /// <summary>
    /// 常量
    /// </summary>
    public struct Const
    {
        private static readonly IniFileHelper ini_helper = new IniFileHelper("Config.ini");

        /// <summary>
        /// 日志
        /// </summary>
        public static readonly LogClient Log = new LogClient("logs", "intercomm", "executable.log", false, true);

        /// <summary>
        /// 大机名称
        /// </summary>
        public static string MachineName = ini_helper.ReadData("Main", "MachineName");

        /// <summary>
        /// 大机类型
        /// </summary>
        public static MachineType MachineType = (MachineType)int.Parse(ini_helper.ReadData("Main", "MachineType"));

        /// <summary>
        /// 单机包裹类对象
        /// </summary>
        public static RadioWrapper<Radio_S1> Wrapper = new RadioWrapper<Radio_S1>(MachineName);

        /// <summary>
        /// 高度非负校正值
        /// </summary>
        public static double HeightOffset = double.Parse(ini_helper.ReadData("Main", "HeightOffset"));

        /// <summary>
        /// 高度备用校正值
        /// </summary>
        public static double HeightOffset2 = double.Parse(ini_helper.ReadData("Main", "HeightOffset2"));

        /// <summary>
        /// 距离拟合校正值
        /// </summary>
        public static double DistOffset = double.Parse(ini_helper.ReadData("Main", "DistOffset"));

        /// <summary>
        /// 距离差阈值，具有两个分界点，小于a时补上拟合校正值，大于等于a小于b时取平均值，大于b时放弃
        /// </summary>
        public static double[] DistDiffThres = ini_helper.ReadData("Main", "DistDiffThres").Split(',').Select(d => double.Parse(d)).ToArray();

        /// <summary>
        /// 建模服务器IP
        /// </summary>
        public static string ModelServerIp = ini_helper.ReadData("Main", "ModelServerIp");

        /// <summary>
        /// 雷达子系统端口
        /// </summary>
        public static int RadarPort = int.Parse(ini_helper.ReadData("Main", "RadarPort"));

        /// <summary>
        /// 北斗子系统端口
        /// </summary>
        public static int GnssPort = int.Parse(ini_helper.ReadData("Main", "GnssPort"));

        /// <summary>
        /// UDP本地端口
        /// </summary>
        public static int UdpLocalPort = int.Parse(ini_helper.ReadData("Main", "UdpLocalPort"));

        /// <summary>
        /// UDP远程端口
        /// </summary>
        public static int UdpRemotePort = int.Parse(ini_helper.ReadData("Main", "UdpRemotePort"));

        /// <summary>
        /// 是否将数据保存到Sqlite
        /// </summary>
        public static bool Save2Sqlite = ini_helper.ReadData("Main", "Save2Sqlite").Equals("1");

        /// <summary>
        /// 是否将数据保存到Oracle数据库
        /// </summary>
        public static bool Save2Oracle = ini_helper.ReadData("Main", "Save2Oracle").Equals("1");

        /// <summary>
        /// 数据库服务器IP
        /// </summary>
        public static string DataServerIp = ini_helper.ReadData("Main", "DataServerIp");

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName = ini_helper.ReadData("Main", "UserName");

        /// <summary>
        /// 用户密码
        /// </summary>
        public static string Password = ini_helper.ReadData("Main", "Password");

        /// <summary>
        /// 是否使用OPC
        /// </summary>
        public static bool OpcEnabled = ini_helper.ReadData("OPC", "OpcEnabled").Equals("1");

        /// <summary>
        /// OPC SERVER IP地址
        /// </summary>
        public static string OpcServerIp = ini_helper.ReadData("OPC", "OpcServerIp");

        /// <summary>
        /// OPC SERVER 名称
        /// </summary>
        public static string OpcServerName = ini_helper.ReadData("OPC", "OpcServerName");

        /// <summary>
        /// 是否写入PLC
        /// </summary>
        public static bool Write2Plc = ini_helper.ReadData("OPC", "Write2Plc").Equals("1");
    }

    /// <summary>
    /// 大机类型
    /// </summary>
    public enum MachineType
    {
        /// <summary>
        /// 堆料机
        /// </summary>
        Stacker = 1,

        /// <summary>
        /// 取料机
        /// </summary>
        Reclaimer = 2
    }
}
