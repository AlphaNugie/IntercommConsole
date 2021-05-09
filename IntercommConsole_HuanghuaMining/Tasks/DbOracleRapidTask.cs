using CommonLib.Clients.Tasks;
using IntercommConsole.Core;
using IntercommConsole.DataUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    public class DbOracleRapidTask : Task
    {
        private readonly DataService_Machine _dataService = new DataService_Machine(); //数据库链接

        /// <summary>
        /// 构造器
        /// </summary>
        public DbOracleRapidTask() : base() { }

        public override void Init() { }

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            if (!DbDef.Save2Oracle)
                return;
            try { int gnss_result = _dataService.UpdateMachinePosture2(Config.MachineName, Const.RadarInfo, Const.GnssInfo); }
            catch (Exception) {  }
        }
    }
}
