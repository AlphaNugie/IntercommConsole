using IntercommConsole.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    break;
                case PostureType.PLC:
                    Posture.WalkingPosition = Const.OpcDatasource.WalkingPositionLeft_Plc;
                    Posture.PitchAngle = Const.OpcDatasource.PitchAngle_Plc;
                    Posture.YawAngle = Const.OpcDatasource.YawAngle_Plc;
                    break;
                case PostureType.AUTOMATIC:
                    //假如编码器行走与北斗行走差值在阈值内，采用编码器，否则采用北斗
                    Posture.WalkingPosition = Math.Abs(Const.OpcDatasource.WalkingPositionLeft_Plc - Const.GnssInfo.WalkingPosition) <= Config.WalkingThreshold ? Const.OpcDatasource.WalkingPositionLeft_Plc : Const.GnssInfo.WalkingPosition;
                    //假如编码器回转与北斗回转差值在阈值内，采用编码器，否则采用北斗
                    Posture.YawAngle = Math.Abs(Const.OpcDatasource.YawAngle_Plc - Const.GnssInfo.YawAngle) <= Config.YawThreshold ? Const.OpcDatasource.YawAngle_Plc : Const.GnssInfo.YawAngle;
                    //假如编码器俯仰与北斗俯仰/惯导俯仰在阈值范围内，采用编码器
                    if (Math.Abs(Const.OpcDatasource.PitchAngle_Plc - Const.GnssInfo.PitchAngle) <= Config.PitchThreshold || Math.Abs(Const.OpcDatasource.PitchAngle_Plc - Const.OpcDatasource.PitchAngle_Ins) <= Config.PitchThreshold)
                        Posture.PitchAngle = Const.OpcDatasource.PitchAngle_Plc;
                    //否则采用北斗
                    else
                        Posture.PitchAngle = Const.GnssInfo.PitchAngle;
                    ////否则假如北斗俯仰与惯导俯仰差值在阈值范围内，采用北斗，否则采用惯导
                    //else if (Math.Abs(Const.GnssInfo.PitchAngle - Const.OpcDatasource.PitchAngle_Ins) <= Config.PitchThreshold)
                    //    Posture.PitchAngle = Const.GnssInfo.PitchAngle;
                    //else
                    //    Posture.PitchAngle = Const.OpcDatasource.PitchAngle_Ins;
                    break;
            }
        }
    }
}
