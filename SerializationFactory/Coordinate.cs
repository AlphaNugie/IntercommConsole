using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationFactory
{
    /// <summary>
    /// 坐标
    /// </summary>
    [ProtoContract]
    public class Coordinate
    {
        /// <summary>
        /// 本地X
        /// </summary>
        [ProtoMember(1)]
        public double XPrime { get; set; }

        /// <summary>
        /// 本地Y
        /// </summary>
        [ProtoMember(2)]
        public double YPrime { get; set; }

        /// <summary>
        /// 单机X
        /// </summary>
        [ProtoMember(3)]
        public double XClaimer { get; set; }

        /// <summary>
        /// 单机Y
        /// </summary>
        [ProtoMember(4)]
        public double YClaimer { get; set; }

        /// <summary>
        /// Z坐标
        /// </summary>
        [ProtoMember(5)]
        public double Z { get; set; }
    }
}
