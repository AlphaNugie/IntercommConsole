using CommonLib.Clients.Tasks;
using IntercommConsole.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    public class PostureTask : Task
    {
        public PostureTask() : base() { }

        public override void Init()
        {
            Posture.WalkingPosition = Posture.PitchAngle = Posture.YawAngle = 0;
        }

        public override void LoopContent()
        {
            switch (Posture.Type)
            {
                case PostureType.GPS:
                    Posture.WalkingPosition = Const.GnssInfo.WalkingPosition;
                    Posture.PitchAngle = Const.GnssInfo.PitchAngle;
                    Posture.YawAngle = Const.GnssInfo.YawAngle;
                    Posture.WalkingValid = Posture.PitchValid = Posture.YawValid = Const.IsGnssValid;
                    Posture.WalkingInvalidResource = Posture.PitchInvalidResource = Posture.YawInvalidResource = Const.IsGnssValid ? PostureInvalidResource.OK : PostureInvalidResource.GPS;
                    break;
                case PostureType.PLC:
                    Posture.WalkingPosition = Const.OpcDatasource.WalkingPositionLeft_Plc;
                    Posture.PitchAngle = Const.OpcDatasource.PitchAngle_Plc;
                    Posture.YawAngle = Const.OpcDatasource.YawAngle_Plc;
                    Posture.WalkingValid = Posture.PitchValid = Posture.YawValid = Const.IsPlcPostureValid;
                    Posture.WalkingInvalidResource = Posture.PitchInvalidResource = Posture.YawInvalidResource = Const.IsPlcPostureValid ? PostureInvalidResource.OK : PostureInvalidResource.PLC;
                    break;
                case PostureType.AUTOMATIC:
                    ////假如编码器行走与北斗行走差值在阈值内，采用编码器，否则采用北斗
                    //Posture.WalkingPosition = Math.Abs(Const.OpcDatasource.WalkingPositionLeft_Plc - Const.GnssInfo.WalkingPosition) <= Config.WalkingThreshold ? Const.OpcDatasource.WalkingPositionLeft_Plc : Const.GnssInfo.WalkingPosition;
                    ////假如编码器回转与北斗回转差值在阈值内，采用编码器，否则采用北斗
                    //Posture.YawAngle = Math.Abs(Const.OpcDatasource.YawAngle_Plc - Const.GnssInfo.YawAngle) <= Config.YawThreshold ? Const.OpcDatasource.YawAngle_Plc : Const.GnssInfo.YawAngle;
                    #region 行走位置校对
                    bool walking_diff_limited = Math.Abs(Const.OpcDatasource.WalkingPositionLeft_Plc - Const.GnssInfo.WalkingPosition) <= Config.WalkingThreshold && Const.IsPlcPostureValid && Const.IsGnssValid; //编码器行走与北斗行走差值是否在阈值内（PLC，北斗数据同时可用）
                    Posture.WalkingPosition = !walking_diff_limited && Const.IsGnssValid ? Const.GnssInfo.WalkingPosition : Const.OpcDatasource.WalkingPositionLeft_Plc; //只有当行走差值不在阈值内同时北斗数据可用的情况下采用北斗行走位置，否则均采用编码器行走位置
                    Posture.WalkingValid = walking_diff_limited || Const.IsGnssValid || Const.IsPlcPostureValid; //行走位置只有当行走差值在阈值内或北斗/编码器可用时可用，否则不可用
                    //不可用的来源数据
                    if (walking_diff_limited)
                        Posture.WalkingInvalidResource = PostureInvalidResource.OK;
                    else if (Const.IsGnssValid)
                        Posture.WalkingInvalidResource = PostureInvalidResource.PLC;
                    else
                        Posture.WalkingInvalidResource = PostureInvalidResource.GPS;
                    #endregion
                    #region 回转角度校对
                    bool yaw_diff_limited = Math.Abs(Const.OpcDatasource.YawAngle_Plc - Const.GnssInfo.YawAngle) <= Config.YawThreshold && Const.IsPlcPostureValid && Const.IsGnssValid; //编码器回转与北斗回转差值是否在阈值内（PLC，北斗数据同时可用）
                    Posture.YawAngle = !yaw_diff_limited && Const.IsGnssValid ? Const.GnssInfo.YawAngle : Const.OpcDatasource.YawAngle_Plc; //只有当回转差值不在阈值内同时北斗数据可用的情况下采用北斗回转角度，否则均采用编码器俯仰角度
                    Posture.YawValid = yaw_diff_limited || Const.IsGnssValid || Const.IsPlcPostureValid; //回转角度只有当回转差值在阈值内或北斗/编码器可用时可用，否则不可用
                    //不可用的来源数据
                    if (yaw_diff_limited)
                        Posture.YawInvalidResource = PostureInvalidResource.OK;
                    else if (Const.IsGnssValid)
                        Posture.YawInvalidResource = PostureInvalidResource.PLC;
                    else
                        Posture.YawInvalidResource = PostureInvalidResource.GPS;
                    #endregion
                    #region 俯仰角度校对
                    //编码器俯仰与北斗俯仰差值是否在阈值内（PLC，北斗数据同时可用）
                    bool pitch_limited_gnss = Math.Abs(Const.OpcDatasource.PitchAngle_Plc - Const.GnssInfo.PitchAngle) <= Config.PitchThreshold && Const.IsPlcPostureValid && Const.IsGnssValid;
                    //编码器俯仰与惯导俯仰差值是否在阈值内（PLC，惯导数据同时可用）
                    bool pitch_limited_ins = Math.Abs(Const.OpcDatasource.PitchAngle_Plc - Const.OpcDatasource.PitchAngle_Ins) <= Config.PitchThreshold && Const.IsPlcPostureValid && Const.IsPlcInsValid;
                    //北斗俯仰与惯导俯仰差值是否在阈值内（北斗，惯导数据同时可用）
                    bool pitch_limited_gnss_ins = Math.Abs(Const.GnssInfo.PitchAngle - Const.OpcDatasource.PitchAngle_Ins) <= Config.PitchThreshold && Const.IsGnssValid && Const.IsPlcInsValid;
                    //俯仰角度的筛选条件：当PLC与北斗/惯导差值足够小时用PLC俯仰；北斗与惯导差值足够小或差距不够小但北斗定位可用时用北斗俯仰；以上都不满足但惯导可用时用惯导俯仰；否则用北斗俯仰
                    if (pitch_limited_gnss || pitch_limited_ins)
                        Posture.PitchAngle = Const.OpcDatasource.PitchAngle_Plc;
                    else if (pitch_limited_gnss_ins || Const.IsGnssValid)
                        Posture.PitchAngle = Const.GnssInfo.PitchAngle;
                    else if (Const.IsPlcInsValid)
                        Posture.PitchAngle = Const.OpcDatasource.PitchAngle_Ins;
                    else
                        Posture.PitchAngle = Const.GnssInfo.PitchAngle;
                    Posture.PitchValid = pitch_limited_gnss || pitch_limited_ins || pitch_limited_gnss_ins || Const.IsGnssValid || Const.IsPlcInsValid; //俯仰角度只在3个差值足够小或北斗/惯导俯仰可用时可用
                    Posture.PitchInvalidResource = pitch_limited_gnss || pitch_limited_ins ? PostureInvalidResource.OK : PostureInvalidResource.PLC;
                    ////假如编码器俯仰与北斗俯仰/惯导俯仰在阈值范围内，采用编码器
                    //if (Math.Abs(Const.OpcDatasource.PitchAngle_Plc - Const.GnssInfo.PitchAngle) <= Config.PitchThreshold || Math.Abs(Const.OpcDatasource.PitchAngle_Plc - Const.OpcDatasource.PitchAngle_Ins) <= Config.PitchThreshold)
                    //    Posture.PitchAngle = Const.OpcDatasource.PitchAngle_Plc;
                    ////否则采用北斗
                    //else
                    //    Posture.PitchAngle = Const.GnssInfo.PitchAngle;
                    ////否则假如北斗俯仰与惯导俯仰差值在阈值范围内，采用北斗，否则采用惯导
                    //else if (Math.Abs(Const.GnssInfo.PitchAngle - Const.OpcDatasource.PitchAngle_Ins) <= Config.PitchThreshold)
                    //    Posture.PitchAngle = Const.GnssInfo.PitchAngle;
                    //else
                    //    Posture.PitchAngle = Const.OpcDatasource.PitchAngle_Ins;
                    #endregion
                    break;
            }
        }
    }
}
