using CommonLib.DataUtil;
using IntercommConsole.Core;
using IntercommConsole.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.DataUtil
{
    /// <summary>
    /// SQLITE数据库操作对象
    /// </summary>
    public class DataService_Sqlite
    {
        private readonly SqliteProvider provider = new SqliteProvider(Config.SqliteFileDir, Config.SqliteFileName);

        /// <summary>
        /// 获取RCMS UDP地址列表
        /// </summary>
        /// <returns></returns>
        public List<RcmsServiceTask> GetRcmsList()
        {
            string sql = "select * from t_rcms_udplist";
            DataTable table = this.provider.Query(sql);
            if (table == null || table.Rows.Count == 0)
                return null;
            List<RcmsServiceTask> list = table.Rows.Cast<DataRow>().Select(row => new RcmsServiceTask(int.Parse(row["port"].ToString()), row["remote_ip"].ToString(), int.Parse(row["remote_port"].ToString()))).ToList();
            return list;
        }
    }
}
