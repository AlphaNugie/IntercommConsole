using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationFactory.Genuine
{
    /// <summary>
    /// 每台雷达的基础信息
    /// </summary>
    public class RadarInfoDetail
    {
        /// <summary>
        /// 雷达ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 雷达名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工作状态，1 收到数据，2 未收到数据
        /// </summary>
        public int Working { get; set; }

        /// <summary>
        /// 当前距离
        /// </summary>
        public double CurrentDistance { get; set; }

        /// <summary>
        /// 报警级数
        /// </summary>
        public int ThreatLevel { get; set; }

        /// <summary>
        /// 报警级数2进制数
        /// </summary>
        public string ThreatLevelBinary { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public ushort Port { get; set; }
    }
}
