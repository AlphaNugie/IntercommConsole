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
    /// 用于雷达数据传输的protobuf数据实体类
    /// </summary>
    [ProtoContract]
    public class RadarProtoInfo
    {
        private double _dist_wheel_left, _dist_wheel_right;

        /// <summary>
        /// 斗轮左侧测距（假如为堆料机则为落料口左侧测距）
        /// </summary>
        [ProtoMember(1)]
        [PropertyMapperTo("WheelLeftDist")]
        public double DistWheelLeft
        {
            get { return this._dist_wheel_left; }
            set
            {
                this._dist_wheel_left = Math.Round(value, 4);
                this.DistWheelAverage = Math.Round((this._dist_wheel_left + this._dist_wheel_right) / 2, 4);
                this.DistWheelDiff = Math.Abs(this._dist_wheel_left - this._dist_wheel_right);
            }
        }

        /// <summary>
        /// 斗轮右侧测距（假如为堆料机则为落料口右侧测距）
        /// </summary>
        [ProtoMember(2)]
        [PropertyMapperTo("WheelRightDist")]
        public double DistWheelRight
        {
            get { return this._dist_wheel_right; }
            set
            {
                this._dist_wheel_right = Math.Round(value, 3);
                this.DistWheelAverage = Math.Round((this._dist_wheel_left + this._dist_wheel_right) / 2, 4);
                this.DistWheelDiff = Math.Abs(this._dist_wheel_left - this._dist_wheel_right);
            }
        }

        /// <summary>
        /// 斗轮平均距离
        /// </summary>
        [ProtoMember(3)]
        public double DistWheelAverage { get; set; }

        /// <summary>
        /// 两侧斗轮距离的最小值
        /// </summary>
        [ProtoMember(4)]
        public double DistWheelMin { get; set; }

        /// <summary>
        /// 两侧斗轮距离的差值（绝对值）
        /// </summary>
        [ProtoMember(5)]
        public double DistWheelDiff { get; set; }

        /// <summary>
        /// 皮带料流雷达下方距离
        /// </summary>
        [ProtoMember(7)]
        public double DistBelt { get; set; }

        /// <summary>
        /// 臂架左侧
        /// </summary>
        [ProtoMember(8)]
        public double DistArmLeft { get; set; }

        /// <summary>
        /// 臂架右侧距离
        /// </summary>
        [ProtoMember(9)]
        public double DistArmRight { get; set; }

        /// <summary>
        /// 臂架下方距离
        /// </summary>
        [ProtoMember(10)]
        public double DistArmBelow { get; set; }

        /// <summary>
        /// 配重左侧距离
        /// </summary>
        [ProtoMember(11)]
        public double DistCounterLeft { get; set; }

        /// <summary>
        /// 配重右侧距离
        /// </summary>
        [ProtoMember(12)]
        public double DistCounterRight { get; set; }

        /// <summary>
        /// 雷达基础信息列表
        /// </summary>
        [ProtoMember(6)]
        public List<RadarInfoDetail> RadarList { get; set; }

        /// <summary>
        /// 斗轮左侧雷达坐标列表
        /// </summary>
        [ProtoMember(13)]
        public List<RadarCoor> WheelLeftCoorList { get; set; }

        /// <summary>
        /// 斗轮右侧雷达坐标列表
        /// </summary>
        [ProtoMember(14)]
        public List<RadarCoor> WheelRightCoorList { get; set; }

        /// <summary>
        /// 构造器
        /// </summary>
        public RadarProtoInfo()
        {
            this.RadarList = new List<RadarInfoDetail>();
            this.WheelLeftCoorList = new List<RadarCoor>();
            this.WheelRightCoorList = new List<RadarCoor>();
        }
    }
}
