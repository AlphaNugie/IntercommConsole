using CommonLib.Clients.Tasks;
using IntercommConsole.Core;
using IntercommConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    /// <summary>
    /// 出垛边检测任务
    /// </summary>
    public class BeyondStackTask : Task
    {
        private const int THRES = 5;
        //private readonly int[] _count_angle = new int[2], _count_rad = new int[2], _count_both = new int[2]; //分别代表左右数值
        //private readonly bool[] result = new bool[2]; //左右临时出垛边结果
        private readonly BeyondStackCounter _countLeft = new BeyondStackCounter(THRES), _countRight = new BeyondStackCounter(THRES);

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LoopContent()
        {
            //Const.OpcDatasource.SetWheelBeyondStackByDist(Const.RadarInfo.OutOfStackLeft, Const.RadarInfo.OutOfStackRight, Const.RadarInfo.DistWheelLeft, Const.RadarInfo.DistWheelRight);
            SetWheelBeyondStackByAngleDist(Const.RadarInfo.SurfaceAngleWheelLeft, Const.RadarInfo.SurfaceAngleWheelRight, Const.RadarInfo.RadiusAverageLeft, Const.RadarInfo.RadiusAverageRight);
        }

        /// <summary>
        /// 根据两侧斗轮雷达拟合平面角与测距设置左右出料堆状态
        /// </summary>
        /// <param name="left_angle">斗轮左侧雷达平面角</param>
        /// <param name="right_angle">斗轮右侧雷达平面角</param>
        /// <param name="left_dist">斗轮左侧雷达测距</param>
        /// <param name="right_dist">斗轮右侧雷达测距</param>
        public void SetWheelBeyondStackByAngleDist(double left_angle, double right_angle, double left_dist, double right_dist)
        {
            //假如平面角度阈值未启用，则角度阈值设为极低值（根据角度将一直判断入垛）；位于底层时，角度阈值减5
            double angleThres = !Config.BeyondStackAngleEnabled ? -90 : Config.BeyondStackAngleThreshold + (Const.RadarInfo.OnBottomLevel ? -5 : 0);
            //假如出料堆底线距离阈值未启用，则底线距离设为极高值（根据底线距离将一直判断入垛）；位于底层时，半径阈值减1
            double baseLine = !Config.BeyondStackBaseEnabled ? 50 : Config.BeyondStackBaseline + (Const.RadarInfo.OnBottomLevel ? -1 : 0);

            _countLeft.SetValue(left_angle, angleThres, left_dist, baseLine);
            _countRight.SetValue(right_angle, angleThres, right_dist, baseLine);
            if (_countLeft.State != 0)
                Const.OpcDatasource.WheelLeftBeyondStack = _countLeft.State == 1 ? 1 : 0;
            if (_countRight.State != 0)
                Const.OpcDatasource.WheelRightBeyondStack = _countRight.State == 1 ? 1 : 0;

            //Const.OpcDatasource.WheelLeftBeyondStack = left_angle < angleThres || left_dist > Config.BeyondStackBaseline ? 1 : 0;
            //Const.OpcDatasource.WheelRightBeyondStack = right_angle < angleThres || right_dist > Config.BeyondStackBaseline ? 1 : 0;
        }

        /// <summary>
        /// 根据两侧斗轮雷达的matlab模型出垛边返回结果与测距设置左右出料堆状态
        /// </summary>
        /// <param name="left_out">斗轮左侧雷达的matlab模型出垛边返回结果</param>
        /// <param name="right_out">斗轮右侧雷达的matlab模型出垛边返回结果</param>
        /// <param name="left_dist">斗轮左侧雷达测距</param>
        /// <param name="right_dist">斗轮右侧雷达测距</param>
        public static void SetWheelBeyondStackByDist(bool left_out, bool right_out, double left_dist, double right_dist)
        {
            Const.OpcDatasource.WheelLeftBeyondStack = left_out || left_dist > Config.BeyondStackBaseline ? 1 : 0;
            Const.OpcDatasource.WheelRightBeyondStack = right_out || right_dist > Config.BeyondStackBaseline ? 1 : 0;
        }
    }
}
