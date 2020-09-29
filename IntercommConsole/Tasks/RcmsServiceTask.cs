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

        public string IpAddress { get { return Const.LocalIp; } }

        public int Port { get; set; }

        public string RemoteIpAddress { get; set; }

        public int RemotePort { get; set; }

        /// <summary>
        /// 构造器
        /// </summary>
        public RcmsServiceTask(int port, string remote_ip, int remote_port) : base()
        {
            this.Port = port;
            this.RemoteIpAddress = remote_ip;
            this.RemotePort = remote_port;
        }

        public override void Init()
        {
            Const.WriteConsoleLog("初始化UDP...");
            udp = new DerivedUdpClient(this.IpAddress, this.Port, true, false);
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
            udp.SendString(json, this.RemoteIpAddress, this.RemotePort);

            _taskLogsBuffer.Add(string.Format("已向{0}:{1}发送，长度:{2}", this.RemoteIpAddress, this.RemotePort, string.IsNullOrWhiteSpace(json) ? 0 : json.Length));
        }
    }
}
