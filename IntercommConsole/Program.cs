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
using ConnectServerWrapper;
using CommonLib.Extensions;
using OpcLibrary;
using System.Data;

namespace IntercommConsole
{
    class Program
    {
        private static RadarProtoInfo radar_info = new RadarProtoInfo(); //雷达信息
        private static GnssProtoInfo gnss_info = new GnssProtoInfo(); //北斗信息
        private static OpcUtilHelper opcHelper = new OpcUtilHelper(1000, true);
        private static string message = string.Empty/*, temp = string.Empty*/;

        /// <summary>
        /// GNSS消息
        /// </summary>
        private static GnssProtoInfo GnssInfo
        {
            get { return gnss_info; }
            set
            {
                gnss_info = value;
                opcHelper.ListGroupInfo.ForEach(group => group.DataSource = gnss_info);
            }
        }

        private static void WriteConsoleLog(string info)
        {
            Const.Log.WriteLogsToFile(info);
            Console.WriteLine(info);
        }

        static void Main(string[] args)
        {
            string localIp = Functions.GetIPAddressV4(); //本地IP
            WriteConsoleLog("IntercommConsole启动，本地IP: " + localIp);
            //Console.WriteLine("IntercommConsole启动，本地IP: " + localIp);
            DataService_Radar dataService = new DataService_Radar(); //数据库链接
            DataService_Machine dataService_Machine = new DataService_Machine();
            DataService_Opc dataService_Opc = new DataService_Opc();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException_Raising); //未捕获异常触发事件

            #region OPC
            OpcInit();
            //OpcWriteValues();
            #endregion
            #region 堆场模型浏览服务连接
            NetworkGateway.Start(Const.DataServerIp, Const.UserName, Const.Password);
            WriteConsoleLog(string.Format("已向服务器{0}发起连接请求...", NetworkGateway.ServerIp));
            //Console.WriteLine(string.Format("已向服务器{0}发起连接请求...", NetworkGateway.ServerIp));
            #endregion
            #region 连接雷达与北斗
            //SocketTcpClient client1 = new SocketTcpClient() { ServerIp = "127.0.0.1", ServerPort = Const.RadarPort }, client2 = new SocketTcpClient() { ServerIp = "127.0.0.1", ServerPort = Const.GnssPort }; //连接雷达和北斗，顺序无所谓
            //client1.OnReceive += new ReceivedEventHandler(Client_DataReceived);
            //client2.OnReceive += new ReceivedEventHandler(Client_DataReceived);
            DerivedTcpClient client1 = new DerivedTcpClient() { ServerIp = "127.0.0.1", ServerPort = Const.RadarPort }, client2 = new DerivedTcpClient() { ServerIp = "127.0.0.1", ServerPort = Const.GnssPort }; //连接雷达和北斗，顺序无所谓
            client1.DataReceived += new DataReceivedEventHandler(Client_DataReceived);
            client2.DataReceived += new DataReceivedEventHandler(Client_DataReceived);
            WriteConsoleLog("等待CLIENT1连接...");
            //Console.WriteLine("等待CLIENT1连接...");
            //while (true)
            //    if (client1.StartConnection())
            //        break;
            while (true)
            {
                try { client1.Connect(); }
                catch (Exception) { continue; }
                break;
            }
            WriteConsoleLog(string.Format("{0}已连接\r\n等待CLIENT2连接...", client1.Name));
            //Console.WriteLine(string.Format("{0}已连接\r\n等待CLIENT2连接...", client1.Name));
            //while (true)
            //    if (client2.StartConnection())
            //        break;
            while (true)
            {
                try { client2.Connect(); }
                catch (Exception) { continue; }
                break;
            }
            WriteConsoleLog(string.Format("{0}已连接", client2.Name));
            //Console.WriteLine(string.Format("{0}已连接", client2.Name));
            #endregion
            #region 启动TCPSERVER
            //Console.WriteLine(string.Format("{0}已连接\r\n启动SERVER...", client2.Name));
            //SocketTcpServer server = new SocketTcpServer() { ServerIp = "127.0.0.1", ServerPort = 25004 };
            //server.Start();
            //Console.WriteLine(string.Format("{0}已启动\r\n初始化UDP...", server.Name));
            #endregion
            #region 初始化UDP
            WriteConsoleLog("初始化UDP...");
            //Console.WriteLine("初始化UDP...");
            DerivedUdpClient udp = new DerivedUdpClient(localIp, Const.UdpLocalPort, true, false);
            WriteConsoleLog(string.Format("{0}已启动", udp.Name));
            //Console.WriteLine(string.Format("{0}已启动", udp.Name));
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
                    Thread.Sleep(900);
                    //if (radar_info.DistWheelAverage == 0)
                    //{
                    //    Console.WriteLine("雷达平均距离0，跳过此次循环");
                    //    continue;
                    //}
                    //渐变的雷达距离校正值，当斗轮两侧距离差不大于第1阶阈值时采用（否则为0），形如d'=(1-d/m)n，d为两侧距离差值，m为第1阶阈值，n为距离拟合校正值
                    double dist_offset = !radar_info.DistWheelDiff.Between(0, Const.DistDiffThres[0]) ? 0 : (1 - radar_info.DistWheelDiff / Const.DistDiffThres[0]) * Const.DistOffset;
                    //煤堆高度=落料口Z坐标 - (斗轮雷达平均距离+渐变的雷达距离校正值)（取料机此值为0）
                    GnssInfo.PileHeight = Math.Round(GnssInfo.LocalCoor_Tipz - (is_stacker ? radar_info.DistWheelAverage + dist_offset : 0) + Const.HeightOffset + Const.HeightOffset2, 3);
                    //double height = Math.Round(GnssInfo.LocalCoor_Tipz - (is_stacker ? radar_info.DistWheelAverage + dist_offset : 0) + Const.HeightOffset + Const.HeightOffset2, 3);
                    string result = string.Format("{0},{1:f3},{2:f3},{3:f3},", Const.MachineName, GnssInfo.LocalCoor_Tipx, GnssInfo.LocalCoor_Tipy, GnssInfo.PileHeight);
                    string result1 = string.Format("{0:yyyy-MM-dd HH:mm:ss}==>单机{1}落料口X:{2},落料口Y:{3},落料口Z:{4},雷达距离:{5},煤堆高度:{6},行走:{7},俯仰:{8},回转:{9}", DateTime.Now, Const.MachineName, GnssInfo.LocalCoor_Tipx, GnssInfo.LocalCoor_Tipy, GnssInfo.LocalCoor_Tipz, radar_info.DistWheelAverage, GnssInfo.PileHeight, GnssInfo.WalkingPosition, GnssInfo.PitchAngle, GnssInfo.YawAngle);
                    result += result.ToCharArray().Sum<char>(c => (int)c);
                    //server.SendData(result);
                    //雷达数据是否合格
                    bool is_radar_valid = !is_stacker || (radar_info.DistWheelLeft != 0 && radar_info.DistWheelRight != 0 && radar_info.DistWheelAverage != 0 && radar_info.DistWheelDiff.Between(0, Const.DistDiffThres[1]));
                    //单机姿态数据是否合格
                    bool is_gnss_valid = GnssInfo.WalkingPosition != 0 || GnssInfo.PitchAngle != 0 || GnssInfo.YawAngle != 0;
                    #region 向建模服务器发送
                    if (is_radar_valid && is_gnss_valid)
                    {
                        udp.SendString(result, Const.ModelServerIp, Const.UdpRemotePort);
                        //WriteConsoleLog(result);
                        Console.WriteLine(result);
                    }
                    #endregion
                    #region 向数据库服务器发送
                    if (is_gnss_valid)
                    {
                        try
                        {
                            Const.Wrapper.Walking = (float)GnssInfo.WalkingPosition;
                            Const.Wrapper.PitchAngle = (float)GnssInfo.PitchAngle;
                            Const.Wrapper.YawAngle = (float)GnssInfo.YawAngle;
                            NetworkGateway.SendProtobufCmd(Const.Wrapper.MachineType, Const.Wrapper.Instance);
                            //WriteConsoleLog("已向3维成像服务器发送数据...");
                            Console.WriteLine("已向3维成像服务器发送数据...");
                        }
                        catch (Exception) { }
                    }
                    #endregion
                    #region 保存到数据库（sqlite3 / Oracle）
                    if (Const.Save2Oracle)
                    {
                        int ora_result = 0;
                        try { ora_result = dataService_Machine.UpdateMachinePosture(Const.MachineName, GnssInfo); }
                        catch (Exception) { }
                        //WriteConsoleLog(string.Format("Oracle数据保存：{0}", ora_result));
                        Console.WriteLine(string.Format("Oracle数据保存：{0}", ora_result));
                    }
                    if (Const.Save2Sqlite)
                    {
                        int sqlite_result = dataService.InsertRadarDistance(GnssInfo, radar_info);
                        //WriteConsoleLog(string.Format("Sqlite数据保存：{0}", sqlite_result));
                        Console.WriteLine(string.Format("Sqlite数据保存：{0}", sqlite_result));
                    }
                    #endregion
                    #region 写入到PLC
                    OpcWriteValues();
                    #endregion

                    Console.WriteLine(result1);
                    Console.WriteLine();
                    //client3.SendString(result);
                }
            }))
            { IsBackground = true };
            #endregion

            int i = 3;
            while(i > 0)
            {
                Console.WriteLine(i--);
                Thread.Sleep(1000);
            }
            //Console.WriteLine("回车以发送数据，再次回车结束");
            //Console.ReadLine();
            thread.Start();
            Console.ReadLine();
            ended = true;
            Thread.Sleep(2000);
            Console.WriteLine("回车退出");
            Console.ReadLine();
        }

        /// <summary>
        /// 未捕获异常触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void UnhandledException_Raising(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            FileClient.WriteFailureInfo(new string[] { string.Format("未处理异常被触发，运行时是否即将终止：{0}，错误信息：{1}", args.IsTerminating, e.Message), e.StackTrace, e.TargetSite.ToString() }, "UnhandledException", "unhandled " + DateTime.Now.ToString("yyyy-MM-dd"));
            //FileClient.WriteExceptionInfo(e, "未处理异常被触发，运行时是否即将终止：" + args.IsTerminating, true);
        }

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
                        double pileHeight = GnssInfo.PileHeight;
                        GnssInfo = ProtobufNetWrapper.DeserializeFromBytes<GnssProtoInfo>(received);
                        GnssInfo.PileHeight = pileHeight;
                        break;
                    case ProtoInfoType.RADAR:
                        radar_info = ProtobufNetWrapper.DeserializeFromBytes<RadarProtoInfo>(received);
                        break;
                }
            }
            catch (Exception e) { message = e.Message; }
        }

        #region OPC
        private static void OpcInit()
        {
            if (!Const.OpcEnabled)
                return;

            WriteConsoleLog(string.Format("开始连接IP地址为{0}的OPC SERVER {1}...", Const.OpcServerIp, Const.OpcServerName));
            //Console.WriteLine(string.Format("开始连接IP地址为{0}的OPC SERVER {1}...", Const.OpcServerIp, Const.OpcServerName));
            DataService_Opc dataService_Opc = new DataService_Opc();
            opcHelper = new OpcUtilHelper(1000, true);
            string[] servers = opcHelper.ServerEnum(Const.OpcServerIp, out message);
            if (!string.IsNullOrWhiteSpace(message))
            {
                WriteConsoleLog(string.Format("枚举过程中出现问题：{0}", message));
                //Console.WriteLine(string.Format("枚举过程中出现问题：{0}", message));
                goto END_OF_OPC;
            }
            if (servers == null || !servers.Contains(Const.OpcServerName))
            {
                WriteConsoleLog(string.Format("无法找到指定OPC SERVER：{0}", Const.OpcServerName));
                //Console.WriteLine(string.Format("无法找到指定OPC SERVER：{0}", Const.OpcServerName));
                goto END_OF_OPC;
            }
            DataTable table = dataService_Opc.GetOpcInfo();
            if (table == null || table.Rows.Count == 0)
            {
                WriteConsoleLog(string.Format("在表中未找到任何OPC记录，将不进行读取或写入", Const.OpcServerName));
                //Console.WriteLine(string.Format("在表中未找到任何OPC记录，将不进行读取或写入", Const.OpcServerName));
                goto END_OF_OPC;
            }
            List<OpcGroupInfo> groups = new List<OpcGroupInfo>();
            List<DataRow> dataRows = table.Rows.Cast<DataRow>().ToList();
            List<OpcItemInfo> items = null;
            int id = 0;
            foreach (var row in dataRows)
            {
                string itemId = row["item_id"].ConvertType<string>();
                if (string.IsNullOrWhiteSpace(itemId))
                    continue;
                int groupId = row["group_id"].ConvertType<int>(), clientHandle = row["record_id"].ConvertType<int>();
                string groupName = row["group_name"].ConvertType<string>(), fieldName = row["field_name"].ConvertType<string>();
                GroupType type = (GroupType)row["group_type"].ConvertType<int>();
                if (groupId != id)
                {
                    id = groupId;
                    groups.Add(new OpcGroupInfo(null, groupName, GnssInfo) { GroupType = type, ListItemInfo = new List<OpcItemInfo>() });
                    OpcGroupInfo groupInfo = groups.Last();
                    items = groupInfo.ListItemInfo;
                }
                items.Add(new OpcItemInfo(itemId, clientHandle, fieldName));
            }
            opcHelper.ListGroupInfo = groups;
            opcHelper.ConnectRemoteServer(Const.OpcServerIp, Const.OpcServerName, out message);
            WriteConsoleLog(string.Format("OPC连接状态：{0}", opcHelper.OpcConnected));
            //Console.WriteLine(string.Format("OPC连接状态：{0}", opcHelper.OpcConnected));
            if (!string.IsNullOrWhiteSpace(message))
                WriteConsoleLog(string.Format("连接过程中出现问题：{0}", message));
                //Console.WriteLine(string.Format("连接过程中出现问题：{0}", message));
            END_OF_OPC:;
        }

        private static void OpcWriteValues()
        {
            if (!Const.OpcEnabled && !Const.Write2Plc)
                return;

            //GnssInfo.WalkingPosition = 542.492;
            //GnssInfo.PitchAngle = 11.2;
            //GnssInfo.YawAngle = 15.12;
            //GnssInfo.PileHeight = 15.23;
            opcHelper.ListGroupInfo.ForEach(group =>
            {
                if (group.GroupType != GroupType.WRITE)
                    return;

                if (!group.WriteValues(out message))
                    WriteConsoleLog(string.Format("写入PLC失败，写入过程中出现问题：{0}", message));
                    //Console.WriteLine(string.Format("写入PLC失败，写入过程中出现问题：{0}", message));
                //group.WriteValues(out message);
                //if (!string.IsNullOrWhiteSpace(message))
                //    Console.WriteLine(string.Format("写入PLC过程中出现问题：{0}", message));
            });
        }
        #endregion
    }
}
