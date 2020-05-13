using CommonLib.DataUtil;
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
        //private readonly OracleProvider provider = new OracleProvider();
        private readonly OracleProvider provider = new OracleProvider("172.17.10.2", "pdborcl", "ysurcms", "8508991");

        /// <summary>
        /// 更新单机姿态
        /// </summary>
        /// <param name="machine_name">单机名称</param>
        /// <param name="walking">行走</param>
        /// <param name="pitch">俯仰</param>
        /// <param name="yaw">回转</param>
        /// <param name="tipx"></param>
        /// <param name="tipy"></param>
        /// <param name="tipz"></param>
        /// <returns></returns>
        public int UpdateMachinePosture(string machine_name, double walking, double pitch, double yaw, double tipx, double tipy, double tipz)
        {
            string sql = string.Format("update t_rcms_machineposture_time t set t.walking = {1}, t.pitch = {2}, t.yaw = {3}, local_tipx = {4}, local_tipy = {5}, local_tipz = {6}, t.time = sysdate where t.machine_name = '{0}'", machine_name, walking, pitch, yaw, tipx, tipy, tipz);
            return this.provider.ExecuteSql(sql);
        }

        /// <summary>
        /// 更新单机姿态
        /// </summary>
        /// <param name="machine_name">单机名称</param>
        /// <param name="gnss_info">GNSS信息实体</param>
        /// <returns></returns>
        public int UpdateMachinePosture(string machine_name, GnssProtoInfo gnss_info)
        {
            return this.UpdateMachinePosture(machine_name, gnss_info.WalkingPosition, gnss_info.PitchAngle, gnss_info.YawAngle, gnss_info.LocalCoor_Tipx, gnss_info.LocalCoor_Tipy, gnss_info.LocalCoor_Tipz);
        }
    }
}
