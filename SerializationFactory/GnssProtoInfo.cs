using CommonLib.Extensions.Property;
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
        /// 是否收到数据
        /// </summary>
        [ProtoMember(7)]
        [PropertyMapperFrom("Working")]
        public bool Working { get; set; }

        /// <summary>
        /// 是否为固定解
        /// </summary>
        [ProtoMember(8)]
        public bool IsFixed { get; set; }

        /// <summary>
        /// 定位质量
        /// </summary>
        [ProtoMember(9)]
        [PropertyMapperFrom("PositionQuality")]
        public string PositionQuality { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [ProtoMember(10)]
        [PropertyMapperFrom("Latitude")]
        public double Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [ProtoMember(11)]
        [PropertyMapperFrom("Longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// 海拔
        /// </summary>
        [ProtoMember(12)]
        [PropertyMapperFrom("Altitude")]
        public double Altitude { get; set; }

        /// <summary>
        /// 定位天线本地X坐标
        /// </summary>
        [ProtoMember(13)]
        [PropertyMapperFrom("LocalCoor_Ante.XPrime")]
        public double LocalCoor_Antex { get; set; }

        /// <summary>
        /// 定位天线本地Y坐标
        /// </summary>
        [ProtoMember(14)]
        [PropertyMapperFrom("LocalCoor_Ante.YPrime")]
        public double LocalCoor_Antey { get; set; }

        /// <summary>
        /// 定位天线本地Z坐标
        /// </summary>
        [ProtoMember(15)]
        [PropertyMapperFrom("LocalCoor_Ante.Z")]
        public double LocalCoor_Antez { get; set; }

        /// <summary>
        /// 臂架顶端本地X坐标
        /// </summary>
        [ProtoMember(1)]
        [PropertyMapperFrom("LocalCoor_Tip.XPrime")]
        public double LocalCoor_Tipx { get; set; }

        /// <summary>
        /// 臂架顶端本地Y坐标
        /// </summary>
        [ProtoMember(2)]
        [PropertyMapperFrom("LocalCoor_Tip.YPrime")]
        public double LocalCoor_Tipy { get; set; }

        /// <summary>
        /// 臂架顶端本地Z坐标
        /// </summary>
        [ProtoMember(3)]
        [PropertyMapperFrom("LocalCoor_Tip.Z")]
        public double LocalCoor_Tipz { get; set; }

        /// <summary>
        /// 行走位置
        /// </summary>
        [ProtoMember(4)]
        [PropertyMapperTo("WalkingPosition")]
        [PropertyMapperFrom("WalkingPosition")]
        public double WalkingPosition { get; set; }

        /// <summary>
        /// 俯仰角
        /// </summary>
        [ProtoMember(5)]
        [PropertyMapperTo("PitchAngle")]
        [PropertyMapperFrom("PitchAngle")]
        public double PitchAngle { get; set; }

        /// <summary>
        /// 回转角
        /// </summary>
        [ProtoMember(6)]
        [PropertyMapperTo("YawAngle")]
        [PropertyMapperFrom("YawAngle")]
        public double YawAngle { get; set; }
    }
}
