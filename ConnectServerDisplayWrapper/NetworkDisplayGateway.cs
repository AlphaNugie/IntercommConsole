using gprotocol;
using JavaClientdll;
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
    public static class NetworkDisplayGateway
    {
        #region 属性
        /// <summary>
        /// 服务器IP
        /// </summary>
        public static string ServerIp
        {
            get { return UserMgr.MomoIp; }
            set { UserMgr.MomoIp = value; }
        }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public static int ServerPort
        {
            get { return UserMgr.port; }
            set { UserMgr.port = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName
        {
            get { return UserMgr.Instance.Uname; }
            set
            {
                UserMgr.Instance.Uname = value;
                if (string.IsNullOrWhiteSpace(UserMgr.Instance.Uname))
                    LoginAsGuest = true;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public static string Password
        {
            get { return UserMgr.Instance.Upwd; }
            set
            {
                UserMgr.Instance.Upwd = value;
                if (string.IsNullOrWhiteSpace(UserMgr.Instance.Upwd))
                    LoginAsGuest = true;
            }
        }

        /// <summary>
        /// 是否以访客身份登录，假如用户名或密码中有任意一个为空白，则自动变为true
        /// </summary>
        public static bool LoginAsGuest { get; set; }
        #endregion

        #region 功能
        /// <summary>
        /// 以设置过的服务器IP、端口号以及设置过的身份启动连接
        /// </summary>
        public static bool Start()
        {
            return Start(ServerIp, ServerPort, UserName, Password);
        }

        /// <summary>
        /// 以指定IP地址、端口号以及当前设置过的身份启动连接
        /// </summary>
        /// <param name="server_ip">服务IP</param>
        /// <param name="server_port">服务端口号</param>
        public static bool Start(string server_ip, int server_port)
        {
            return Start(server_ip, server_port, UserName, Password);
        }

        /// <summary>
        /// 以指定IP地址，端口号，用户名以及用户密码启动连接，假如用户名或密码为空则以来客身份登录
        /// </summary>
        /// <param name="server_ip">服务IP</param>
        /// <param name="server_port">服务端口号</param>
        /// <param name="user_name">用户名，假如为空则以来客身份登录</param>
        /// <param name="password">用户密码，假如为空则以来客身份登录</param>
        public static bool Start(string server_ip, int server_port, string user_name, string password)
        {
            ServerIp = server_ip;
            ServerPort = server_port;
            UserName = user_name;
            Password = password;
            try
            {
                UserMgr.Publisher = true;
                if (LoginAsGuest)
                    UserMgr.Instance.init(ServerIp, ServerPort);
                else
                    UserMgr.Instance.init(ServerIp, ServerPort, UserName, Password);
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
        /// <param name="machine_name">单机名称</param>
        /// <param name="working">是否重载</param>
        /// <param name="walking_pos">走行位置</param>
        /// <param name="pitch_angle">俯仰角度</param>
        /// <param name="yaw_angle">回转角度</param>
        /// <param name="dist_land">下沿距地面距离（高程坐标）</param>
        public static bool SendMachineMovements(string machine_name, int working, double walking_pos, double pitch_angle, double yaw_angle, double dist_land)
        {
            try
            {
                //大机名称, 工作状态, 移动, 旋转, 俯仰, 距离地面高度
                MachineMsg.Instance.MachineMovement(machine_name, working, (float)walking_pos, (float)yaw_angle, (float)pitch_angle, (float)dist_land);
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
        /// <param name="machine_name">单机名称</param>
        /// <param name="wheel">斗轮是否转动</param>
        /// <param name="arm_belt">悬皮状态：0 未启动，1 空载，2 重载</param>
        /// <param name="ground_belt">地面皮带状态：0 未启动，1 空载，2 重载</param>
        /// <param name="coal_on_belt">悬皮是否有煤</param>
        public static bool SendMachineWorkStatus(string machine_name, int wheel, int arm_belt, int ground_belt, int coal_on_belt)
        {
            try
            {
                //单机各项状态：悬皮，地面皮带，斗轮
                MachineMsg.Instance.MachineWorkStatus(machine_name, (coal_on_belt + arm_belt) * arm_belt, (coal_on_belt + ground_belt) * ground_belt, wheel);
            }
            catch (Exception)
            {
                Start();
                return false;
            }
            return true;
        }
        #endregion
    }
}
