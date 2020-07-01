using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Model
{
    /// <summary>
    /// 策略工控机数据源
    /// </summary>
    public class StrategyDataSource
    {
        /// <summary>
        /// 排序时间戳
        /// </summary>
        public string DataId { get { return DateTime.Now.ToString("yyyyMMddHHmmss"); } }

        private double _material_height = 0;
        /// <summary>
        /// 料堆高度
        /// </summary>
        public double MaterialHeight
        {
            get { return _material_height; }
            set { _material_height = Math.Round(value, 2); }
        }

        private double _material_volume = 0;
        /// <summary>
        /// 实测体积
        /// </summary>
        public double MaterialVolume
        {
            get { return _material_volume; }
            set { _material_volume = Math.Round(value, 2); }
        }

        private double _repose_angle = 0;
        /// <summary>
        /// 实测安息角
        /// </summary>
        public double ReposeAngle
        {
            get { return _repose_angle; }
            set { _repose_angle = Math.Round(value, 2); }
        }

        private double _blank_dist = 0;
        /// <summary>
        /// 落料口到落料点距离
        /// </summary>
        public double BlankingDistance
        {
            get { return _blank_dist; }
            set { _blank_dist = Math.Round(value, 2); }
        }

        private double _run_pos = 0;
        /// <summary>
        /// 大机行走位置（北斗）
        /// </summary>
        public double RunningPosition
        {
            get { return _run_pos; }
            set { _run_pos = Math.Round(value, 2); }
        }

        private double _rot_angle = 0;
        /// <summary>
        /// 大机回转角（北斗）
        /// </summary>
        public double RotationAngle
        {
            get { return _rot_angle; }
            set { _rot_angle = Math.Round(value, 2); }
        }

        private double _pitch_angle = 0;
        /// <summary>
        /// 大机俯仰角（北斗）
        /// </summary>
        public double PitchAngle
        {
            get { return _pitch_angle; }
            set { _pitch_angle = Math.Round(value, 2); }
        }

        private double _run_pos_diff = 0;
        /// <summary>
        /// 大机走行位置差值（北斗 - 编码器）
        /// </summary>
        public double RunningDiff
        {
            get { return _run_pos_diff; }
            set { _run_pos_diff = Math.Round(value, 2); }
        }

        private double _rot_angle_diff = 0;
        /// <summary>
        /// 大机回转位置差值（北斗 - 编码器）
        /// </summary>
        public double RotationDiff
        {
            get { return _rot_angle_diff; }
            set { _rot_angle_diff = Math.Round(value, 2); }
        }

        private double _pitch_angle_diff = 0;
        /// <summary>
        /// 大机俯仰位置差值（北斗 - 编码器）
        /// </summary>
        public double PitchDiff
        {
            get { return _pitch_angle_diff; }
            set { _pitch_angle_diff = Math.Round(value, 2); }
        }

        private double _wheel_left_dist = 0;
        /// <summary>
        /// 斗轮左侧测距
        /// </summary>
        public double WheelLeftDist
        {
            get { return _wheel_left_dist; }
            set { _wheel_left_dist = Math.Round(value, 2); }
        }

        private double _wheel_right_dist = 0;
        /// <summary>
        /// 斗轮右侧测距
        /// </summary>
        public double WheelRightDist
        {
            get { return _wheel_right_dist; }
            set { _wheel_right_dist = Math.Round(value, 2); }
        }

        private List<RadarCoor> _list_left_coors = new List<RadarCoor>();
        /// <summary>
        /// 斗轮左雷达点数据列表
        /// </summary>
        public List<RadarCoor> ListLeftRadarCoors
        {
            get { return _list_left_coors; }
            set { _list_left_coors = value; }
        }

        private List<RadarCoor> _list_right_coors = new List<RadarCoor>();
        /// <summary>
        /// 斗轮右雷达点数据列表
        /// </summary>
        public List<RadarCoor> ListRightRadarCoors
        {
            get { return _list_right_coors; }
            set { _list_right_coors = value; }
        }

        /// <summary>
        /// 防碰数据，一串由各个雷达防碰级别2进制字符串所拼接成的字符串
        /// </summary>
        public string CollisionInfo { get; set; }
    }
}
