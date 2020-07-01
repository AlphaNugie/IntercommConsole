//using ARS408.Model;
using CommonLib.DataUtil;
using IntercommConsole.Core;
using SerializationFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.DataUtil
{
    /// <summary>
    /// 雷达SQLITE服务类
    /// </summary>
    public class DataService_Radar
    {
        //private readonly SqliteProvider provider = new SqliteProvider(string.Empty, "base.db");
        private readonly SqliteProvider provider = new SqliteProvider(Config.SqliteFileDir, Config.SqliteFileName);

        /// <summary>
        /// 插入雷达测距值
        /// </summary>
        /// <param name="radar_id">雷达ID</param>
        /// <param name="dist">距离</param>
        /// <returns></returns>
        public int InsertRadarDistance(double tipx, double tipy, double tipz, double radar_left, double radar_right, double radar_aveg)
        {
            string sqlString = string.Format("insert into t_radar_distances_his (tipx, tipy, tipz, radar_left, radar_right, stack_dist) values ({0}, {1}, {2}, {3}, {4}, {5})", tipx, tipy, tipz, radar_left, radar_right, radar_aveg);
            return this.provider.ExecuteSql(sqlString);
        }

        /// <summary>
        /// 插入雷达测距值
        /// </summary>
        /// <param name="radar_id">雷达ID</param>
        /// <param name="dist">距离</param>
        /// <returns></returns>
        public int InsertRadarDistance(GnssProtoInfo gnss_info, RadarProtoInfo radar_info)
        {
            return this.InsertRadarDistance(gnss_info.LocalCoor_Tipx, gnss_info.LocalCoor_Tipy, gnss_info.LocalCoor_Tipz, radar_info.DistWheelLeft, radar_info.DistWheelRight, radar_info.DistWheelAverage);
        }
    }
}
