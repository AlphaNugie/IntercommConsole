using CommonLib.DataUtil;
using IntercommConsole.Core;
using IntercommConsole.Model;
using SerializationFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.DataUtil
{
    public class DataService_Machine
    {
        private readonly OracleProvider provider = new OracleProvider(DbDef.HostAddress, DbDef.ServiceName, DbDef.UserName, DbDef.Password);

        /// <summary>
        /// 更新单机姿态
        /// </summary>
        /// <param name="machine_name">单机名称</param>
        /// <param name="radar_info">雷达信息实体</param>
        /// <param name="gnss_info">GNSS信息实体</param>
        /// <returns></returns>
        public int UpdateMachinePosture(string machine_name, RadarProtoInfo radar_info, GnssProtoInfo gnss_info, OpcDataSource opc_source)
        {
            string sql = string.Format("update t_rcms_machineposture_time t set t.walking = {1}, t.pitch = {2}, t.yaw = {3}, local_tipx = {4}, local_tipy = {5}, local_tipz = {6}, walking_left_plc = {7}, walking_right_plc = {8}, pitch_plc = {9}, yaw_plc = {10}, local_tipz_abs = {11}, working = {12}, isfixed = {13}, quality = '{14}', latitude = {15}, longitude = {16}, altitude = {17}, coal_on_belt = {18}, local_antex = {19}, local_antey = {20}, local_antez = {21}, stream_per_hour = {22}, stream_total = {23}, wheel_left_dist = {24}, wheel_right_dist = {25}, belt_dist = {26}, wheel_power_raw = {27}, wheel_power_polished = {28}, wheel_left_radius = {29:f3}, wheel_right_radius = {30:f3}, wheel_left_surface_angle = {31}, wheel_right_surface_angle = {32}, beyond_stack_left = {33}, beyond_stack_right = {34}, t.time = sysdate where t.machine_name = '{0}'", machine_name, gnss_info.WalkingPosition, gnss_info.PitchAngle, gnss_info.YawAngle, gnss_info.LocalCoor_Tipx, gnss_info.LocalCoor_Tipy, gnss_info.LocalCoor_Tipz, opc_source.WalkingPositionLeft_Plc, opc_source.WalkingPositionRight_Plc, opc_source.PitchAngle_Plc, opc_source.YawAngle_Plc, gnss_info.LocalCoor_Tipz + Config.HeightOffset, gnss_info.Working ? 1 : 0, gnss_info.IsFixed ? 1 : 0, gnss_info.PositionQuality, gnss_info.Longitude, gnss_info.Latitude, gnss_info.Altitude, opc_source.CoalOnBeltPlc, gnss_info.LocalCoor_Antex, gnss_info.LocalCoor_Antey, gnss_info.LocalCoor_Antez, opc_source.StreamPerHour, opc_source.StreamTotal, radar_info.DistWheelLeft, radar_info.DistWheelRight, radar_info.DistBelt, opc_source.WheelPowerRaw, opc_source.WheelPowerPolished, radar_info.RadiusAverageLeft, radar_info.RadiusAverageRight, radar_info.SurfaceAngleWheelLeft, radar_info.SurfaceAngleWheelRight, opc_source.WheelLeftBeyondStack, opc_source.WheelRightBeyondStack);
            return this.provider.ExecuteSql(sql);
        }

        /// <summary>
        /// 更新单机落料口/斗轮下沿、回转轴本地坐标
        /// </summary>
        /// <param name="machine_name">单机名称</param>
        /// <param name="radar_info">雷达信息实体</param>
        /// <param name="gnss_info">GNSS信息实体</param>
        /// <returns></returns>
        public int UpdateMachinePosture2(string machine_name, RadarProtoInfo radar_info, GnssProtoInfo gnss_info)
        {
            string sql = string.Format("update t_rcms_machineposture2_time t set t.local_tipx = {1}, t.local_tipy = {2}, t.local_tipz = {3}, t.local_centrex = {4}, t.local_centrey = {5}, t.local_centrez = {6}, t.distances = '{7}', t.time = sysdate where t.machine_name = '{0}'", machine_name, gnss_info.LocalCoor_Tipx, gnss_info.LocalCoor_Tipy, gnss_info.LocalCoor_Tipz, gnss_info.LocalCoor_Centrex, gnss_info.LocalCoor_Centrey, gnss_info.LocalCoor_Centrez, radar_info.BlockDistances);
            return this.provider.ExecuteSql(sql);
        }

        public bool UpdateRadarInfo(string machine_name, RadarProtoInfo radar_info)
        {
            if (radar_info == null || radar_info.RadarList == null || radar_info.RadarList.Count == 0)
                return false;
            string[] sqls = radar_info.RadarList.Select(radar => string.Format("update t_rcms_radarinfo_time t set t.radar_id = {3}, t.radar_name = '{4}', effective = {5}, distance = {6}, threat_level = {7}, time = sysdate where t.machine_name = '{0}' and t.ip_address = '{1}' and port = {2}", machine_name, radar.IpAddress, radar.Port, radar.Id, radar.Name, radar.Working, radar.CurrentDistance, radar.ThreatLevel)).ToArray();
            return this.provider.ExecuteSqlTrans(sqls);
        }

        /// <summary>
        /// 出垛边角度、回转折返角度历史记录新增
        /// </summary>
        /// <param name="group_id">作业记录组ID</param>
        /// <param name="walk">行走位置</param>
        /// <param name="pitch">俯仰角度</param>
        /// <param name="yaw">回转角度</param>
        /// <param name="type">当前角度类型：1 左侧出垛边，2 右侧出垛边，3 左侧回转折返，4 右侧回转折返</param>
        /// <param name="direction">方向：1 左，2 右</param>
        /// <param name="time">记录时间</param>
        /// <returns></returns>
        public int InsertAngleRecord(string machine_name, string group_id, double walk, double pitch, double yaw, int type, int direction, DateTime time)
        {
            int result = 0;
            string sqlString = string.Format("insert into t_rcms_angle_record_his (group_id, walking, pitch_angle, yaw_angle, angle_type, direction, time, machine_name) values ('{0}', {1}, {2}, {3}, {4}, {5}, to_date('{6}', 'yyyy-mm-dd hh24:mi:ss'), '{7}')", group_id, walk, pitch, yaw, type, direction, time.ToString("yyyy-MM-dd HH:mm:ss"), machine_name);
            try { result = this.provider.ExecuteSql(sqlString); }
            catch (Exception) { }
            return result;
        }
    }
}
