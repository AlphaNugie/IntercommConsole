using CommonLib.DataUtil;
using IntercommConsole.Model;
using SerializationFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole
{
    public class DataService_Machine
    {
        private readonly OracleProvider provider = new OracleProvider("172.17.10.2", "pdborcl", "ysurcms", "8508991");

        /// <summary>
        /// 更新单机姿态
        /// </summary>
        /// <param name="machine_name">单机名称</param>
        /// <param name="gnss_info">GNSS信息实体</param>
        /// <returns></returns>
        public int UpdateMachinePosture(string machine_name, GnssProtoInfo gnss_info, OpcDatasource opc_source)
        {
            string sql = string.Format("update t_rcms_machineposture_time t set t.walking = {1}, t.pitch = {2}, t.yaw = {3}, local_tipx = {4}, local_tipy = {5}, local_tipz = {6}, walking_left_plc = {7}, walking_right_plc = {8}, pitch_plc = {9}, yaw_plc = {10}, local_tipz_abs = {11}, working = {12}, isfixed = {13}, quality = '{14}', latitude = {15}, longitude = {16}, altitude = {17}, coal_on_belt = {18}, local_antex = {19}, local_antey = {20}, local_antez = {21}, t.time = sysdate where t.machine_name = '{0}'", machine_name, gnss_info.WalkingPosition, gnss_info.PitchAngle, gnss_info.YawAngle, gnss_info.LocalCoor_Tipx, gnss_info.LocalCoor_Tipy, gnss_info.LocalCoor_Tipz, opc_source.WalkingPositionLeft_Plc, opc_source.WalkingPositionRight_Plc, opc_source.PitchAngle_Plc, opc_source.YawAngle_Plc, gnss_info.LocalCoor_Tipz + Config.HeightOffset, gnss_info.Working ? 1 : 0, gnss_info.IsFixed ? 1 : 0, gnss_info.PositionQuality, gnss_info.Longitude, gnss_info.Latitude, gnss_info.Altitude, opc_source.CoalOnBelt ? 1 : 0, gnss_info.LocalCoor_Antex, gnss_info.LocalCoor_Antey, gnss_info.LocalCoor_Antez);
            return this.provider.ExecuteSql(sql);
        }

        public bool UpdateRadarInfo(string machine_name, RadarProtoInfo radar_info)
        {
            if (radar_info == null || radar_info.RadarList == null || radar_info.RadarList.Count == 0)
                return false;
            string[] sqls = radar_info.RadarList.Select(radar => string.Format("update t_rcms_radarinfo_time t set t.radar_id = {3}, t.radar_name = '{4}', effective = {5}, distance = {6}, threat_level = {7}, time = sysdate where t.machine_name = '{0}' and t.ip_address = '{1}' and port = {2}", machine_name, radar.IpAddress, radar.Port, radar.Id, radar.Name, radar.Working, radar.CurrentDistance, radar.ThreatLevel)).ToArray();
            return this.provider.ExecuteSqlTrans(sqls);
        }
    }
}
