using ConnectServerWrapper;
using gprotocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Core
{
    /// <summary>
    /// 变量（不要替换）
    /// </summary>
    public static partial class Const
    {
        /// <summary>
        /// 单机包裹类对象
        /// </summary>
        public static RadioWrapper<Radio_S1> Wrapper = new RadioWrapper<Radio_S1>(Config.MachineName);

        /// <summary>
        /// 单机雷达报警包括类
        /// </summary>
        public static RadioAlarmWrapper<Radio_S1Alarm> WrapperAlarm = new RadioAlarmWrapper<Radio_S1Alarm>();
    }
}
