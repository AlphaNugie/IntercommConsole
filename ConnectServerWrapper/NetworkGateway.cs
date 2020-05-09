using gprotocol;
using Operation_server;
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
        /// <summary>
        /// 服务器IP
        /// </summary>
        public static string ServerIp
        {
            get { return network.Instance.server_ip; }
            set { network.Instance.server_ip = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName
        {
            get { return login_server.Instance.login_name; }
            set { login_server.Instance.login_name = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public static string Password
        {
            get { return login_server.Instance.login_upwd; }
            set { login_server.Instance.login_upwd = value; }
        }

        /// <summary>
        /// 以默认服务器IP以及来宾身份启动连接
        /// </summary>
        public static bool Start()
        {
            return Start(ServerIp, UserName, Password);
        }

        /// <summary>
        /// 以特定IP地址以及来宾身份启动连接
        /// </summary>
        /// <param name="server_ip">服务IP</param>
        public static bool Start(string server_ip)
        {
            return Start(server_ip, UserName, Password);
        }

        /// <summary>
        /// 以特定IP地址，用户名以及用户密码启动连接，假如用户名或密码为空则以来客身份登录
        /// </summary>
        /// <param name="server_ip">服务IP</param>
        /// <param name="user_name">用户名，假如为空则以来客身份登录</param>
        /// <param name="password">用户密码，假如为空则以来客身份登录</param>
        public static bool Start(string server_ip, string user_name, string password)
        {
            ServerIp = server_ip;
            try
            {
                network.Instance.Start();
                network.Instance.Update();
                login_server.Instance.init();
                if (string.IsNullOrWhiteSpace(user_name) || string.IsNullOrWhiteSpace(password))
                    login_server.Instance.guest_login();
                else
                {
                    UserName = user_name;
                    Password = password;
                    login_server.Instance.on_login();
                }
            }
            catch (Exception)
            {
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
            try { network.Instance.send_protobuf_cmd((int)Stype.Logic, (int)cmd, body); }
            catch (Exception)
            {
                Start();
                return false;
            }
            return true;
        }
    }
}
