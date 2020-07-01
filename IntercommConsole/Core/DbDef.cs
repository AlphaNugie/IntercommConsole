using CommonLib.Clients;

namespace IntercommConsole.Core
{
    /// <summary>
    /// 数据库设置
    /// </summary>
    public static class DbDef
    {
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
            HostAddress = Config.IniHelper.ReadData("Oracle", "HostAddress");
            ServiceName = Config.IniHelper.ReadData("Oracle", "ServiceName");
            UserName = Config.IniHelper.ReadData("Oracle", "UserName");
            Password = new EncryptionClient().DecryptAES(Config.IniHelper.ReadData("Oracle", "Password"));
        }
    }
}
