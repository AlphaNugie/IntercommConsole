using CommonLib.Clients.Tasks;
using CommonLib.Function;
using IntercommConsole.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    public class OpcCommTask : Task
    {
        //private bool _prevSignal = true;

        public override void Init()
        {
            //throw new NotImplementedException();
        }

        public override void LoopContent()
        {
            //假如1个周期过去PLC可读或可写有任意一个为false，则代表出现错误，否则无问题
            Const.OpcDatasource.PlcErrorOccured = !Const.OpcDatasource.PlcReadable || !Const.OpcDatasource.PlcWritable;
            if (Const.OpcDatasource.PlcErrorOccured)
            {
                Const.WriteConsoleLog("PLC出现故障，PLC Error Occured：true");
                //Const.WriteConsoleLog("准备重启操作系统...");
                //ExitWindowsUtils.Reboot(true);
            }
            Const.OpcDatasource.PlcReadable = false;
            Const.OpcDatasource.PlcWritable = false;
            //throw new NotImplementedException();
        }
    }
}
