using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationFactory
{
    /// <summary>
    /// 通过ProtobufNet传输的消息类型
    /// </summary>
    public enum ProtoInfoType
    {
        /// <summary>
        /// GNSS消息
        /// </summary>
        GNSS = 1,

        /// <summary>
        /// 雷达消息
        /// </summary>
        RADAR = 2,

        /// <summary>
        /// 雷达详细信息
        /// </summary>
        RADAR_DETAIL = 3
    }

    /// <summary>
    /// 定位解算状态
    /// </summary>
    [Flags]
    public enum SolutionType
    {
        /// <summary>
        /// 定位不可用或无解
        /// </summary>
        Invalid = 1,

        /// <summary>
        /// 单点定位
        /// </summary>
        Single = 2,

        /// <summary>
        /// 浮点解
        /// </summary>
        Float = 4,

        /// <summary>
        /// 固定解
        /// </summary>
        Fixed = 8,

        /// <summary>
        /// 其它情况
        /// </summary>
        Other = 16
    }
}
