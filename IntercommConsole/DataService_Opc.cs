using CommonLib.DataUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole
{
    public class DataService_Opc
    {
        private readonly SqliteProvider provider = new SqliteProvider(string.Empty, "base.db");

        public DataTable GetOpcInfo()
        {
            string sql = "select * from t_plc_opcgroup g left join t_plc_opcitem i on g.group_id = i.opcgroup_id order by g.group_id, i.record_id";
            return this.provider.Query(sql);
        }
    }
}
