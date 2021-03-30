using CommonLib.Clients;
using CommonLib.Function;

namespace ChartDisplay
{
    /// <summary>
    /// 设置
    /// </summary>
    public static class Def
    {
        private static readonly IniFileHelper _iniHelper = new IniFileHelper(@"..\..\..\IntercommConsole\bin\Debug\Config.ini");
        /// <summary>
        /// 配置文件管理器
        /// </summary>
        public static IniFileHelper IniHelper { get { return _iniHelper; } }

        /// <summary>
        /// 单机名称
        /// </summary>
        public static string MachineName { get; set; }

        /// <summary>
        /// 是否将数据保存到Oracle数据库
        /// </summary>
        public static bool Save2Oracle { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public static string HostAddress { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public static string ServiceName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public static string Password { get; set; }

        /// <summary>
        /// 更新数据库设置
        /// </summary>
        public static void Update()
        {
            MachineName = _iniHelper.ReadData("Main", "MachineName");
            Save2Oracle = _iniHelper.ReadData("Oracle", "Save2Oracle").Equals("1");
            HostAddress = _iniHelper.ReadData("Oracle", "HostAddress");
            ServiceName = _iniHelper.ReadData("Oracle", "ServiceName");
            UserName = _iniHelper.ReadData("Oracle", "UserName");
            Password = new EncryptionClient().DecryptAES(_iniHelper.ReadData("Oracle", "Password"));
        }
    }
}
