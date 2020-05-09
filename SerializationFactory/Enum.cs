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
        RADAR = 2
    }
}
