using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Model
{
    public class RadarCoor
    {
        private double _dist_long;
        /// <summary>
        /// 纵向距离
        /// </summary>
        public double DistLong
        {
            get { return _dist_long; }
            set { _dist_long = Math.Round(value, 2); }
        }

        private double _dist_lat;
        /// <summary>
        /// 横向距离
        /// </summary>
        public double DistLat
        {
            get { return _dist_lat; }
            set { _dist_lat = Math.Round(value, 2); }
        }

        private double _rcs;
        /// <summary>
        /// 雷达散射截面
        /// </summary>
        public double Rcs
        {
            get { return _rcs; }
            set { _rcs = Math.Round(value, 2); }
        }

        private double _x;
        /// <summary>
        /// X坐标
        /// </summary>
        public double X
        {
            get { return _x; }
            set { _x = Math.Round(value, 2); }
        }

        private double _y;
        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y
        {
            get { return _y; }
            set { _y = Math.Round(value, 2); }
        }

        private double _z;
        /// <summary>
        /// Z坐标
        /// </summary>
        public double Z
        {
            get { return _z; }
            set { _z = Math.Round(value, 2); }
        }
    }
}
