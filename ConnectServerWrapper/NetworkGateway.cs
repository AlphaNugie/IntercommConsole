using gprotocol;
//using Operation_server;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConnectServerWrapper
{
    /// <summary>
    /// 网络操作包裹类
    /// </summary>
    public static class NetworkGateway
    {
        #region 属性
        /// <summary>
        /// 服务器IP
        /// </summary>
        public static string ServerIp
        {
            get { return network.Instance.server_ip; }
            set { network.Instance.server_ip = value; }
        }

        /// <summary>
        /// UDP服务器IP
        /// </summary>
        public static string UdpServerIp
        {
            get { return network.Instance.udp_server_ip; }
            set { network.Instance.udp_server_ip = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName { get; set; }

        //旧版本
        ///// <summary>
        ///// 用户名
        ///// </summary>
        //public static string UserName
        //{
        //    get { return login_server.Instance.login_name; }
        //    set { login_server.Instance.login_name = value; }
        //}

        //新版本
        /// <summary>
        /// 密码
        /// </summary>
        public static string Password { get; set; }

        //旧版本
        ///// <summary>
        ///// 密码
        ///// </summary>
        //public static string Password
        //{
        //    get { return login_server.Instance.login_upwd; }
        //    set { login_server.Instance.login_upwd = value; }
        //}

        ///// <summary>
        ///// 是否已连上3D模型server
        ///// </summary>
        //public static bool Connected { get { return login_server.Instance.is_connect; } }

        private static bool _logged_in = false;
        /// <summary>
        /// 是否已登录
        /// </summary>
        public static bool LoggedIn
        {
            get { return _logged_in; }
            private set { _logged_in = value; }
        }
        #endregion

        #region 功能
        /// <summary>
        /// 以默认服务器IP、默认UDP IP以及来宾身份启动连接
        /// </summary>
        public static bool Start()
        {
            return Start(ServerIp, UdpServerIp, UserName, Password);
        }

        /// <summary>
        /// 以特定IP地址、默认UDP IP以及来宾身份启动连接
        /// </summary>
        /// <param name="server_ip">服务IP</param>
        public static bool Start(string server_ip)
        {
            return Start(server_ip, UdpServerIp, UserName, Password);
        }

        /// <summary>
        /// 以特定IP地址、特定UDP IP以及来宾身份启动连接
        /// </summary>
        /// <param name="server_ip">服务IP</param>
        /// <param name="udp_server_ip">UDP IP</param>
        public static bool Start(string server_ip, string udp_server_ip)
        {
            return Start(server_ip, udp_server_ip, UserName, Password);
        }

        /// <summary>
        /// 以特定IP地址，UDP IP地址，用户名以及用户密码启动连接，假如用户名或密码为空则以来客身份登录
        /// </summary>
        /// <param name="server_ip">服务IP</param>
        /// <param name="udp_server_ip">UDP服务器IP</param>
        /// <param name="user_name">用户名，假如为空则以来客身份登录</param>
        /// <param name="password">用户密码，假如为空则以来客身份登录</param>
        public static bool Start(string server_ip, string udp_server_ip, string user_name, string password)
        {
            ServerIp = server_ip;
            UdpServerIp = string.IsNullOrWhiteSpace(udp_server_ip) ? server_ip : udp_server_ip;
            try
            {
                event_manager.Instance.add_event_listener("connect success!!!", ConnectSuccess); //登录成功
                //event_manager.Instance.add_event_listener("connect error!!!", ConnectError); //登录失败
                //event_manager.Instance.add_event_listener("login_logic_server", BeginSend); //重新发送
                UserName = user_name;
                Password = password;
                network.Instance.Start();
                //network.Instance.Update();
                //login_server.Instance.init();
                ////Login(UserName, Password);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 以特定IP地址（UDP IP地址默认相同），用户名以及用户密码启动连接，假如用户名或密码为空则以来客身份登录
        /// </summary>
        /// <param name="server_ip">服务IP</param>
        /// <param name="user_name">用户名，假如为空则以来客身份登录</param>
        /// <param name="password">用户密码，假如为空则以来客身份登录</param>
        public static bool Start(string server_ip, string user_name, string password)
        {
            return Start(server_ip, null, user_name, password);
        }

        /// <summary>
        /// 以特定用户名以及用户密码登录
        /// </summary>
        public static bool Login()
        {
            return Login(UserName, Password);
        }

        /// <summary>
        /// 以特定用户名以及用户密码登录，假如用户名或密码为空则以来客身份登录
        /// </summary>
        /// <param name="user_name">用户名，假如为空则以来客身份登录</param>
        /// <param name="password">用户密码，假如为空则以来客身份登录</param>
        public static bool Login(string user_name, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user_name) || string.IsNullOrWhiteSpace(password))
                    login_server.Instance.guest_login();
                else
                {
                    UserName = user_name;
                    Password = password;
                    //login_server.Instance.on_login(UserName, Password); //新版本
                    login_server.Instance.User_login(UserName, Password); //新版本
                    //login_server.Instance.on_login(); //旧版本
                }
            }
            catch (Exception)
            {
                return LoggedIn = false;
            }
            return LoggedIn = true;
        }

        /// <summary>
        /// 发送单机数据
        /// </summary>
        /// <param name="cmd">单机类型</param>
        /// <param name="body">发送实体类</param>
        public static bool SendProtobufCmd(SendMode mode, Cmd cmd, IExtensible body)
        {
            //if (!LoggedIn)
            //    return false;
            try {
                switch (mode)
                {
                    case SendMode.TCP:
                        network.Instance.send_protobuf_cmd((int)Stype.Logic, (int)cmd, body);
                        break;
                    case SendMode.UDP:
                        network.Instance.udp_send_protobuf_cmd((int)Stype.Logic, (int)cmd, body);
                        break;
                }
            }
            catch (Exception)
            {
                Start();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送单机数据
        /// </summary>
        /// <param name="cmd">单机类型</param>
        /// <param name="body">发送实体类</param>
        public static bool SendProtobufCmd(Cmd cmd, IExtensible body)
        {
            return SendProtobufCmd(SendMode.TCP, cmd, body);
            ////if (!LoggedIn)
            ////    return false;
            //try { network.Instance.send_protobuf_cmd((int)Stype.Logic, (int)cmd, body); }
            //catch (Exception)
            //{
            //    Start();
            //    return false;
            //}
            //return true;
        }
        #endregion

        private static void ConnectSuccess(string name, object udata)
        {
            Login();
        }

        //private static void ConnectError(string name, object udata)
        //{
        //    //throw new NotImplementedException();
        //}

        //private static void BeginSend(string name, object udata)
        //{
        //    LoggedIn = true;
        //    //throw new NotImplementedException();
        //}
    }

    public enum SendMode
    {
        TCP = 1, 

        UDP
    }
}
