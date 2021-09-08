using CommonLib.Clients.Tasks;
using IntercommConsole.Core;
using IntercommConsole.DataUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    public class DbOracleTask : Task
    {
        private readonly DataService_Machine _dataService = new DataService_Machine(); //数据库链接

        /// <summary>
        /// 构造器
        /// </summary>
        public DbOracleTask() : base() { }

        public override void Init() { }

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            if (!DbDef.Save2Oracle)
                return;

            int gnss_result = 0;
            bool radar_result = false;
            try { gnss_result = _dataService.UpdateMachinePosture(Config.MachineName, Const.RadarInfo, Const.GnssInfo, Const.OpcDatasource); }
            catch (Exception e) { Const.WriteConsoleLog("GNSS数据保存失败：" + e.Message); }
            try { radar_result = _dataService.UpdateRadarInfo(Config.MachineName, Const.RadarInfo); }
            catch (Exception e) { Const.WriteConsoleLog("雷达数据保存失败：" + e.Message); }
            AddLogs(string.Format("GNSS数据保存：{0}", gnss_result), string.Format("雷达数据保存：{0}", radar_result));
            //_taskLogsBuffer = new List<string>() { string.Format("GNSS数据保存：{0}", gnss_result), string.Format("雷达数据保存：{0}", radar_result) };
        }
    }
}
