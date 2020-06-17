using CommonLib.Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Tasks
{
    /// <summary>
    /// 策略工控机数据发送任务
    /// </summary>
    public class StrategyServiceTask : Task
    {
        private DerivedUdpClient udp = null;

        /// <summary>
        /// 构造器
        /// </summary>
        public StrategyServiceTask() : base() { }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            Const.WriteConsoleLog("初始化策略工控机数据发送UDP...");
            udp = new DerivedUdpClient(Const.LocalIp, Config.UdpStrategyLocalPort, true, false);
            Const.WriteConsoleLog(string.Format("策略工控机数据发送UDP{0}已启动", udp.Name));
        }

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            //序列化设置
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy() //lowerCamelCase
                    //NamingStrategy = new DefaultNamingStrategy() //UpperCamelCase
                    //NamingStrategy = new SnakeCaseNamingStrategy() //snake_case
                    //NamingStrategy = new KebabCaseNamingStrategy() //kebab-case
                },
                Formatting = Formatting.None //无特殊格式，一行输出
                //Formatting = Formatting.Indented //带缩进多行输出
            };
            string result = JsonConvert.SerializeObject(Const.StrategyDataSource, setting);
            udp.SendString(result, Config.StrategyIPCIp, Config.UdpStrategyRemotePort);

            _taskLogs = new List<string>() { result };
        }
    }
}
