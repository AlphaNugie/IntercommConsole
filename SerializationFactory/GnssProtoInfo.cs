using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationFactory
{
    /// <summary>
    /// 用于GNSS数据传输的protobuf数据实体类
    /// </summary>
    [ProtoContract]
    public class GnssProtoInfo
    {
        /// <summary>
        /// 臂架顶端本地X坐标
        /// </summary>
        [ProtoMember(1)]
        public double LocalCoor_Tipx { get; set; }

        /// <summary>
        /// 臂架顶端本地Y坐标
        /// </summary>
        [ProtoMember(2)]
        public double LocalCoor_Tipy { get; set; }

        /// <summary>
        /// 臂架顶端本地Z坐标
        /// </summary>
        [ProtoMember(3)]
        public double LocalCoor_Tipz { get; set; }

        /// <summary>
        /// 行走位置
        /// </summary>
        [ProtoMember(4)]
        public double WalkingPosition { get; set; }

        /// <summary>
        /// 俯仰角
        /// </summary>
        [ProtoMember(5)]
        public double PitchAngle { get; set; }

        /// <summary>
        /// 回转角
        /// </summary>
        [ProtoMember(6)]
        public double YawAngle { get; set; }
    }
}
