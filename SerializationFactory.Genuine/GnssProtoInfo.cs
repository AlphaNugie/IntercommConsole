using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationFactory.Genuine
{
    /// <summary>
    /// 用于GNSS数据传输的数据实体类
    /// </summary>
    public class GnssInfo
    {
        /// <summary>
        /// 是否收到数据
        /// </summary>
        public bool Working { get; set; }

        /// <summary>
        /// 是否为固定解
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// 是否为固定解
        /// </summary>
        public bool TrackDirection_Received { get; set; }

        /// <summary>
        /// 定位质量
        /// </summary>
        public string PositionQuality { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 海拔
        /// </summary>
        public double Altitude { get; set; }

        /// <summary>
        /// 定位天线本地X坐标
        /// </summary>
        public double LocalCoor_Antex { get; set; }

        /// <summary>
        /// 定位天线本地Y坐标
        /// </summary>
        public double LocalCoor_Antey { get; set; }

        /// <summary>
        /// 定位天线本地Z坐标
        /// </summary>
        public double LocalCoor_Antez { get; set; }

        /// <summary>
        /// 臂架顶端本地X坐标
        /// </summary>
        public double LocalCoor_Tipx { get; set; }

        /// <summary>
        /// 臂架顶端本地Y坐标
        /// </summary>
        public double LocalCoor_Tipy { get; set; }

        /// <summary>
        /// 臂架顶端本地Z坐标
        /// </summary>
        public double LocalCoor_Tipz { get; set; }

        /// <summary>
        /// 行走位置
        /// </summary>
        public double WalkingPosition { get; set; }

        /// <summary>
        /// 俯仰角
        /// </summary>
        public double PitchAngle { get; set; }

        /// <summary>
        /// 回转角
        /// </summary>
        public double YawAngle { get; set; }
    }
}
