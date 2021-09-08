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
        #region 斗轮/堆料机雷达测距
        private double _dist_wheel_left, _dist_wheel_right;

        /// <summary>
        /// 斗轮左侧测距（假如为堆料机则为落料口左侧测距）
        /// </summary>
        [ProtoMember(1)]
        [PropertyMapperTo("WheelLeftDist")]
        public double DistWheelLeft
        {
            get { return _dist_wheel_left; }
            set
            {
                _dist_wheel_left = Math.Round(value, 4);
                DistWheelAverage = Math.Round((_dist_wheel_left + _dist_wheel_right) / 2, 4);
                DistWheelDiff = Math.Abs(_dist_wheel_left - _dist_wheel_right);
                DistWheelMin = Math.Min(_dist_wheel_left, _dist_wheel_right);
            }
        }

        /// <summary>
        /// 斗轮右侧测距（假如为堆料机则为落料口右侧测距）
        /// </summary>
        [ProtoMember(2)]
        [PropertyMapperTo("WheelRightDist")]
        public double DistWheelRight
        {
            get { return _dist_wheel_right; }
            set
            {
                _dist_wheel_right = Math.Round(value, 4);
                DistWheelAverage = Math.Round((_dist_wheel_left + _dist_wheel_right) / 2, 4);
                DistWheelDiff = Math.Abs(_dist_wheel_left - _dist_wheel_right);
                DistWheelMin = Math.Min(_dist_wheel_left, _dist_wheel_right);
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
        #endregion

        #region 斗轮/堆料机雷达测角
        private double _angle_wheel_left, _angle_wheel_right;

        /// <summary>
        /// 斗轮左侧角度（假如为堆料机则为落料口左侧角度）
        /// </summary>
        [ProtoMember(37)]
        public double AngleWheelLeft
        {
            get { return _angle_wheel_left; }
            set
            {
                _angle_wheel_left = Math.Round(value, 2);
                AngleWheelAverage = Math.Round((_angle_wheel_left + _angle_wheel_right) / 2, 2);
                AngleWheelDiff = Math.Abs(_angle_wheel_left - _angle_wheel_right);
                AngleWheelMin = Math.Min(_angle_wheel_left, _angle_wheel_right);
            }
        }

        /// <summary>
        /// 斗轮右侧角度（假如为堆料机则为落料口右侧角度）
        /// </summary>
        [ProtoMember(38)]
        public double AngleWheelRight
        {
            get { return _angle_wheel_right; }
            set
            {
                _angle_wheel_right = Math.Round(value, 2);
                AngleWheelAverage = Math.Round((_angle_wheel_left + _angle_wheel_right) / 2, 2);
                AngleWheelDiff = Math.Abs(_angle_wheel_left - _angle_wheel_right);
                AngleWheelMin = Math.Min(_angle_wheel_left, _angle_wheel_right);
            }
        }

        private double _angle_wheel_average = 0;
        /// <summary>
        /// 斗轮平均角度
        /// </summary>
        [ProtoMember(39)]
        public double AngleWheelAverage
        {
            get { return _angle_wheel_average; }
            set { _angle_wheel_average = value > 0 ? 0 : value; }
        }

        /// <summary>
        /// 两侧斗轮角度的最小值
        /// </summary>
        [ProtoMember(40)]
        public double AngleWheelMin { get; set; }

        /// <summary>
        /// 两侧斗轮角度的差值（绝对值）
        /// </summary>
        [ProtoMember(41)]
        public double AngleWheelDiff { get; set; }
        #endregion

        /// <summary>
        /// 皮带料流雷达下方距离
        /// </summary>
        [ProtoMember(7)]
        public double DistBelt { get; set; }

        /// <summary>
        /// 臂架左侧
        /// </summary>
        [ProtoMember(8)]
        [PropertyMapperTo("ArmLeftDist")]
        public double DistArmLeft { get; set; }

        /// <summary>
        /// 臂架右侧距离
        /// </summary>
        [ProtoMember(9)]
        [PropertyMapperTo("ArmRightDist")]
        public double DistArmRight { get; set; }

        /// <summary>
        /// 臂架下方距离
        /// </summary>
        [ProtoMember(10)]
        [PropertyMapperTo("ArmBelowDist")]
        public double DistArmBelow { get; set; }

        /// <summary>
        /// 配重左侧距离
        /// </summary>
        [ProtoMember(11)]
        [PropertyMapperTo("CounterLeftDist")]
        public double DistCounterLeft { get; set; }

        /// <summary>
        /// 配重右侧距离
        /// </summary>
        [ProtoMember(12)]
        [PropertyMapperTo("CounterRightDist")]
        public double DistCounterRight { get; set; }

        /// <summary>
        /// 大臂左前距离
        /// </summary>
        [ProtoMember(19)]
        [PropertyMapperTo("DistLeftFront")]
        public double DistLeftFront { get; set; }

        /// <summary>
        /// 大臂左中距离
        /// </summary>
        [ProtoMember(20)]
        [PropertyMapperTo("DistLeftMiddle")]
        public double DistLeftMiddle { get; set; }

        /// <summary>
        /// 大臂左后距离
        /// </summary>
        [ProtoMember(21)]
        [PropertyMapperTo("DistLeftBack")]
        public double DistLeftBack { get; set; }

        /// <summary>
        /// 大臂右前距离
        /// </summary>
        [ProtoMember(22)]
        [PropertyMapperTo("DistRightFront")]
        public double DistRightFront { get; set; }

        /// <summary>
        /// 大臂右中距离
        /// </summary>
        [ProtoMember(23)]
        [PropertyMapperTo("DistRightMiddle")]
        public double DistRightMiddle { get; set; }

        /// <summary>
        /// 大臂右后距离
        /// </summary>
        [ProtoMember(24)]
        [PropertyMapperTo("DistRightBack")]
        public double DistRightBack { get; set; }

        /// <summary>
        /// 左前距离级别
        /// </summary>
        [ProtoMember(31)]
        [PropertyMapperTo("LevelLeftFront")]
        public double LevelLeftFront { get; set; }

        /// <summary>
        /// 左中距离级别
        /// </summary>
        [ProtoMember(32)]
        [PropertyMapperTo("LevelLeftMiddle")]
        public double LevelLeftMiddle { get; set; }

        /// <summary>
        /// 左后距离级别
        /// </summary>
        [ProtoMember(33)]
        [PropertyMapperTo("LevelLeftBack")]
        public double LevelLeftBack { get; set; }

        /// <summary>
        /// 右前距离级别
        /// </summary>
        [ProtoMember(34)]
        [PropertyMapperTo("LevelRightFront")]
        public double LevelRightFront { get; set; }

        /// <summary>
        /// 右中距离级别
        /// </summary>
        [ProtoMember(35)]
        [PropertyMapperTo("LevelRightMiddle")]
        public double LevelRightMiddle { get; set; }

        /// <summary>
        /// 右后距离级别
        /// </summary>
        [ProtoMember(36)]
        [PropertyMapperTo("LevelRightBack")]
        public double LevelRightBack { get; set; }

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

        private double _slope_wheel_left;
        /// <summary>
        /// 斗轮左侧雷达一次拟合斜率
        /// </summary>
        [ProtoMember(15)]
        public double SlopeWheelLeft
        {
            get { return _slope_wheel_left; }
            set { _slope_wheel_left = Math.Round(value, 3); }
        }

        private double _slope_wheel_right;
        /// <summary>
        /// 斗轮右侧雷达一次拟合斜率
        /// </summary>
        [ProtoMember(16)]
        public double SlopeWheelRight
        {
            get { return _slope_wheel_right; }
            set { _slope_wheel_right = Math.Round(value, 3); }
        }

        private double _surface_wheel_left;
        /// <summary>
        /// 斗轮左侧雷达一次拟合斜率
        /// </summary>
        [ProtoMember(17)]
        public double SurfaceAngleWheelLeft
        {
            get { return _surface_wheel_left; }
            set { _surface_wheel_left = Math.Round(value, 3); }
        }

        private double _surface_wheel_right;
        /// <summary>
        /// 斗轮右侧雷达一次拟合斜率
        /// </summary>
        [ProtoMember(18)]
        public double SurfaceAngleWheelRight
        {
            get { return _surface_wheel_right; }
            set { _surface_wheel_right = Math.Round(value, 3); }
        }

        /// <summary>
        /// 左侧是否出垛边（由matlab模型决定）
        /// </summary>
        [ProtoMember(25)]
        public bool OutOfStackLeft { get; set; }

        /// <summary>
        /// 右侧是否出垛边（由matlab模型决定）
        /// </summary>
        [ProtoMember(26)]
        public bool OutOfStackRight { get; set; }

        /// <summary>
        /// 左侧平均半径
        /// </summary>
        [ProtoMember(27)]
        public double RadiusAverageLeft { get; set; }

        /// <summary>
        /// 右侧平均半径
        /// </summary>
        [ProtoMember(28)]
        public double RadiusAverageRight { get; set; }

        /// <summary>
        /// 是否位于底层
        /// </summary>
        [ProtoMember(29)]
        public bool OnBottomLevel { get; set; }

        /// <summary>
        /// 所有网格测距值的集合，格式为“a0,a1,...a5;b0,b1,...b5;c0,c1,...,c5;d0,d1,...,d5”
        /// abcd大项分别代表原始值、远距过滤值、变化差异过滤纸以及卡尔曼滤波过滤值，内部序号分别代表左前中后、右前中后
        /// </summary>
        [ProtoMember(30)]
        public string BlockDistances { get; set; }

        /// <summary>
        /// 构造器
        /// </summary>
        public RadarProtoInfo()
        {
            RadarList = new List<RadarInfoDetail>();
            WheelLeftCoorList = new List<RadarCoor>();
            WheelRightCoorList = new List<RadarCoor>();
        }
    }
}
