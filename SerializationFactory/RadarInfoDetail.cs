using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationFactory
{
    /// <summary>
    /// 每台雷达的基础信息
    /// </summary>
    [ProtoContract]
    public class RadarInfoDetail
    {
        /// <summary>
        /// 雷达ID
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// 雷达名称
        /// </summary>
        [ProtoMember(2)]
        public string Name { get; set; }

        /// <summary>
        /// 工作状态，1 收到数据，2 未收到数据
        /// </summary>
        [ProtoMember(3)]
        public int Working { get; set; }

        /// <summary>
        /// 当前距离
        /// </summary>
        [ProtoMember(4)]
        public double CurrentDistance { get; set; }

        /// <summary>
        /// 报警级数
        /// </summary>
        [ProtoMember(5)]
        public int ThreatLevel { get; set; }

        /// <summary>
        /// 报警级数2进制数
        /// </summary>
        [ProtoMember(6)]
        public string ThreatLevelBinary { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [ProtoMember(7)]
        public string IpAddress { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        [ProtoMember(8)]
        public ushort Port { get; set; }
    }
}
