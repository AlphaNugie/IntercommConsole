using IntercommConsole.Core;
using IntercommConsole.DataUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Tasks
{
    public class DbSqliteTask : Task
    {
        private readonly DataService_Radar _dataService = new DataService_Radar(); //数据库链接

        /// <summary>
        /// 构造器
        /// </summary>
        public DbSqliteTask() : base() { }

        public override void Init() { }

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            if (!Config.Save2Sqlite)
                return;

            int sqlite_result = 0;
            try { sqlite_result = _dataService.InsertRadarDistance(Const.GnssInfo, Const.RadarInfo); }
            catch (Exception e) { Const.WriteConsoleLog("Sqlite保存失败：" + e.Message); }
            _taskLogsBuffer = new List<string>() { string.Format("Sqlite数据保存：{0}", sqlite_result) };
        }
    }
}
