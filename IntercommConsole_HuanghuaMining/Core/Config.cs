using CommonLib.Clients;
using CommonLib.Function;
using ConnectServerWrapper;
using gprotocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IntercommConsole.Core
{
    /// <summary>
    /// 配置
    /// </summary>
    public partial class Config
    {
        private static readonly IniFileHelper _iniHelper = new IniFileHelper("Config.ini");

        /// <summary>
        /// INI配置文件读取工具
        /// </summary>
        public static IniFileHelper IniHelper { get { return _iniHelper; } }

        /// <summary>
        /// 刷新配置的线程
        /// </summary>
        public static Thread Thread_RefreshConfigs { get; set; }

        /// <summary>
        /// 大机名称
        /// </summary>
        public static string MachineName { get; set; }

        /// <summary>
        /// 大机类型
        /// </summary>
        public static MachineType MachineType { get; set; }

        #region Main
        /// <summary>
        /// 高度非负校正值
        /// </summary>
        public static double HeightOffset { get; set; }

        /// <summary>
        /// 高度备用校正值
        /// </summary>
        public static double HeightOffset2 { get; set; }

        /// <summary>
        /// 距离拟合校正值
        /// </summary>
        public static double DistOffset { get; set; }

        /// <summary>
        /// 距离差阈值，具有两个分界点，小于a时补上拟合校正值，大于等于a小于b时取平均值，大于b时放弃
        /// </summary>
        public static List<double> DistDiffThres { get; set; }

        /// <summary>
        /// 建模服务器IP
        /// </summary>
        public static string ModelServerIp { get; set; }

        /// <summary>
        /// 建模服务数据发送UDP本地端口
        /// </summary>
        public static int UdpModelLocalPort { get; set; }

        /// <summary>
        /// 建模服务数据发送UDP远程端口
        /// </summary>
        public static int UdpModelRemotePort { get; set; }

        /// <summary>
        /// 策略工控机IP
        /// </summary>
        public static string StrategyIPCIp { get; set; }

        /// <summary>
        /// 策略工控机数据发送UDP本地端口
        /// </summary>
        public static int UdpStrategyLocalPort { get; set; }

        /// <summary>
        /// 策略工控机数据发送UDP远程端口
        /// </summary>
        public static int UdpStrategyRemotePort { get; set; }

        /// <summary>
        /// 雷达子系统端口
        /// </summary>
        public static int RadarPort { get; set; }

        /// <summary>
        /// 北斗子系统端口
        /// </summary>
        public static int GnssPort { get; set; }

        /// <summary>
        /// 建模服务IP
        /// </summary>
        public static string DataServerIp { get; set; }

        /// <summary>
        /// 建模UDP服务IP
        /// </summary>
        public static string DataUdpServerIp { get; set; }

        /// <summary>
        /// 建模纯展示服务IP
        /// </summary>
        public static string DataDisplayServerIp { get; set; }

        /// <summary>
        /// 建模纯展示服务端口
        /// </summary>
        public static int DataDisplayServerPort { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public static string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserNameDisplay { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public static string PasswordDisplay { get; set; }
        #endregion

        #region OPC
        /// <summary>
        /// 是否使用OPC
        /// </summary>
        public static bool OpcEnabled { get; set; }

        /// <summary>
        /// OPC SERVER IP地址
        /// </summary>
        public static string OpcServerIp { get; set; }

        /// <summary>
        /// OPC SERVER 名称
        /// </summary>
        public static string OpcServerName { get; set; }

        /// <summary>
        /// 是否写入PLC
        /// </summary>
        public static bool Write2Plc { get; set; }

        /// <summary>
        /// OPC读取与写入间隔（毫秒）
        /// </summary>
        public static int OpcLoopInterval { get; set; }

        /// <summary>
        /// 向PLC写入的垛位高度修正值类型
        /// </summary>
        public static PileHeightCorrType PileHeightCorrType { get; set; }

        /// <summary>
        /// 垛位高度修正的固定值
        /// </summary>
        public static double PileHeightCorr { get; set; }

        /// <summary>
        /// 地面高度基础数据列表
        /// </summary>
        public static List<double> GroundHeightValues { get; set; }
        #endregion

        #region Calc
        /// <summary>
        /// 是否使用高斯滤波
        /// </summary>
        public static bool UseGaussianFilter { get; set; }

        /// <summary>
        /// 滤波样本数量
        /// </summary>
        public static ushort FilterLength { get; set; }

        /// <summary>
        /// 高斯分布（正态分布）标准差，越大越发散，越小越集中
        /// </summary>
        public static double Sigma { get; set; }
        #endregion

        #region Belt
        ///// <summary>
        ///// 是否使用料流雷达距离判断是否有料流
        ///// </summary>
        //public static bool DistBeltThresholdEnabled { get; set; }

        /// <summary>
        /// 建模时料流有效性判断模式
        /// </summary>
        public static CoalValidMode CoalValidMode { get; set; }

        ///// <summary>
        ///// 料流距离阈值
        ///// </summary>
        //public static double DistBeltThreshold { get; set; }

        /// <summary>
        /// 根据料流雷达距离判断料流大小的阶段阈值（大于等于最大的值为0级）
        /// </summary>
        public static List<double> DistBeltLevels { get; set; }

        /// <summary>
        /// 料流信号维持的时间长度（秒）
        /// </summary>
        public static double BeltSignalDuration { get; set; }
        #endregion

        #region Wheel
        /// <summary>
        /// 出料堆判断斗轮两侧雷达距离差的阈值
        /// </summary>
        public static double BeyondStackThreshold { get; set; }

        /// <summary>
        /// 出料堆判断两侧雷达测距值的分界线
        /// </summary>
        public static double BeyondStackBorder { get; set; }

        /// <summary>
        /// 出料堆判断两侧雷达拟合平面角度的阈值（低于此值则出边界）
        /// </summary>
        public static double BeyondStackAngleThreshold { get; set; }

        /// <summary>
        /// 平面角度的阈值是否可用，不可用则不参与判断
        /// </summary>
        public static bool BeyondStackAngleEnabled { get; set; }

        /// <summary>
        /// 出料堆的底线距离，有一侧距离超出此值，则亦判断出料堆
        /// </summary>
        public static double BeyondStackBaseline { get; set; }

        /// <summary>
        /// 出料堆的底线距离是否可用，不可用则不参与判断
        /// </summary>
        public static bool BeyondStackBaseEnabled { get; set; }

        /// <summary>
        /// 是否启用出垛边角度记录
        /// </summary>
        public static bool EnableAngleRecording { get; set; }

        /// <summary>
        /// 垛边角度记录数据源
        /// </summary>
        public static AngleSource AngleRecordingSource { get; set; }
        #endregion

        #region PostureAdjustment
        /// <summary>
        /// 单机姿态多系统校正过程中行走位置的阈值，超过此阈值则为偏离
        /// </summary>
        public static double WalkingThreshold { get; set; }

        /// <summary>
        /// 单机姿态多系统校正过程中俯仰角的阈值，超过此阈值则为偏离
        /// </summary>
        public static double PitchThreshold { get; set; }

        /// <summary>
        /// 单机姿态多系统校正过程中回转角的阈值，超过此阈值则为偏离
        /// </summary>
        public static double YawThreshold { get; set; }
        #endregion

        #region Sqlite
        /// <summary>
        /// 是否将数据保存到Sqlite
        /// </summary>
        public static bool Save2Sqlite { get; set; }

        private static string _sqlite_file_dir = string.Empty;
        /// <summary>
        /// Sqlite文件路径，可为相对路径
        /// </summary>
        public static string SqliteFileDir
        {
            get { return _sqlite_file_dir; }
            set { _sqlite_file_dir = string.IsNullOrWhiteSpace(value) || value.Contains("not found") ? string.Empty : value; }
        }

        private static string _sqlite_file_name = "base.db";
        /// <summary>
        /// Sqlite文件名称，包括后缀
        /// </summary>
        public static string SqliteFileName
        {
            get { return _sqlite_file_name; }
            set { _sqlite_file_name = string.IsNullOrWhiteSpace(value) || value.Contains("not found") ? "base.db" : value; }
        }
        #endregion

        /// <summary>
        /// 配置文件初始化
        /// </summary>
        public static void Init()
        {
            Thread_RefreshConfigs = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    try { Update(); }
                    catch (Exception) { }
                    Thread.Sleep(1000);
                }
            }))
            { IsBackground = true };
            Thread_RefreshConfigs.Start();
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        public static void Update()
        {
            MachineName = _iniHelper.ReadData("Main", "MachineName");
            MachineType = (MachineType)int.Parse(_iniHelper.ReadData("Main", "MachineType"));

            #region Main
            HeightOffset = double.Parse(_iniHelper.ReadData("Main", "HeightOffset"));
            HeightOffset2 = double.Parse(_iniHelper.ReadData("Main", "HeightOffset2"));
            DistOffset = double.Parse(_iniHelper.ReadData("Main", "DistOffset"));
            DistDiffThres = _iniHelper.ReadData("Main", "DistDiffThres").Split(',').Select(d => double.Parse(d.Trim())).ToList();
            ModelServerIp = _iniHelper.ReadData("Main", "ModelServerIp");
            UdpModelLocalPort = int.Parse(_iniHelper.ReadData("Main", "UdpLocalPort"));
            UdpModelRemotePort = int.Parse(_iniHelper.ReadData("Main", "UdpRemotePort"));
            StrategyIPCIp = _iniHelper.ReadData("Main", "StrategyIPCIp");
            UdpStrategyLocalPort = int.Parse(_iniHelper.ReadData("Main", "UdpStrategyLocalPort"));
            UdpStrategyRemotePort = int.Parse(_iniHelper.ReadData("Main", "UdpStrategyRemotePort"));
            RadarPort = int.Parse(_iniHelper.ReadData("Main", "RadarPort"));
            GnssPort = int.Parse(_iniHelper.ReadData("Main", "GnssPort"));
            DataServerIp = _iniHelper.ReadData("Main", "DataServerIp");
            DataUdpServerIp = _iniHelper.ReadData("Main", "DataUdpServerIp");
            DataDisplayServerIp = _iniHelper.ReadData("Main", "DataDisplayServerIp");
            DataDisplayServerPort = int.Parse(_iniHelper.ReadData("Main", "DataDisplayServerPort"));
            UserName = _iniHelper.ReadData("Main", "UserName");
            Password = _iniHelper.ReadData("Main", "Password");
            UserNameDisplay = _iniHelper.ReadData("Main", "UserNameDisplay");
            PasswordDisplay = _iniHelper.ReadData("Main", "PasswordDisplay");
            #endregion

            #region OPC
            OpcEnabled = _iniHelper.ReadData("OPC", "OpcEnabled").Equals("1");
            OpcServerIp = _iniHelper.ReadData("OPC", "OpcServerIp");
            OpcServerName = _iniHelper.ReadData("OPC", "OpcServerName");
            Write2Plc = _iniHelper.ReadData("OPC", "Write2Plc").Equals("1");
            OpcLoopInterval = int.Parse(_iniHelper.ReadData("OPC", "LoopInterval"));
            PileHeightCorrType = (PileHeightCorrType)int.Parse(_iniHelper.ReadData("OPC", "PileHeightCorrType"));
            PileHeightCorr = double.Parse(_iniHelper.ReadData("OPC", "PileHeightCorr"));
            try { GroundHeightValues = File.ReadAllLines(_iniHelper.ReadData("OPC", "GroundHeightFile")).Select(line => double.Parse(line)).ToList(); }
            catch (Exception) { }
            #endregion

            #region Calc
            UseGaussianFilter = _iniHelper.ReadData("Calc", "UseGaussianFilter").Equals("1");
            FilterLength = ushort.Parse(_iniHelper.ReadData("Calc", "FilterLength"));
            Sigma = double.Parse(_iniHelper.ReadData("Calc", "Sigma"));
            #endregion

            #region Belt
            //DistBeltThresholdEnabled = _iniHelper.ReadData("Belt", "UseThreshold").Equals("1");
            CoalValidMode = (CoalValidMode)int.Parse(_iniHelper.ReadData("Belt", "CoalValidMode"));
            //DistBeltThreshold = double.Parse(_iniHelper.ReadData("Belt", "DistBeltThreshold"));
            DistBeltLevels = _iniHelper.ReadData("Belt", "DistBeltLevels").Split(',').Select(s => double.Parse(s.Trim())).OrderByDescending(s => s).ToList();
            BeltSignalDuration = double.Parse(_iniHelper.ReadData("Belt", "Duration"));
            #endregion

            #region Wheel
            //BeyondStackThreshold = double.Parse(_iniHelper.ReadData("Wheel", "BeyondStackThreshold"));
            //BeyondStackBorder = double.Parse(_iniHelper.ReadData("Wheel", "BeyondStackBorder"));
            BeyondStackAngleThreshold = double.Parse(_iniHelper.ReadData("Wheel", "BeyondStackAngleThreshold"));
            BeyondStackAngleEnabled = _iniHelper.ReadData("Wheel", "BeyondStackAngleEnabled").Equals("1");
            BeyondStackBaseline = double.Parse(_iniHelper.ReadData("Wheel", "BeyondStackBaseline"));
            BeyondStackBaseEnabled = _iniHelper.ReadData("Wheel", "BeyondStackBaseEnabled").Equals("1");
            EnableAngleRecording = _iniHelper.ReadData("Wheel", "EnableAngleRecording").Equals("1");
            AngleRecordingSource = (AngleSource)int.Parse(_iniHelper.ReadData("Wheel", "AngleRecordingSource"));
            #endregion

            #region PostureAdjustment
            WalkingThreshold = double.Parse(_iniHelper.ReadData("PostureAdjustment", "WalkingThreshold"));
            PitchThreshold = double.Parse(_iniHelper.ReadData("PostureAdjustment", "PitchThreshold"));
            YawThreshold = double.Parse(_iniHelper.ReadData("PostureAdjustment", "YawThreshold"));
            Posture.Type = (PostureType)int.Parse(_iniHelper.ReadData("PostureAdjustment", "PostureType"));
            #endregion

            #region Sqlite
            Save2Sqlite = _iniHelper.ReadData("Sqlite", "Save2Sqlite").Equals("1");
            SqliteFileDir = _iniHelper.ReadData("Sqlite", "FileDir");
            SqliteFileName = _iniHelper.ReadData("Sqlite", "FileName");
            #endregion
        }
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

    /// <summary>
    /// 角度数据源
    /// </summary>
    public enum AngleSource
    {
        PLC = 1,

        BEIDOU = 2
    }

    /// <summary>
    /// 向PLC写入的垛位高度修正值类型
    /// </summary>
    public enum PileHeightCorrType
    {
        /// <summary>
        /// 固定数值修正
        /// </summary>
        SolidValue = 1,

        /// <summary>
        /// 地形基础数据修正(地面高度文本文件)
        /// </summary>
        GroundHeightFile = 2
    }

    /// <summary>
    /// 料流有效性判断模式
    /// </summary>
    public enum CoalValidMode
    {
        /// <summary>
        /// 料流永远有效（无论料流雷达或悬皮的状态如何）
        /// </summary>
        AlwaysValid = 0,

        /// <summary>
        /// 根据皮带料流雷达判断
        /// </summary>
        RadarDist = 1,

        /// <summary>
        /// 根据悬皮皮带状态判断
        /// </summary>
        BeltStatus = 2
    }
}
