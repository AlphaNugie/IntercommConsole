using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Model
{
    /// <summary>
    /// 垛边判断计数器
    /// </summary>
    class BeyondStackCounter
    {
        private readonly uint _thres = 5; //变化阈值

        /// <summary>
        /// 角度阈值变化累计
        /// </summary>
        public int CountAngle { get; set; }

        /// <summary>
        /// 平面角度阈值是否可用，不可用则不参与判断
        /// </summary>
        public bool AngleEnabled { get; set; }

        /// <summary>
        /// 半径阈值变化累计
        /// </summary>
        public int CountRad { get; set; }

        /// <summary>
        /// 半径阈值是否可用，不可用则不参与判断
        /// </summary>
        public bool RadEnabled { get; set; }

        /// <summary>
        /// 单次判断结果累计
        /// </summary>
        public int CountBoth { get; set; }

        /// <summary>
        /// 出垛入垛状态：1 出垛，-1 入垛，0 未知
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 使用给定判断阈值初始化
        /// </summary>
        /// <param name="thres"></param>
        public BeyondStackCounter(uint thres)
        {
            _thres = thres;
        }

        /// <summary>
        /// 输入角度、半径以及对应的阈值，用来进行垛边判断
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="angleThres">角度阈值</param>
        /// <param name="dist">半径</param>
        /// <param name="distThres">半径阈值</param>
        public void SetValue(double angle, double angleThres, double dist, double distThres)
        {
            //角度累计，角度小于阈值+1、大于阈值-1、等于阈值不变；假如阈值-角度与计数符号不同，意味着变化趋势反转，计数置零，否则加上阈值-角度的符号
            CountAngle = (angleThres - angle) * CountAngle < 0 ? 0 : CountAngle + Math.Sign(angleThres - angle);
            //半径累计，半径大于阈值+1、小于阈值-1、等于阈值不变；假如半径-阈值与计数符号不同，意味着变化趋势反转，计数置零，否则加上半径-阈值的符号
            CountRad = (dist - distThres) * CountRad < 0 ? 0 : CountRad + Math.Sign(dist - distThres);
            //单次判断结果，1出垛，-1入垛，0未知；假如角度、半径累计至少有一个判断出垛则出垛，否则假如至少有一个判断入垛则入垛
            int result = 0;
            if (CountAngle >= _thres || CountRad >= _thres)
                result = 1;
            else if (CountAngle <= -1 * _thres && CountRad <= -1 * _thres)
                result = -1;
            //单次判断累计，累加单次判断结果；假如单次判断结果未知或单次判断结果与累计值符号相反（变化趋势无法维持），计数置零，否则继续累加单次判断结果
            CountBoth = result == 0 || result * CountBoth < 0 ? 0 : CountBoth + result;
            if (Math.Abs(CountBoth) >= _thres)
                State = Math.Sign(CountBoth);
        }
    }
}
