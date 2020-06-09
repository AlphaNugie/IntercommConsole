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
    /// 配置
    /// </summary>
    public partial struct Config
    {
        private static readonly IniFileHelper _iniHelper = new IniFileHelper("Config.ini");

        /// <summary>
        /// 大机名称
        /// </summary>
        public static string MachineName = _iniHelper.ReadData("Main", "MachineName");

        /// <summary>
        /// 大机类型
        /// </summary>
        public static MachineType MachineType = (MachineType)int.Parse(_iniHelper.ReadData("Main", "MachineType"));

        ///// <summary>
        ///// 单机包裹类对象
        ///// </summary>
        //public static RadioWrapper<Radio_S1> Wrapper = new RadioWrapper<Radio_S1>(MachineName);

        #region Main
        /// <summary>
        /// 高度非负校正值
        /// </summary>
        public static double HeightOffset = double.Parse(_iniHelper.ReadData("Main", "HeightOffset"));

        /// <summary>
        /// 高度备用校正值
        /// </summary>
        public static double HeightOffset2 = double.Parse(_iniHelper.ReadData("Main", "HeightOffset2"));

        /// <summary>
        /// 距离拟合校正值
        /// </summary>
        public static double DistOffset = double.Parse(_iniHelper.ReadData("Main", "DistOffset"));

        /// <summary>
        /// 距离差阈值，具有两个分界点，小于a时补上拟合校正值，大于等于a小于b时取平均值，大于b时放弃
        /// </summary>
        public static double[] DistDiffThres = _iniHelper.ReadData("Main", "DistDiffThres").Split(',').Select(d => double.Parse(d)).ToArray();

        /// <summary>
        /// 建模服务器IP
        /// </summary>
        public static string ModelServerIp = _iniHelper.ReadData("Main", "ModelServerIp");

        /// <summary>
        /// 雷达子系统端口
        /// </summary>
        public static int RadarPort = int.Parse(_iniHelper.ReadData("Main", "RadarPort"));

        /// <summary>
        /// 北斗子系统端口
        /// </summary>
        public static int GnssPort = int.Parse(_iniHelper.ReadData("Main", "GnssPort"));

        /// <summary>
        /// UDP本地端口
        /// </summary>
        public static int UdpLocalPort = int.Parse(_iniHelper.ReadData("Main", "UdpLocalPort"));

        /// <summary>
        /// UDP远程端口
        /// </summary>
        public static int UdpRemotePort = int.Parse(_iniHelper.ReadData("Main", "UdpRemotePort"));

        /// <summary>
        /// 是否将数据保存到Sqlite
        /// </summary>
        public static bool Save2Sqlite = _iniHelper.ReadData("Main", "Save2Sqlite").Equals("1");

        /// <summary>
        /// 是否将数据保存到Oracle数据库
        /// </summary>
        public static bool Save2Oracle = _iniHelper.ReadData("Main", "Save2Oracle").Equals("1");

        /// <summary>
        /// 数据库服务器IP
        /// </summary>
        public static string DataServerIp = _iniHelper.ReadData("Main", "DataServerIp");

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName = _iniHelper.ReadData("Main", "UserName");

        /// <summary>
        /// 用户密码
        /// </summary>
        public static string Password = _iniHelper.ReadData("Main", "Password");
        #endregion

        #region OPC
        /// <summary>
        /// 是否使用OPC
        /// </summary>
        public static bool OpcEnabled = _iniHelper.ReadData("OPC", "OpcEnabled").Equals("1");

        /// <summary>
        /// OPC SERVER IP地址
        /// </summary>
        public static string OpcServerIp = _iniHelper.ReadData("OPC", "OpcServerIp");

        /// <summary>
        /// OPC SERVER 名称
        /// </summary>
        public static string OpcServerName = _iniHelper.ReadData("OPC", "OpcServerName");

        /// <summary>
        /// 是否写入PLC
        /// </summary>
        public static bool Write2Plc = _iniHelper.ReadData("OPC", "Write2Plc").Equals("1");
        #endregion

        #region Calc
        /// <summary>
        /// 是否使用高斯滤波
        /// </summary>
        public static bool UseGaussianFilter = _iniHelper.ReadData("Calc", "UseGaussianFilter").Equals("1");

        /// <summary>
        /// 滤波样本数量
        /// </summary>
        public static ushort FilterLength = ushort.Parse(_iniHelper.ReadData("Calc", "FilterLength"));

        /// <summary>
        /// 高斯分布（正态分布）标准差，越大越发散，越小越集中
        /// </summary>
        public static double Sigma = double.Parse(_iniHelper.ReadData("Calc", "Sigma"));
        #endregion

        #region Belt
        /// <summary>
        /// 是否使用
        /// </summary>
        public static bool DistBeltThresholdEnabled = _iniHelper.ReadData("Belt", "UseThreshold").Equals("1");

        /// <summary>
        /// 料流距离阈值
        /// </summary>
        public static double DistBeltThreshold = double.Parse(_iniHelper.ReadData("Belt", "DistBeltThreshold"));

        /// <summary>
        /// 料流信号维持的时间长度（秒）
        /// </summary>
        public static double BeltSignalDuration = double.Parse(_iniHelper.ReadData("Belt", "Duration"));
        #endregion
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
