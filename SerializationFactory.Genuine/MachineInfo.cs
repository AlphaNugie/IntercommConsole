using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationFactory.Genuine
{
    /// <summary>
    /// 单机相关数据信息
    /// </summary>
    public class MachineInfo
    {
        /// <summary>
        /// 单机名称
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 雷达信息
        /// </summary>
        public List<RadarInfoDetail> RadarDetails { get; set; }

        /// <summary>
        /// 北斗信息
        /// </summary>
        public GnssInfo GnssInfo { get; set; }
    }
}
