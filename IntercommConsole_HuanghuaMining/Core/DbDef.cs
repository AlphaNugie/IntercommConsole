using CommonLib.Clients;

namespace IntercommConsole.Core
{
    /// <summary>
    /// 数据库设置
    /// </summary>
    public static class DbDef
    {
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
            Save2Oracle = Config.IniHelper.ReadData("Oracle", "Save2Oracle").Equals("1");
            HostAddress = Config.IniHelper.ReadData("Oracle", "HostAddress");
            ServiceName = Config.IniHelper.ReadData("Oracle", "ServiceName");
            UserName = Config.IniHelper.ReadData("Oracle", "UserName");
            Password = new EncryptionClient().DecryptAES(Config.IniHelper.ReadData("Oracle", "Password"));
        }
    }
}
