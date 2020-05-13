using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Clients;
using CommonLib.Events;
using System.Threading;
//using SocketHelper;
//using CarServer;
using CommonLib.Function;
using System.Net.Sockets;
using System.Net;
using System.IO;
using ProtobufNetLibrary;
using SerializationFactory;
//using static SocketHelper.SocketTcpClient;
using gprotocol;
using ConnectServerWrapper;
using CommonLib.Extensions;

namespace IntercommConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string local_ip = Functions.GetIPAddressV4(); //本地IP
            DataService_Radar dataservice = new DataService_Radar(); //数据库链接
            DataService_Machine dataservice_machine = new DataService_Machine();
            //RadarProtoInfo info = new RadarProtoInfo() { DistWheelAverage = 7.5, RadarList = new List<RadarInfoDetail>() { new RadarInfoDetail() { CurrentDistance = 7.5 } } };
            //string resulttemp = ProtobufNetWrapper.SerializeToString(info);
            //RadarProtoInfo info2 = ProtobufNetWrapper.DeserializeFromString<RadarProtoInfo>(resulttemp);

            #region 堆场模型浏览服务连接
            NetworkGateway.Start(Const.DataServerIp, Const.UserName, Const.Password);
            Console.WriteLine(string.Format("已向服务器{0}发起连接请求...", NetworkGateway.ServerIp));
            #endregion
            #region 连接雷达与北斗
            //SocketTcpClient client1 = new SocketTcpClient() { ServerIp = "127.0.0.1", ServerPort = Const.RadarPort }, client2 = new SocketTcpClient() { ServerIp = "127.0.0.1", ServerPort = Const.GnssPort }; //连接雷达和北斗，顺序无所谓
            //client1.OnReceive += new ReceivedEventHandler(Client_DataReceived);
            //client2.OnReceive += new ReceivedEventHandler(Client_DataReceived);
            DerivedTcpClient client1 = new DerivedTcpClient() { ServerIp = "127.0.0.1", ServerPort = Const.RadarPort }, client2 = new DerivedTcpClient() { ServerIp = "127.0.0.1", ServerPort = Const.GnssPort }; //连接雷达和北斗，顺序无所谓
            client1.DataReceived += new DataReceivedEventHandler(Client_DataReceived);
            client2.DataReceived += new DataReceivedEventHandler(Client_DataReceived);
            Console.WriteLine("等待CLIENT1连接...");
            //while (true)
            //    if (client1.StartConnection())
            //        break;
            while (true)
            {
                try { client1.Connect(); }
                catch (Exception) { continue; }
                break;
            }
            Console.WriteLine(string.Format("{0}已连接\r\n等待CLIENT2连接...", client1.Name));
            //while (true)
            //    if (client2.StartConnection())
            //        break;
            while (true)
            {
                try { client2.Connect(); }
                catch (Exception) { continue; }
                break;
            }
            Console.WriteLine(string.Format("{0}已连接", client2.Name));
            #endregion
            #region 启动TCPSERVER
            //Console.WriteLine(string.Format("{0}已连接\r\n启动SERVER...", client2.Name));
            //SocketTcpServer server = new SocketTcpServer() { ServerIp = "127.0.0.1", ServerPort = 25004 };
            //server.Start();
            //Console.WriteLine(string.Format("{0}已启动\r\n初始化UDP...", server.Name));
            #endregion
            #region 初始化UDP
            Console.WriteLine("初始化UDP...");
            DerivedUdpClient udp = new DerivedUdpClient(local_ip, Const.UdpLocalPort, true, false);
            Console.WriteLine(string.Format("{0}已启动", udp.Name));
            #endregion
            #region 第3个TCPCLIENT
            //DerivedTcpClient client3 = new DerivedTcpClient() { ServerIp = "127.0.0.1", ServerPort = 25006 };
            //Console.WriteLine("等待CLIENT3连接...");
            //while (true)
            //{
            //    try { client3.Connect(); }
            //    catch (Exception) { continue; }
            //    break;
            //}
            //Console.WriteLine(string.Format("{0}已连接", client3.Name));
            #endregion
            #region 数据发送
            bool ended = false, is_stacker = Const.MachineType == MachineType.Stacker;
            Thread thread = new Thread(new ThreadStart(() =>
            {
                while (!ended)
                {
                    Thread.Sleep(1000);
                    if (radar_info.DistWheelAverage == 0)
                        continue;
                    //渐变的雷达距离校正值，当斗轮两侧距离差不大于第1阶阈值时采用（否则为0），形如d'=(1-d/m)n，d为两侧距离差值，m为第1阶阈值，n为距离拟合校正值
                    double dist_offset = !radar_info.DistWheelDiff.Between(0, Const.DistDiffThres[0]) ? 0 : (1 - radar_info.DistWheelDiff / Const.DistDiffThres[0]) * Const.DistOffset;
                    //煤堆高度=落料口Z坐标 - (斗轮雷达平均距离+渐变的雷达距离校正值)（取料机此值为0）
                    double height = Math.Round(gnss_info.LocalCoor_Tipz - (is_stacker ? radar_info.DistWheelAverage + dist_offset : 0) + Const.HeightOffset + Const.HeightOffset2, 3);
                    string result = string.Format("{0},{1:f3},{2:f3},{3:f3},", Const.MachineName, gnss_info.LocalCoor_Tipx, gnss_info.LocalCoor_Tipy, height);
                    string result1 = string.Format("{0:yyyy-MM-dd HH:mm:ss}==>单机{1}落料口X:{2},落料口Y:{3},落料口Z:{4},雷达距离:{5},煤堆高度:{6},行走:{7},俯仰:{8},回转:{9}", DateTime.Now, Const.MachineName, gnss_info.LocalCoor_Tipx, gnss_info.LocalCoor_Tipy, gnss_info.LocalCoor_Tipz, radar_info.DistWheelAverage, height, gnss_info.WalkingPosition, gnss_info.PitchAngle, gnss_info.YawAngle);
                    result += result.ToCharArray().Sum<char>(c => (int)c);
                    //server.SendData(result);
                    //雷达数据是否合格
                    bool is_radar_valid = !is_stacker || (radar_info.DistWheelLeft != 0 && radar_info.DistWheelRight != 0 && radar_info.DistWheelDiff.Between(0, Const.DistDiffThres[1]));
                    //单机姿态数据是否合格
                    bool is_gnss_valid = gnss_info.WalkingPosition != 0 || gnss_info.PitchAngle != 0 || gnss_info.YawAngle != 0;
                    #region 向建模服务器发送
                    if (is_radar_valid && is_gnss_valid)
                    {
                        udp.SendString(result, Const.ModelServerIp, Const.UdpRemotePort);
                        Console.WriteLine(result);
                    }
                    #endregion
                    #region 向数据库服务器发送
                    if (is_gnss_valid)
                    {
                        try
                        {
                            Const.Wrapper.Walking = (float)gnss_info.WalkingPosition;
                            Const.Wrapper.PitchAngle = (float)gnss_info.PitchAngle;
                            Const.Wrapper.YawAngle = (float)gnss_info.YawAngle;
                            NetworkGateway.SendProtobufCmd(Const.Wrapper.MachineType, Const.Wrapper.Instance);
                            Console.WriteLine("已向3维成像服务器发送数据...");
                        }
                        catch (Exception) { }
                    }
                    #endregion
                    #region 保存到数据库（sqlite3 / Oracle）
                    if (Const.Save2Oracle)
                    {
                        int ora_result = 0;
                        try { ora_result = dataservice_machine.UpdateMachinePosture(Const.MachineName, gnss_info); }
                        catch (Exception) { }
                        Console.WriteLine(string.Format("Oracle数据保存：{0}", ora_result));
                    }
                    if (Const.Save2Sqlite)
                    {
                        int sqlite_result = dataservice.InsertRadarDistance(gnss_info, radar_info);
                        Console.WriteLine(string.Format("Sqlite数据保存：{0}", sqlite_result));
                    }
                    #endregion

                    Console.WriteLine(result1);
                    Console.WriteLine();
                    //client3.SendString(result);
                }
            }))
            { IsBackground = true };
            #endregion

            Console.WriteLine("回车以发送数据，再次回车结束");
            Console.ReadLine();
            thread.Start();
            Console.ReadLine();
            ended = true;
            Thread.Sleep(2000);
            Console.WriteLine("回车退出");
            Console.ReadLine();
        }

        private static RadarProtoInfo radar_info = new RadarProtoInfo(); //雷达信息
        private static GnssProtoInfo gnss_info = new GnssProtoInfo(); //北斗信息
        private static string message = string.Empty;


        /// <summary>
        /// 接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private static void Client_DataReceived(object sender, DataReceivedEventArgs eventArgs)
        {
            byte[] received = eventArgs.ReceivedData;
            ProtoInfoType type = (ProtoInfoType)ProtobufNetWrapper.ReadValueFromByteArray(received, 0);
            message = string.Empty;
            try
            {
                switch (type)
                {
                    case ProtoInfoType.GNSS:
                        gnss_info = ProtobufNetWrapper.DeserializeFromBytes<GnssProtoInfo>(received);
                        break;
                    case ProtoInfoType.RADAR:
                        radar_info = ProtobufNetWrapper.DeserializeFromBytes<RadarProtoInfo>(received);
                        break;
                }
            }
            catch (Exception e) { message = e.Message; }
        }
    }
}
