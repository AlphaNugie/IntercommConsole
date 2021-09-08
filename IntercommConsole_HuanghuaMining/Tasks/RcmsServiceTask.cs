using CommonLib.Clients;
using CommonLib.Clients.Tasks;
using IntercommConsole.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SerializationFactory.Genuine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntercommConsole.Tasks
{
    public class RcmsServiceTask : Task
    {
        private DerivedUdpClient udp = null;

        public string Name { get; set; }

        public string IpAddress { get { return Const.LocalIp; } }

        public int Port { get; set; }

        public string RemoteIpAddress { get; set; }

        public int RemotePort { get; set; }

        /// <summary>
        /// RCMS通信服务构造器
        /// </summary>
        /// <param name="name">RCMS客户端名称</param>
        /// <param name="port">本地端口</param>
        /// <param name="remote_ip">客户端远程IP</param>
        /// <param name="remote_port">客户端远程端口</param>
        public RcmsServiceTask(string name, int port, string remote_ip, int remote_port) : base()
        {
            Name = name;
            Port = port;
            RemoteIpAddress = remote_ip;
            RemotePort = remote_port;
        }

        public override void Init()
        {
            Const.WriteConsoleLog("初始化UDP...");
            udp = new DerivedUdpClient(IpAddress, Port, true, false);
            Const.WriteConsoleLog(string.Format("{0}已启动", udp.Name));
        }

        public override void LoopContent()
        {
            string json = JsonConvert.SerializeObject(Const.RcmsDataSource, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new DefaultNamingStrategy() },
                Formatting = Formatting.None
            });
            //MachineInfo info = JsonConvert.DeserializeObject<MachineInfo>(json);
            udp.SendString(json, RemoteIpAddress, RemotePort);

            AddLog(string.Format("已向{0}:{1}发送，长度:{2}", RemoteIpAddress, RemotePort, string.IsNullOrWhiteSpace(json) ? 0 : json.Length));
            //_taskLogsBuffer.Add(string.Format("已向{0}:{1}发送，长度:{2}", RemoteIpAddress, RemotePort, string.IsNullOrWhiteSpace(json) ? 0 : json.Length));
        }
    }
}
